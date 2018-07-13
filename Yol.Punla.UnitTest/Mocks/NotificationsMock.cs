using Plugin.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yol.Punla.UnitTest.Mocks
{
    public class NotificationsMock : INotifications
    {
        public Task Cancel(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task CancelAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetBadge()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetScheduledNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestPermission()
        {
            return Task.FromResult<bool>(true);
        }

        public Task Send(Notification notification)
        {
            return Task.CompletedTask;
        }

        public Task SetBadge(int value)
        {
            throw new NotImplementedException();
        }

        public void Vibrate(int ms = 300)
        {
            throw new NotImplementedException();
        }
    }
}
