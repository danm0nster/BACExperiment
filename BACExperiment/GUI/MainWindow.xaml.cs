using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
using WiimoteLib;

namespace BACExperiment
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* Declare 2 wiimotes for the application to use. The number of remotes can be changed if we need too.
        For now I am keeping in mind the necesity we have . I also add a WiimoteStatus user controll for each 
        of the wii remotes. That way we can store the methods of updating the gui with each change the wii remote
        has , without too much hassle and with a better idea of where everything happens .*/

        Wiimote wm1;
        WiimoteInfo wm1_info;
        Wiimote wm2;
        WiimoteInfo wm2_info;

        public MainWindow()
        {
            InitializeComponent();

            // Allow access to the wm1_info and wm2_info to each of their GroupBox 
            wm1_info = new WiimoteInfo(wm1_progressbar, WM1Status_Lbl, WM1_Path_Lbl , WM1_Accel_X , WM1_Accel_Y , WM1_Accel_Z , WM1_IR1 ,WM1_IR2 , WM1_IR3 , WM1_IR4 , Console_Textbox_WM1);
            wm2_info = new WiimoteInfo(wm2_progressbar, WM2Status_Lbl, WM2_Path_Lbl, WM2_Accel_X, WM2_Accel_Y, WM2_Accel_Z, WM1_IR1, WM2_IR2, WM2_IR3, WM2_IR4, Console_Textbox_WM2);
        }

      

        /*
      In this class will reside most of the code that will have a active roll in the recording and calling of more complex methods.
      First step is to input the code that will call the FileHandler class and will have a Wiimote instance in memory prepared for use.
      Because I am making use of litterate programming and explaining the logic behind all this , I will declare the variables and methods 
      as I explain the thaught process.
       
      First thing to do is declare the containers for the wii remotes 
            
            */
       

      

        private void InitializeWiimote1()
        {

            // Initialize the wiimotes
            // Not shure if the connection between the wii remote and the wiimoteinfo should be done before or after connecting the wii remote.
        
            wm1 = new Wiimote();
            Console.WriteLine("Wiimote 1 has been initiated");
            Console_Textbox_WM1.AppendText("Wiimote 1 has been initiated\n");
            wm1_info.Wiimote = wm1;
            Console.WriteLine("Wiimote 1 has connected to it's container;");
            Console_Textbox_WM1.AppendText("Wiimote 1 has been initiated\n");


            //After initializing the wii remote we add the event change listener to each one of them
            wm1.WiimoteChanged += wm1_WiimoteChanged;
            Console.WriteLine("Wiimote 1 now has change listener;");
            Console_Textbox_WM1.AppendText("Wiimote 1 now has change listener\n");

           

            try {
                wm1.Connect();
                wm1.SetReportType(InputReport.IRAccel, true);
            }
            catch(WiimoteException ex)
            {
                Console.WriteLine(ex.ToString());
                Console_Textbox_WM1.AppendText(ex.ToString()+"\n");
            }
                /*
            We have the possibility with the WiimoteLib to add extentions to our programm such as
            the wii drum kit , wii nunchucks etc. for our experiment it is not currently neccesary. 
            */



        }
        private void InitializeWiimote2()
        {

            // Initialize the wiimotes
            // Not shure if the connection between the wii remote and the wiimoteinfo should be done before or after connecting the wii remote.

          
            wm2 = new Wiimote();
            Console.WriteLine("Wiimote 2 initialized;");
            wm2_info.Wiimote = wm2;
            Console.WriteLine("Wiimote 2 has connected to it's container;");

            //After initializing the wii remote we add the event change listener to each one of them
      
            wm2.WiimoteChanged += wm2_wiimoteChanged;
            Console.WriteLine("Wiimote 2 now has change listener;");

            wm2.Connect();
            wm2.SetReportType(InputReport.IRAccel, true);
            /*
            We have the possibility with the WiimoteLib to add extentions to our programm such as
            the wii drum kit , wii nunchucks etc. for our experiment it is not currently neccesary. 
            */



        }

        void wm1_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            // current state information
            WiimoteState ws = args.WiimoteState;

            // write out the state of the A button    
            Console.WriteLine(ws.IRState.Midpoint.ToString());
           
        }

        private void wm2_wiimoteChanged(object sender , WiimoteChangedEventArgs args)
        {
            // current state info
            WiimoteState ws = args.WiimoteState;
            // "A" button state
            Console.WriteLine(ws.ButtonState.A);
            WM1_Accel_X.Content = ws.AccelState.Values.X.ToString();
            WM1_Accel_Y.Content = ws.AccelState.Values.X.ToString();

        }

        private void WM1_Detect_Click(object sender, RoutedEventArgs e)
        {
            InitializeWiimote1();
            Console.WriteLine("WM1_Detect_Click");
            //Wiimotes might be connected . 

            // set report type to return the IR sensor and accelerometer data . Buttons always come
            // in which case we might get a false positive.
        }

        private void WM1_Disconect_Click(object sender, RoutedEventArgs e)
        {
            wm1.Disconnect();
            Console.WriteLine("Wiimote 1 has been disconected ;");
        }

        private void WM2_Detect_Click(object sender, RoutedEventArgs e)
        {

            InitializeWiimote2();
            Console.WriteLine("WM2_Detect_Click");
        }

        private void WM2_Disconect_Click(object sender, RoutedEventArgs e)
        {
            wm2.Disconnect();
            Console.WriteLine("Wiimote 2 has been disconected ;");
        }

       
    }
}
