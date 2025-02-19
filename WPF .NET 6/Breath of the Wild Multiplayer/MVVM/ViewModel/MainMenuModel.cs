using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.MVVM.View;
using Breath_of_the_Wild_Multiplayer.Source_files;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class MainMenuModel : ObservableObject
    {
        public ServerBrowserModel serverBrowserVM { get; set; }
        public SettingsPanelModel settingsPanelVM { get; set; }
        public serverInterfaceModel serverInterfaceVM { get; set; }
        public ModelSelectModel modelSelectVM { get; set; }

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

        private ObservableCollection<Brush> _circleColors;

        public ObservableCollection<Brush> circleColors
        {
            get { return _circleColors; }
            set
            {
                _circleColors = value;
                OnPropertyChanged();
            }
        }

        private string _title;

        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<bool> _buttonStatus;

        public ObservableCollection<bool> buttonStatus
        {
            get { return _buttonStatus; }
            set
            {
                _buttonStatus = value;
                OnPropertyChanged();
            }
        }

        private bool _movingLeft;

        public bool movingLeft
        {
            get { return _movingLeft; }
            set
            {
                _movingLeft = value;
                OnPropertyChanged();
            }
        }

        private bool _movingRight;

        public bool movingRight
        {
            get { return _movingRight; }
            set
            {
                _movingRight = value;
                OnPropertyChanged();
            }
        }

        private List<object> viewList = new List<object>();
        private List<string> titleList = new List<string>();
        private int index;

        private bool _changingBackground;

        public bool changingBackground
        {
            get { return _changingBackground; }
            set
            {
                _changingBackground = value;
                OnPropertyChanged();
            }
        }

        private int _viewPosition;

        public int viewPosition
        {
            get { return _viewPosition; }
            set
            {
                _viewPosition = value;
                OnPropertyChanged();
            }
        }

        private string _leftWindow;

        public string leftWindow
        {
            get { return _leftWindow; }
            set
            {
                _leftWindow = value;
                OnPropertyChanged();
            }
        }

        private string _rightWindow;

        public string rightWindow
        {
            get { return _rightWindow; }
            set
            {
                _rightWindow = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand moveLeft { get; set; }
        public RelayCommand moveRight { get; set; }

        public MainMenuModel()
        {
            SharedData.MainMenu = this;

            serverBrowserVM = new ServerBrowserModel();
            settingsPanelVM = new SettingsPanelModel();
            serverInterfaceVM = new serverInterfaceModel();
            modelSelectVM = new ModelSelectModel();

            findCurrentBackground();

            buttonStatus = new ObservableCollection<bool>();
            buttonStatus.Add(true);
            buttonStatus.Add(true);

            viewList.Add(settingsPanelVM);
            viewList.Add(serverBrowserVM);
            viewList.Add(modelSelectVM);

            titleList.Add("Settings");
            titleList.Add("Lobby Browser");
            titleList.Add("Model Selection");

            circleColors = new ObservableCollection<Brush>();

            circleColors.Add(new SolidColorBrush(Color.FromArgb(0x44, 0xFF, 0xFF, 0xFF)));
            circleColors.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)));
            circleColors.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)));

            movingLeft = false;
            movingRight = false;

            index = 1;
            viewPosition = 0;
            _changingBackground = false;

            updateView(index);

            moveLeft = new RelayCommand(async o =>
            {
                if (index > 0 && !movingLeft && !movingRight)
                {
                    movingLeft = true;
                    index--;

                    if (index != 0 && index != viewList.Count - 1)
                        viewPosition = 0;

                    await Sleep(300);
                    updateView(index);
                    await Sleep(400);
                    movingLeft = false;

                    if (index == 0)
                        viewPosition = 1;
                    else if (index == viewList.Count - 1)
                        viewPosition = 2;
                    else
                        viewPosition = 0;
                }
            });

            moveRight = new RelayCommand(async o =>
            {
                if (index < viewList.Count - 1 && !movingLeft && !movingRight)
                {
                    movingRight = true;
                    index++;

                    if (index != 0 && index != viewList.Count - 1)
                    {
                        viewPosition = 0;
                    }

                    await Sleep(300);
                    updateView(index);
                    await Sleep(400);

                    if (index == 0)
                    {
                        viewPosition = 1;
                    }
                    else if (index == viewList.Count - 1)
                    {
                        viewPosition = 2;
                    }

                    movingRight = false;
                }
            });
        }

        void findCurrentBackground()
        {
            string BackgroundsPath = Directory.GetCurrentDirectory() + "/Backgrounds/";

            List<Background> Backgrounds = new List<Background>() { new Background() { Filename = "Random" } };

            List<string> Extensions = new List<string>() { ".png", ".jpg" };

            Backgrounds.AddRange(settingsPanelVM.FindBackgrounds(BackgroundsPath, Extensions));

            if (Backgrounds.Count == 1)
            {
                Properties.Settings.Default.background = "Random";
                Properties.Settings.Default.backgroundDir = "";
                Properties.Settings.Default.backgroundExt = "";
                Properties.Settings.Default.actualBackground = "\\Images\\mainWindowBackground.png";
                Properties.Settings.Default.Save();
                SharedData.SettingsPanel.timer.Start();
                return;
            }

            if (Properties.Settings.Default.background == "Random" && string.IsNullOrEmpty(Properties.Settings.Default.backgroundDir) && string.IsNullOrEmpty(Properties.Settings.Default.backgroundExt))
            {
                Properties.Settings.Default.actualBackground = "\\Images\\mainWindowBackground.png";
                Properties.Settings.Default.Save();
                SharedData.SettingsPanel.timer.Start();
                return;
            }

            if (!File.Exists($"{Properties.Settings.Default.backgroundDir}\\{Properties.Settings.Default.background}{Properties.Settings.Default.backgroundExt}"))
            {
                Properties.Settings.Default.background = "Random";
                Properties.Settings.Default.backgroundDir = "";
                Properties.Settings.Default.backgroundExt = "";
                Properties.Settings.Default.actualBackground = "\\Images\\mainWindowBackground.png";
                Properties.Settings.Default.Save();
                SharedData.SettingsPanel.timer.Start();
                return;
            }

            Properties.Settings.Default.actualBackground = $"{Properties.Settings.Default.backgroundDir}\\{Properties.Settings.Default.background}{Properties.Settings.Default.backgroundExt}";
        }

        private async Task Sleep(int i) => await Task.Delay(i);

        private void updateView(int index)
        {
            if (index == 0)
            {
                buttonStatus[0] = false;
                buttonStatus[1] = true;
            }
            else if (index == viewList.Count - 2)
            {
                buttonStatus[0] = true;
                buttonStatus[1] = true; // Made false so that the last view is not visible
            }
            else if (index == viewList.Count - 1)
            {
                buttonStatus[0] = true;
                buttonStatus[1] = false;
            }
            else
            {
                buttonStatus[0] = true;
                buttonStatus[1] = true;
            }

            if (index == 0)
            {
                leftWindow = "";
                rightWindow = titleList[index + 1];
            }
            else if (index == viewList.Count - 1)
            {
                leftWindow = titleList[index - 1];
                rightWindow = "";
            }
            else
            {
                leftWindow = titleList[index - 1];
                rightWindow = titleList[index + 1];
            }

            currentView = viewList[index];
            title = titleList[index];

            for (int i = 0; i < circleColors.Count; i++)
                circleColors[i] = new SolidColorBrush(Color.FromArgb(0x44, 0xFF, 0xFF, 0xFF));

            circleColors[index] = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
        }
    }
}
