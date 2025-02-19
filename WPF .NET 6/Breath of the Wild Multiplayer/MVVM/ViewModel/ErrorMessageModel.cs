using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.Source_files;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class ErrorMessageModel : ObservableObject
    {
        private string _Message;

        public string Message
        {
            get { return _Message; }
            set { 
                _Message = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AcceptClick { get; set; }

        public ErrorMessageModel()
        {
            if (SharedData.ErrorMessage != null)
                this.Message = SharedData.ErrorMessage.Message;
            else
            {
                this.Message = "";
                SharedData.ErrorMessage = this;
            }

            AcceptClick = new RelayCommand(o =>
            {
                CloseWindow();
            });
        }

        public void CloseWindow()
        {
            SharedData.MainView.closeTopView();
            SharedData.ErrorMessage = null;
        }
    }
}
