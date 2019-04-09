using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfaced(ParentInterface = typeof(IFacilitiesService))]
    public class FacilitiesService : GatewayServiceBase, IFacilitiesService
    {
        public FacilitiesService(IServiceMapper serviceMapper) : base(serviceMapper)
        {
           
        }

        public async Task<IEnumerable<MentalHealthFacility>> GetAllFacilities()
        {
            await Task.Delay(1);
            return FakeData.FakeMentalFacility.Facilities;
        }

        public async Task<IEnumerable<MentalHealthFacility>> GetCrisisHotlines()
        {
            await Task.Delay(1);
            return FakeData.FakeMentalFacility.Facilities.Where(x => x.FirstName == "Crisis" && x.LastName == "Hotline");
        }

        public async Task<IEnumerable<Wiki>> GetWikisFromRemote()
        {
            await Task.Delay(1);
            return FakeData.FakeWikis.Wikis;
        }
    }
}
