using System.Threading.Tasks;

namespace Yol.Punla.Utility
{
    public interface IHelperUtility
    {
        Task<bool> CheckIfPermissionToLocationGranted();
        bool ResetPackageVersionNo();
    }
}
