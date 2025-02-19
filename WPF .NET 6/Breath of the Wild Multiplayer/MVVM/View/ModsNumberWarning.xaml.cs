using System.Windows;

namespace Breath_of_the_Wild_Multiplayer.MVVM.View
{
    public partial class ModsNumberWarning : Window
    {
        public ModsNumberWarning()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void DontShowButton_Click(object sender, RoutedEventArgs e)
        {
            Breath_of_the_Wild_Multiplayer.Properties.Settings.Default.ModsNumberWarning = false;
            Breath_of_the_Wild_Multiplayer.Properties.Settings.Default.Save();
            Close();
        }
    }
}
