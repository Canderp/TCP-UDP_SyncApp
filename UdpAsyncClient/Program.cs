using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpAsyncClient
{
    public class StateObject
{
    // Client  socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 1024;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}
    class Program
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            IPEndPoint lep = new IPEndPoint(IPAddress.Parse("127.0.0.7"), 11000);

            Socket s = new Socket(lep.Address.AddressFamily,
                                        SocketType.Dgram,
                                              ProtocolType.Udp);

            IPEndPoint sender = new IPEndPoint(IPAddress.Parse("127.0.0.7"), 11000);
            EndPoint tempRemoteEP = (EndPoint)sender;
            s.Connect(sender);

            try
            {
                while (true)
                {
                    allDone.Reset();
                    StateObject so2 = new StateObject();
                    so2.workSocket = s;
                    Console.WriteLine("Attempting to Receive data from host.contoso.com");

                    s.BeginReceiveFrom(so2.buffer, 0, StateObject.BufferSize, 0, ref tempRemoteEP,
                                          new AsyncCallback(Async_Send_Receive_ReceiveFrom_Callback), so2);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Async_Send_Receive_ReceiveFrom_Callback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;

            EndPoint lep = new IPEndPoint(IPAddress.Parse("127.0.0.7"), 11000);

            s.EndReceiveFrom(ar,ref lep);
        }
    }

    
}
