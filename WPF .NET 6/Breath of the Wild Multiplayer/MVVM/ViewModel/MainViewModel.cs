using System;
using Breath_of_the_Wild_Multiplayer.Source_files;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;
using Breath_of_the_Wild_Multiplayer.MVVM.View;
using SharpYaml;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public string VersionInText => $"V{MainWindow.VERSION} By the lon lon ranch";
        public MainMenuModel MainMenuVM { get; set; }
        public IngameMenuModel IngameMenuVM { get; set; }

        private bool _disableBackground;

        public bool disableBackground
        {
            get { return _disableBackground; }
            set { 
                _disableBackground = value;
                OnPropertyChanged();
            }
        }


        private string _appTitle;

        public string appTitle
        {
            get { return _appTitle; }
            set {
                _appTitle = value;
                OnPropertyChanged();
            }
        }

        public string Version;

        private object _currentView;

        public object currentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private bool _isTopView;

        public bool isTopView
        {
            get { return _isTopView; }
            set { 
                _isTopView = value;
                OnPropertyChanged();
            }
        }

        private object _currentTopView;

        public object currentTopView
        {
            get { return _currentTopView; }
            set { 
                _currentTopView = value;
                OnPropertyChanged();
            }
        }

        private Brush _barColor;

        public Brush barColor
        {
            get { return _barColor; }
            set { 
                _barColor = value;
                OnPropertyChanged();
            }
        }

        [DllImport("Uxtheme.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "#95")]
        public static extern int GetImmersiveColorFromColorSetEx(int dwImmersiveColorSet, int dwImmersiveColorType, bool bIgnoreHighContrast, int dwHighContrastCacheMode);

        [DllImport("Uxtheme.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "#96")]
        public static extern int GetImmersiveColorTypeFromName(IntPtr pName);

        [DllImport("Uxtheme.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "#98")]
        public static extern int GetImmersiveUserColorSetPreference(bool bForceCheckRegistry, bool bSkipCheckOnFail);

        private Window _window;

        public Window Window
        {
            get { return _window; }
            set { _window = value; }
        }

        public RelayCommand changeTopView { get; set; }

        public MainViewModel()
        {

            SharedData.MainView = this;
            this.MainMenuVM = new MainMenuModel();
            this.IngameMenuVM = new IngameMenuModel();
            SharedData.LoadingMessage = new LoadingModel();
            this.disableBackground = false;
            GameFilesModifier.CreateModifiedModel();

            currentView = this.MainMenuVM;

            SearchUpdate();

            currentTopView = null;
            isTopView = false;

            //TODO: Remove? Or do we want to give the option to use the system color
            //nColorSystemAccent = GetImmersiveColorFromColorSetEx(GetImmersiveUserColorSetPreference(false, false), GetImmersiveColorTypeFromName(Marshal.StringToHGlobalUni("ImmersiveSystemAccent")), false, 0);
            //colorSystemAccent = System.Drawing.ColorTranslator.FromWin32(nColorSystemAccent);

            barColor = new SolidColorBrush(Color.FromArgb(255, 63, 63, 63));

            changeTopView = new RelayCommand(o => this.updateTopView(o));
            if (Properties.Settings.Default.CemuWarning == false)
            {
                this.customUpdateTopView(new CemuWarningModel(), () =>
                {
                    if (Properties.Settings.Default.playerName == "")
                    {
                        this.customUpdateTopView(new ChangeNameModel());
                    }
                });
            }
            else if (Properties.Settings.Default.playerName == "")
            {
                this.updateTopView(new ChangeNameModel());
            }

        }
        public void SearchUpdate()
        {
#if (DEBUG)
            return;
#endif

            if (!File.Exists(Directory.GetCurrentDirectory() + "\\BOTWM_Autoupdater.exe"))
                return;

            try
            {
                var task = Task.Run(() => Github.GithubIntegration.GetLatestVersion());
                task.Wait();

                (string LatestVersion, _) = task.Result;

                if (File.Exists(Directory.GetCurrentDirectory() + "/Version.txt"))
                {
                    this.Version = File.ReadAllText(Directory.GetCurrentDirectory() + "/Version.txt");
                    this.appTitle = $"Breath of the Wild Multiplayer v. {this.Version}";

                    if (this.Version == LatestVersion)
                        return;
                }

                ProcessStartInfo updater = new ProcessStartInfo();
                updater.FileName = Directory.GetCurrentDirectory() + "/BOTWM_Autoupdater.exe";
                updater.UseShellExecute = false;

                using (Process process = Process.Start(updater)) { }

                Environment.Exit(Environment.ExitCode);
            }
            catch(Exception ex)
            {
                return;
            }
        }

        public void updateTopView(object topViewData)
        {
            this.isTopView = true;
            this.currentTopView = topViewData;
        }

        private Action onTopViewClosed; // callback :thumbsup:
        public void customUpdateTopView(object topViewData, Action onClose = null)
        {
            this.isTopView = true;
            this.currentTopView = topViewData;
            this.onTopViewClosed = onClose;
        }

        public void customCloseTopView()
        {
            this.isTopView = false;
            this.currentTopView = null;

            if (onTopViewClosed != null)
            {
                Action temp = onTopViewClosed;
                onTopViewClosed = null; 
                temp.Invoke();
            }
        }

        public void closeTopView()
        {
            this.isTopView = false;
            this.currentTopView = null;
        }
    }
}
