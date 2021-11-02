using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketTcpClient
{
    class Program
    {
        static int port = 8005;
        static string address = "127.0.0.1";
        static void Main(string[] args)
        {
            try
            {
                SendMessage();
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SendMessage()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // подключаемся к хосту
            socket.Connect(ipPoint);
            Console.Write("Введите запрос: ");
            string message = Console.ReadLine();
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);

            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }

            while (socket.Available > 0);
            Console.WriteLine("Ответ сервера: " + builder.ToString());

            if (message.IndexOf("Stop") == -1)
                SendMessage();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}