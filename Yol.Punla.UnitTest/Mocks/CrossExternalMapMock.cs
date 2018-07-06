using Plugin.ExternalMaps.Abstractions;
using System.Threading.Tasks;

namespace Yol.Punla.UnitTest.Mocks
{
    public class CrossExternalMapMock : IExternalMaps
    {
        public Task<bool> NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default)
        {
            return Task.FromResult<bool>(true);
        }
    }
}
