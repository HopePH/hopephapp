using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IFacilitiesService))]
    public class FacilitiesServiceFake : IFacilitiesService
    {
        public Task<IEnumerable<MentalHealthFacility>> GetAllFacilities() => Task.FromResult(FakeData.FakeMentalFacility.Facilities);

        public Task<IEnumerable<MentalHealthFacility>> GetCrisisHotlines() => Task.FromResult(FakeData.FakeMentalFacility.Facilities.Where(x => x.FirstName == "Crisis" && x.LastName == "Hotline"));

        public Task<IEnumerable<Wiki>> GetWikisFromRemote() => Task.FromResult(FakeData.FakeWikis.Wikis);
    }
}
