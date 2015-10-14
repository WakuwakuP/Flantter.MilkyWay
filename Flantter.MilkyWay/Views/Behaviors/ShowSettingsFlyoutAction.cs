﻿using CoreTweet;
using Flantter.MilkyWay.Models.Twitter.Objects;
using Flantter.MilkyWay.Views.Contents;
using Flantter.MilkyWay.Views.Contents.SettingsFlyouts;
using Flantter.MilkyWay.Views.Controls;
using Flantter.MilkyWay.Views.Util;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Flantter.MilkyWay.Views.Behaviors
{
    public class ShowSettingsFlyoutAction : DependencyObject, IAction
    {
        private List<ExtendedSettingsFlyout> _SettingsFlyoutList = null;

        public ShowSettingsFlyoutAction()
        {
            this._SettingsFlyoutList = new List<ExtendedSettingsFlyout>();
        }

        public object Execute(object sender, object parameter)
        {
            var notification = parameter as ShowSettingsFlyoutNotification;
            if (notification == null)
                return null;

            ExtendedSettingsFlyout settingsFlyout = null;
            IEnumerable<ExtendedSettingsFlyout> settingsFlyoutList = null;

            switch (notification.SettingsFlyoutType)
            {
                case "Search":
                    if (_SettingsFlyoutList.Where(x => x.IsOpen).Count() > 0)
                        break;

                    settingsFlyoutList = _SettingsFlyoutList.Where(x => x is SearchSettingsFlyout && !x.IsOpen);
                    if (settingsFlyoutList.Count() > 0)
                    {
                        settingsFlyout = settingsFlyoutList.First();
                    }
                    else
                    {
                        settingsFlyout = new SearchSettingsFlyout();
                        ((SearchSettingsFlyout)settingsFlyout).ViewModel = new ViewModels.SettingsFlyouts.SearchSettingsFlyoutViewModel();
                        ((SearchSettingsFlyout)settingsFlyout).DataContext = ((SearchSettingsFlyout)settingsFlyout).ViewModel;
                        this._SettingsFlyoutList.Add(settingsFlyout);
                    }
                    ((SearchSettingsFlyout)settingsFlyout).ViewModel.ClearCommand.Execute();

                    ((SearchSettingsFlyout)settingsFlyout).ViewModel.IconSource.Value = notification.UserIcon;
                    ((SearchSettingsFlyout)settingsFlyout).ViewModel.Tokens.Value = notification.Tokens;
                    ((SearchSettingsFlyout)settingsFlyout).ViewModel.StatusSearchWords.Value = notification.Content as string;

                    settingsFlyout.Show();
                    break;
                case "UserProfile":
                    settingsFlyoutList = _SettingsFlyoutList.Where(x => x is UserProfileSettingsFlyout && !x.IsOpen);
                    if (settingsFlyoutList.Count() > 0)
                    {
                        settingsFlyout = settingsFlyoutList.First();
                    }
                    else
                    {
                        settingsFlyout = new UserProfileSettingsFlyout();
                        ((UserProfileSettingsFlyout)settingsFlyout).ViewModel = new ViewModels.SettingsFlyouts.UserProfileSettingsFlyoutViewModel();
                        this._SettingsFlyoutList.Add(settingsFlyout);
                    }
                    ((UserProfileSettingsFlyout)settingsFlyout).ViewModel.ClearCommand.Execute();

                    ((UserProfileSettingsFlyout)settingsFlyout).ViewModel.IconSource.Value = notification.UserIcon;
                    ((UserProfileSettingsFlyout)settingsFlyout).ViewModel.Tokens.Value = notification.Tokens;
                    ((UserProfileSettingsFlyout)settingsFlyout).ViewModel.ScreenName.Value = notification.Content as string;

                    ((UserProfileSettingsFlyout)settingsFlyout).ViewModel.UpdateCommand.Execute();

                    settingsFlyout.Show();
                    break;
                case "Conversation":
                    settingsFlyoutList = _SettingsFlyoutList.Where(x => x is ConversationSettingsFlyout && !x.IsOpen);
                    if (settingsFlyoutList.Count() > 0)
                    {
                        settingsFlyout = settingsFlyoutList.First();
                    }
                    else
                    {
                        settingsFlyout = new ConversationSettingsFlyout();
                        ((ConversationSettingsFlyout)settingsFlyout).ViewModel = new ViewModels.SettingsFlyouts.ConversationSettingsFlyoutViewModel();
                        this._SettingsFlyoutList.Add(settingsFlyout);
                    }
                    ((ConversationSettingsFlyout)settingsFlyout).ViewModel.ClearCommand.Execute();

                    ((ConversationSettingsFlyout)settingsFlyout).ViewModel.IconSource.Value = notification.UserIcon;
                    ((ConversationSettingsFlyout)settingsFlyout).ViewModel.Tokens.Value = notification.Tokens;
                    ((ConversationSettingsFlyout)settingsFlyout).ViewModel.ConversationStatus.Value = notification.Content as Models.Twitter.Objects.Status;

                    ((ConversationSettingsFlyout)settingsFlyout).ViewModel.UpdateCommand.Execute();

                    settingsFlyout.Show();
                    break;
            }

            return null;
        }
    }
    public class ShowSettingsFlyoutNotification : Notification
    {
        /// <summary>
        /// SettingsFlyoutの種類
        /// </summary>
        public string SettingsFlyoutType { get; set; }

        /// <summary>
        /// CoreTweetのTokens
        /// </summary>
        public Tokens Tokens { get; set; }

        /// <summary>
        /// ユーザーのアイコン
        /// </summary>
        public string UserIcon { get; set; }
    }

}
