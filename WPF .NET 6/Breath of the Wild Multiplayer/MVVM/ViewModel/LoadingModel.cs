using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.Source_files;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class LoadingModel : ObservableObject
    {
        private string _Message;

        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                OnPropertyChanged(nameof(this.Message));
            }
        }

        public LoadingModel()
        {
            // The ViewModel is loaded multiple times which can cause issues with messages. With this, we make sure that the first message doesn't disappear
            if (SharedData.LoadingMessage != null)
                this.Message = SharedData.LoadingMessage.Message;

            SharedData.LoadingMessage = this;
        }

        public void UpdateMessage(string msg)
        {
            this.Message = msg;
        }

    }
}
