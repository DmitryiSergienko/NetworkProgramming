using DZ_1_2ClientViewModel.Services;
using System.Net.Sockets;

namespace DZ_1_2ClientView.Services
{
    public class TcpClientService : IConnectionService
    {
        private TcpClient? _client;

        public bool IsConnected => _client?.Connected == true;

        public async Task<bool> ConnectAsync(string host, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(host, port);
                return true;
            }
            catch
            {
                _client?.Dispose();
                _client = null;
                return false;
            }
        }
        public void Disconnect()
        {
            _client?.Dispose();
            _client = null;
        }
    }
}