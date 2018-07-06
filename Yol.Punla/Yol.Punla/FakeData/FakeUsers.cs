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
                    UserName = "hynrbf@gmail.com"
                },
                new Entity.Contact
                {
                    FirstName = "Alfon",
                    LastName = "Salano",
                    EmailAdd = "alfeo.salano@gmail.com",
                    Password = "123456Aa@",
                    AliasName = "mark2",
                    FBLink = "",
                    FBId = "", 
                    GenderCode = "male",
                    MobilePhone = "09951762612",
                    PhotoURL = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg",
                    Birthdate = "",
                    UserName = "alfeo.salano@gmail.com"
                },
                new Entity.Contact
                {
                    Id = 1933,
                    FirstName = "Robert",
                    LastName = "Lima",
                    EmailAdd = "robertjlima38@gmail.com",
                    Password = "123456Aa@",
                    AliasName = "Stevenny",
                    FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    FBId = "168866520312631",
                    GenderCode = "male",
                    MobilePhone = "09128878374",
                    PhotoURL = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg",
                    Birthdate = "",
                    RemoteId = 1933
                },
                new Entity.Contact
                {
                    Id = 1934,
                    FirstName = "Worde",
                    LastName = "Salinas",
                    EmailAdd = "",
                    Password = "123456Aa@",
                    AliasName = "Worde5",
                    FBLink = "https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D",
                    FBId = "157823074843148",
                    GenderCode = "male",
                    MobilePhone = "09477691857",
                    PhotoURL = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg",
                    Birthdate = "",
                    RemoteId = 1934
                }
            };
        }
    }
}
