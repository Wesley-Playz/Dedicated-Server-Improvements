using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.Source_files;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class CemuWarningModel : ObservableObject
    {
        public RelayCommand AcceptClick { get; set; }

        public CemuWarningModel()
        {
            AcceptClick = new RelayCommand(o =>
            {
                SharedData.MainView.customCloseTopView();
            });
        }
    }
}
