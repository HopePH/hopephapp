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
                new PostFeed
                {
                    PostFeedID = 1111,
                    Title = "Depression is something everyone should be aware of.",
                    DatePosted = "12-06-2017T12:50:00.000z",
                    ContentText = "According to @Glassdoor, these signs mean you're overthinking things at work:",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    Comments = new ObservableCollection<PostFeed>(new List<PostFeed>
                    {
                        new PostFeed
                        {
                            DatePosted = "12-06-2017T12:52:00.000z",
                            ContentText = DummyText
                        },
                        new PostFeed
                        {
                            DatePosted = "12-08-2017T12:52:00.000z",
                            ContentText = DummyText
                        }
                    }),
                    NoOfComments = 2,
                    NoOfSupports = 8,
                    SupportersIdsList = new List<int>
                    {
                        12,354,1187,234,1045,109,1100,1090
                    },
                    PosterEmail = "alfiesalano@yahoo.com",
                    PosterFirstName = "Peter",
                    PosterLastName = "Smith",
                    PosterId = 348,
                    ReferenceUrl = "blogs.depression.com",
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        RemoteId = 1893,
                        PhotoURL = "http://www.miscellaneoushi.com/thumbnails/detail/20121023/aircraft%20wood%20toys%20children%20objects%20ornaments%201600x900%20wallpaper_www.miscellaneoushi.com_13.jpg",
                        FirstName = "Peter",
                        LastName = "Smith",
                        Description = "Advocate at PMHA",
                        EmailAdd = "alfiesalano@yahoo.com"
                    },
                    AliasName = "Alfies"
                },
                new PostFeed
                {
                    PostFeedID = 1112,
                    Title = "Mental Health is pushed to be passed in Congress",
                    DatePosted = "12-04-2017T08:50:00.000z",
                    ContentText = "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill.",
                    ContentURL = "https://ak1.picdn.net/shutterstock/videos/18409891/thumb/7.jpg",
                    Comments = new ObservableCollection<PostFeed>(new List<PostFeed>
                    {
                        new PostFeed
                        {
                            DatePosted = "12-03-2017T08:52:00.000z",
                            ContentText = "Comment : " + DummyText
                        },
                        new PostFeed
                        {
                            DatePosted = "12-02-2017T08:52:00.000z",
                            ContentText = "Comment : " + DummyText
                        },
                        new PostFeed
                        {
                            DatePosted = "12-05-2017T08:52:00.000z",
                            ContentText = "Comment : " + DummyText
                        }
                    }),
                    NoOfComments = 3,
                    NoOfSupports = 5,
                    SupportersIdsList = new List<int>
                    {
                        12,354,1187,234,1045
                    },
                    PosterEmail = "alfiesalano@yahoo.com",
                     PosterFirstName = "Peter",
                    PosterLastName = "Smith",
                    PosterId = 348,
                    ReferenceUrl = "blogs.psyclinic.com",
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        RemoteId = 1893,
                        PhotoURL = "https://cdn.allwallpaper.in/wallpapers/2400x1350/711/cgi-black-background-light-bulbs-minimalistic-objects-2400x1350-wallpaper.jpg",
                        FirstName = "John",
                        LastName = "Doe",
                        Description = "Psychometrician at Philippine Psychological Hospital",
                        EmailAdd = "alfiesalano@yahoo.com"
                    },
                    AliasName = "Alfies"
                },
                new PostFeed
                {
                    PostFeedID = 1111,
                    Title = "This is a new post from @hynrbf",
                    DatePosted = "12-06-2017T12:50:00.000z",
                    ContentText = "This is a post",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    Comments = new ObservableCollection<PostFeed>(new List<PostFeed>
                    {
                        new PostFeed
                        {
                            DatePosted = "12-06-2017T12:52:00.000z",
                            ContentText = DummyText
                        },
                        new PostFeed
                        {
                            DatePosted = "12-08-2017T12:52:00.000z",
                            ContentText = DummyText
                        }
                    }),
                    NoOfComments = 2,
                    NoOfSupports = 8,
                    SupportersIdsList = new List<int>
                    {
                        12,354,1187,234,1045,109,1100,1090
                    },
                    PosterEmail = "hynrbf@gmail.com",
                    PosterFirstName = "hyn",
                    PosterLastName = "rbf",
                    PosterId = 348,
                    ReferenceUrl = "blogs.depression.com",
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        RemoteId = 1893,
                        PhotoURL = "http://www.miscellaneoushi.com/thumbnails/detail/20121023/aircraft%20wood%20toys%20children%20objects%20ornaments%201600x900%20wallpaper_www.miscellaneoushi.com_13.jpg",
                        FirstName = "hyn",
                        LastName = "rbf",
                        Description = "Advocate at PMHA",
                        EmailAdd = "hynrbf@gmail.com"
                    }
                },
                new PostFeed
                {
                    PostFeedID = 1120,
                    Title = "New Post",
                    DatePosted = DateTime.Now.ToString(Barrack.Constants.DateTimeFormat),
                    ContentText = "Newly added Post",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    Comments = new ObservableCollection<PostFeed>(),
                    NoOfComments = 0,
                    NoOfSupports = 0,
                    SupportersIdsList = new List<int>(),
                    PosterEmail = "hynrbf@gmail.com",
                    PosterFirstName = "hyn",
                    PosterLastName = "rbf",
                    PosterId = 1890,
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        RemoteId = 1890,
                        PhotoURL = "http://www.miscellaneoushi.com/thumbnails/detail/20121023/aircraft%20wood%20toys%20children%20objects%20ornaments%201600x900%20wallpaper_www.miscellaneoushi.com_13.jpg",
                        FirstName = "Haiyan",
                        LastName = "Rbf",
                        Description = "Advocate at PMHA",
                        EmailAdd = "hynrbf@gmail.com"
                    }
                },
                new PostFeed
                {
                    PostFeedID = 1120,
                    Title = "New Post",
                    DatePosted = DateTime.Now.ToString(Barrack.Constants.DateTimeFormat),
                    ContentText = "This is a edited post",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    Comments = new ObservableCollection<PostFeed>(),
                    NoOfComments = 0,
                    NoOfSupports = 0,
                    SupportersIdsList = new List<int>(),
                    PosterEmail = "hynrbf@gmail.com",
                    PosterFirstName = "hyn",
                    PosterLastName = "rbf",
                    PosterId = 1890,
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        RemoteId = 1890,
                        PhotoURL = "http://www.miscellaneoushi.com/thumbnails/detail/20121023/aircraft%20wood%20toys%20children%20objects%20ornaments%201600x900%20wallpaper_www.miscellaneoushi.com_13.jpg",
                        FirstName = "Haiyan",
                        LastName = "Rbf",
                        Description = "Advocate at PMHA",
                        EmailAdd = "hynrbf@gmail.com"
                    }
                },
                new PostFeed
                {
                    PostFeedID = 1133,
                    Title = "New Post",
                    DatePosted = DateTime.Now.ToString(Barrack.Constants.DateTimeFormat),
                    ContentText = "Please download the new version of the app",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    Comments = new ObservableCollection<PostFeed>(new List<PostFeed>
                    {
                        new PostFeed
                        {
                            DatePosted = "12-06-2017T12:52:00.000z",
                            ContentText = "Download app comments"
                        }
                    }),
                    NoOfComments = 0,
                    NoOfSupports = 0,
                    SupportersIdsList = new List<int>(),
                    PosterEmail = "hynrbf@gmail.com",
                    PosterFirstName = "Worde",
                    PosterLastName = "Salinas",
                    PosterId = 1934,
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        RemoteId = 1934,
                        PhotoURL = "http://www.miscellaneoushi.com/thumbnails/detail/20121023/aircraft%20wood%20toys%20children%20objects%20ornaments%201600x900%20wallpaper_www.miscellaneoushi.com_13.jpg",
                        FirstName = "Worde",
                        LastName = "Salinas",
                        Description = "Coach",
                        EmailAdd = "hynrbf@gmail.com"
                    }
                },
                new PostFeed
                {
                    PostFeedID = 1134,
                    Title = "New Post",
                    DatePosted = DateTime.Now.ToString(Barrack.Constants.DateTimeFormat),
                    ContentText = "Please download the new version of the app",
                    ContentURL = "https://ak6.picdn.net/shutterstock/videos/3997156/thumb/1.jpg",
                    NoOfComments = 0,
                    NoOfSupports = 0,
                    SupportersIdsList = new List<int>(),
                    PosterEmail = "robertjlima38@gmail.com",
                    PosterFirstName = "Robert",
                    PosterLastName = "Lima",
                    PosterId = 1933,
                    PosterProfilePhotoFB = "https://graph.facebook.com/270140636845346/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    Poster = new Contact
                    {
                        RemoteId = 1933,
                        PhotoURL = "http://www.miscellaneoushi.com/thumbnails/detail/20121023/aircraft%20wood%20toys%20children%20objects%20ornaments%201600x900%20wallpaper_www.miscellaneoushi.com_13.jpg",
                        FirstName = "Robert",
                        LastName = "Lima",
                        Description = "Coach",
                        EmailAdd = "robertjlima38@gmail.com"
                    },
                    PostFeedParentId = 1111,
                    PostFeedLevel = 1
                }
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
