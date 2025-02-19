using System.Windows;

namespace PythonCheckerApp
{
    public partial class PythonWarning : Window
    {
        public PythonWarning()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void DontShowButton_Click(object sender, RoutedEventArgs e)
        {
            Breath_of_the_Wild_Multiplayer.Properties.Settings.Default.PythonStoreWarning = false;
            Breath_of_the_Wild_Multiplayer.Properties.Settings.Default.Save();
            Close();
        }
    }
}
