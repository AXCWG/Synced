using System.Windows;
using System.Windows.Controls;

namespace Synced.Client.WizardPages
{
    /// <summary>
    /// Page2.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        private Wizard wizard;
        public Page2(Wizard wizard)
        {
            this.wizard = wizard;
            InitializeComponent();
        }

        private void WizardServer_TextChanged(object sender, TextChangedEventArgs e)
        {
            wizard.Info.Addr = (e.OriginalSource as System.Windows.Controls.TextBox)!.Text;
        }

        private void WizardUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            wizard.Info.Username = (e.OriginalSource as System.Windows.Controls.TextBox)!.Text;
        }

        private void WizardPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            wizard.Info.Password = (e.OriginalSource as System.Windows.Controls.PasswordBox)!.Password;
        }
    }
}
