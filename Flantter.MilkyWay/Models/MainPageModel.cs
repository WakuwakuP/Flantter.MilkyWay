﻿using Flantter.MilkyWay.Models.Exceptions;
using Flantter.MilkyWay.Models.Services;
using Flantter.MilkyWay.Models.Twitter.Objects;
using Flantter.MilkyWay.Setting;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Flantter.MilkyWay.Models
{
    public class MainPageModel : BindableBase
    {
        private bool _Initialized = false;

        #region Accounts
        private ObservableCollection<AccountModel> _Accounts;
        private ReadOnlyObservableCollection<AccountModel> _ReadOnlyAccounts;
        public ReadOnlyObservableCollection<AccountModel> ReadOnlyAccounts
        {
            get
            {
                return _ReadOnlyAccounts;
            }
        }
        #endregion

        #region Initialize
        public async Task Initialize()
        {
            if (_Initialized)
                return;

            Connecter.Instance.Initialize();

            await Task.WhenAll(this._Accounts.Select(x => x.Initialize()));

            Notifications.Core.Instance.Initialize();

            if (Setting.SettingService.Setting.EnablePlugins)
                await Task.Run(() => Plugin.Core.Instance.Initialize());

            _Initialized = true;
        }
        #endregion

        #region Constructor
        private MainPageModel()
        {
            this._Accounts = new ObservableCollection<AccountModel>();
            this._ReadOnlyAccounts = new ReadOnlyObservableCollection<AccountModel>(this._Accounts);
            
            foreach (var account in AdvancedSettingService.AdvancedSetting.Accounts)
            {
                this._Accounts.Add(new AccountModel(account));
            }
        }
        #endregion

        #region Destructor
        ~MainPageModel()
        {
        }
        #endregion


        public void CopyTweetToClipBoard(object tweet)
        {
            var status = tweet as Status;
            if (status != null)
            {
                try
                {
                    var textPackage = new DataPackage();
                    textPackage.SetText(status.Text);
                    Clipboard.SetContent(textPackage);
                }
                catch
                {
                }

                return;
            }

            var directMessage = tweet as DirectMessage;
            if (directMessage != null)
            {
                try
                {
                    var textPackage = new DataPackage();
                    textPackage.SetText(directMessage.Text);
                    Clipboard.SetContent(textPackage);
                }
                catch
                {
                }

                return;
            }
        }

        public void CopyTweetUrlToClipBoard(object tweet)
        {
            var status = tweet as Status;
            if (status == null)
                return;

            try
            {
                var textPackage = new DataPackage();
                textPackage.SetText("https://twitter.com/" + status.User.ScreenName + "/status/" + status.Id.ToString());
                Clipboard.SetContent(textPackage);
            }
            catch
            {
            }
        }

        public async Task ChangeBackgroundImage(StorageFile file)
        {
            SettingService.Setting.BackgroundImagePath = "";

            try
            {
                await file.CopyAsync(ApplicationData.Current.LocalFolder, "background_image" + file.FileType, NameCollisionOption.ReplaceExisting);
                SettingService.Setting.BackgroundImagePath = "ms-appdata:///local/" + "background_image" + file.FileType;
            }
            catch
            {
            }
        }

        public async Task ChangeTheme(StorageFile file)
        {
            try
            {
                await file.CopyAsync(ApplicationData.Current.LocalFolder, "Theme.xaml", NameCollisionOption.ReplaceExisting);
                SettingService.Setting.CustomThemePath = "ms-appdata:///local/" + "Theme.xaml";
            }
            catch
            {
            }
        }

        public async Task MuteClient(string client)
        {
            if (AdvancedSettingService.AdvancedSetting.MuteClients == null)
                AdvancedSettingService.AdvancedSetting.MuteClients = new ObservableCollection<string>();

            if (!AdvancedSettingService.AdvancedSetting.MuteClients.Contains(client))
            {
                AdvancedSettingService.AdvancedSetting.MuteClients.Add(client);
                await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();
            }
        }

        public async Task MuteWord(string word)
        {
            if (AdvancedSettingService.AdvancedSetting.MuteWords == null)
                AdvancedSettingService.AdvancedSetting.MuteWords = new ObservableCollection<string>();

            if (!AdvancedSettingService.AdvancedSetting.MuteWords.Contains(word))
            {
                AdvancedSettingService.AdvancedSetting.MuteWords.Add(word);
                await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();
            }
        }

        public async Task DeleteMuteUser(string screenName)
        {
            if (AdvancedSettingService.AdvancedSetting.MuteUsers == null)
                AdvancedSettingService.AdvancedSetting.MuteUsers = new ObservableCollection<string>();

            if (AdvancedSettingService.AdvancedSetting.MuteUsers.Contains(screenName))
            {
                AdvancedSettingService.AdvancedSetting.MuteUsers.Remove(screenName);
                await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();
            }
        }

        public async Task DeleteMuteClient(string client)
        {
            if (AdvancedSettingService.AdvancedSetting.MuteClients == null)
                AdvancedSettingService.AdvancedSetting.MuteClients = new ObservableCollection<string>();

            if (AdvancedSettingService.AdvancedSetting.MuteClients.Contains(client))
            {
                AdvancedSettingService.AdvancedSetting.MuteClients.Remove(client);
                await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();
            }
        }

        public async Task DeleteMuteWord(string word)
        {
            if (AdvancedSettingService.AdvancedSetting.MuteWords == null)
                AdvancedSettingService.AdvancedSetting.MuteWords = new ObservableCollection<string>();

            if (AdvancedSettingService.AdvancedSetting.MuteWords.Contains(word))
            {
                AdvancedSettingService.AdvancedSetting.MuteWords.Remove(word);
                await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();
            }
        }

        public async Task AddAccount(AccountSetting account)
        {
            AdvancedSettingService.AdvancedSetting.Accounts.Add(account);
            await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();

            var accountModel = new AccountModel(account);
            this._Accounts.Add(accountModel);
            await accountModel.Initialize();
        }

        public async Task ChangeAccount(AccountSetting account)
        {
            foreach (var accountModel in this._Accounts)
            {
                if (account.UserId == accountModel.AccountSetting.UserId)
                    accountModel.IsEnabled = true;
                else
                    accountModel.IsEnabled = false;

                accountModel.LeftSwipeMenuIsOpen = false;
            }

            foreach (var accountSetting in AdvancedSettingService.AdvancedSetting.Accounts)
            {
                if (account.UserId == accountSetting.UserId)
                    accountSetting.IsEnabled = true;
                else
                    accountSetting.IsEnabled = false;
            }

            await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();
        }

        public async Task DeleteAccount(AccountSetting account)
        {
            var accountModel = this._Accounts.First(x => x.AccountSetting.UserId == account.UserId);

            accountModel.LeftSwipeMenuIsOpen = false;

            accountModel.Dispose();
            this._Accounts.Remove(accountModel);
            
            AdvancedSettingService.AdvancedSetting.Accounts.Remove(account);

            if (accountModel.IsEnabled)
            {
                accountModel.IsEnabled = false;
                this._Accounts.First().IsEnabled = true;

                AdvancedSettingService.AdvancedSetting.Accounts.First(x => x.UserId == this._Accounts.First().AccountSetting.UserId).IsEnabled = true;
            }

            await AdvancedSettingService.AdvancedSetting.SaveToAppSettings();

            Connecter.Instance.RemoveAccount(account);
        }
        
        #region Instance
        private static MainPageModel _Instance = new MainPageModel();
        public static MainPageModel Instance
        {
            get { return _Instance; }
        }
        #endregion
    }
}
