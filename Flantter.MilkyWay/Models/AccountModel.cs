﻿using CoreTweet;
using Flantter.MilkyWay.Common;
using Flantter.MilkyWay.Models.Services;
using Flantter.MilkyWay.Models.Twitter;
using Flantter.MilkyWay.Setting;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Flantter.MilkyWay.Models
{
    public class AccountModel : BindableBase
    {
        #region IsEnabled変更通知プロパティ
        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { this.SetProperty(ref this._IsEnabled, value); }
        }
        #endregion

        #region ProfileImageUrl変更通知プロパティ
        private string _ProfileImageUrl;
        public string ProfileImageUrl
        {
            get { return this._ProfileImageUrl; }
            set { this.SetProperty(ref this._ProfileImageUrl, value); }
        }
        #endregion

        #region ProfileBannerUrl変更通知プロパティ
        private string _ProfileBannerUrl;
        public string ProfileBannerUrl
        {
            get { return this._ProfileBannerUrl; }
            set { this.SetProperty(ref this._ProfileBannerUrl, value); }
        }
        #endregion

        #region Name変更通知プロパティ
        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set { this.SetProperty(ref this._Name, value); }
        }
        #endregion

        #region ScreenName変更通知プロパティ
        private string _ScreenName;
        public string ScreenName
        {
            get { return this._ScreenName; }
            set { this.SetProperty(ref this._ScreenName, value); }
        }
        #endregion

        #region UserId変更通知プロパティ
        private long _UserId;
        public long UserId
        {
            get { return this._UserId; }
            set { this.SetProperty(ref this._UserId, value); }
        }
        #endregion

        #region IncludeFollowingsActivity変更通知プロパティ
        private bool _IncludeFollowingsActivity;
        public bool IncludeFollowingsActivity
        {
            get { return this._IncludeFollowingsActivity; }
            set { this.SetProperty(ref this._IncludeFollowingsActivity, value); }
        }
        #endregion

        #region PossiblySensitive変更通知プロパティ
        private bool _PossiblySensitive;
        public bool PossiblySensitive
        {
            get { return this._PossiblySensitive; }
            set { this.SetProperty(ref this._PossiblySensitive, value); }
        }
        #endregion

        #region Columns
        private ObservableCollection<ColumnModel> _Columns;
        private ReadOnlyObservableCollection<ColumnModel> _ReadOnlyColumns;
        public ReadOnlyObservableCollection<ColumnModel> ReadOnlyColumns
        {
            get
            {
                return _ReadOnlyColumns;
            }
        }
        #endregion

        #region Tokens
        public Tokens Tokens { get; set; }
        #endregion
        #region AccountSetting
        private AccountSetting _AccountSetting;
        #endregion

        #region Initialize
        public void Initialize()
        {
            Connecter.Instance.AddAccount(this._AccountSetting);

            this.IsEnabled = this._AccountSetting.IsEnabled;
            this.IncludeFollowingsActivity = this._AccountSetting.IncludeFollowingsActivity;
            this.Name = this._AccountSetting.Name;
            this.PossiblySensitive = this._AccountSetting.PossiblySensitive;
            this.ProfileBannerUrl = this._AccountSetting.ProfileBannerUrl;
            this.ProfileImageUrl = this._AccountSetting.ProfileImageUrl;
            this.ScreenName = this._AccountSetting.ScreenName;
            this.UserId = this._AccountSetting.UserId;

            foreach (var columnModel in this._Columns)
            {
                Task.Run(async () => await columnModel.Initialize());
            }

            Task.Run(async () => 
            {
                var user = await this.Tokens.Users.ShowAsync(user_id => this.UserId);
                this.ProfileImageUrl = user.ProfileImageUrl.Replace("_normal", "");
                this.ProfileBannerUrl = user.ProfileBannerUrl;
                this.Name = user.Name;

                this._AccountSetting.ProfileImageUrl = user.ProfileImageUrl.Replace("_normal", "");
                this._AccountSetting.ProfileBannerUrl = user.ProfileBannerUrl;
                this._AccountSetting.Name = user.Name;
            });
        }
        #endregion

        #region Constructor
        public AccountModel()
        {
            this._Columns = new ObservableCollection<ColumnModel>();
            this._ReadOnlyColumns = new ReadOnlyObservableCollection<ColumnModel>(this._Columns);

            this.ProfileImageUrl = "";
            this.IsEnabled = true;
        }

        public AccountModel(AccountSetting account)
        {
            this._Columns = new ObservableCollection<ColumnModel>();
            this._ReadOnlyColumns = new ReadOnlyObservableCollection<ColumnModel>(this._Columns);
            
            this.Tokens = Tokens.Create(account.ConsumerKey, account.ConsumerSecret, account.AccessToken, account.AccessTokenSecret, account.UserId, account.ScreenName);
            this.Tokens.ConnectionOptions.UserAgent = TwitterConnectionHelper.GetUserAgent(this.Tokens);
            
            this._AccountSetting = account;
            
            foreach (var column in account.Column)
                this._Columns.Add(new ColumnModel(column, account, this));
        }
        #endregion

        public void AddColumn(ColumnSetting column)
        {
            if (column.Index == -1)
                column.Index = this._Columns.Count;

            //this._AccountSetting.Column.Add(column);

            var columnModel = new ColumnModel(column, this._AccountSetting, this);
            this._Columns.Add(columnModel);

            Task.Run(async () => await columnModel.Initialize());
        }

        public void DisconnectAllFilterStreaming(object sender = null)
        {
            foreach (var column in this._Columns)
            {
                if (column == sender)
                    continue;

                if (column.Action == SettingSupport.ColumnTypeEnum.Search || column.Action == SettingSupport.ColumnTypeEnum.List)
                    column.Streaming = false;
            }
        }

        public async Task GetMentionStatus(Twitter.Objects.Status status)
        {
            try
            {
                var mentionStatus = await this.Tokens.Statuses.ShowAsync(id => status.InReplyToStatusId);
                status.MentionStatus = new Twitter.Objects.Status(mentionStatus);

                Connecter.Instance.TweetReceive_OnCommandExecute(this, new TweetEventArgs(status.MentionStatus, this._UserId, new List<string>() { "none://" }, false));
            }
            catch
            {
                // Todo : 通知
            } 
        }

        public async Task Retweet(Twitter.Objects.Status status)
        {
            try
            {
                var retweetedResponse = await this.Tokens.Statuses.RetweetAsync(id => status.Id);
                status.IsRetweeted = true;
            }
            catch
            {
                // Todo : 通知
            }
        }

        public async Task DestroyRetweet(Twitter.Objects.Status status)
        {
            try
            {
                long currentUserRetweet = 0;
                if (status.HasRetweetInformation)
                {
                    currentUserRetweet = status.RetweetInformation.Id;
                }
                else
                {
                    var retweetedStatus = await this.Tokens.Statuses.ShowAsync(id => status.Id, include_my_retweet => true);
                    if (!retweetedStatus.CurrentUserRetweet.HasValue)
                        return;

                    currentUserRetweet = retweetedStatus.CurrentUserRetweet.Value;
                }

                var retweetedResponse = await this.Tokens.Statuses.RetweetAsync(id => currentUserRetweet);
                status.IsRetweeted = false;
            }
            catch
            {
                // Todo : 通知
            }
        }

        public async Task Favorite(Twitter.Objects.Status status)
        {
            try
            {
                var retweetedResponse = await this.Tokens.Favorites.CreateAsync(id => status.Id);
                status.IsFavorited = true;
            }
            catch
            {
                // Todo : 通知
            }
        }

        public async Task DestroyFavorite(Twitter.Objects.Status status)
        {
            try
            {
                var retweetedResponse = await this.Tokens.Favorites.DestroyAsync(id => status.Id);
                status.IsFavorited = false;
            }
            catch
            {
                // Todo : 通知
            }
        }
    }
}
