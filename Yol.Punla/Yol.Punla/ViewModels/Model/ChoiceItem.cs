using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Yol.Punla.ViewModels
{
    public class ChoiceItem : INotifyPropertyChanged
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
