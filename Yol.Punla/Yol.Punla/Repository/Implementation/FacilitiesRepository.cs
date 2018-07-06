using System.Collections.Generic;
using System.Linq;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;

namespace Yol.Punla.Repository
{
    [ModuleIgnore]
    public class FacilitiesRepository : Database, IFacilitiesRepository
    {
        public FacilitiesRepository(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks){ }

        public IEnumerable<MentalHealthFacility> GetLocalFacilities() => Query<MentalHealthFacility>("select * from MentalHealthFacility");

        public IEnumerable<MentalHealthFacility> GetLocalCrisisHotlines() => Query<MentalHealthFacility>("SELECT * FROM MentalHealthFacility WHERE FirstName = 'Crisis' AND Lastname = 'Hotline'");

        public IEnumerable<Wiki> GetWikisFromLocal() => Query<Wiki>("SELECT * FROM Wiki").ToList();

        public void DeleteTableByType<T>() { }        
    }
}
