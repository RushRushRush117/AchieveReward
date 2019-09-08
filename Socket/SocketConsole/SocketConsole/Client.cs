using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketConsole
{
    class Client
    {
        int port = 6000;
        string ipAdress = "127.0.0.0";
        IPAddress ip;
        IPEndPoint ipe;

        public void NetWorkInit()
        {
            ip = IPAddress.Parse(ipAdress);
            ipe = new IPEndPoint(ip, port);
            Socket mClientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            mClientSocket.Connect(ipe);
            Console.WriteLine("建立连接成功");

            string sendStr = "send to server : hello ";
            byte[] buffer = Encoding.ASCII.GetBytes(sendStr);
            mClientSocket.Send(buffer);

            string recStr = "";
            byte[] _buffer = new byte[4096];
            mClientSocket.Receive(_buffer,)
        }
        
    }
}
