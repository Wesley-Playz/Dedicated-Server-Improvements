using Breath_of_the_Wild_Multiplayer.Source_files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;

namespace Breath_of_the_Wild_Multiplayer.MVVM.Model
{
    public class MessageQueue : ObservableObject
    {
        public List<string> Queue = new List<string>();

        private string _currentMessage;

        public string CurrentMessage
        {
            get { return _currentMessage; }
            set { 
                _currentMessage = value;
                Debug.WriteLine($"Big message changed to: {value}");
                OnPropertyChanged();
            }
        }

        private bool _show;

        public bool Show
        {
            get { return _show; }
            set { 
                _show = value;
                Debug.WriteLine($"Current big message is now{(value ? "" : " not")} showing");
                OnPropertyChanged();
            }
        }

        private DispatcherTimer Timer = new DispatcherTimer();

        public MessageQueue()
        {
            Show = false;
            Timer.Interval = TimeSpan.FromSeconds(4);
            Timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(Timer.Interval == TimeSpan.FromSeconds(4))
            {
                Show = false;
                CurrentMessage = "";
                Timer.Interval = TimeSpan.FromSeconds(1);
            }
            else
            {
                Timer.Interval = TimeSpan.FromSeconds(4);

                if(Queue.Count == 0)
                {
                    Timer.Stop();
                }
                else
                {
                    Show = true;
                    CurrentMessage = Queue[0];
                    Queue.RemoveAt(0);
                }
            }
        }

        public void AddMessage(string msg)
        {
            if(string.IsNullOrEmpty(CurrentMessage) && Queue.Count == 0)
            {
                Debug.WriteLine($"Adding message: {msg} as new message");
                CurrentMessage = msg;
                Show = true;
                Timer.Start();
            }
            else
            {
                Debug.WriteLine($"Adding message: {msg} as queue message");
                Queue.Add(msg);
            }
        }
    }
}
