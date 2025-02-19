using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Breath_of_the_Wild_Multiplayer.Source_files
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
