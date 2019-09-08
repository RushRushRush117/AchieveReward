using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SocketConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 6000;
            string host = "127.0.0.1";
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipe);
            socket.Listen(0);
            Console.WriteLine("监听已经打开");

            Socket accpetSocket = socket.Accept();
            Console.WriteLine("连接已经建立");
            Byte[] buffer = new Byte[4096];
            accpetSocket.Receive(buffer);
            string str= Encoding.ASCII.GetString(buffer);

            Console.WriteLine("接受到信息 : " + str);

            string sendStr = "Send to Client :Hello";
            Byte[] _buffer = Encoding.ASCII.GetBytes(sendStr);
            accpetSocket.Send(_buffer);
            accpetSocket.Close();
            socket.Close();
        }
    }
}
