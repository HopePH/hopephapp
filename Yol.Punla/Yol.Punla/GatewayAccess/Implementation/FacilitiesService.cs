using System.Collections.Generic;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;
using Yol.Punla.Mapper;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfaced(ParentInterface = typeof(IFacilitiesService))]
    public class FacilitiesService : GatewayServiceBase, IFacilitiesService
    {
        private readonly string _getAllFacilities = null;
        private const string GETALLFACILITIES = "Provider/GetAllContactDetails?companyName=";

        private readonly string _getCrisisHotlines = null;
        private const string GETCRISISHOTLINES = "Provider/GetCrisisHotline";

        private readonly string _getWikis = null;
        private const string GETWIKIS = "Wiki/GetWikis?companyName=";

        public FacilitiesService(IServiceMapper serviceMapper) : base(serviceMapper)
        {
            _getAllFacilities = BaseAPI + GETALLFACILITIES + CompanyName;
            _getCrisisHotlines = BaseAPI + GETCRISISHOTLINES;
            _getWikis = BaseAPI + GETWIKIS + CompanyName;
        }

        public async Task<IEnumerable<MentalHealthFacility>> GetAllFacilities()
        {
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.MentalHealthFacilityK>>($"{_getAllFacilities}");
            return ServiceMapper.Instance.Map<IEnumerable<MentalHealthFacility>>(remoteItems);
        }

        public async Task<IEnumerable<MentalHealthFacility>> GetCrisisHotlines()
        {
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.MentalHealthFacilityK>>($"{_getCrisisHotlines}");
            return ServiceMapper.Instance.Map<IEnumerable<MentalHealthFacility>>(remoteItems);
        }

        public async Task<IEnumerable<Wiki>> GetWikisFromRemote()
        {
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.WikiK>>($"{_getWikis}");
            return ServiceMapper.Instance.Map<IEnumerable<Wiki>>(remoteItems);
        }
    }
}
