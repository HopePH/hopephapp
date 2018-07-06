using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yol.Punla.Managers
{
    public interface IMentalHealthManager
    {
        Task<IEnumerable<Entity.MentalHealthFacility>> GetAllFacilities(bool isForcedGetToTheRest = false);
        Task<IEnumerable<Entity.MentalHealthFacility>> GetCrisisHotlines();
        Task<IEnumerable<Entity.Wiki>> GetWikis();
    }
}
