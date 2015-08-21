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
using System.Drawing;


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
        private StimulyWindow stimulyWindow;
       
        public MainWindow()
        {
            InitializeComponent();
            service = new Service(this);
            Graphics g;
                   
            
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

        public void OnNext(WiimoteInfo remote)
        {

            bool updated = false;

            //The update method.
            if (remote == service.wiimote1_info)
            {
                Action action = () =>
                {
                    WM1Status_Lbl.Content = remote.Astatus.ToString();
                    WM1_Accel_X.Content = remote.Accelerometer[0].ToString();
                    WM1_Accel_Y.Content = remote.Accelerometer[1].ToString();
                    WM1_Accel_Z.Content = remote.Accelerometer[2].ToString();
                    WM1_IR1.Content = String.Concat(" X = ", remote.IRState[0, 0]/10, "; Y = ", remote.IRState[0, 1]/10, "; Size = ", remote.IRState[0, 2]);
                    WM1_IR2.Content = String.Concat(" X = ", remote.IRState[1, 0]/10, "; Y = ", remote.IRState[1, 1]/10, "; Size = ", remote.IRState[1, 2]);
                    WM1_IR3.Content = String.Concat(" X = ", remote.IRState[2, 0]/10, "; Y = ", remote.IRState[2, 1]/10, "; Size = ", remote.IRState[2, 2]);
                    WM1_IR4.Content = String.Concat(" X = ", remote.IRState[3, 0]/10, "; Y = ", remote.IRState[3, 1]/10, "; Size = ", remote.IRState[3, 2]);
                    wm1_progressbar.Value = remote.battery;
                   
                    Canvas.SetLeft(IRSensor11, remote.IRState[0, 0]); Canvas.SetTop(IRSensor11, remote.IRState[0, 1]);
                    Canvas.SetLeft(IRSensor12, remote.IRState[1, 0]); Canvas.SetTop(IRSensor12, remote.IRState[1, 1]);
                    Canvas.SetLeft(IRSensor13, remote.IRState[2, 0]); Canvas.SetTop(IRSensor13, remote.IRState[2, 1]);
                    Canvas.SetLeft(IRSensor14, remote.IRState[3, 0]); Canvas.SetTop(IRSensor14, remote.IRState[3, 1]);
                    Canvas.SetLeft(MidPoint1, remote.IRState[4, 0]); Canvas.SetTop(MidPoint1, remote.IRState[4, 1]);
                   
                };
                Dispatcher.Invoke(action);
                updated = true;
            }
            else if ( remote == service.wiimote2_info)
            {
                Action action = () =>
            {
                WM2Status_Lbl.Content = remote.Astatus.ToString();
                WM2_Accel_X.Content = remote.Accelerometer[0].ToString();
                WM2_Accel_Y.Content = remote.Accelerometer[1].ToString();
                WM2_Accel_Z.Content = remote.Accelerometer[2].ToString();
                WM2_IR1.Content = String.Concat(" X = ", remote.IRState[0, 0], "; Y = ", remote.IRState[0, 1], "; Size = ", remote.IRState[0, 2]);
                WM2_IR2.Content = String.Concat(" X = ", remote.IRState[1, 0], "; Y = ", remote.IRState[1, 1], "; Size = ", remote.IRState[1, 2]);
                WM2_IR3.Content = String.Concat(" X = ", remote.IRState[2, 0], "; Y = ", remote.IRState[2, 1], "; Size = ", remote.IRState[2, 2]);
                WM2_IR4.Content = String.Concat(" X = ", remote.IRState[3, 0], "; Y = ", remote.IRState[3, 1], "; Size = ", remote.IRState[3, 2]);
                // WM2_Path_Lbl.Content = remote.path.ToString();
                Canvas.SetLeft(IRSensor21, remote.IRState[0, 0]); Canvas.SetTop(IRSensor21, remote.IRState[0, 1]);
                Canvas.SetLeft(IRSensor22, remote.IRState[1, 0]); Canvas.SetTop(IRSensor22, remote.IRState[1, 1]);
                Canvas.SetLeft(IRSensor23, remote.IRState[2, 0]); Canvas.SetTop(IRSensor23, remote.IRState[2, 1]);
                Canvas.SetLeft(IRSensor24, remote.IRState[3, 0]); Canvas.SetTop(IRSensor24, remote.IRState[3, 1]);
                Canvas.SetLeft(MidPoint2, remote.IRState[4, 0]); Canvas.SetTop(MidPoint2, remote.IRState[4, 1]);
                wm2_progressbar.Value = remote.battery;
                
            };
                Dispatcher.Invoke(action);
                updated = true;
            }

            Console.WriteLine(" Updated =" + updated);

            }

        public void DrawSensor(System.Drawing.Color color , double x , double y , int Size )
        {
            Ellipse sensor = new Ellipse();
            sensor.Width = Size + 1;
            sensor.Height = Size + 1;
            sensor.Opacity = 100;
            Canvas.SetTop(sensor , x);
            //Canvas.SetBottom(sensor , y);
            Canvas.SetLeft(sensor, y);
            IRImage.Children.Add(sensor);
            Console.WriteLine("Point drawn");
          
        }

        private void IRForm_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
