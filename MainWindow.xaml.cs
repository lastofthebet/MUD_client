﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MUD_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Label LogManager;
        public MainWindow()
        {
            InitializeComponent();
           
        }
        public static void StartClient(Label Log)
        {
            byte[] bytes = new byte[1024];

            try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    sender.Connect(remoteEP);

                    Log.Content = Log.Content + "Socket connected to { 0}"+ sender.RemoteEndPoint.ToString();


                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Log.Content = Log.Content + "\nArgumentNullException : {0}"+ ane.ToString();
                }
                catch (SocketException se)
                {
                    Log.Content = Log.Content + "\nSocketException : {0}" + se.ToString();
                }
                catch (Exception e)
                {
                    Log.Content = Log.Content + "\nUnexpected exception : {0}" + e.ToString();
                }

            }
            catch (Exception e)
            {
                Log.Content = Log.Content + "\n" + e.ToString();
            }
        }

        private void Log_Initialized(object sender, EventArgs e)
        {
           Log.VerticalAlignment = VerticalAlignment.Bottom;
           Log.VerticalContentAlignment = VerticalAlignment.Bottom;
           Log.Content = Log.Content + "\ntest";
           StartClient(Log);
        }
    }
}
