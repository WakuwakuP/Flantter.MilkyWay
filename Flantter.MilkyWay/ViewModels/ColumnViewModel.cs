﻿using Flantter.MilkyWay.Common;
using Flantter.MilkyWay.Models;
using Flantter.MilkyWay.Models.Twitter.Objects;
using Flantter.MilkyWay.Setting;
using Flantter.MilkyWay.ViewModels.Twitter.Objects;
using Flantter.MilkyWay.Views.Util;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Reactive.Concurrency;
using Flantter.MilkyWay.ViewModels.Services;
using System.Reactive.Subjects;
using System.Reactive.Disposables;

namespace Flantter.MilkyWay.ViewModels
{
    public class ColumnViewModel : IDisposable
    {
        protected CompositeDisposable Disposable { get; private set; } = new CompositeDisposable();

        public ColumnModel Model { get; set; }

        public ReactiveProperty<double> Height { get; private set; }
        public ReactiveProperty<double> Width { get; private set; }
        public ReactiveProperty<double> Left { get; private set; }

		public ReactiveProperty<Symbol> ActionSymbol { get; private set; }
		public ReactiveProperty<string> Name { get; private set; }
        public ReactiveProperty<string> ScreenName { get; private set; }
		public ReactiveProperty<bool> EnableCreateFilterColumn { get; private set; }
		public ReactiveProperty<Symbol> StreamingSymbol { get; private set; }
        public ReactiveProperty<bool> IsEnabledStreaming { get; private set; }
        public ReactiveProperty<bool> Updating { get; private set; }

        public ReadOnlyReactiveCollection<object> Tweets { get; private set; }

        public ReactiveProperty<int> Index { get; private set; }

        public ReactiveProperty<int> SelectedIndex { get; private set; }

        public ReactiveProperty<int> UnreadCount { get; private set; }
        public ReactiveProperty<bool> UnreadCountIncrementalTrigger { get; private set; }
        public ReactiveProperty<bool> IsScrollControlEnabled { get; private set; }
        public ReactiveProperty<bool> IsScrollLockEnabled { get; private set; }
        public ReactiveProperty<bool> IsScrollLockToTopEnabled { get; private set; }

        public ReactiveCommand StreamingCommand { get; private set; }

        public ReactiveCommand ScrollToTopCommand { get; private set; }

        public ReactiveCommand IncrementalLoadCommand { get; private set; }

        public ReactiveCommand RefreshCommand { get; private set; }

        public ReactiveCommand TweetDoubleTappedActionCommand { get; private set; }

        #region Constructor

