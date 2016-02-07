﻿using CoreTweet;
using CoreTweet.Core;
using Flantter.MilkyWay.Common;
using Flantter.MilkyWay.Setting;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flantter.MilkyWay.Models.SettingsFlyouts
{
    public class RetweetersSettingsFlyoutModel : BindableBase
    {
        public RetweetersSettingsFlyoutModel()
        {
            this.Retweeters = new ObservableCollection<Twitter.Objects.User>();
        }

        #region Tokens変更通知プロパティ
        private CoreTweet.Tokens _Tokens;
        public CoreTweet.Tokens Tokens
        {
            get { return this._Tokens; }
            set { this.SetProperty(ref this._Tokens, value); }
        }
        #endregion

        #region Id変更通知プロパティ
        private long _Id;
        public long Id
        {
            get { return this._Id; }
            set { this.SetProperty(ref this._Id, value); }
        }
        #endregion

        #region Updating変更通知プロパティ
        private bool _Updating;
        public bool Updating
        {
            get { return this._Updating; }
            set { this.SetProperty(ref this._Updating, value); }
        }
        #endregion

        public ObservableCollection<Twitter.Objects.User> Retweeters { get; set; }

        private long retweetersCursor = 0;
        public async Task UpdateRetweeters(bool useCursor = false)
        {
            if (this.Updating)
                return;

            if (this.Id == 0 || this.Tokens == null)
                return;

            if (useCursor && retweetersCursor == 0)
                return;

            this.Updating = true;

            if (!useCursor || retweetersCursor == 0)
                this.Retweeters.Clear();

            Cursored<long> retweetersIds;
            try
            {
                if (useCursor && retweetersCursor != 0)
                    retweetersIds = await Tokens.Statuses.RetweetersIdsAsync(id => this._Id, cursor => retweetersCursor);
                else
                    retweetersIds = await Tokens.Statuses.RetweetersIdsAsync(id => this._Id);
            }
            catch
            {
                if (!useCursor || retweetersCursor == 0)
                    this.Retweeters.Clear();

                this.Updating = false;
                return;
            }

            retweetersCursor = retweetersIds.NextCursor;

            ListedResponse<User> retweeters;
            try
            {
                retweeters = await Tokens.Users.LookupAsync(user_id => retweetersIds);
            }
            catch
            {
                if (!useCursor || retweetersCursor == 0)
                    this.Retweeters.Clear();

                this.Updating = false;
                return;
            }

            if (!useCursor || retweetersCursor == 0)
                this.Retweeters.Clear();

            foreach (var item in retweeters)
            {
                var user = new Twitter.Objects.User(item);
                this.Retweeters.Add(user);
            }

            this.Updating = false;
        }
    }
}
