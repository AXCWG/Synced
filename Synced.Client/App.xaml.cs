using Synced.Client.Windows;
using Synced.Client.Windows.Resources;
using Application = System.Windows.Application;

namespace Synced.Client
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private NotifyIcon? _notifyIcon;
        public AppRuntime ApplicationAppRuntime { get; } = new AppRuntime();
        public App()
        {
            if (!ApplicationAppRuntime.AppConfig.Init)
            {
                new Wizard().Show();
            }
            _notifyIcon = new()
            {
                Icon = new Func<Icon>(() =>
                {
                    using var stream = Application.GetResourceStream(new Uri("pack://application:,,,/Assets/icox16x32x48.ico", UriKind.Absolute))?.Stream;
                    return new Icon(stream ?? throw new InvalidOperationException("Icon resource not found."));
                })(),
                Visible = true,
                Text = "Synced Client",
                ContextMenuStrip = new Func<ContextMenuStrip>(() =>
                {
                    var c = new ContextMenuStrip();
                    c.Items.Add(AppLocalization.ToolStripExit, null, (s, e) => Application.Current.Shutdown());
                    return c;
                })()
            };
        }
    }

}
