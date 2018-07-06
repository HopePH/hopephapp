using System;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.Repository;

namespace Yol.Punla.Managers
{
    [ModuleIgnore]
    public abstract class ManagerBase
    {
        private readonly ILocalTableTrackingRepository _localTableTrackingRepository;
        private bool _isExpired;

        protected bool IsInternetConnected
        {
            get => AppCrossConnectivity.Instance.IsConnected;
        }

        protected ManagerBase(ILocalTableTrackingRepository localTableTrackingRepository)
        {
            _localTableTrackingRepository = localTableTrackingRepository;
        }

        protected bool CheckingIfLocalTableHasExpired(string localTableNames)
        {
            try
            {
                var tableTracked = _localTableTrackingRepository.GetTrackedTable(localTableNames);
                TimeSpan timeDiff = TimeSpan.MinValue;
                int hoursToExpire = int.Parse(AppSettingsProvider.Instance.GetValue("CacheDB" + localTableNames));
                TimeSpan expirationSpan = new TimeSpan(hoursToExpire, 0, 10);

                if (tableTracked != null)
                    timeDiff = DateTime.Now.Subtract(DateTime.Parse(tableTracked.LastUpdated));

                if (timeDiff == TimeSpan.MinValue || (tableTracked != null && timeDiff >= expirationSpan))
                    _isExpired = true;
                else
                    _isExpired = false;

                return _isExpired;
            }
            catch (SQLite.SQLiteException)
            {
                return _isExpired;
            }           
        }

        protected void UpdateLocalTableTracking(string tableName)
        {
            try
            {
                _localTableTrackingRepository.UpdateItem(new LocalTableTracking { LastUpdated = DateTime.Now.ToString(), TableName = tableName });
            }
            catch (SQLite.SQLiteException)
            {
            }
        }
    }
}
