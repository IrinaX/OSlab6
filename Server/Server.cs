using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            const string IP = "127.0.0.1";
            const int PORT = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(5);//5 endPoints
            var listener = tcpSocket.Accept();//сокет для нового клиента
            while (true)
            {
                try
                {
                    var buffer = new byte[256];//буфер на 256 байт
                    var size = 0; //количество реально полученных байтов
                    var data = new StringBuilder(); //здесь будем сохранять полученные данные

                    do
                    {
                        size = listener.Receive(buffer);
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size));//раскодировка полученных байтов
                    } while (listener.Available > 0);//пока есть данные, обрабатываем их

                    Console.WriteLine("Answer: " + data);

                    Console.WriteLine("Type a message: ");
                    var message = Console.ReadLine();
                    var newData = Encoding.UTF8.GetBytes(message);//получаем введенные данные
                    listener.Send(newData);//отправляем данные
                }
                catch
                {

                    Console.WriteLine("Connection error! Enter any key to continue...");
                    Console.ReadLine();
                    return;
                }
            }
            listener.Shutdown(SocketShutdown.Both);//закрываем сокет на клиенте и сервере
            listener.Close();
        }
    }
}
