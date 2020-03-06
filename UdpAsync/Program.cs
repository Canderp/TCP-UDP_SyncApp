using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpAsync
{
    class Program
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            IPEndPoint lep = new IPEndPoint(IPAddress.Parse("127.0.0.7"), 11000);

            Socket s = new Socket(lep.Address.AddressFamily,
                                        SocketType.Stream,
                                              ProtocolType.Tcp);
            try
            {

                while (true)
                {
                    allDone.Reset();

                    byte[] buff = Encoding.ASCII.GetBytes("This is a test");

                    Console.WriteLine("Sending Message Now..");
                    s.BeginSendTo(buff, 0, buff.Length, 0, lep, new AsyncCallback(Async_Send_Receive_SendTo_Callback), s);

                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Async_Send_Receive_SendTo_Callback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;

            s.EndSendTo(ar);
        }
    }
}
