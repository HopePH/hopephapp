using SQLite;
using System.Windows.Input;

namespace Yol.Punla.Entity
{
    public class MentalHealthFacility
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int UniqueVMId { get; set; }
        public double DistanceFromUser { get; set; }

        [Ignore]
        public string GroupNameTruncated
        {
            get
            {
                if (!string.IsNullOrEmpty(GroupName))
                {
                    if (GroupName.Length > 36)
                        return GroupName.Substring(0, 36) + "...";
                }

                return GroupName;
            }
        }

        [Ignore]
        public double LongitudeNonNull
        {
            get
            {
                if (Longitude == null)
                    return 0;

                return (double)Longitude;
            }
        }

        [Ignore]
        public double LatitudeNonNull {
            get
            {
                if (Latitude == null)
                    return 0;

                return (double)Latitude;
            }
        }

        [Ignore]
        public System.Drawing.Color AvailableColor { get; set; }

        [Ignore]
        public ICommand TapCommand { get; set; }

        [Ignore]
        public object TapCommandParameter { get; set; }
    }
}
