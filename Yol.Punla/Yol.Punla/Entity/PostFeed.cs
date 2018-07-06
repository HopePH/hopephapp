using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Yol.Punla.Localized;

namespace Yol.Punla.Entity
{
    public class PostFeed
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PostFeedID { get; set; }
        public int PostFeedLevel { get; set; }
        public int PostFeedParentId { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        public string ContentURL { get; set; }
        public int PosterId { get; set; }
        public string PosterProfilePhotoFB { get; set; }
        public string PosterProfilePhoto { get; set; }
        public string PosterEmail { get; set; }
        public string PosterFirstName { get; set; }
        public string PosterLastName { get; set; }
        public string DatePosted { get; set; }
        public string DateModified { get; set; }
        public string ReferenceUrl { get; set; }
        public int NoOfComments { get; set; }
        public int NoOfSupports { get; set; }
        public bool IsSelfPosted { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSelfSupported { get; set; }
        public string AliasName { get; set; }
        public string NoOfSupportsIDs { get; set; }
        public string ContactsWhoLiked { get; set; }
        public string ContactGender { get; set; }

        [Ignore]
        public bool HasPostedImage
        {
            get
            {
                return string.IsNullOrEmpty(ContentURL) ? false : true;
            }
        }

        private Contact _poster;
        [Ignore]
        public Contact Poster
        {
            get => _poster;
            set
            {
                _poster = value;
                this.AliasName = _poster.AliasName;
                this.PosterProfilePhotoFB = _poster.FBLink;
            }
        }
        
        [Ignore]
        public string PosterFullName
        {
            get
            {
                if (Poster != null)
                    return Poster.FirstName + " " + Poster.LastName;
                return PosterFirstName + " " + PosterLastName;
            }
        }

        [Ignore]
        public IEnumerable<int> SupportersIdsList { get; set; }

        [Ignore]
        public ObservableCollection<PostFeed> Comments { get; set; }

        [Ignore]
        public bool HasUnReadComments { get; set; }

        [Ignore]
        public string NotificationsHeaderText
        {
            get
            {
                if (HasUnReadComments)
                    return AppStrings.NotificationsHasUnReadComments;

                return AppStrings.NotificationsShowSupport;
            }
        }
    }
}
