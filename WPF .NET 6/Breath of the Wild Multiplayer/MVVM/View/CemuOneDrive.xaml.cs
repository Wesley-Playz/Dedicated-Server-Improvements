using System.Windows;

namespace OneDriveApp
{
    public partial class OneDrive : Window
    {
        public OneDrive()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void DontShowButton_Click(object sender, RoutedEventArgs e)
        {
            Breath_of_the_Wild_Multiplayer.Properties.Settings.Default.CemuOneDrive = false;
            Breath_of_the_Wild_Multiplayer.Properties.Settings.Default.Save();
            Close();
        }
    }
}
