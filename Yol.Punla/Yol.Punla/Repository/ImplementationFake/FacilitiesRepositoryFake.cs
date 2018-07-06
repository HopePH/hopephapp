using System.Collections.Generic;
using System.Linq;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;

namespace Yol.Punla.Repository
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IFacilitiesRepository))]
    public class FacilitiesRepositoryFake : IFacilitiesRepository
    {
        public IEnumerable<MentalHealthFacility> GetLocalFacilities() => FakeData.FakeMentalFacility.Facilities;

        public IEnumerable<MentalHealthFacility> GetLocalCrisisHotlines() => 
            FakeData.FakeMentalFacility.Facilities.Where(x => x.FirstName == "Crisis" && x.LastName == "Hotline");

        public IEnumerable<Wiki> GetWikisFromLocal() => FakeData.FakeWikis.Wikis;

        public void DeleteTableByType<T>() { }
        
        public void UpdateItem<T>(T item) { }        
    }
}
