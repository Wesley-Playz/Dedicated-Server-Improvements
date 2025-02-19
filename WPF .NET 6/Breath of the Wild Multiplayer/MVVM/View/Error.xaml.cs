using System.Windows;

namespace Errorpopup
{
    public partial class Error : Window
    {
        public Error()
        {
            InitializeComponent();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public static void ShowErrorPopup(string message)
        {
            var errorWindow = new Error();
            errorWindow.MessageText.Text = message;
            errorWindow.ShowDialog();
        }
    }
}
