using System.Collections.Generic;

namespace Yol.Punla.Repository
{
    public interface INavigationStackRepository
    {
        void UpdateItem<T>(T item);
        void DeleteTableByType<T>();
        void DeleteTable(object objToDelete);

        Entity.NavigationStackDefinition GetTopPage();
        Entity.NavigationStackDefinition GetPageByName(string pageName);
        List<Entity.NavigationStackDefinition> GetNavigationStack();
    }
}
