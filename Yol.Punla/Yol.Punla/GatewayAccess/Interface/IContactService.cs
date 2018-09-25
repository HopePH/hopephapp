using System.Collections.Generic;
using System.Threading.Tasks;
using Yol.Punla.Entity;

namespace Yol.Punla.GatewayAccess
{
    public interface IContactService
    {
        Task<IEnumerable<SurveyQuestion>> GetSurveyQuestions();
        Task<Entity.Contact> GetUserProfile(string EmailAdd);
        Task<int> PostReceiver(Entity.Contact receiver);
        Task<string> PostVerificationCode(string emailAddress);
        Task<Entity.Contact> GetUserViaEmail(string emailAddress, string companyName);
    }
}
