using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Drawing;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;

namespace Serial_Communication_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region variables
        //Richtextbox
        FlowDocument mcFlowDoc = new FlowDocument();
        Paragraph para = new Paragraph();
        //Serial 
        SerialPort serial = new SerialPort();
        string recieved_data;
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            //overwite to ensure state
            Connect_btn.Content = "Connect";
        }

        private void Connect_Comms(object sender, RoutedEventArgs e)
        {
            if (Connect_btn.Content == "Connect")
            {
                //Sets up serial port
                serial.PortName = "COM3";
                serial.BaudRate = 9600;
                serial.Handshake = System.IO.Ports.Handshake.None;
                serial.Parity = Parity.None;
                serial.DataBits = 8;
                serial.StopBits = StopBits.Two;
                serial.ReadTimeout = 100;
                serial.WriteTimeout = 50;
                serial.Open();



                serial.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                // Collecting the characters received to our 'buffer' (string).



            }
            else
            {
                try // just in case serial port is not open could also be acheved using if(serial.IsOpen)
                {
                    serial.Close();
                    Connect_btn.Content = "Connect";
                }
                catch
                {
                }
            }
        }

        #region Recieving

        private delegate void UpdateUiTextDelegate(string text);
        public EventHandler g;
        int cb;
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(UpdateTextBox), indata);

        }

        //private delegate void UpdateTextBox(string text);

        private void UpdateTextBox(string text)
        {
            cb = Convert.ToInt32(text);
            RectangleLogic();
        }
        private void RectangleLogic()
        {
            if (cb == 0)
            {
                key1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Green);
                key2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
                key3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
                
            }
            else
            {
                key1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
                key2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
                key3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
            }
        }

        /*   private void WriteData(string text)
           {
               // Assign the value of the recieved_data to the RichTextBox.
               para.Inlines.Add(text);
               mcFlowDoc.Blocks.Add(para);
               Commdata.Document = mcFlowDoc;
           }
           */
        #endregion


        #region Sending

   /*     private void Send_Data(object sender, RoutedEventArgs e)
        {
            SerialCmdSend(SerialData.Text);
            SerialData.Text = "";
        } */
        public void SerialCmdSend(string data)
        {
            if (serial.IsOpen)
            {
                try
                {
                    // Send the binary data out the port
                    byte[] hexstring = Encoding.ASCII.GetBytes(data);
                    //There is a intermitant problem that I came across
                    //If I write more than one byte in succesion without a 
                    //delay the PIC i'm communicating with will Crash
                    //I expect this id due to PC timing issues ad they are
                    //not directley connected to the COM port the solution
                    //Is a ver small 1 millisecound delay between chracters
                    foreach (byte hexval in hexstring)
                    {
                        byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
                        serial.Write(_hexval, 0, 1);
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    para.Inlines.Add("Failed to SEND" + data + "\n" + ex + "\n");
                    mcFlowDoc.Blocks.Add(para);
                    //Commdata.Document = mcFlowDoc;
                }
            }
            else
            {
            }
        }

        #endregion


        #region Form Controls

        private void Close_Form(object sender, RoutedEventArgs e)
        {
            if (serial.IsOpen) serial.Close();
            this.Close();
        }
        private void Max_size(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }
        private void Min_size(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Minimized) this.WindowState = WindowState.Minimized;
            else this.WindowState = WindowState.Normal;
        }
        private void Move_Window(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}
