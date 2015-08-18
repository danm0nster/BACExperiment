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
    public partial class MainWindow : Window , IObserver<WiimoteInfo>
    {
        /* Declare 2 wiimotes for the application to use. The number of remotes can be changed if we need too.
        For now I am keeping in mind the necesity we have . I also add a WiimoteStatus user controll for each 
        of the wii remotes. That way we can store the methods of updating the gui with each change the wii remote
        has , without too much hassle and with a better idea of where everything happens .*/

        /* Wiimote wm1;
         WiimoteInfo wm1_info;
         Wiimote wm2;
         WiimoteInfo wm2_info; */

        /* Wiiremotes were moved in the service class for architecturall efficiency issues */

        private Service service;
       
        public MainWindow()
        {
            InitializeComponent();
            service = new Service();
            
        }

        



        /*
      In this class will reside most of the code that will have a active roll in the recording and calling of more complex methods.
      First step is to input the code that will call the FileHandler class and will have a Wiimote instance in memory prepared for use.
      Because I am making use of litterate programming and explaining the logic behind all this , I will declare the variables and methods 
      as I explain the thaught process.
       
      First thing to do is declare the containers for the wii remotes 
            
            */






        private void WM1_Detect_Click(object sender, RoutedEventArgs e)
        {
            try {
                service.SetUpWiimoteInfo(service.wiimote1_info);
                Console.WriteLine("WM1_Detect_Click");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //Wiimotes might be connected . 

            // set report type to return the IR sensor and accelerometer data . Buttons always come
            // in which case we might get a false positive.
        }

        private void WM1_Disconect_Click(object sender, RoutedEventArgs e)
        {
            service.DisconnectWiimoteFromInfo(service.wiimote1_info);
            Console.WriteLine("Wiimote 1 has been disconected ;");
        }

        private void WM2_Detect_Click(object sender, RoutedEventArgs e)
        {

            service.SetUpWiimoteInfo(service.wiimote2_info);
            Console.WriteLine("WM2_Detect_Click");
        }

        private void WM2_Disconect_Click(object sender, RoutedEventArgs e)
        {
            service.DisconnectWiimoteFromInfo(service.wiimote2_info);
            Console.WriteLine("Wiimote 2 has been disconected ;");
        }

        // Interface for the Observer pattern

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(WiimoteInfo value)
        {
            throw new NotImplementedException();
        }


    }
}
