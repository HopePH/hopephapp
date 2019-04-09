using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfaced(ParentInterface = typeof(IContactService))]
    public class ContactService : GatewayServiceBase, IContactService
    {
        public ContactService(IServiceMapper serviceMapper) : base(serviceMapper)
        {
           
        }

        public async Task<Contact> GetUserProfile(string EmailAdd)
        {
            await Task.Delay(1);
            return FakeData.FakeUsers.Contacts.Where(c => c.EmailAdd == EmailAdd).FirstOrDefault();
        }

        public async Task<int> PostReceiver(Contact receiver)
        {
            //chito. let's assume that the newly inserted contact has an ID of 1234
            await Task.Delay(1);
            return 1234;
        }

        public async Task<string> PostVerificationCode(string emailAddress)
        {
            await Task.Delay(1);
            return "1111";
        }

        public async Task<Contact> GetUserViaEmail(string emailAddress, string companyName)
        {
            await Task.Delay(1);
            return FakeData.FakeUsers.Contacts.Where(c => c.EmailAdd == emailAddress).FirstOrDefault();
        }

        public async Task<IEnumerable<SurveyQuestion>> GetSurveyQuestions()
        {
            await Task.Delay(1);
            return FakeData.FakeSurveys.Surveys;
        }
    }
}
