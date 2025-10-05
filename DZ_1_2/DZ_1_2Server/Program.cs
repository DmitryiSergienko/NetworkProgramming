//Задание 1. Создайте решение (проект) с помощью, которого можно узнавать кулинарные рецепты. Типичный пример работы    
//▪ клиентское приложение подключается к серверу;    
//▪ клиентское приложение посылает запрос с указанием списка продуктов;
//▪ сервер возвращает рецепты, содержащие указанные продукты;  
//▪ клиент может послать новый запрос или отключиться.

//Задание 2. Добавьте оконный интерфейс для управления сервером. Также добавьте оконный интерфейс для управления клиентом.

//Дополнительно:
//Задание 3. Добавьте механизм логирования в сервер.
//Этот механизм должны сохранять информацию о клиентах, их запросах, времени соединения и т.д.
//Задание 4. Редактирование рецептов, включая изображения.

using System.Threading.Tasks;

namespace DZ_1_2Server 
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            using var server = new Server(port: 7777);

            var waitForExit = new TaskCompletionSource<bool>();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                waitForExit.SetResult(true);
            };

            await server.StartAsync();
            Console.WriteLine("Сервер работает. Нажмите Ctrl+C для остановки.");

            await waitForExit.Task;
            await server.StopAsync();
        }
    }
}