using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            const string IP = "127.0.0.1";
            const int PORT = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocket.Connect(tcpEndPoint);//подключаемся к серверу

            while (true)
            {
                try
                {
                    Console.WriteLine("Type a message: ");
                    var message = Console.ReadLine();

                    var data = Encoding.UTF8.GetBytes(message);//получаем введенные данные

                    tcpSocket.Send(data);//отправляем данные
                    var buffer = new byte[256];//буфер на 256 байт
                    var size = 0; //количество реально полученных байтов
                    var answer = new StringBuilder(); //здесь будем сохранять ответ с сервера

                    do
                    {
                        size = tcpSocket.Receive(buffer);

                        answer.Append(Encoding.UTF8.GetString(buffer, 0, size));//раскодировка полученных байтов


                    } while (tcpSocket.Available > 0);//пока есть данные, обрабатываем их

                    Console.WriteLine("Answer: " + answer);

                }
                catch 
                {

                    Console.WriteLine("Connection error! Enter any key to continue...");
                    Console.ReadLine();
                    return;
                }
            }
            tcpSocket.Shutdown(SocketShutdown.Both);//закрываем сокет на клиенте и сервере
            tcpSocket.Close();
        }
    }
}
