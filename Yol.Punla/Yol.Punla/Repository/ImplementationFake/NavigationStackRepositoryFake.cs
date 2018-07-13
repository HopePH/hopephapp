using System.Collections.Generic;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository.ImplementationFake
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(INavigationStackRepository))]
    public class NavigationStackRepositoryFake : INavigationStackRepository
    {
        public void DeleteTable(object objToDelete)
        {
           
        }

        public void DeleteTableByType<T>()
        {
            
        }

        public List<Entity.NavigationStackDefinition> GetNavigationStack()
        {
            return null;
        }

        public Entity.NavigationStackDefinition GetPageByName(string pageName)
        {
            return null;
        }

        public Entity.NavigationStackDefinition GetTopPage()
        {
            return null;
        }

        public void UpdateItem<T>(T item)
        {
           
        }
    }
}
