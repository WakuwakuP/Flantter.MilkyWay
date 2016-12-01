﻿using Flantter.MilkyWay.Views.Util;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flantter.MilkyWay.ViewModels.Services
{
    public class Notice
    {
        private static Notice _Instance = new Notice();
        private Notice()
        {
            this.ShowUserProfileCommand = new ReactiveCommand();
            this.ShowConversationCommand = new ReactiveCommand();
            this.ShowMediaCommand = new ReactiveCommand();
            this.ShowStatusDetailCommand = new ReactiveCommand();
            this.LoadMentionCommand = new ReactiveCommand();
            this.ReplyCommand = new ReactiveCommand();
            this.RetweetCommand = new ReactiveCommand();
            this.FavoriteCommand = new ReactiveCommand();
            this.RetweetFavoriteCommand = new ReactiveCommand();
            this.UrlClickCommand = new ReactiveCommand();
            this.ReplyToAllCommand = new ReactiveCommand();
            this.ReplyToStatusesCommand = new ReactiveCommand();
            this.SendDirectMessageCommand = new ReactiveCommand();
            this.UrlQuoteRetweetCommand = new ReactiveCommand();
            this.CopyTweetCommand = new ReactiveCommand();
            this.CopyTweetUrlCommand = new ReactiveCommand();
            this.ShowRetweetersCommand = new ReactiveCommand();
            this.MuteUserCommand = new ReactiveCommand();
            this.MuteClientCommand = new ReactiveCommand();
            this.DeleteTweetCommand = new ReactiveCommand();
            this.DeleteRetweetCommand = new ReactiveCommand();
            this.DeleteFromCollectionCommand = new ReactiveCommand();
            this.AddToCollectionCommand = new ReactiveCommand();
            this.ShowUserListsCommand = new ReactiveCommand();
            this.ShowUserCollectionsCommand = new ReactiveCommand();
            this.OpenStatusUrlCommand = new ReactiveCommand();
            this.ShowSearchCommand = new ReactiveCommand();
            this.ShareStatusCommand = new ReactiveCommand();
            this.ShowListMembersCommand = new ReactiveCommand();
            this.ShowListStatusesCommand = new ReactiveCommand();
            this.ShowCollectionStatusesCommand = new ReactiveCommand();
            this.ShowRetweetsOfMeCommand = new ReactiveCommand();
            this.ShowUserFollowInfoCommand = new ReactiveCommand();
            this.RetweetStatusesCommand = new ReactiveCommand();
            this.FavoriteStatusesCommand = new ReactiveCommand();
            this.ShowMyListsCommand = new ReactiveCommand();
            this.ShowMyCollectionsCommand = new ReactiveCommand();
            this.GetGapStatusCommand = new ReactiveCommand();

            this.AddListColumnCommand = new ReactiveCommand();
            this.AddFilterColumnCommand = new ReactiveCommand();
            this.AddCollectionColumnCommand = new ReactiveCommand();

            this.TweetAreaAccountChangeCommand = new ReactiveCommand();
            this.TweetAreaDeletePictureCommand = new ReactiveCommand();
            this.TweetAreaOpenCommand = new ReactiveCommand();

            this.SearchSettingsFlyoutDeleteSearchQueryCommand = new ReactiveCommand();

            this.ShowSettingsFlyoutCommand = new ReactiveCommand();

            this.DonateCommand = new ReactiveCommand();

            this.AuthAccountCommand = new ReactiveCommand();
            this.AddAccountCommand = new ReactiveCommand();
            this.AddColumnCommand = new ReactiveCommand();
            this.DeleteAccountCommand = new ReactiveCommand();
            this.DeleteColumnCommand = new ReactiveCommand();

            this.SortColumnCommand = new ReactiveCommand();
            this.ChangeColumnSelectedIndexCommand = new ReactiveCommand();

            this.OpenCollectionCommand = new ReactiveCommand();

            this.ShowLeftSwipeMenuCommand = new ReactiveCommand();
            this.ShowChangeAccountCommand = new ReactiveCommand();
            this.ChangeAccountCommand = new ReactiveCommand();
            this.ExitAppCommand = new ReactiveCommand();

            this.ShowMainSettingCommand = new ReactiveCommand();
            this.ShowBehaviorSettingCommand = new ReactiveCommand();
            this.ShowPostingSettingCommand = new ReactiveCommand();
            this.ShowDisplaySettingCommand = new ReactiveCommand();
            this.ShowNotificationSettingCommand = new ReactiveCommand();
            this.ShowMuteSettingCommand = new ReactiveCommand();
            this.ShowDatabaseSettingCommand = new ReactiveCommand();
            this.ShowAccountsSettingCommand = new ReactiveCommand();
            this.ShowAccountSettingCommand = new ReactiveCommand();
            this.ShowAdvancedSettingCommand = new ReactiveCommand();
            this.ShowAppInfoCommand = new ReactiveCommand();
            this.ShowSupportAccountCommand = new ReactiveCommand();
            this.ShowColumnSettingCommand = new ReactiveCommand();
            this.ChangeBackgroundImageCommand = new ReactiveCommand();
            this.ChangeThemeCommand = new ReactiveCommand();

            this.DeleteMuteUserCommand = new ReactiveCommand();
            this.DeleteMuteClientCommand = new ReactiveCommand();
            this.UpdateMuteFilterCommand = new ReactiveCommand();

            this.CopySelectedTweetCommand = new ReactiveCommand();
            this.ReplyToSelectedTweetCommand = new ReactiveCommand();
            this.SendDirectMessageToSelectedTweetCommand = new ReactiveCommand();
            this.FavoriteSelectedTweetCommand = new ReactiveCommand();
            this.RetweetSelectedTweetCommand = new ReactiveCommand();
            this.ShowUserProfileOfSelectedTweetCommand = new ReactiveCommand();
            this.ShowConversationOfSelectedTweetCommand = new ReactiveCommand();
            this.ChangeSelectedTweetCommand = new ReactiveCommand();

            this.IncrementColumnSelectedIndexCommand = new ReactiveCommand();
            this.DecrementColumnSelectedIndexCommand = new ReactiveCommand();

            this.DeleteDatabaseFileCommand = new ReactiveCommand();

            this.ShowFilePickerMessenger = new Messenger();
            this.ShowComfirmMessageDialogMessenger = new Messenger();
            this.ShowMessageDialogMessenger = new Messenger();
            this.ShowAuthorizePopupMessenger = new Messenger();
        }

        public static Notice Instance
        {
            get { return _Instance; }
        }

        public Messenger ShowFilePickerMessenger { get; private set; }
        public Messenger ShowComfirmMessageDialogMessenger { get; private set; }
        public Messenger ShowMessageDialogMessenger { get; private set; }
        public Messenger ShowAuthorizePopupMessenger { get; private set; }

        public ReactiveCommand ShowUserProfileCommand { get; private set; }
        public ReactiveCommand ShowConversationCommand { get; private set; }
        public ReactiveCommand ShowMediaCommand { get; private set; }
        public ReactiveCommand ShowStatusDetailCommand { get; private set; }
        public ReactiveCommand LoadMentionCommand { get; private set; }
        public ReactiveCommand ReplyCommand { get; private set; }
        public ReactiveCommand RetweetCommand { get; private set; }
        public ReactiveCommand FavoriteCommand { get; private set; }
        public ReactiveCommand RetweetFavoriteCommand { get; private set; }
        public ReactiveCommand UrlClickCommand { get; private set; }
        public ReactiveCommand ReplyToAllCommand { get; private set; }
        public ReactiveCommand ReplyToStatusesCommand { get; private set; }
        public ReactiveCommand SendDirectMessageCommand { get; private set; }
        public ReactiveCommand UrlQuoteRetweetCommand { get; private set; }
        public ReactiveCommand CopyTweetCommand { get; private set; }
        public ReactiveCommand CopyTweetUrlCommand { get; private set; }
        public ReactiveCommand ShowRetweetersCommand { get; private set; }
        public ReactiveCommand MuteUserCommand { get; private set; }
        public ReactiveCommand MuteClientCommand { get; private set; }
        public ReactiveCommand DeleteTweetCommand { get; private set; }
        public ReactiveCommand DeleteRetweetCommand { get; private set; }
        public ReactiveCommand DeleteFromCollectionCommand { get; private set; }
        public ReactiveCommand AddToCollectionCommand { get; private set; }
        public ReactiveCommand ShowUserListsCommand { get; private set; }
        public ReactiveCommand ShowUserCollectionsCommand { get; private set; }
        public ReactiveCommand OpenStatusUrlCommand { get; private set; }
        public ReactiveCommand ShowSearchCommand { get; private set; }
        public ReactiveCommand ShareStatusCommand { get; private set; }
        public ReactiveCommand ShowListMembersCommand { get; private set; }
        public ReactiveCommand ShowListStatusesCommand { get; private set; }
        public ReactiveCommand ShowCollectionStatusesCommand { get; private set; }
        public ReactiveCommand ShowRetweetsOfMeCommand { get; private set; }
        public ReactiveCommand ShowUserFollowInfoCommand { get; private set; }
        public ReactiveCommand RetweetStatusesCommand { get; private set; }
        public ReactiveCommand FavoriteStatusesCommand { get; private set; }
        public ReactiveCommand ShowMyListsCommand { get; private set; }
        public ReactiveCommand ShowMyCollectionsCommand { get; private set; }
        public ReactiveCommand GetGapStatusCommand { get; private set; }

        public ReactiveCommand AddListColumnCommand { get; private set; }
        public ReactiveCommand AddFilterColumnCommand { get; private set; }
        public ReactiveCommand AddCollectionColumnCommand { get; private set; }

        public ReactiveCommand TweetAreaAccountChangeCommand { get; private set; }
        public ReactiveCommand TweetAreaDeletePictureCommand { get; private set; }
        public ReactiveCommand TweetAreaOpenCommand { get; private set; }

        public ReactiveCommand SearchSettingsFlyoutDeleteSearchQueryCommand { get; private set; }

        public ReactiveCommand ShowSettingsFlyoutCommand { get; private set; }
        
        public ReactiveCommand DonateCommand { get; private set; }

        public ReactiveCommand AuthAccountCommand { get; private set; }
        public ReactiveCommand AddAccountCommand { get; private set; }
        public ReactiveCommand AddColumnCommand { get; private set; }
        public ReactiveCommand DeleteAccountCommand { get; private set; }
        public ReactiveCommand DeleteColumnCommand { get; private set; }

        public ReactiveCommand SortColumnCommand { get; private set; }
        public ReactiveCommand ChangeColumnSelectedIndexCommand { get; private set; }

        public ReactiveCommand OpenCollectionCommand { get; private set; }

        public ReactiveCommand ShowLeftSwipeMenuCommand { get; private set; }
        public ReactiveCommand ShowChangeAccountCommand { get; private set; }
        public ReactiveCommand ChangeAccountCommand { get; private set; }
        public ReactiveCommand ExitAppCommand { get; private set; }

        public ReactiveCommand ShowMainSettingCommand { get; private set; }
        public ReactiveCommand ShowBehaviorSettingCommand { get; private set; }
        public ReactiveCommand ShowPostingSettingCommand { get; private set; }
        public ReactiveCommand ShowDisplaySettingCommand { get; private set; }
        public ReactiveCommand ShowNotificationSettingCommand { get; private set; }
        public ReactiveCommand ShowMuteSettingCommand { get; private set; }
        public ReactiveCommand ShowDatabaseSettingCommand { get; private set; }
        public ReactiveCommand ShowAccountsSettingCommand { get; private set; }
        public ReactiveCommand ShowAccountSettingCommand { get; private set; }
        public ReactiveCommand ShowAdvancedSettingCommand { get; private set; }
        public ReactiveCommand ShowAppInfoCommand { get; private set; }
        public ReactiveCommand ShowSupportAccountCommand { get; private set; }
        public ReactiveCommand ShowColumnSettingCommand { get; private set; }
        public ReactiveCommand ChangeBackgroundImageCommand { get; private set; }
        public ReactiveCommand ChangeThemeCommand { get; private set; }
        
        public ReactiveCommand DeleteMuteUserCommand { get; private set; }
        public ReactiveCommand DeleteMuteClientCommand { get; private set; }
        public ReactiveCommand UpdateMuteFilterCommand { get; private set; }

        public ReactiveCommand CopySelectedTweetCommand { get; private set; }
        public ReactiveCommand ReplyToSelectedTweetCommand { get; private set; }
        public ReactiveCommand SendDirectMessageToSelectedTweetCommand { get; private set; }
        public ReactiveCommand FavoriteSelectedTweetCommand { get; private set; }
        public ReactiveCommand RetweetSelectedTweetCommand { get; private set; }
        public ReactiveCommand ShowUserProfileOfSelectedTweetCommand { get; private set; }
        public ReactiveCommand ShowConversationOfSelectedTweetCommand { get; private set; }
        public ReactiveCommand ChangeSelectedTweetCommand { get; private set; }
        public ReactiveCommand IncrementColumnSelectedIndexCommand { get; private set; }
        public ReactiveCommand DecrementColumnSelectedIndexCommand { get; private set; }

        public ReactiveCommand DeleteDatabaseFileCommand { get; private set; }
    }

    public class NoticeProvider
    {
        public Notice Notice { get { return Notice.Instance; } }
    }


    public class ShareNotice
    {
        private static ShareNotice _Instance = new ShareNotice();
        private ShareNotice()
        {
            this.ShareContractAccountChangeCommand = new ReactiveCommand(new SynchronizationContextScheduler(SynchronizationContext.Current));
        }

        public static ShareNotice Instance
        {
            get { return _Instance; }
        }

        public ReactiveCommand ShareContractAccountChangeCommand { get; private set; }
        
    }

    public class ShareNoticeProvider
    {
        public ShareNotice Notice { get { return ShareNotice.Instance; } }
    }
}
