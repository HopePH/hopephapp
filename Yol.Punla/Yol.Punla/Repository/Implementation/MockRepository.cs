
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository
{
    [ModuleIgnore]
    public class MockRepository : Database, IMockRepository
    {
        public MockRepository(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks)
        {

        }

    }
}
