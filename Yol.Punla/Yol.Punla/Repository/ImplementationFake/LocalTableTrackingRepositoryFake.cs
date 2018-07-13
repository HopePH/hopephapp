using System;
using System.Collections.Generic;
using Unity;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.FakeEntries;

namespace Yol.Punla.Repository
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(ILocalTableTrackingRepository))]
    public class LocalTableTrackingRepositoryFake : ILocalTableTrackingRepository
    {
        public void DeleteTableByType<T>()
        {
            
        }

        public LocalTableTracking GetTrackedTable(string tableName)
        {
            if (AppUnityContainer.Instance.Resolve<FakeLoadingMorePost>().IsNotExpiredTime)
                return new LocalTableTracking {  LastUpdated = DateTime.Now.ToString(), TableName = tableName};

            return null;
        }

        public IEnumerable<LocalTableTracking> GetTrackedTables()
        {
            return null;
        }

        public void UpdateItem<T>(T item)
        {
            //do nothing
        }
    }
}
