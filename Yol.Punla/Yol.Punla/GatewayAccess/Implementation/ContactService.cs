using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.Mapper;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfaced(ParentInterface = typeof(IContactService))]
    public class ContactService : GatewayServiceBase, IContactService
    {
        private readonly string _getUserByUsername = null;
        private const string GETUSERBYUSERNAME = "Receiver/GetReceiverContactByUserName?userName={0}&facebookId={1}&companyName={2}";

        private readonly string _postingReceiver = null;
        private const string POSTINGRECEIVER = "Receiver/SavingReceiverDetails";

        private readonly string _postVerifyEmail = null;
        private const string POSTVERIFYEMAIL = "Receiver/VerifyEmail?toEmailAddress={0}&aliasName={1}";

        private readonly string _getUserViaEmail = null;
        private const string GETUSERVIAEMAIL = "Receiver/GetUserViaEmail?emailAddress={0}&companyName={1}";

        public ContactService(IServiceMapper serviceMapper) : base(serviceMapper)
        {
            _getUserByUsername = BaseAPI + GETUSERBYUSERNAME;
            _postingReceiver = BaseAPI + POSTINGRECEIVER;
            _postVerifyEmail = BaseAPI + POSTVERIFYEMAIL;
            _getUserViaEmail = BaseAPI + GETUSERVIAEMAIL;
        }

        public async Task<Entity.Contact> GetUserProfile(string EmailAdd, string FbId)
        {
            if (string.IsNullOrEmpty(EmailAdd.Trim()))
                EmailAdd = "xyz";

            var remoteItems = await GetRemoteAsync<Contract.ContactK>(string.Format(_getUserByUsername,EmailAdd,FbId, CompanyName));
            return ServiceMapper.Instance.Map<Entity.Contact>(remoteItems);
        }

        public async Task<int> PostReceiver(Entity.Contact receiver)
        {
            Debug.WriteLine("HOPEPH Posting the receiver.");
            var contractItem = ServiceMapper.Instance.Map<Contract.ContactK>(receiver);
            contractItem.TypeCode = "Receiver";
            contractItem.EmailAddress = receiver.EmailAdd;
            contractItem.DateModified = DateTime.Now.ToString(Constants.DateTimeFormat);
            contractItem.UserName = receiver.EmailAdd;
            contractItem.PhotoURL = receiver.PhotoURL ?? receiver.FBLink;   // if photoURL has no value, set it equal to fblink
            contractItem.FBLink = receiver.FBLink;
            contractItem.CompanyName = CompanyName;

            var result = await PostRemoteAsync<int>(_postingReceiver, CastToStringContent<Contract.ContactK>(contractItem));
            return result;
        }

        public async Task<string> PostVerificationCode(string emailAddress)
        {
            string endPoint = string.Format(_postVerifyEmail, emailAddress, "na");
            return await PostRemoteAsync<string>(endPoint, new StringContent(""));
        }

        public async Task<Contact> GetUserViaEmail(string emailAddress, string companyName)
        {
            string endPoint = string.Format(_getUserViaEmail, emailAddress, companyName);
            return await GetRemoteAsync<Contact>(endPoint);
        }
    }
}
