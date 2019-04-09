using System.Collections.Generic;

namespace Yol.Punla.FakeData
{
    public static class FakeUsers
    {
        private static IEnumerable<Entity.Contact> ContactsField;
        public static IEnumerable<Entity.Contact> Contacts { get => ContactsField; set => ContactsField = value; }

        public static void Init()
        {
            ContactsField = new List<Entity.Contact>
            {
                new Entity.Contact
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
                new Entity.Contact
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
                new Entity.Contact
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
                new Entity.Contact
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
                new Entity.Contact
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
                new Entity.Contact
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
            };
        }
    }
}
