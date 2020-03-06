using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

// State object for receiving data from remote device.  
public class StateObject
{
    // Client socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 256;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousClient
{
    // The port number for the remote device.  
    private const int port = 11000;

    // ManualResetEvent instances signal completion.  
    private static ManualResetEvent allDone =
        new ManualResetEvent(false);
    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private static ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private static ManualResetEvent receiveDone =
        new ManualResetEvent(false);

    // The response from the remote device.  
    private static String response = String.Empty;

    private static void StartClient()
    {
        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.  
            // The name of the   
            // remote device is "host.contoso.com".  
            IPAddress ipAddress = IPAddress.Parse("127.0.0.7");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            Socket client = new Socket(ipAddress.AddressFamily,
                    SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine("Type Yes to fetch resources from the server.");
            var input = Console.ReadLine();

            client.Connect(remoteEP);


            while (input == "Yes")
            {
                // Create a TCP/IP socket.  

                StateObject so = new StateObject();

                so.workSocket = client;
                // Connect to the remote endpoint.  
                //client.BeginConnect(remoteEP,
                //    new AsyncCallback(ConnectCallback), client);
                //connectDone.WaitOne();


                // Send test data to the remote device.

                //Send(client, Console.ReadLine());
                //sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                //receiveDone.WaitOne();

                // Write the response to the console.  
                //Console.WriteLine("Response received : {0}", response);

                // Release the socket.  
                

                Console.WriteLine("Resources Fetched from the Server.");
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            client.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}",
                client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.  
            connectDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Receive(Socket client)
    {
        try
        {
            allDone.Reset();

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = client;
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.7"), port);

            var tempRempteEP = (EndPoint)remoteEP;


            client.BeginReceiveFrom(
                state.buffer,
                0, 
                StateObject.BufferSize, 
                0,
                ref tempRempteEP, 
                ReceiveCallback, 
                state);
            
                allDone.WaitOne();


            // Begin receiving the data from the remote device.  
            //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            //    new AsyncCallback(ReceiveCallback), state);

            //Console.WriteLine("Response received : {0}", response);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            IPEndPoint sender = new IPEndPoint(IPAddress.Parse("127.0.0.7"), 11000);
            // Retrieve the state object and the client socket   
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            EndPoint ep = (EndPoint)sender;
            // Read data from the remote device.  
            int bytesRead = client.EndReceiveFrom(ar,ref ep);

            //int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                //// There might be more data, so store the data received so far.  
                //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                //response = state.sb.ToString();
                //// Get the rest of the data.  
                ///

                Console.WriteLine(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                client.BeginReceiveFrom(state.buffer,
                    0,
                    StateObject.BufferSize,
                    0,
                    ref ep,
                    ReceiveCallback,
                    state);

                //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                // All the data has arrived; put it in response.
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                // Signal that all bytes have been received.  
                //receiveDone.Set();

                client.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Send(Socket client, String data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            // Signal that all bytes have been sent.  
            sendDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        StartClient();

        Console.ReadLine();
        return 0;
    }
}