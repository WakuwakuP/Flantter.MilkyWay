﻿using Flantter.MilkyWay.Common;
using Flantter.MilkyWay.Models;
using Flantter.MilkyWay.Models.Twitter.Objects;
using Flantter.MilkyWay.Setting;
using Flantter.MilkyWay.ViewModels.Service;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Flantter.MilkyWay.ViewModels.Twitter.Objects
{
    public class StatusViewModel : ExtendedBindableBase
    {
        public StatusViewModel(Status status, ColumnModel column)
        {

            this.Model = status;

            this.CreatedAt = status.CreatedAt.ToLocalTime().ToString();
            this.Source = status.Source;
            this.Text = status.Text;
            this.ScreenName = status.User.ScreenName;
            this.Name = status.User.Name;
            this.ProfileImageUrl = string.IsNullOrWhiteSpace(status.User.ProfileImageUrl) ? "http://localhost/" : status.User.ProfileImageUrl;
            this.Entities = status.Entities;

            // BackgroundBrush
            this.BackgroundBrush = "Default";
            if (status.HasRetweetInformation)
                this.BackgroundBrush = "Retweet";
            else if (status.InReplyToUserId == column.OwnerUserId)
                this.BackgroundBrush = "Mention";
            else if (status.IsFavorited)
                this.BackgroundBrush = "Favorite";
            else if (status.User.Id == column.OwnerUserId)
                this.BackgroundBrush = "MyStatus";

            this.RetweetInformationVisibility = status.HasRetweetInformation;
            this.MediaVisibility = status.Entities.Media.Count == 0 ? false : true;
            this.MediaEntities = new List<MediaEntityViewModel>();
            foreach (var mediaEntity in status.Entities.Media)
                this.MediaEntities.Add(new MediaEntityViewModel(mediaEntity));

            this.UrlEntities = new List<UrlEntityViewModel>();
            foreach (var urlEntity in status.Entities.Urls)
                this.UrlEntities.Add(new UrlEntityViewModel(urlEntity));

            this.IsFavorited = status.IsFavorited;
            this.IsRetweeted = status.IsRetweeted;

            // RetweetInformation
            if (status.RetweetInformation != null)
            {
                if (status.RetweetCount >= 2)
                    this.RetweetInformationText = "Retweet by " + status.RetweetInformation.User.ScreenName + " ( " + status.RetweetInformation.User.Name + " ) and " + status.RetweetCount.ToString() + " others";
                else
                    this.RetweetInformationText = "Retweet by " + status.RetweetInformation.User.ScreenName + " ( " + status.RetweetInformation.User.Name + " )";

                this.RetweetInformationProfileImageUrl = string.IsNullOrWhiteSpace(status.RetweetInformation.User.ProfileImageUrl) ? "http://localhost/" : status.RetweetInformation.User.ProfileImageUrl;
                this.RetweetInformationScreenName = status.RetweetInformation.User.ScreenName;
            }
            else
            {
                this.RetweetInformationText = "";
                this.RetweetInformationProfileImageUrl = "http://localhost/";
            }

            // RetweetCounter
            if (!status.HasRetweetInformation && status.RetweetCount > 0)
            {
                this.RetweetCounterVisibility = true;
                this.RetweetCounterText = "Retweeted " + status.RetweetCount.ToString() + " times";
            }
            else
            {
                this.RetweetCounterVisibility = false;
                this.RetweetCounterText = "";
            }

            // TriangleIcon
            if (!status.IsRetweeted && status.IsFavorited)
                this.FavoriteTriangleIconVisibility = true;
            else
                this.FavoriteTriangleIconVisibility = false;
            if (status.IsRetweeted && !status.IsFavorited)
                this.RetweetTriangleIconVisibility = true;
            else
                this.RetweetTriangleIconVisibility = false;
            if (status.IsRetweeted && status.IsFavorited)
                this.RetweetFavoriteTriangleIconVisibility = true;
            else
                this.RetweetFavoriteTriangleIconVisibility = false;
            
            this.QuotedStatusVisibility = status.QuotedStatusId != 0 ? true : false;
            this.QuotedStatusId = status.QuotedStatusId;
            if (status.QuotedStatus != null)
            {
                this.QuotedStatusName = status.QuotedStatus.User.Name;
                this.QuotedStatusScreenName = status.QuotedStatus.User.ScreenName;
                this.QuotedStatusText = status.QuotedStatus.Text;
                this.QuotedStatusEntities = status.QuotedStatus.Entities;
                this.QuotedStatusProfileImageUrl = string.IsNullOrWhiteSpace(status.QuotedStatus.User.ProfileImageUrl) ? "http://localhost/" : status.QuotedStatus.User.ProfileImageUrl;
            }
            else
            {
                this.QuotedStatusProfileImageUrl = "http://localhost/";
            }

            this.MentionStatusVisibility = (status.InReplyToStatusId != 0);
            this.IsMentionStatusLoaded = (status.MentionStatus != null);
            this.IsMentionStatusLoading = false;

            this.RetweetCount = status.RetweetCount;
            this.IsMyStatus = (status.User.Id == column.OwnerUserId);

            this.Notice = Service.Notice.Instance;
        }

        public Status Model { get; private set; }

        public string CreatedAt { get; set; }

        public string Source { get; set; }

        public string Text { get; set; }

        public string ScreenName { get; set; }

        public string Name { get; set; }
        
        public string ProfileImageUrl { get; set; }

        public Entities Entities { get; set; }

        public bool IsFavorited { get; set; }

        public bool IsRetweeted { get; set; }

        public string BackgroundBrush { get; set; }

        public bool RetweetInformationVisibility { get; set; }
        
        public bool MediaVisibility { get; set; }

        public List<MediaEntityViewModel> MediaEntities { get; private set; }

        public List<UrlEntityViewModel> UrlEntities { get; private set; }

        public string RetweetInformationText { get; set; }

        public string RetweetInformationProfileImageUrl { get; set; }

        public string RetweetInformationScreenName { get; set; }


        public bool RetweetCounterVisibility { get; set; }

        public string RetweetCounterText { get; set; }


        public bool RetweetTriangleIconVisibility { get; set; }

        public bool FavoriteTriangleIconVisibility { get; set; }

        public bool RetweetFavoriteTriangleIconVisibility { get; set; }


        public bool QuotedStatusVisibility { get; set; }

        public string QuotedStatusScreenName { get; set; }

        public string QuotedStatusName { get; set; }

        public string QuotedStatusText { get; set; }

        public string QuotedStatusProfileImageUrl { get; set; }

        public long QuotedStatusId { get; set; }

        public Entities QuotedStatusEntities { get; set; }


        public bool MentionStatusVisibility { get; set; }

        public bool IsMentionStatusLoaded { get; set; }

        public bool IsMentionStatusLoading { get; set; }

        public string MentionStatusScreenName { get; set; }

        public string MentionStatusName { get; set; }

        public string MentionStatusText { get; set; }

        public string MentionStatusProfileImageUrl { get; set; }

        public long MentionStatusId { get; set; }

        public Entities MentionStatusEntities { get; set; }
        
        public int RetweetCount { get; set; }

        public bool IsMyStatus { get; set; }

        public Service.Notice Notice { get; set; }
    }
}
