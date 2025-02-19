using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.Source_files;
using System;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class ServerEditorModel : ObservableObject
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { 
                _Name = value;
                OnPropertyChanged();
                this.ValidateInputs();
            }
        }

        private string _IP;

        public string IP
        {
            get { return _IP; }
            set
            {
                _IP = value;
                OnPropertyChanged();
                this.ValidateInputs();
            }
        }

        private string _Port;

        public string Port
        {
            get { return _Port; }
            set { 
                _Port = value;
                OnPropertyChanged();
                this.ValidateInputs();
            }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }

        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { 
                _Title = value;
                OnPropertyChanged();
            }
        }

        private bool _NameEnabled;

        public bool NameEnabled
        {
            get { return _NameEnabled; }
            set { 
                _NameEnabled = value; 
                OnPropertyChanged();
            }
        }

        private bool _ConfirmEnabled;

        public bool ConfirmEnabled
        {
            get { return _ConfirmEnabled; }
            set { 
                _ConfirmEnabled = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConfirmClick { get; set; }
        public RelayCommand CancelClick { get; set; }
        private int ServerIndex;

        public ServerEditorModel()
        {
            if(SharedData.ServerEditor != null)
            {
                this.ServerIndex = SharedData.ServerEditor.ServerIndex;
                this.Name = SharedData.ServerEditor.Name;
                this.IP = SharedData.ServerEditor.IP;
                this.Port = SharedData.ServerEditor.Port;
                this.Password = SharedData.ServerEditor.Password;
                this.Title = SharedData.ServerEditor.Title;
                this.NameEnabled = SharedData.ServerEditor.NameEnabled;

                if (this.ServerIndex >= 0)
                {
                    this.Title = "Edit server";
                    this.NameEnabled = true;
                }
                else if (this.ServerIndex == -2)
                {
                    this.Title = "Direct connection";
                    this.NameEnabled = false;
                }
            }
            else
            {
                ServerIndex = -1;
                Name = "";
                IP = "";
                Port = "";
                Password = "";
                Title = "Register server";
                NameEnabled = true;

                SharedData.ServerEditor = this;
            }

            this.ValidateInputs();
 
            ConfirmClick = new RelayCommand(o =>
            {
                if (SharedData.TryFunction(SharedData.ServerBrowser.ModifyServer, new ServerInfoModel()
                {
                    serverIndex = this.ServerIndex,
                    name = this.Name,
                    ip = this.IP,
                    port = this.Port,
                    password = this.Password
                }))
                {
                    CloseWindow();
                }
            });

            CancelClick = new RelayCommand(o =>
            {
                CloseWindow();
            });
        }

        public void Setup(int serverIndex, string name = "", string ip = "", string port = "", string password = "")
        {
            this.ServerIndex = serverIndex;
            this.Name = name;
            this.IP = ip;
            this.Port = port;
            this.Password = password;

            if(serverIndex == -1)
            {
                this.Title = "Edit server";
            }
            else if(serverIndex == -2)
            {
                this.Title = "Direct connection";
                this.NameEnabled = false;
            }
        }

        public void ValidateInputs(string? name = null, string? ip = null, string? port = null)
        {
            string _Name = name == null ? this.Name : name;
            string _IP = ip == null ? this.IP : ip;
            string _Port = port == null ? this.Port : port;

            if((this.ServerIndex != -2 && string.IsNullOrEmpty(_Name)) || string.IsNullOrEmpty(_IP) || string.IsNullOrEmpty(_Port))
            {
                this.ConfirmEnabled = false;
                return;
            }

            this.ConfirmEnabled = true;
        }

        public void CloseWindow()
        {
            SharedData.MainView.closeTopView();
            SharedData.ServerEditor = null;
        }
    }
}
