﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flantter.MilkyWay.Models.Twitter.Objects
{
    public class Status : ITweet
    {
        private static readonly Regex SourceRegex = new Regex(@"^<a href="".+"" rel=""nofollow"">(.+)</a>$", RegexOptions.Compiled);
        private static readonly Regex ContentRegex = new Regex(@"<(""[^""]*""|'[^']*'|[^'"">])*>", RegexOptions.Compiled);

        public Status(CoreTweet.Status cOrigStatus)
        {
            var cStatus = cOrigStatus;
            if (cStatus.RetweetedStatus != null)
                cStatus = cOrigStatus.RetweetedStatus;

            this.CreatedAt = cStatus.CreatedAt.DateTime;
            this.Entities = new Entities(cStatus.ExtendedTweet?.Entities ?? cStatus.Entities, cStatus.ExtendedTweet?.ExtendedEntities ?? cStatus.ExtendedEntities);
            this.FavoriteCount = cStatus.FavoriteCount.HasValue ? cStatus.FavoriteCount.Value : 0;
            this.RetweetCount = cStatus.RetweetCount.HasValue ? cStatus.RetweetCount.Value : 0;
            this.InReplyToStatusId = cStatus.InReplyToStatusId.HasValue ? cStatus.InReplyToStatusId.Value : 0;
            this.InReplyToScreenName = cStatus.InReplyToScreenName;
            this.InReplyToUserId = cStatus.InReplyToUserId.HasValue ? cStatus.InReplyToUserId.Value : 0;
            this.Id = cStatus.Id;
            this.Text = cStatus.ExtendedTweet?.FullText ?? cStatus.FullText ?? cStatus.Text;
            this.User = (cStatus.User != null) ? new User(cStatus.User) : null;
            this.IsFavorited = cStatus.IsFavorited.HasValue ? cStatus.IsFavorited.Value : false;
            this.IsRetweeted = cStatus.IsRetweeted.HasValue ? cStatus.IsRetweeted.Value : false;
            this.RetweetInformation = (cOrigStatus.RetweetedStatus != null) ? new RetweetInformation(cOrigStatus) : null;
            this.HasRetweetInformation = (cOrigStatus.RetweetedStatus != null);
            this.MentionStatus = null;
            this.QuotedStatus = cStatus.QuotedStatus != null && cStatus.QuotedStatus.User != null ? new Status(cStatus.QuotedStatus) : null;
            this.QuotedStatusId = (cStatus.QuotedStatusId.HasValue && this.QuotedStatus != null) ? cStatus.QuotedStatusId.Value : 0;

            var sourceMatch = SourceRegex.Match(cStatus.Source);
            if (sourceMatch.Success)
                this.Source = sourceMatch.Groups[1].Value;
            else
                this.Source = cStatus.Source;
        }

        public Status(Mastonet.Entities.Status cOrigStatus)
        {
            var cStatus = cOrigStatus;
            if (cStatus.Reblog != null)
                cStatus = cStatus.Reblog;

            this.CreatedAt = cStatus.CreatedAt;
            this.Entities = new Entities(cStatus.MediaAttachments, cStatus.Mentions, cStatus.Tags, cStatus.Content);
            this.FavoriteCount = cStatus.FavouritesCount;
            this.RetweetCount = cStatus.ReblogCount;
            this.InReplyToStatusId = cStatus.InReplyToId.HasValue ? cStatus.InReplyToId.Value : 0;
            this.InReplyToScreenName = "";
            this.InReplyToUserId = cStatus.InReplyToAccountId.HasValue ? cStatus.InReplyToAccountId.Value : 0;
            this.Id = cStatus.Id;
            this.Text = ContentRegex.Replace(cStatus.Content, "");
            this.User = (cStatus.Account != null) ? new User(cStatus.Account) : null;
            this.IsFavorited = cStatus.Favourited.HasValue ? cStatus.Favourited.Value : false;
            this.IsRetweeted = cStatus.Reblogged.HasValue ? cStatus.Reblogged.Value : false;
            this.RetweetInformation = (cOrigStatus.Reblog != null) ? new RetweetInformation(cOrigStatus) : null;
            this.HasRetweetInformation = (cOrigStatus.Reblog != null);
            this.MentionStatus = null;
            this.QuotedStatus = null;
            this.QuotedStatusId = 0;
            
            this.Source = cStatus.Application != null ? cStatus.Application.Name : "Web";
        }

        public Status()
        {
        }

        #region CreatedAt変更通知プロパティ
        public DateTime CreatedAt { get; set; }
        #endregion

        #region Entities変更通知プロパティ
        public Entities Entities { get; set; }
        #endregion

        #region FavoriteCount変更通知プロパティ
        public int FavoriteCount { get; set; }
        #endregion

        #region RetweetCount変更通知プロパティ
        public int RetweetCount { get; set; }
        #endregion

        #region InReplyToStatusId変更通知プロパティ
        public long InReplyToStatusId { get; set; }
        #endregion

        #region InReplyToScreenName変更通知プロパティ
        public string InReplyToScreenName { get; set; }
        #endregion

        #region InReplyToUserId変更通知プロパティ
        public long InReplyToUserId { get; set; }
        #endregion

        #region Id変更通知プロパティ
        public long Id { get; set; }
        #endregion

        #region Source変更通知プロパティ
        public string Source { get; set; }
        #endregion

        #region Text変更通知プロパティ
        public string Text { get; set; }
        #endregion

        #region User変更通知プロパティ
        public User User { get; set; }
        #endregion

        #region IsFavorited変更通知プロパティ
        public bool IsFavorited { get; set; }
        #endregion

        #region IsRetweeted変更通知プロパティ
        public bool IsRetweeted { get; set; }
        #endregion

        #region RetweetInformation変更通知プロパティ
        public RetweetInformation RetweetInformation { get; set; }
        #endregion

        #region HasRetweetInformation変更通知プロパティ
        public bool HasRetweetInformation { get; set; }
        #endregion

        #region MentionStatus変更通知プロパティ
        public Status MentionStatus { get; set; }
        #endregion

        #region QuotedStatus変更通知プロパティ
        public Status QuotedStatus { get; set; }
        #endregion

        #region QuotedStatusId変更通知プロパティ
        public long QuotedStatusId { get; set; }
        #endregion
    }

    public class RetweetInformation
    {
        public RetweetInformation(CoreTweet.Status cOrigStatus)
        {
            if (cOrigStatus.RetweetedStatus == null)
                return;

            this.User = new User(cOrigStatus.User);
            this.Id = cOrigStatus.Id;
            this.CreatedAt = cOrigStatus.CreatedAt.DateTime;
        }

        public RetweetInformation(Mastonet.Entities.Status cOrigStatus)
        {
            if (cOrigStatus.Reblog == null)
                return;

            this.User = new User(cOrigStatus.Account);
            this.Id = cOrigStatus.Id;
            this.CreatedAt = cOrigStatus.CreatedAt;
        }

        public RetweetInformation()
        {
        }

        #region User変更通知プロパティ
        public User User { get; set; }
        #endregion
        #region Id変更通知プロパティ
        public long Id { get; set; }
        #endregion
        #region CreatedAt変更通知プロパティ
        public DateTime CreatedAt { get; set; }
        #endregion
    }
}
