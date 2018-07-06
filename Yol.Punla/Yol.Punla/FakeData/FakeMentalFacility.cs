using System.Collections.Generic;

namespace Yol.Punla.FakeData
{
    public static class FakeMentalFacility
    {
        private static IEnumerable<Entity.MentalHealthFacility> FacilitiesField;
        public static IEnumerable<Entity.MentalHealthFacility> Facilities { get => FacilitiesField; set => FacilitiesField = value; }

        public static void Init()
        {
            FacilitiesField = new List<Entity.MentalHealthFacility>
            {
                new Entity.MentalHealthFacility
                {
                    GroupName = "HRMC Medical and Rehabilitation Foundation Center Inc.",
                    Location = "#30 Sampaguita Extension, Lopezville Mayamot, Antipolo City",
                    PhoneNumber = "250 1583",
                    PhotoUrl = "hrmc.png",
                    Latitude = 14.572798,
                    Longitude = 121.048330
                },
                new Entity.MentalHealthFacility
                {
                    GroupName = "Bridges of Hope",
                    Location = "Quezon City",
                    PhoneNumber = "503 3483",
                    PhotoUrl = "bridgesforhope.jpg",
                    Latitude = 14.573595,
                    Longitude = 121.058297
                },
                new Entity.MentalHealthFacility
                {
                    GroupName = "Estrellas Home Care Clinic & Hospital Inc.",
                    Location = "177 Kamias Road Extension, Quezon City",
                    PhoneNumber = "(2) 922 3751",
                    PhotoUrl = "ehcchi.jpg"
                },
                new Entity.MentalHealthFacility
                {
                    GroupName = "JRT Psychiatric and Custodial Homecare",
                    Location = "7 Langka St., Barangay Quirino 2-B Project 2, Quezon City",
                    PhoneNumber = "(2) 921 9610",
                    PhotoUrl = "sjpch.jpg"
                },
                 new Entity.MentalHealthFacility
                {
                     GroupName = "New Day Recovery Center",
                     Location = "Babista Compd., Beach Club Rd., Lanang, Davao City",
                     PhoneNumber = "026226874",
                     PhotoUrl = @"https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/newdayrecorycenter.png",
                     Latitude = 7.1047938,
                     Longitude = 125.6442052
                },
                 new Entity.MentalHealthFacility
                {
                     GroupName = "Nanay Drug and Alcohol Rehabilitation Center",
                     Location = "Angat Road, Gulod Poblacion, Pandi, Bulacan",
                     PhoneNumber = "0916 627 5132",
                     PhotoUrl = "hrmc.png"
                },
                new Entity.MentalHealthFacility
                {
                    GroupName = "Natasha Foundation",
                    PhoneNumber = "028044673",
                    FirstName = "Crisis",
                    LastName = "Hotline"
                },
                new Entity.MentalHealthFacility
                {
                    GroupName = "Natasha Foundation 2",
                    PhoneNumber = "09175584673",
                    FirstName = "Crisis",
                    LastName = "Hotline"
                }
            };
        }
    }
}
