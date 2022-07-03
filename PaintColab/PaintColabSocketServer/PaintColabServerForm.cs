using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintColabSocketServer
{
    public partial class PaintColabServerForm : Form
    {
        public PaintColabServerForm()
        {
            InitializeComponent();
        }

        private static int port = 11000;

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        SynchronizedCache messageList = new SynchronizedCache();

        private void PaintColabServerForm_Load(object sender, EventArgs e)
        {

        }

        private void startServerButton_Click(object sender, EventArgs e)
        {
            port = int.Parse(portTextBox.Text);
            //Thread server = new Thread(new ThreadStart(StartListening));
            //server.Start();

            Thread t1 = new Thread(Start);
            t1.Start();
        }

        public void Start()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);

            // Create a TCP/IP socket.
            Socket host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.

            host.Bind(localEndPoint);
            host.Listen(5);

            NetworkStream s;
            while (true)
            {
                Console.WriteLine("Сервер ждет нового подключения\r\n");

                s = new NetworkStream(host.Accept());
                Thread t = new Thread(Read);
                t.Start(s);

                Console.WriteLine("Сервер подключил нового клиента\r\n");

            }
        }
        public void Read(Object networkStream)
        {
            int messCount = 0;

            StreamReader sr = new StreamReader((NetworkStream)networkStream);
            StreamWriter sw = new StreamWriter((NetworkStream)networkStream);
            var task = sr.ReadLineAsync();
            string temp;
            while (true)
            {
                if (task.IsCompleted)
                {
                    temp = task.Result;
                    messageList.Add(temp);

                    //Console.WriteLine("Client says: " + temp + "\r\n");

                    task = sr.ReadLineAsync();
                }
                while (messCount < messageList.Count)
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(messageList.Read(messCount++));
                    sw.Flush();
                }
            }
        }
        public static void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".
            Console.WriteLine(Dns.GetHostName());
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept( new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString( state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                    // Echo the data back to the client.  
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }
    public class SynchronizedCache
    {
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private List<string> innerCache = new List<string>();

        public int Count
        { get { return innerCache.Count; } }

        public string Read(int key)
        {
            cacheLock.EnterReadLock();
            try
            {
                return innerCache[key];
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public void Add(string value)
        {
            cacheLock.EnterWriteLock();
            try
            {
                innerCache.Add(value);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public bool AddWithTimeout(int key, string value, int timeout)
        {
            if (cacheLock.TryEnterWriteLock(timeout))
            {
                try
                {
                    innerCache.Add(value);
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //public AddOrUpdateStatus AddOrUpdate(int key, string value)
        //{
        //    cacheLock.EnterUpgradeableReadLock();
        //    try
        //    {
        //        string result = null;
        //        if (innerCache.Contains(result))
        //        {
        //            if (result == value)
        //            {
        //                return AddOrUpdateStatus.Unchanged;
        //            }
        //            else
        //            {
        //                cacheLock.EnterWriteLock();
        //                try
        //                {
        //                    innerCache[key] = value;
        //                }
        //                finally
        //                {
        //                    cacheLock.ExitWriteLock();
        //                }
        //                return AddOrUpdateStatus.Updated;
        //            }
        //        }
        //        else
        //        {
        //            cacheLock.EnterWriteLock();
        //            try
        //            {
        //                innerCache.Add(key, value);
        //            }
        //            finally
        //            {
        //                cacheLock.ExitWriteLock();
        //            }
        //            return AddOrUpdateStatus.Added;
        //        }
        //    }
        //    finally
        //    {
        //        cacheLock.ExitUpgradeableReadLock();
        //    }
        //}

        public void Delete(string key)
        {
            cacheLock.EnterWriteLock();
            try
            {
                innerCache.Remove(key);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        };

        ~SynchronizedCache()
        {
            if (cacheLock != null) cacheLock.Dispose();
        }
    }

}
