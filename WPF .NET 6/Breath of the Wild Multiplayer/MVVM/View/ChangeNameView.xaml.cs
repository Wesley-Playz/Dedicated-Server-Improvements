using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.MVVM.ViewModel;
using System.Diagnostics;


namespace Breath_of_the_Wild_Multiplayer.MVVM.View
{
    /// <summary>
    /// Lógica de interacción para ChangeNameView.xaml
    /// </summary>
    public partial class ChangeNameView : UserControl
    {
        public ChangeNameView()
        {
            InitializeComponent();
        }

        // This is the handler for the 'Accept' button click event
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(PlayerNameTextBox.Text))
            {
                Properties.Settings.Default.playerName = "link";
                Properties.Settings.Default.Save();
                return;
            }


            bool isNameValid = PlayerNameTextBox.Text.All(c => c <= 127 && c != ';');

            if (isNameValid)
            {
                Properties.Settings.Default.playerName = PlayerNameTextBox.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.playerName = "link";
                Properties.Settings.Default.Save();
                return;
            }
        }
    }
}
