using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yol.Punla.GatewayAccess
{
    public interface IFacilitiesService
    {
        Task<IEnumerable<Entity.MentalHealthFacility>> GetAllFacilities();
        Task<IEnumerable<Entity.MentalHealthFacility>> GetCrisisHotlines();
        Task<IEnumerable<Entity.Wiki>> GetWikisFromRemote();
    }
}
