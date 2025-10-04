using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerApp {
    public partial class ServerForm : Form {
        private const int port = 8888;
        private TcpListener listener;
        public ServerForm() {
            InitializeComponent();
            Task.Run(() => StartServer()); // запуск сервера в отдельном потоке
        }
        private void StartServer() {
            try {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                Log("Сервер запущен...");
                while (true) {
                    TcpClient client = listener.AcceptTcpClient();
                    HandleClient(client);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HandleClient(TcpClient client) {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string data = Encoding.Unicode.GetString(buffer, 0, bytesRead);
            string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            Log($"В {DateTime.Now:t} от [{clientIP}] получена строка: {data}");
            string serverIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.
                AddressFamily == AddressFamily.InterNetwork).ToString(); // получение IP-адреса сервера
            string response = $"Привет, клиент! IP-адрес сервера: {serverIP}";
            byte[] responseData = Encoding.Unicode.GetBytes(response);
            stream.Write(responseData, 0, responseData.Length);
            Log($"Отправлено сообщение клиенту: {response}");
            client.Close();
        }
        private void Log(string message) {
            if (InvokeRequired) {
                Invoke(new MethodInvoker(delegate { Log(message); }));
                return;
            }
            textBoxLog.Text += $"{DateTime.Now:t} {message}\r\n";
        }
    }
}

/*Пошаговая инструкция как создать решение в Visual Studio и подключить к нему серверное и клиентское(ие) 
приложения:
1.  Открыть Visual Studio: Запустить Visual Studio на компьютере.
2.  Создать новый проект: Выбрать "Файл" -> "Создать" -> "Новый проект" из главного меню.
3.  Выбрать тип проекта: В окне "Создать новый проект" выбрать тип проекта. Например "Windows Forms 
    (.NET Framework)".
4.  Указать имя и местоположение: Ввести имя для решения, выбрать местоположение и нажать кнопку "Создать".
5.  Добавить новые проекты к решению:
	• щёлкнуть правой кнопкой мыши на решении в обозревателе решений;
	• выбрать "Добавить" -> "Новый проект";
	• добавить проект "Windows Forms App (.NET Framework)" для серверного и клиентского приложений, 
    и дать им соответствующие имена.
6.  Добавить код в каждый проект.
7.  Установить стартовые проекты:
	• щёлкнуть правой кнопкой мыши на решении в обозревателе решений;
	• выбрать "Свойства";
	• в разделе "Общие" найти опцию "Несколько запускаемых проектов";
	• выбрать "Несколько запускаемых проектов" и установить для каждого проекта соответствующий статус 
    "Запуск", при необходимости изменить очерёдность их запуска.
8.  Запустить решение: Нажать F5 или выбрать "Отладка" -> "Запуск" из главного меню, чтобы запустить 
    решение.
9.  Окно сервера не отображается – запустить его в отдельном потоке:
    public ServerForm() {
        InitializeComponent();
        Task.Run(() => StartServer()); // запуск сервера в отдельном потоке
    }
10. Если исходный проект не нужен, удалить его из решения:
    • щёлкнуть правой кнопкой мыши на проекте в обозревателе решений и выбрать "Удалить";
    • удалить каталог (папку) проекта из каталога (папки) решения.*/

/*Задание 1. Переведите первое практическое задание из консольного интерфейса в оконный интерфейс.
Практическое задание 1. Разработайте два консольных приложения, использующих сокеты. Одно приложение – 
сервер, второе – клиент. Клиентское приложение посылает приветствие серверу. Сервер отвечает. И клиент, 
и сервер отображают полученное сообщение. Пример вывода:
Сервер:
В 10:25 от [IP-адрес] получена строка: Привет, сервер!
Клиент:
В 10:26 от [IP-адрес] получена строка: Привет, клиент!
Используйте механизм синхронных сокетов.*/