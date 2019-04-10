using SQLite;

namespace Yol.Punla.Entity
{
    public class Contact 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int RemoteId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string MobilePhone { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DateModified { get; set; }
        public string UserModified { get; set; }
        public string TypeCode { get; set; }
        public string GenderCode { get; set; }
        public string AliasName { get; set; }
        public string PhotoURL { get; set; }
        public string EmailAdd { get; set; }
        public decimal Price { get; set; }
        public double? RadiusInKilometers { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Description { get; set; }
        public string Birthdate { get; set; }
        public string FBLink { get; set; }
        public string FBUserLink { get; set; }
        public string FBId { get; set; }
        public string CompanyName { get; set; }
    }
}
