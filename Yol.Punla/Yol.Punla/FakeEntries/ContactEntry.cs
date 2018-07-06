using Yol.Punla.AttributeBase;

namespace Yol.Punla.FakeEntries
{
    [DefaultModuleFake]
    public class ContactEntry
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string PhotoURL { get; set; }
        public string BirthDay { get; set; }
        public string AliasName { get; set; }
    }
}
