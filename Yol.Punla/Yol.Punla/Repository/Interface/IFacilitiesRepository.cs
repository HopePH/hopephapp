using System.Collections.Generic;

namespace Yol.Punla.Repository
{
    public interface IFacilitiesRepository
    {
        IEnumerable<Entity.MentalHealthFacility> GetLocalFacilities();
        IEnumerable<Entity.MentalHealthFacility> GetLocalCrisisHotlines();
        IEnumerable<Entity.Wiki> GetWikisFromLocal();

        void UpdateItem<T>(T item);
        void DeleteTableByType<T>();
    }
}
