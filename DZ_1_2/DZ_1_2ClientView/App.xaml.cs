using DZ_1_2ClientView.Services;
using DZ_1_2ClientViewModel;
using System.Windows;

namespace DZ_1_2ClientView
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var connectionService = new TcpClientService();
            var viewModel = new MWVM(connectionService);

            var mainWindow = new MainWindow { DataContext = viewModel };
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}