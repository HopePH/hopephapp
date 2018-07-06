using Xamarin.Forms;
using Yol.Punla.Localized;

namespace Yol.Punla.Triggers
{
    public class SearchBarFocusedEventTrigger : TriggerAction<SearchBar>
    {
        
        protected override void Invoke(SearchBar sender)
        {
            Acr.UserDialogs.UserDialogs.Instance.Alert(AppStrings.SearchAvailableNextVersionText);
            sender.Unfocus();   // this does not work. After this line, the seachbar still has the focus in iOs
        }
    }
}