        public ColumnViewModel(ColumnModel column)
        {
            this.Model = column;
            
			this.ActionSymbol = column.ObserveProperty(x => x.Action).Select(x =>
			{
				switch (x)
				{
					case SettingSupport.ColumnTypeEnum.Home:
						return Symbol.Home;
					case SettingSupport.ColumnTypeEnum.Mentions:
						return Symbol.Account;
					case SettingSupport.ColumnTypeEnum.DirectMessages:
						return Symbol.Mail;
					case SettingSupport.ColumnTypeEnum.Favorites:
						return Symbol.Favorite;
					case SettingSupport.ColumnTypeEnum.Events:
						return Symbol.Important;
					case SettingSupport.ColumnTypeEnum.Search:
						return Symbol.Find;
					case SettingSupport.ColumnTypeEnum.List:
						return Symbol.Bullets;
					case SettingSupport.ColumnTypeEnum.UserTimeline:
						return Symbol.Contact;
					case SettingSupport.ColumnTypeEnum.Filter:
						return Symbol.Repair;
					default:
						return Symbol.Help;
				}
			}).ToReactiveProperty().AddTo(this.Disposable);
			this.EnableCreateFilterColumn = column.ObserveProperty(x => x.Action).Select(x => x == SettingSupport.ColumnTypeEnum.Home).ToReactiveProperty().AddTo(this.Disposable);
            this.Name = column.ObserveProperty(x => x.Name).ToReactiveProperty().AddTo(this.Disposable);
            this.ScreenName = column.ObserveProperty(x => x.ScreenName).ToReactiveProperty().AddTo(this.Disposable);
			this.StreamingSymbol = column.ObserveProperty(x => x.Streaming).Select(x => x ? Symbol.Pause : Symbol.Play).ToReactiveProperty().AddTo(this.Disposable);
            this.Index = column.ObserveProperty(x => x.Index).ToReactiveProperty().AddTo(this.Disposable);
            this.IsEnabledStreaming = column.ObserveProperty(x => x.Action).Select(x =>
            {
                switch (x)
                {
                    case SettingSupport.ColumnTypeEnum.Search:
                        return true;
                    case SettingSupport.ColumnTypeEnum.List:
                        return true;
                    case SettingSupport.ColumnTypeEnum.Home:
                        return true;
                    default:
                        return false;
                }
            }).ToReactiveProperty().AddTo(this.Disposable);
            this.Updating = column.ObserveProperty(x => x.Updating).ToReactiveProperty().AddTo(this.Disposable);

            this.SelectedIndex = column.ToReactivePropertyAsSynchronized(x => x.SelectedIndex).AddTo(this.Disposable);

            this.UnreadCount = column.ToReactivePropertyAsSynchronized(x => x.UnreadCount).AddTo(this.Disposable);
            this.UnreadCountIncrementalTrigger = column.ToReactivePropertyAsSynchronized(x => x.UnreadCountIncrementalTrigger).AddTo(this.Disposable);
            this.IsScrollControlEnabled = column.ObserveProperty(x => x.IsScrollControlEnabled).ToReactiveProperty().AddTo(this.Disposable);
            this.IsScrollLockEnabled = column.ObserveProperty(x => x.IsScrollLockEnabled).ToReactiveProperty().AddTo(this.Disposable);
            this.IsScrollLockToTopEnabled = new ReactiveProperty<bool>().AddTo(this.Disposable);

            this.StreamingCommand = column.ObserveProperty(x => x.Action).Select(x =>
            {
                switch (x)
                {
                    case SettingSupport.ColumnTypeEnum.Home:
                        return true;
                    case SettingSupport.ColumnTypeEnum.List:
                        return true;
                    case SettingSupport.ColumnTypeEnum.Search:
                        return true;
                    default:
                        return false;
                }
            }).ToReactiveCommand().AddTo(this.Disposable);
            this.StreamingCommand.SubscribeOn(ThreadPoolScheduler.Default).Subscribe(_ => { this.Model.Streaming = !this.Model.Streaming; }).AddTo(this.Disposable);

            this.ScrollToTopCommand = column.ObserveProperty(x => x.UnreadCount).Select(x => x != 0).ToReactiveCommand().AddTo(this.Disposable);
            this.ScrollToTopCommand.SubscribeOn(ThreadPoolScheduler.Default).Subscribe(async _ => 
            {
                this.IsScrollLockToTopEnabled.Value = true;
                await Task.Delay(100);
                this.IsScrollLockToTopEnabled.Value = false;
            }).AddTo(this.Disposable);

            this.IncrementalLoadCommand = new ReactiveCommand().AddTo(this.Disposable);
            this.IncrementalLoadCommand.SubscribeOn(ThreadPoolScheduler.Default).Subscribe(async _ => 
            {
                var id = this.Model.Tweets.Last().Id;
                var status = this.Model.Tweets.Last() as Status;
                if (status != null && status.HasRetweetInformation)
                    id = status.RetweetInformation.Id;
                
                await this.Model.Update(id);
            }).AddTo(this.Disposable);

            this.RefreshCommand = new ReactiveCommand().AddTo(this.Disposable);
            this.RefreshCommand.SubscribeOn(ThreadPoolScheduler.Default).Subscribe(async _ =>
            {
                await this.Model.Update();
            }).AddTo(this.Disposable);

            this.TweetDoubleTappedActionCommand = new ReactiveCommand().AddTo(this.Disposable);
            this.TweetDoubleTappedActionCommand.SubscribeOn(ThreadPoolScheduler.Default).Subscribe(_ =>
            {
                if (this.SelectedIndex.Value == -1)
                    return;

                var tweet = this.Tweets[this.SelectedIndex.Value];

                string screenName = string.Empty;
                if (tweet is StatusViewModel)
                    screenName = ((StatusViewModel)tweet).ScreenName;
                if (tweet is DirectMessageViewModel)
                    screenName = ((DirectMessageViewModel)tweet).ScreenName;
                if (tweet is EventMessageViewModel)
                    screenName = ((EventMessageViewModel)tweet).ScreenName;

                var status = this.Tweets[this.SelectedIndex.Value] as StatusViewModel;

                switch (SettingService.Setting.DoubleTappedAction)
                {
                    case SettingSupport.DoubleTappedActionEnum.None:
                        break;
                    case SettingSupport.DoubleTappedActionEnum.StatusDetail:
                        if (status == null)
                            break;

                        Notice.Instance.ShowStatusDetailCommand.Execute(status.Model);
                        break;
                    case SettingSupport.DoubleTappedActionEnum.UserProfile:
                        if (string.IsNullOrWhiteSpace(screenName))
                            break;
                        
                        Notice.Instance.ShowUserProfileCommand.Execute(screenName);
                        break;
                    case SettingSupport.DoubleTappedActionEnum.Favorite:
                        if (status == null)
                            break;

                        Notice.Instance.FavoriteCommand.Execute(status);
                        break;
                    case SettingSupport.DoubleTappedActionEnum.Reply:
                        if (status != null)
                            Notice.Instance.ReplyCommand.Execute(status);
                        else if (string.IsNullOrWhiteSpace(screenName))
                            Notice.Instance.ReplyCommand.Execute(screenName);

                        break;
                    case SettingSupport.DoubleTappedActionEnum.Retweet:
                        if (status == null)
                            break;

                        Notice.Instance.RetweetCommand.Execute(status);
                        break;
                    default:
                        break;
                }

            }).AddTo(this.Disposable);

            this.Height = LayoutHelper.Instance.ColumnHeight;

            this.Width = LayoutHelper.Instance.ColumnWidth;

            this.Left = Observable.CombineLatest<int, double, double>(
                this.Index,
                LayoutHelper.Instance.ColumnWidth,
                (index, columnWidth) =>
                {
                    if (WindowSizeHelper.Instance.ClientWidth < 352.0)
                        return index * (columnWidth + 10.0) + 352.0;
                    else
                        return 5.0 + index * (columnWidth + 10.0) + 352.0;
                }).ToReactiveProperty().AddTo(this.Disposable);
            
            this.Tweets = this.Model.Tweets.ToReadOnlyReactiveCollection(item => 
            {
                if (item is Status)
                    return new StatusViewModel((Status)item, this.Model.Tokens.UserId) as object;
                else if (item is DirectMessage)
                    return new DirectMessageViewModel((DirectMessage)item, this.Model.Tokens.UserId) as object;
                else
                    return new EventMessageViewModel((EventMessage)item, this.Model.Tokens.UserId) as object;
            }).AddTo(this.Disposable);
        }
        #endregion

        #region Destructor
        ~ColumnViewModel()
        {
        }
        #endregion

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
