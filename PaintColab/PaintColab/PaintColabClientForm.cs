using Newtonsoft.Json;
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

namespace PaintColab
{
    public partial class PaintColabClientForm : Form
    {
        public PaintColabClientForm()
        {
            InitializeComponent();
        }

        // The port number for the remote device.  
        private int port = 11000;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private Socket client;

        // The response from the remote device.  
        private static String response = String.Empty;

        int R = 50;
        int G = 50;
        int B = 50;
        int A = 125;
        bool isMouseOverT, isMouseOverA, isMouseOverR, isMouseOverG, isMouseOverB = false;
        private int penThickness = 1;
        private Color penColor = Color.Black;
        private bool drawing = false;
        private Point prevMousePos;

        StreamReader sr;
        StreamWriter sw;
        Task<String> curTask = null;
        Graphics g;

        private void PaintColabClientForm_Load(object sender, EventArgs e)
        {
            g = canvasPanel.CreateGraphics();
            thicknessLabel.Text = $"T: {penThickness}";
            alphaLabel.Text = $"A: {A}";
            redLabel.Text = $"R: {R}";
            greenLabel.Text = $"G: {G}";
            blueLabel.Text = $"B: {B}";
        }

        private void connectServerButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    port = int.Parse(serverPortTextBox.Text);
            //    // Establish the remote endpoint for the socket.  
            //    // The name of the
            //    // remote device is "host.contoso.com".  
            //    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //    IPAddress ipAddress = ipHostInfo.AddressList[0];
            //    IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            //    // Create a TCP/IP socket.  
            //    client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //    // Connect to the remote endpoint.  
            //    client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            //    connectDone.WaitOne();

            //    // Send test data to the remote device.  
            //    Send(client, "This is a test<EOF>");
            //    sendDone.WaitOne();

            //    // Receive the response from the remote device.  
            //    Receive(client);
            //    receiveDone.WaitOne();

            //    // Write the response to the console.  
            //    Console.WriteLine("Response received : {0}", response);

            //    // Release the socket.  
            //    //client.Shutdown(SocketShutdown.Both);
            //    //client.Close();

            //}
            //catch (Exception except)
            //{
            //    Console.WriteLine(except.ToString());
            //}


            Start();
            timer1.Enabled = true;
        }

        public async void Start()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[2];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.
            client.Connect(remoteEP);
            NetworkStream ns = new NetworkStream(client);
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);
            curTask = sr.ReadLineAsync();

        }

        public void Read()
        {
            while (true)
            {
                var task = sr.ReadLineAsync();
                string temp;
                while (true)
                {
                    if (task.IsCompleted)
                    {
                        temp = task.Result;
                        task = sr.ReadLineAsync();
                    }
                }
            }

        }

        public void Write(String s)
        {
            sw.AutoFlush = true;
            sw.WriteLine(s);
            sw.Flush();
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
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
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
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
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
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
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


        private void canvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            prevMousePos = e.Location;
            drawing = true;
        }

        private void canvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            prevMousePos = e.Location;
            drawing = false;
        }

        private void canvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing&& prevMousePos!=e.Location) {
                var pen = new Pen(penColor, penThickness);
                //g.DrawLine(new Pen(penColor, penThickness), prevMousePos, e.Location);
                if (client!=null && client.Connected) {
                    String mes = JsonConvert.SerializeObject(new Line(Color.FromArgb(A, R, G, B).ToArgb(), penThickness, prevMousePos.X, prevMousePos.Y, e.X, e.Y));
                    Write(mes);
                }
                prevMousePos=e.Location;
            }
        }

        private void PaintColabClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private void thicknessLabel_MouseHover(object sender, EventArgs e)
        {
            isMouseOverT = true;
        }

        private void thicknessLabel_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverT = false;
        }

        private void redLabel_MouseHover(object sender, EventArgs e)
        {
            isMouseOverR = true;
        }

        private void redLabel_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverR = false;
        }

        private void greenLabel_MouseHover(object sender, EventArgs e)
        {
            isMouseOverG = true;
        }

        private void greenLabel_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverG = false;
        }

        private void blueLabel_MouseHover(object sender, EventArgs e)
        {
            isMouseOverB = true;
        }

        private void blueLabel_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverB = false;
        }

        private void alphaLabel_MouseHover(object sender, EventArgs e)
        {
            isMouseOverA = true;
        }

        private void alphaLabel_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverA = false;
        }

        private void form_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int scroll = e.Delta * SystemInformation.MouseWheelScrollLines / 360;
            if (isMouseOverA) { 
                A = Math.Abs((A+scroll)%255); 
                alphaLabel.Text = $"A: {A}";
            }
            if (isMouseOverR) { 
                R = Math.Abs((R+scroll)%255); 
                redLabel.Text = $"R: {R}";
            }
            if (isMouseOverG) { 
                G = Math.Abs((G+scroll)%255); 
                greenLabel.Text = $"G: {G}";
            }
            if (isMouseOverB) { 
                B = Math.Abs((B+scroll)%255); 
                blueLabel.Text = $"B: {B}";
            }
            if (isMouseOverT) { 
                penThickness = Math.Abs((penThickness+scroll)%255); 
                thicknessLabel.Text = $"T: {penThickness}";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (curTask.IsCompleted)
            {
                Line line = JsonConvert.DeserializeObject<Line>(curTask.Result);
                var p = new Pen(Color.FromArgb(line.Color), line.Thickness);
                p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(p, line.X1, line.Y1, line.X2, line.Y2);

                curTask = sr.ReadLineAsync();
            }
        }

        private void canvasPanel_Resize(object sender, EventArgs e)
        {
            g = canvasPanel.CreateGraphics();
        }

    }

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
}
