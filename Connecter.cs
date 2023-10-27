using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MUD_client
{
    internal class Connecter
    {
        private static IPHostEntry host = Dns.GetHostEntry("localhost");
        private static IPAddress ipAddress = host.AddressList[0];
        private static IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 11000);
        public static Socket StartConnection()
        {
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method
            return sender;
        }
        public static String connect(Socket sender)
        {
            sender.Connect(remoteEndPoint);
            return "Socket connected to {0}" +sender.RemoteEndPoint.ToString();
        }
        public static String recieveData(Socket handler)
        {
            try
            { // Incoming data from the client.
                string data = null;
                byte[] bytes = null;
                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    return data;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void SendData(Socket handler, string data)
        {
            try
            {
                byte[] message = Encoding.ASCII.GetBytes(data);
                handler.Send(message);
            }
            catch (Exception e) { throw e; }
        }
        public static void Shutdown(Socket handler)
        {
            try { 
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            }
            catch (Exception e) { throw e; 
            }
            
        }
    }
}
