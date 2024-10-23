using System.Diagnostics;
using Wpf.Ui.Common.Interfaces;

namespace Awake.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
    {
        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }

        public SettingsPage(ViewModels.SettingsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void 元素法典_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://docs.qq.com/doc/DWGh4QnZBVlJYRkly") { UseShellExecute = true });
        }

        private void 解构原典_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://docs.qq.com/doc/DR1Z4VkFEZGl4Sk9S") { UseShellExecute = true });
        }
    }
}