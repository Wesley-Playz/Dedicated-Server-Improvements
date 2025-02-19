using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.Source_files;
using System;
using System.Diagnostics;
using System.Windows;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class IngameMenuModel : ObservableObject
    {
        private static int ClassId = 0;
        private float fHWindowSize = 1f;
        private float fVWindowSize = 1f;
        private int InstanceId;

        public GridLength HBlackBarSize
        {
            get {
                return new GridLength((1f - fHWindowSize)/2, GridUnitType.Star); 
            }
        }

        public GridLength HWindowSize
        {
            get {
                return new GridLength(fHWindowSize, GridUnitType.Star);
            }
        }

        public GridLength VBlackBarSize
        {
            get
            {
                return new GridLength((1f - fVWindowSize) / 2, GridUnitType.Star);
            }
        }

        public GridLength VWindowSize
        {
            get
            {
                return new GridLength(fVWindowSize, GridUnitType.Star);
            }
        }

        public RelayCommand ChangeWindowState { get; set; }

        public MinigameTimer MinigameTimer { get; set; }
        public MessageQueue MainMessage { get; set; }
        public ChatModel ChatData { get; set; }

        public IngameMenuModel()
        {
            InstanceId = ClassId++;
            CemuFollower.SizeChanged += new EventHandler<CemuFollower.RECT>(this.UpdateWindowSize);

            if(InstanceId == 1)
            {
                NamedPipes.PipeReceived += new EventHandler<string>(this.ProcessPipeMessage);
            }

            MinigameTimer = new MinigameTimer();

            MainMessage = new MessageQueue();
            ChatData = new ChatModel();

            Debug.WriteLine($"IngameModel Start - Id:{InstanceId}");

            ChangeWindowState = new RelayCommand(o =>
            {
                CemuFollower.Borderless();
            });
        }

        private void UpdateWindowSize(object sender, CemuFollower.RECT e)
        {
            int windowWidth = e.Right - e.Left;
            int windowHeight = e.Bottom - e.Top;

            float renderWidth = windowHeight * 16f / 9f;
            float renderHeight = windowWidth * 9f / 16f;

            if(windowHeight == 0 || windowWidth == 0)
                return;

            if(renderWidth / windowWidth > 1)
            {
                this.fVWindowSize = 1f;
                this.fHWindowSize = renderHeight / windowHeight;
            }
            else
            {
                this.fVWindowSize = renderWidth / windowWidth;
                this.fHWindowSize = 1;
            }

            OnPropertyChanged(nameof(HBlackBarSize));
            OnPropertyChanged(nameof(HWindowSize));
            OnPropertyChanged(nameof(VBlackBarSize));
            OnPropertyChanged(nameof(VWindowSize));
        }

        private void ProcessPipeMessage(object sender, string message)
        {
            if(message.Contains("|"))
            {
                string[] MessageData = message.Split("|");
                Debug.WriteLine($"{message} - Id: {InstanceId}");

                if (MessageData[0] == "Notification") // Notification message looks like: "Notification|MessageContents"
                {
                    this.MainMessage.AddMessage(MessageData[1]);
                }
                else if (MessageData[0] == "Timer") // Timer message looks like: "Timer|<Action: start, end or restart> <CountMode: up or down> <StartTime> <MaxTime>" Last 3 parameters are optional, only if action is start
                {
                    string[] timerParams = MessageData[1].Split(" ");

                    if (timerParams[0] == "start")
                    {
                        MinigameTimer.CountModeEnum countMode = timerParams[1] == "up" ? MinigameTimer.CountModeEnum.Countup : MinigameTimer.CountModeEnum.Countdown;
                        int startTime = Int16.Parse(timerParams[2]);
                        int maxTime = Int16.Parse(timerParams[3]);
                        MinigameTimer.Start(countMode, TimeSpan.FromSeconds(startTime), TimeSpan.FromSeconds(maxTime));
                    }
                    else
                    {
                        MinigameTimer.Stop();
                    }
                }
            }
        }
    }
}
