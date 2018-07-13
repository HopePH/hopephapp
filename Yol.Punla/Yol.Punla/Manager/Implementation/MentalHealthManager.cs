using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;
using Yol.Punla.GatewayAccess;
using Yol.Punla.Repository;

namespace Yol.Punla.Managers
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IMentalHealthManager))]
    [DefaultModuleInterfaced(ParentInterface = typeof(IMentalHealthManager))]
    public class MentalHealthManager : ManagerBase, IMentalHealthManager
    {
        readonly IFacilitiesService _facilitiesService;
        readonly IFacilitiesRepository _facilitiesRepository;
        private IEnumerable<Wiki> _cachedWikis;
        private IEnumerable<MentalHealthFacility> _cachedMentalHealthFacilities;

        public bool WasForcedGetToTheRest { get; set; }

        public MentalHealthManager(IFacilitiesService facilitiesService, 
            IFacilitiesRepository facilitiesRepository,
            ILocalTableTrackingRepository localTableTrackingRepository) : base(localTableTrackingRepository)
        {
            _facilitiesService = facilitiesService;
            _facilitiesRepository = facilitiesRepository;
        }

        public async Task<IEnumerable<MentalHealthFacility>> GetAllFacilities(bool isForcedGetToTheRest = false)
        {
            try
            {
                WasForcedGetToTheRest = isForcedGetToTheRest;
                _cachedMentalHealthFacilities = _facilitiesRepository.GetLocalFacilities();

                if (!IsInternetConnected)
                    return _cachedMentalHealthFacilities;

                if (CheckingIfLocalTableHasExpired(nameof(MentalHealthFacility)) || WasForcedGetToTheRest)
                {
                    _cachedMentalHealthFacilities = await _facilitiesService.GetAllFacilities();
                    SaveMentalFacilitiesToLocalDB(_cachedMentalHealthFacilities);
                    return _cachedMentalHealthFacilities;
                }
                
                if (_cachedMentalHealthFacilities == null || (_cachedMentalHealthFacilities != null && _cachedMentalHealthFacilities.Count() < 1))
                {
                    _cachedMentalHealthFacilities = await _facilitiesService.GetAllFacilities();
                    SaveMentalFacilitiesToLocalDB(_cachedMentalHealthFacilities);
                }

                return _cachedMentalHealthFacilities;
            }
            catch (SQLite.SQLiteException)
            {
                return _cachedMentalHealthFacilities;
            }
        }

        public async Task<IEnumerable<MentalHealthFacility>> GetCrisisHotlines()
        {
            //chito. make this stable, as this should be getting in local table but not messing the local mental facilities table
            return await _facilitiesService.GetCrisisHotlines();
        }

        public async Task<IEnumerable<Wiki>> GetWikis()
        {
            try
            {
                _cachedWikis = _facilitiesRepository.GetWikisFromLocal();

                if (!IsInternetConnected)
                    return _cachedWikis;

                if (CheckingIfLocalTableHasExpired(nameof(Wiki)) || (_cachedWikis != null && _cachedWikis.Count() <= 0))
                {
                    _cachedWikis = await _facilitiesService.GetWikisFromRemote();
                    SaveWikisLocally(_cachedWikis);
                }

                return _cachedWikis;
            }
            catch (SQLite.SQLiteException)
            {
                return _cachedWikis;
            }
        }

        public void SaveMentalFacilitiesToLocalDB(IEnumerable<MentalHealthFacility> mentalHealthFacilities)
        {
            _facilitiesRepository.DeleteTableByType<MentalHealthFacility>();

            if (mentalHealthFacilities != null && mentalHealthFacilities.Count() > 0)
            {
                foreach (var item in mentalHealthFacilities)
                    _facilitiesRepository.UpdateItem(item);

                UpdateLocalTableTracking(nameof(MentalHealthFacility));
            }
        }

        public void SaveWikisLocally(IEnumerable<Wiki> wikis)
        {
            if(wikis != null && wikis.Count() > 0)
            {
                foreach (var item in wikis)
                    _facilitiesRepository.UpdateItem(item);

                UpdateLocalTableTracking(nameof(Wiki));
            }
        }
    }
}
