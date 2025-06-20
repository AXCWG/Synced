using Synced.Client.Windows.Resources;
using System.Windows;
using System.Windows.Controls;

namespace Synced.Client
{
    /// <summary>
    /// Wizard.xaml 的交互逻辑
    /// </summary>
    public partial class Wizard : Window
    {
        public class Information
        {
            public string Addr
            {
                get; set;
            } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
        private Page[] PageAvail { get; set; }
        public Information Info { get; } = new Information();

        private int Index
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
                WizardFrame.Content = PageAvail[field];
                WizardBack.IsEnabled = !(Index == 0);
                if (Index == PageAvail.Length - 1)
                {
                    WizardNext.Content = "完成";
                }
                else
                {
                    WizardNext.Content = AppLocalization.NextStep;
                }
            }
        }

        public Wizard()
        {
            PageAvail = [new WizardPages.Page1(), new WizardPages.Page2(this), new WizardPages.Page3()];
            InitializeComponent();

            WizardFrame.Content =
                //new Uri($"WizardPages/Page{PageAvail[Index]}.xaml", UriKind.Relative);
                new WizardPages.Page1();
            WizardBack.IsEnabled = !(Index == 0);
        }

        private void WizardBack_Click(object sender, RoutedEventArgs e)
        {
            Index--;
        }

        private void WizardNext_Click(object sender, RoutedEventArgs e)
        {
            if (Index == PageAvail.Length - 1)
            {
                try
                {
                    new Uri(Info.Addr);
                }
                catch (Exception)
                {
                    Index = 1;
                    return;
                }
                (Synced.Client.App.Current as App)!.ApplicationAppRuntime.AppConfig.Nodes!.Add(new() { ServerUri = new Uri(Info.Addr), Username = Info.Username, Password = Info.Password });
                Hide();
                return;
            }
            Index++;

        }
    }
}
