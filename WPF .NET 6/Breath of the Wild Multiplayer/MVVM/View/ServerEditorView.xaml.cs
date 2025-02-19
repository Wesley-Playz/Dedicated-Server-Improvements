using Breath_of_the_Wild_Multiplayer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Breath_of_the_Wild_Multiplayer.MVVM.View
{
    /// <summary>
    /// Lógica de interacción para ServerEditorView.xaml
    /// </summary>
    public partial class ServerEditorView : UserControl
    {
        public ServerEditorView()
        {
            InitializeComponent();
            if (this.DataContext is ServerEditorModel model && string.IsNullOrEmpty(model.Port))
            {
                model.Port = "5050"; // default port
            }
        }

        // Name validations

        private void TextBox_ValidateName(object sender, TextCompositionEventArgs e) => ((ServerEditorModel)this.DataContext).ValidateInputs(name: e.Text);

        private void NameTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
                ((ServerEditorModel)this.DataContext).ValidateInputs(name: ((TextBox)e.OriginalSource).Text);
        }

        private void TextBox_PasteValidateName(object sender, ExecutedRoutedEventArgs e) => ((ServerEditorModel)this.DataContext).ValidateInputs(name: Clipboard.GetText());

        // IP validations

        private void TextBox_ValidateIP(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidateIP(e.Text);
            ((ServerEditorModel)this.DataContext).ValidateInputs(ip: e.Text);
        }

        private void IPTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
                ((ServerEditorModel)this.DataContext).ValidateInputs(ip: ((TextBox)e.OriginalSource).Text);
        }

        private void TextBox_PasteValidateIP(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
                e.Handled = !ValidateIP(Clipboard.GetText());

            ((ServerEditorModel)this.DataContext).ValidateInputs(ip: Clipboard.GetText());
        }

        // Port validations

        private void TextBox_ValidatePort(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidatePort(e.Text);
            ((ServerEditorModel)this.DataContext).ValidateInputs(port: e.Text);
        }

        private void PortTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
               ((ServerEditorModel)this.DataContext).ValidateInputs(port: ((TextBox)e.OriginalSource).Text);
        }

        private void TextBox_PasteValidatePort(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
                e.Handled = !ValidatePort(Clipboard.GetText());

            ((ServerEditorModel)this.DataContext).ValidateInputs(port: Clipboard.GetText());
        }

        //private bool ValidateIP(string text) => !new Regex("[^0-9.]+").IsMatch(text);
        private bool ValidateIP(string text) => true;

        private bool ValidatePort(string text) => !new Regex("[^0-9]+").IsMatch(text);
    }
}
