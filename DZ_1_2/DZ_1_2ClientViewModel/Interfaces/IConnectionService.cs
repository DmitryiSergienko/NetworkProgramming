using DZ_1_2ClientModel;

namespace DZ_1_2ClientViewModel.Services
{
    public interface IConnectionService
    {
        Task<bool> ConnectAsync(string host, int port);
        void Disconnect();
        bool IsConnected { get; }
    }
}