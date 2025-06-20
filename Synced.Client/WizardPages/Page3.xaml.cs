using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Synced.Client.WizardPages
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page3 : Page
    {
        private BindingList<object> WizardFolderListSource { get; }
        public Page3()
        {
            //void AddingDir()
            //{

            //var col1GridLen = new GridLength(1, GridUnitType.Star);

            //g.ColumnDefinitions.Add(new(){Width = new GridLength(1, GridUnitType.Star) });
            //g.ColumnDefinitions.Add(new(){ Width = new GridLength(1, GridUnitType.Auto)});
            //}
            WizardFolderListSource = new()
            {
                AllowNew = true
                ,
                AllowRemove = true
                ,
                AllowEdit = true
                ,
                RaiseListChangedEvents = true
            }
            ;
            InitializeComponent();
            WizardFolderList.ItemsSource = WizardFolderListSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var f = new OpenFolderDialog();
            f.Multiselect = true;
            f.Title = "选择要同步的文件夹";
            var r = f.ShowDialog();
            if ((r is not null) && r.Value)
            {
                f.FolderNames.ToList().ForEach(i =>
                {
                    WizardFolderListSource.Add(new Func<Grid>(() =>
                    {
                        var g = new Grid();

                        g.ColumnDefinitions.Add(new() { Width = new GridLength(1, GridUnitType.Star) });
                        g.ColumnDefinitions.Add(new() { Width = new GridLength(1, GridUnitType.Auto) });
                        g.Children.Add(new Func<System.Windows.Controls.TextBlock>(() =>
                        {
                            var t = new System.Windows.Controls.TextBlock();
                            t.TextWrapping = TextWrapping.Wrap;
                            Grid.SetColumn(t, 0);
                            t.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                            t.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                            t.Text = i;
                            return t;
                        })());
                        g.Children.Add(new Func<System.Windows.Controls.Button>(() =>
                        {
                            var t = new System.Windows.Controls.Button()
                            {
                                Content = "...",
                                Width = Height,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                                ,
                                VerticalAlignment = VerticalAlignment.Stretch
                            };
                            Grid.SetColumn(t, 1);
                            return t;
                        })());
                        return g;

                    })());
                });
            }





        }
    }
}
