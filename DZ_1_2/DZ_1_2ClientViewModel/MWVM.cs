using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DZ_1_2ClientViewModel.Services;
using System.Windows;

namespace DZ_1_2ClientViewModel
{
    public partial class MWVM : ObservableObject
    {
        private readonly IConnectionService _connectionService;

        [ObservableProperty]
        private string _connectionButtonText = "������ ���.";

        public MWVM(IConnectionService connectionService) 
        {
            _connectionService = connectionService;
        }

        [RelayCommand]
        private async Task ConnectToServer()
        {
            if (_connectionService.IsConnected)
            {
                _connectionService.Disconnect();
                ConnectionButtonText = "������ ���.";
                MessageBox.Show("����������� �����������"!);
            }
            else
            {
                var success = await _connectionService.ConnectAsync("127.0.0.1", 7777);
                ConnectionButtonText = success ? "������ ����." : "������ ���.";
                MessageBox.Show("����������� ������������"!);
            }
        }
        [RelayCommand]
        private void AddRecipe()
        {

        }
        [RelayCommand]
        private void EditRecipe()
        {

        }
        [RelayCommand]
        private void DeleteRecipe()
        {

        }
    }
}