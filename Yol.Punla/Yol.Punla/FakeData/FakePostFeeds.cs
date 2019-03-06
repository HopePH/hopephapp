using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Yol.Punla.Entity;
using Yol.Punla.Localized;

namespace Yol.Punla.FakeData
{
    public static class FakePostFeeds
    {
        private static readonly string DummyText = AppStrings.LoremIpsumText;
        private static IEnumerable<PostFeed> PostField;
        public static IEnumerable<PostFeed> Posts { get => PostField; set => PostField = value; }

        public static void Init()
        {
            PostField = new List<PostFeed>
            {
                #region TOP LEVEL POST
		        new PostFeed
                {
                    PostFeedID = 1000,
                    Title = "Depression is something everyone should be aware of.",
                    DatePosted = "12-06-2017T12:50:00.000z",
                    ContentText = "According to @Glassdoor, these signs mean you're overthinking things at work:",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    NoOfComments = 2,
                    NoOfSupports = 4,
                    SupportersIdsList = new List<int> { 1000, 1001, 1004, 1005 },
                    PosterEmail = "alfeo.salano@gmail.com",
                    PosterFirstName = "Alfon",
                    PosterLastName = "Salano",
                    PosterId = 1001,
                    ReferenceUrl = "blogs.depression.com",
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        Id = 1001,
                        FirstName = "Alfon",
                        LastName = "Salano",
                        EmailAdd = "alfeo.salano@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "mark2",
                        FBLink = "",
                        FBId = "",
                        GenderCode = "male",
                        MobilePhone = "09951762612",
                        PhotoURL = "https://cdn.business2community.com/wp-content/uploads/2014/04/profile-picture-300x300.jpg",
                        Birthdate = "",
                        UserName = "alfeo.salano@gmail.com",
                        RemoteId = 1001
                    },
                    AliasName = "Alfies"
                },
                new PostFeed
                {
                    PostFeedID = 2000,
                    Title = "Mental Health is pushed to be passed in Congress",
                    DatePosted = "12-04-2017T08:50:00.000z",
                    ContentText = "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill.",
                    ContentURL = "https://ak1.picdn.net/shutterstock/videos/18409891/thumb/7.jpg",
                    NoOfComments = 3,
                    NoOfSupports = 2,
                    SupportersIdsList = new List<int> { 1002, 1005 },
                    PosterEmail = "wordesalinas@gmail.com",
                    PosterFirstName = "Worde",
                    PosterLastName = "Salinas",
                    PosterId = 1003,
                    ReferenceUrl = "blogs.psyclinic.com",
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        Id = 1003,
                        FirstName = "Worde",
                        LastName = "Salinas",
                        EmailAdd = "wordesalinas@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Worde5",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "male",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQCdlqi5EjYtoTkgD0LO9X4JFkNSv3g6Jf3dZJB1NIB9r2RlErTqA",
                        Birthdate = "",
                        RemoteId = 1003
                },
                    AliasName = "Alfies"
                },
                new PostFeed
                {
                    PostFeedID = 3000,
                    Title = "This is a new post from @hynrbf",
                    DatePosted = "12-06-2017T12:50:00.000z",
                    ContentText = "This is a post",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    NoOfComments = 2,
                    NoOfSupports = 2,
                    SupportersIdsList = new List<int> { 1000,1005 },
                    PosterEmail = "stephaniehugh@gmail.com",
                    PosterFirstName = "Stephanie",
                    PosterLastName = "Hugh",
                    PosterId = 1004,
                    ReferenceUrl = "blogs.depression.com",
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        Id = 1004,
                        FirstName = "Stephanie",
                        LastName = "Hugh",
                        EmailAdd = "stephaniehugh@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Steph",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "female",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://www.mills.edu/uniquely-mills/students-faculty/student-profiles/images/student-profile-gabriela-mills-college.jpg",
                        Birthdate = "",
                        RemoteId = 1004
                    },
                },
                new PostFeed
                {
                    PostFeedID = 4000,
                    Title = "Mental Health Mobile Application is now in Google play and Apple Store.",
                    DatePosted = DateTime.Now.ToString(Barrack.Constants.DateTimeFormat),
                    ContentText = "A Mental Health mobile application is now developed and launch for Android and iOS to the market. It aims to make people aware of common mental health issues, helps, and trends.",
                    ContentURL = "https://d255me3ukr1mgj.cloudfront.net/images/article_images/simon1470202347511_aspR_1.574_w982_h624_e.jpg",
                    Comments = new ObservableCollection<PostFeed>(),
                    NoOfComments = 0,
                    NoOfSupports = 0,
                    SupportersIdsList = new List<int>(),
                    PosterEmail = "hynrbf@gmail.com",
                    PosterFirstName = "hyn",
                    PosterLastName = "rbf",
                    PosterId = 1000,
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        Id = 1000,
                        FirstName = "Chito",
                        LastName = "Salano",
                        EmailAdd = "hynrbf@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Chito1",
                        FBLink = "",
                        FBId = "",
                        GenderCode = "male",
                        MobilePhone = "026500987",
                        PhotoURL = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg",
                        Birthdate = "",
                        UserName = "hynrbf@gmail.com",
                        RemoteId = 1000
                    },
                },
                #endregion

                #region COMMENTS
                new PostFeed
                {
                    PostFeedID = 1001,
                    DatePosted = "12-06-2017T12:52:00.000z",
                    ContentText = "That's right!!! We have to be aware of that.",
                    PostFeedParentId = 1000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1003,
                        FirstName = "Worde",
                        LastName = "Salinas",
                        EmailAdd = "",
                        Password = "123456Aa@",
                        AliasName = "Worde5",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "male",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQCdlqi5EjYtoTkgD0LO9X4JFkNSv3g6Jf3dZJB1NIB9r2RlErTqA",
                        Birthdate = "",
                        RemoteId = 1003
                    },
                },
                new PostFeed
                {
                    PostFeedID = 1002,
                    DatePosted = "12-04-2017T12:52:00.000z",
                    ContentText = "...Mental health education must be promoted around the country.",
                    PostFeedParentId = 1000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1005,
                        FirstName = "Catherine",
                        LastName = "Murphy",
                        EmailAdd = "catherinemurphy@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Cate",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "female",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://mobirise.com/bootstrap-template/profile-template/assets/images/timothy-paul-smith-256424-1200x800.jpg",
                        Birthdate = "",
                        RemoteId = 1005
                    }
                },
                new PostFeed
                {
                    PostFeedID = 2001,
                    DatePosted = "12-03-2017T08:52:00.000z",
                    ContentText = "Yes!!!",
                    PostFeedParentId = 2000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1002,
                        FirstName = "Robert",
                        LastName = "Lima",
                        EmailAdd = "robertjlima38@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Stevenny",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "168866520312631",
                        GenderCode = "male",
                        MobilePhone = "09128878374",
                        PhotoURL = "https://amp.businessinsider.com/images/5899ffcf6e09a897008b5c04-750-750.jpg",
                        Birthdate = "",
                        RemoteId = 1002
                    },
                },
                new PostFeed
                {
                    PostFeedID = 2002,
                    DatePosted = "12-02-2017T08:52:00.000z",
                    ContentText = "..Good job...",
                    PostFeedParentId = 2000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1003,
                        FirstName = "Worde",
                        LastName = "Salinas",
                        EmailAdd = "",
                        Password = "123456Aa@",
                        AliasName = "Worde5",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "male",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQCdlqi5EjYtoTkgD0LO9X4JFkNSv3g6Jf3dZJB1NIB9r2RlErTqA",
                        Birthdate = "",
                        RemoteId = 1003
                    },
                },
                new PostFeed
                {
                    PostFeedID = 2003,
                    DatePosted = "12-05-2017T08:52:00.000z",
                    ContentText = "THAT'S GOOD NEWS...",
                    PostFeedParentId = 2000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1000,
                        FirstName = "Chito",
                        LastName = "Salano",
                        EmailAdd = "hynrbf@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Chito1",
                        FBLink = "",
                        FBId = "",
                        GenderCode = "male",
                        MobilePhone = "026500987",
                        PhotoURL = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg",
                        Birthdate = "",
                        UserName = "hynrbf@gmail.com",
                        RemoteId = 1000
                    },
                },
                new PostFeed
                {
                    PostFeedID = 3001,
                    DatePosted = "12-06-2017T12:52:00.000z",
                    ContentText = DummyText,
                    PostFeedParentId = 3000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1000,
                        FirstName = "Chito",
                        LastName = "Salano",
                        EmailAdd = "hynrbf@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Chito1",
                        FBLink = "",
                        FBId = "",
                        GenderCode = "male",
                        MobilePhone = "026500987",
                        PhotoURL = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg",
                        Birthdate = "",
                        UserName = "hynrbf@gmail.com",
                        RemoteId = 1000
                    },
                },
                new PostFeed
                {
                    PostFeedID = 3001,
                    DatePosted = "12-08-2017T12:52:00.000z",
                    ContentText = "Hello.....",
                    PostFeedParentId = 3000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1005,
                        FirstName = "Catherine",
                        LastName = "Murphy",
                        EmailAdd = "catherinemurphy@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Cate",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "female",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://mobirise.com/bootstrap-template/profile-template/assets/images/timothy-paul-smith-256424-1200x800.jpg",
                        Birthdate = "",
                        RemoteId = 1005
                    }
                },
                new PostFeed
                {
                    PostFeedID = 4001,
                    DatePosted = "12-08-2017T12:52:00.000z",
                    ContentText = "..how is it already downloadable? I have a friend who has been suffering clinical depression. Maybe she could find some help there.",
                    PostFeedParentId = 4000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1005,
                        FirstName = "Catherine",
                        LastName = "Murphy",
                        EmailAdd = "catherinemurphy@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Cate",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "female",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://mobirise.com/bootstrap-template/profile-template/assets/images/timothy-paul-smith-256424-1200x800.jpg",
                        Birthdate = "",
                        RemoteId = 1005
                    }
                },
                new PostFeed
                {
                    PostFeedID = 4002,
                    DatePosted = "12-08-2017T12:52:00.000z",
                    ContentText = "Amazing.....",
                    PostFeedParentId = 4000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1003,
                        FirstName = "Worde",
                        LastName = "Salinas",
                        EmailAdd = "wordesalinas@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Worde5",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "male",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQCdlqi5EjYtoTkgD0LO9X4JFkNSv3g6Jf3dZJB1NIB9r2RlErTqA",
                        Birthdate = "",
                        RemoteId = 1003
                    },
                },
                new PostFeed
                {
                    PostFeedID = 4003,
                    DatePosted = "12-08-2017T12:52:00.000z",
                    ContentText = ".....I already checked the android version up. It's cool..",
                    PostFeedParentId = 4000,
                    PostFeedLevel = 1,
                    Poster = new Contact
                    {
                        Id = 1005,
                        FirstName = "Catherine",
                        LastName = "Murphy",
                        EmailAdd = "catherinemurphy@gmail.com",
                        Password = "123456Aa@",
                        AliasName = "Cate",
                        FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                        FBId = "157823074843148",
                        GenderCode = "female",
                        MobilePhone = "09477691857",
                        PhotoURL = "https://mobirise.com/bootstrap-template/profile-template/assets/images/timothy-paul-smith-256424-1200x800.jpg",
                        Birthdate = "",
                        RemoteId = 1005
                    }
                }
	            #endregion
            };
        }

        public static void EditingPostFeedContent(int postFeedId, string updatedContent)
        {
            var postInAction = PostField.Where(x => x.PostFeedID == postFeedId).FirstOrDefault();
            postInAction.ContentText = updatedContent;
        }

        public static void AddingNewPostFeedContent(PostFeed newPostFeed)
        {
            List<PostFeed> updatedPostFeedList = new List<PostFeed>(PostField);
            updatedPostFeedList.Add(newPostFeed);
            PostField = updatedPostFeedList;
        }
    }
}
