﻿using Flantter.MilkyWay.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.UI.Notifications;
using WinRTXamlToolkit.Async;

namespace Flantter.MilkyWay.Setting
{
    public class AccountSetting : BindableBase
    {
        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set { this.SetProperty(ref this._Name, value); }
        }

        private string _ScreenName;
        public string ScreenName
        {
            get { return this._ScreenName; }
            set { this.SetProperty(ref this._ScreenName, value); }
        }

        private long _UserId;
        public long UserId
        {
            get { return this._UserId; }
            set { this.SetProperty(ref this._UserId, value); }
        }

        private string _ConsumerKey;
        public string ConsumerKey
        {
            get { return _ConsumerKey; }
            set { this.SetProperty(ref this._ConsumerKey, value); }
        }

        private string _ConsumerSecret;
        public string ConsumerSecret
        {
            get { return _ConsumerSecret; }
            set { this.SetProperty(ref this._ConsumerSecret, value); }
        }

        private string _AccessToken;
        public string AccessToken
        {
            get { return _AccessToken; }
            set { this.SetProperty(ref this._AccessToken, value); }
        }

        private string _AccessTokenSecret;
        public string AccessTokenSecret
        {
            get { return _AccessTokenSecret; }
            set { this.SetProperty(ref this._AccessTokenSecret, value); }
        }

        public ObservableCollection<ColumnSetting> Column { get; set; }

        private bool _IncludeFollowingsActivity;
        public bool IncludeFollowingsActivity
        {
            get { return _IncludeFollowingsActivity; }
            set { this.SetProperty(ref this._IncludeFollowingsActivity, value); }
        }

        private bool _PossiblySensitive;
        public bool PossiblySensitive
        {
            get { return _PossiblySensitive; }
            set { this.SetProperty(ref this._PossiblySensitive, value); }
        }

        private string _ProfileImageUrl;
        public string ProfileImageUrl
        {
            get { return _ProfileImageUrl; }
            set { this.SetProperty(ref this._ProfileImageUrl, value); }
        }

        private string _ProfileBannerUrl;
        public string ProfileBannerUrl
        {
            get { return _ProfileBannerUrl; }
            set { this.SetProperty(ref this._ProfileBannerUrl, value); }
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { this.SetProperty(ref this._IsEnabled, value); }
        }

        private SettingSupport.PlatformEnum _Platform;
        public SettingSupport.PlatformEnum Platform 
        {
            get { return _Platform; }
            set { this.SetProperty(ref this._Platform, value); }
        }

        private string _Instance;
        public string Instance
        {
            get { return _Instance; }
            set { this.SetProperty(ref this._Instance, value); }
        }
    }

    public class ColumnSetting : BindableBase
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.SetProperty(ref this._Name, value); }
        }

        private SettingSupport.ColumnTypeEnum _Action;
        public SettingSupport.ColumnTypeEnum Action
        {
            get { return _Action; }
            set { this.SetProperty(ref this._Action, value); }
        }

        private string _Parameter;
        public string Parameter
        {
            get { return _Parameter; }
            set { this.SetProperty(ref this._Parameter, value); }
        }

        private string _Filter;
        public string Filter
        {
            get { return _Filter; }
            set { this.SetProperty(ref this._Filter, value); }
        }

        private bool _DisableStartupRefresh;
        public bool DisableStartupRefresh
        {
            get { return _DisableStartupRefresh; }
            set { this.SetProperty(ref this._DisableStartupRefresh, value); }
        }

        private bool _AutoRefresh;
        public bool AutoRefresh
        {
            get { return _AutoRefresh; }
            set { this.SetProperty(ref this._AutoRefresh, value); }
        }

        private double _AutoRefreshTimerInterval;
        public double AutoRefreshTimerInterval
        {
            get { return _AutoRefreshTimerInterval; }
            set { this.SetProperty(ref this._AutoRefreshTimerInterval, value); }
        }
        
        private bool _Streaming;
        public bool Streaming
        {
            get { return _Streaming; }
            set { this.SetProperty(ref this._Streaming, value); }
        }

        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { this.SetProperty(ref this._Index, value); }
        }

        private int _FetchingNumberOfTweet;
        public int FetchingNumberOfTweet
        {
            get { return _FetchingNumberOfTweet; }
            set { this.SetProperty(ref this._FetchingNumberOfTweet, value); }
        }

        private long _Identifier;
        public long Identifier
        {
            get { return _Identifier; }
            set { this.SetProperty(ref this._Identifier, value); }
        }
    }

    public class AdvancedSettingService : AdvancedSettingServiceBase<AdvancedSettingService>
    {
        private AsyncLock _asyncLock = new AsyncLock();

        public async Task SaveToAppSettings()
        {
            using (await _asyncLock.LockAsync())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(Dict);
                    var writeStorageFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync("setting.xml", CreationCollisionOption.ReplaceExisting);
                    using (var s = await writeStorageFile.OpenStreamForWriteAsync())
                    using (var st = new System.IO.StreamWriter(s))
                    {
                        st.Write(json);
                    }
                }
                catch
                {
                }
            }
        }
        public async Task LoadFromAppSettings()
        {
            using (await _asyncLock.LockAsync())
            {
                try
                {
                    var json = string.Empty;

                    var readStorageFile = await ApplicationData.Current.RoamingFolder.GetFileAsync("setting.xml");
                    using (var s = await readStorageFile.OpenStreamForReadAsync())
                    using (var st = new System.IO.StreamReader(s))
                    {
                        json = st.ReadToEnd();
                    }

                    var jTokens = JToken.Parse(json);

                    this.Dict = new Dictionary<string, object>();
                    foreach (JProperty jProperty in jTokens)
                    {
                        if (jProperty.Name == "Accounts")
                            this.Dict[jProperty.Name] = jProperty.Value.ToObject<ObservableCollection<AccountSetting>>();
                        else
                            this.Dict[jProperty.Name] = jProperty.Value.ToObject<ObservableCollection<string>>();
                    }
                }
                catch
                {
                }
            }
        }

        // アカウント,カラム設定
        public ObservableCollection<AccountSetting> Accounts { get { return GetValue((ObservableCollection<AccountSetting>)null); } set { SetValue(value); OnPropertyChanged(); } }
        
        // ミュート設定
        public ObservableCollection<string> MuteUsers { get { return GetValue((ObservableCollection<string>)null); } set { SetValue(value); OnPropertyChanged(); } }
        public ObservableCollection<string> MuteClients { get { return GetValue((ObservableCollection<string>)null); } set { SetValue(value); OnPropertyChanged(); } }
        public ObservableCollection<string> MuteWords { get { return GetValue((ObservableCollection<string>)null); } set { SetValue(value); OnPropertyChanged(); } }
    }

    public class AdvancedSettingProvider
    {
        public AdvancedSettingService AdvancedSetting { get { return AdvancedSettingService.AdvancedSetting; } }
    }
}
