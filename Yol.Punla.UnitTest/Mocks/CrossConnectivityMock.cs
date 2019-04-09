using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yol.Punla.UnitTest.Mocks
{
    public class CrossConnectivityMock : IConnectivity
    {
        private bool _isConnected = true;

        public bool IsConnected => _isConnected;

        public IEnumerable<ConnectionType> ConnectionTypes => throw new NotImplementedException();

        public IEnumerable<ulong> Bandwidths => throw new NotImplementedException();

        public event ConnectivityChangedEventHandler ConnectivityChanged;

        public event ConnectivityTypeChangedEventHandler ConnectivityTypeChanged;

        public CrossConnectivityMock()
        {

        }

        public void Dispose()
        {
            
        }

        public Task<bool> IsReachable(string host, int msTimeout = 5000)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> IsRemoteReachable(string uri, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsRemoteReachable(Uri uri, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void SetConnectionManually(bool isConnected)
        {
            _isConnected = isConnected;
        }
    }
}
