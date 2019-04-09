namespace Yol.Punla.ViewModels
{
    public interface IMainTabPageSelectedTab
    {
        int SelectedTab { get; set; }
        void SetSelectedTab(int tabIndex);
    }
}
