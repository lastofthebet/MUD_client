using System;
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
        public Socket senderManager;
        private static Label EncounterManager;
        private static Label KillManager;
        private static Label DungeonManager;
        public MainWindow()
        {
            InitializeComponent();
            StartClient(LogManager);

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
                //IPHostEntry host = Dns.GetHostEntry("localhost");
                //IPAddress ipAddress = host.AddressList[0];
                //IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = Connecter.StartConnection();
                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    Log.Content = Log.Content + "Socket connected to { 0}" + Connecter.connect(sender);


                    Connecter.SendData(sender, "Request");

                    // Receive the response from the remote device.

                    
                    String stats= Connecter.recieveData(sender);
                    Log.Content = Log.Content+ "\nEchoed test = {0}"+ stats;

                    DungeonManager.Content= String.Concat(stats[0], stats[1]);
                    KillManager.Content=String.Concat(stats[3], stats[4]);
                    EncounterManager.Content = String.Concat(stats[6],stats[7]);


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
           //Log.Content = Log.Content + "\ntest";
           LogManager = Log;
        }

        private void End_connection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Release the socket.
                senderManager.Shutdown(SocketShutdown.Both);
                senderManager.Close();
            }
            catch (Exception ex)
            {
                LogManager.Content=Log.Content + "/n" +ex.ToString();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Encounters_count_Initialized(object sender, EventArgs e)
        {
            Encounters_count.Content = 0;
            EncounterManager = Encounters_count;
        }

        private void Kill_count_Initialized(object sender, EventArgs e)
        {
            Kill_count.Content = 0;
            KillManager = Kill_count;
        }

        private void Dungeon_count_Initialized(object sender, EventArgs e)
        {
            Dungeon_count.Content = 0;
            DungeonManager = Dungeon_count; //if both objects can be mirrored what happens to one happens to another
        }
    }
}
