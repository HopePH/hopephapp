using System.Threading.Tasks;
using Yol.Punla.Utility;

namespace Yol.Punla.UnitTest.Mocks
{
    public class HelperUtilityMock : IHelperUtility
    {
        public Task<bool> CheckIfPermissionToLocationGranted()
        {
            return Task.FromResult<bool>( true);
        }

        public bool ResetPackageVersionNo()
        {
            return true;
        }
    }
}
