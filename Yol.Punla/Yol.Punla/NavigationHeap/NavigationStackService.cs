/* Chito.05102017.Notes
 * 1. Do not use the below because those wont work in unittest
 *   Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack
 *   Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack?.Last();
 *   
 * */
using System.Collections.Generic;
using Yol.Punla.AttributeBase;
using Yol.Punla.Repository;

namespace Yol.Punla.NavigationHeap
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(INavigationStackService))]
    [DefaultModuleInterfaced(ParentInterface = typeof(INavigationStackService))]
    public class NavigationStackService : INavigationStackService
    {
        private readonly INavigationStackRepository _navigationStackRepository;
        private Entity.NavigationStackDefinition _item;

        private string _currentlyRemovedPage;
        public string CurrentlyRemovedPage
        {
            get => _currentlyRemovedPage;
        }

        private string _currentPage;
        public string CurrentStack
        {
            get => _currentPage;
        }

        private readonly Stack<string> _navigationStack = new Stack<string>();
        public Stack<string> NavigationStack
        {
            get => _navigationStack;
        }

        public bool IsDisableNavPagePop { get; set; } = false;

        public NavigationStackService(INavigationStackRepository navigationStackRepository)
            => _navigationStackRepository = navigationStackRepository;

        public void UpdateStackState(string page)
        {
            try
            {
                _currentPage = page;
                _navigationStack.Push(_currentPage);

                var item = GetDBRecord(page);
                _navigationStackRepository.UpdateItem(item);
            }
            catch (SQLite.SQLiteException) { }
        }

        public void ResetStackStateTo(string page)
        {
            ResetNavigationStack();
            UpdateStackState(page);
        }

        public void RemovePageFromNavigationStack(string page)
        {
            try
            {
                _currentlyRemovedPage = _navigationStack.Peek();
                _navigationStack.Pop();
                var dbItem = GetDBRecord(page);
                _currentPage = _navigationStack.Peek();
                _navigationStackRepository.DeleteTable(dbItem);
            }
            catch (SQLite.SQLiteException) { }
        }

        private void ResetNavigationStack()
        {
            try
            {
                _navigationStack.Clear();
                _navigationStackRepository.DeleteTableByType<Entity.NavigationStackDefinition>();
            }
            catch (SQLite.SQLiteException) { }
        }

        private Entity.NavigationStackDefinition GetDBRecord(string page)
        {
            try
            {
                _item = _navigationStackRepository.GetPageByName(page);
                if (_item == null) _item = new Entity.NavigationStackDefinition { PageName = page };
                return _item;
            }
            catch (SQLite.SQLiteException)
            {
                return _item;
            }
        }
    }
}
