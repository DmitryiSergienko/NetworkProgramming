using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DZ_1_2Server
{
    public class Server : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly CancellationTokenSource _cts = new();
        private Task? _serverTask;
        private readonly TaskCompletionSource _acceptLoopStarted = new();

        public Server(int port = 7777)
        {
            _listener = new TcpListener(IPAddress.IPv6Any, port);
            _listener.Server.DualMode = true;
        }        

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine($"[Server] Запущен на порту {_listener.LocalEndpoint}");

            _serverTask = AcceptClientsAsync(_cts.Token);
            await _acceptLoopStarted.Task;
        }

        private async Task AcceptClientsAsync(CancellationToken ct)
        {
            try
            {
                _acceptLoopStarted.TrySetResult();

                while (!ct.IsCancellationRequested)
                {
                    var client = await _listener.AcceptTcpClientAsync(ct);
                    _ = HandleClientAsync(client, ct);
                }
            }
            catch (OperationCanceledException)
            {
                _acceptLoopStarted.TrySetCanceled();
            }
            catch (Exception ex)
            {
                _acceptLoopStarted.TrySetException(ex);
                Console.WriteLine($"[Server] Ошибка: {ex}");
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
        {
            try
            {
                using (client)
                {
                    var buffer = new byte[1024];
                    var stream = client.GetStream();
                    
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
                    if (bytesRead == 0) return;

                    var request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    Console.WriteLine($"[Client] {request}");

                    var response = "OK\n";
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(response), ct);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client] Ошибка: {ex.Message}");
            }
        }

        public async Task StopAsync()
        {
            _cts.Cancel();
            _listener.Stop();
            if (_serverTask != null) 
            {
                await _serverTask;
            }
            Console.WriteLine("[Server] Остановлен.");
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _listener?.Stop();
        }
    }
}