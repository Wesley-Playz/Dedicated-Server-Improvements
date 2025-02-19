using Breath_of_the_Wild_Multiplayer.Source_files;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace Breath_of_the_Wild_Multiplayer.MVVM.Model
{
    public class ChatMessage
    {
        public enum MessageType
        {
            Log,
            UserMessage,
            ServerMessage
        }

        static Dictionary<int, Color> MessageColors = new Dictionary<int, Color>()
        {
            { -2, Colors.Orange }, // Logs
            { -1, Colors.White }, // Server
            { 1, Colors.Green },
            { 2, Colors.Red },
            { 3, Colors.Blue },
            { 4, Colors.Purple },
            { 5, Colors.Yellow },
            { 6, Colors.Silver },
            { 7, Colors.Brown },
            { 8, Colors.Pink },
            { 9, Colors.Teal },
            { 10, Colors.Gold }
        };

        public string Sender { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public DateTime ReceivedAt { get; private set; }
        public int PlayerNumber { get; set; }

        public string MessageSender
        {
            get
            {
                if (this.Type == MessageType.Log)
                {
                    return $"[{Sender}]";
                }
                else
                {
                    return $"{Sender}:";
                }
            }
        }

        public SolidColorBrush MessageSenderColor
        {
            get
            {
                return new SolidColorBrush(MessageColors[this.PlayerNumber]);
            }
        }

        public ChatMessage(string sender, string content, int playerNumber = -1)
        {
            Sender = sender;
            Content = content;
            
            switch(playerNumber)
            {
                case -2:
                    this.Type = MessageType.Log; break;
                case -1:
                    this.Type = MessageType.ServerMessage; break;
                default:
                    this.Type = MessageType.UserMessage; break;
            }

            ReceivedAt = DateTime.Now;
            PlayerNumber = playerNumber < 10 ? playerNumber : playerNumber - 10;
        }
    }

    public class ChatModel : ObservableObject
    {
        private ChatMessage.MessageType? _filter;

        public ChatMessage.MessageType? Filter
        {
            get { return _filter; }
            set { 
                _filter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Messages));
            }
        }

        private ObservableCollection<ChatMessage> _messages;

        public ObservableCollection<ChatMessage> Messages
        {
            get 
            { 
                if(Filter == null)
                {
                    return _messages;
                }

                return new ObservableCollection<ChatMessage>(_messages.Where(msg => msg.Type == Filter));
            }
            set { 
                _messages = value;
                OnPropertyChanged();
            }
        }

        public ChatModel()
        {
            this.Messages = new ObservableCollection<ChatMessage>();
        }

        public void AddMessage()
        {
            this.Messages.Add(new ChatMessage("Logger", "LogMessage", -2));
            this.Messages.Add(new ChatMessage("Server", "ServerMessage", -1));
            this.Messages.Add(new ChatMessage("Mango", "LogMessage", 1));
            this.Messages.Add(new ChatMessage("Sweet", "LogMessage", 2));
            this.Messages.Add(new ChatMessage("Edgar", "LogMessage", 3));
            this.Messages.Add(new ChatMessage("Brandon", "LogMessage", 4));
        }
    }
}
