using System.Collections.Generic;
using System.Linq;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository
{
    [ModuleIgnore]
    public class NavigationStackRepository : Database, INavigationStackRepository
    {
        public NavigationStackRepository(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks)
        {

        }

        public void DeleteTableByType<T>()
        {
            base.DeleteAll<T>();
        }

        public void DeleteTable(object objToDelete)
        {
            base.Delete(objToDelete);
        }

        public Entity.NavigationStackDefinition GetTopPage()
        {
            return this.Query<Entity.NavigationStackDefinition>("select * from NavigationStackDefinition order by Id desc").FirstOrDefault();
        }

        public Entity.NavigationStackDefinition GetPageByName(string pageName)
        {
            return this.Query<Entity.NavigationStackDefinition>("select * from NavigationStackDefinition where PageName=? ", pageName).FirstOrDefault();
        }

        public List<Entity.NavigationStackDefinition> GetNavigationStack()
        {
            return this.Query<Entity.NavigationStackDefinition>("select * from NavigationStackDefinition order by Id desc");
        }

    }
}
