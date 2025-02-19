using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.Source_files;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class ChangeNameModel : ObservableObject
    {
        public RelayCommand AcceptClick { get; set; }

        public ChangeNameModel()
        {
            AcceptClick = new RelayCommand(o =>
            {
                SharedData.MainView.customCloseTopView();
            });
        }
    }
}
