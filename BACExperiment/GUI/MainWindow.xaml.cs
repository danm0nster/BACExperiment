using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            service = Service.getInstance(this);     
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
                service.ConnectWiimoteToInfo(0);
                Console.WriteLine("WM1_Detect_Click");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        private void WM1_Disconect_Click(object sender, RoutedEventArgs e)
        {
            try {
                service.DisconnectWiimoteFromInfo(0);
                Console.WriteLine("Wiimote 1 has been disconected ;");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void WM2_Detect_Click(object sender, RoutedEventArgs e)
        {
            try {
                service.ConnectWiimoteToInfo(1);
                Console.WriteLine("WM2_Detect_Click");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void WM2_Disconect_Click(object sender, RoutedEventArgs e)
        {
            try {
                service.DisconnectWiimoteFromInfo(1);
                Console.WriteLine("Wiimote 2 has been disconected ;");
            }
            catch(Exception ex )
            {
                Console.WriteLine(ex.ToString());
            }
            }

        // Interface for the Observer pattern

        public string OnNext(WiimoteInfo remote , Wiimote wm)
        {

            //The update method.
            if (wm.Equals(remote.mWiimotes[0]))
            {
                Action action = () =>
                {
                    WM1Status_Lbl.Content = wm.WiimoteState.ButtonState.A.ToString();
                    WM1_Accel_X.Content = wm.WiimoteState.AccelState.Values.X.ToString();
                    WM1_Accel_Y.Content = wm.WiimoteState.AccelState.Values.Y.ToString();
                    WM1_Accel_Z.Content = wm.WiimoteState.AccelState.Values.Z.ToString();
                    WM1_IR1.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[0].Size + 1);
                    WM1_IR2.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[1].Size + 1);
                    WM1_IR3.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[2].Size + 1);
                    WM1_IR4.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[3].Size + 1);
                    wm1_progressbar.Value = remote.battery;


                    // Have to find a way to delay execution of this since in the first ever call there are no 
                    try {

                        TranslateTransform moveTo1 = new TranslateTransform();
                        moveTo1.X = wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10; ; moveTo1.Y = wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10;
                        IRSensor21.RenderTransform = moveTo1;
                        TranslateTransform moveTo2 = new TranslateTransform();
                        moveTo2.X = wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10; moveTo2.Y = wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10;
                        IRSensor22.RenderTransform = moveTo2;
                        TranslateTransform moveTo3 = new TranslateTransform();
                        moveTo3.X = wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10; moveTo3.Y = wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10;
                        IRSensor23.RenderTransform = moveTo3;
                        TranslateTransform moveTo4 = new TranslateTransform();
                        moveTo4.X = wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10; moveTo4.Y = wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10;
                        IRSensor24.RenderTransform = moveTo4;


                        /*
                        (IRSensor11.RenderTransform as TranslateTransform).X = wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10;
                        (IRSensor11.RenderTransform as TranslateTransform).Y = wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10;

                        (IRSensor12.RenderTransform as TranslateTransform).X = wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10;
                        (IRSensor12.RenderTransform as TranslateTransform).Y = wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10;

                        (IRSensor13.RenderTransform as TranslateTransform).X = wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10;
                        (IRSensor13.RenderTransform as TranslateTransform).Y = wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10;

                        (IRSensor14.RenderTransform as TranslateTransform).X = wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10;
                        (IRSensor14.RenderTransform as TranslateTransform).Y = wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10;

                        (MidPoint1.RenderTransform as TranslateTransform).X = wm.WiimoteState.IRState.RawMidpoint.X / 10;
                        (MidPoint1.RenderTransform as TranslateTransform).Y = wm.WiimoteState.IRState.RawMidpoint.Y / 10;

                        */

                        if (stimulyWindow != null)
                        {
                            try
                            {
                                stimulyWindow.moveElipse1ToCoordinate(new System.Windows.Point(wm.WiimoteState.IRState.RawMidpoint.X, wm.WiimoteState.IRState.RawMidpoint.Y));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    
                };
                

                Dispatcher.BeginInvoke(action);
            }
            else if (wm.Equals(remote.mWiimotes[1]))
            {
                Action action = () =>
            {
                WM2Status_Lbl.Content = wm.WiimoteState.ButtonState.A.ToString();
                WM2_Accel_X.Content = remote.Accelerometer[0].ToString();
                WM2_Accel_Y.Content = remote.Accelerometer[1].ToString();
                WM2_Accel_Z.Content = remote.Accelerometer[2].ToString();
                WM2_IR1.Content = String.Concat(" X = ", remote.IRState[0, 0], "; Y = ", remote.IRState[0, 1], "; Size = ", remote.IRState[0, 2]);
                WM2_IR2.Content = String.Concat(" X = ", remote.IRState[1, 0], "; Y = ", remote.IRState[1, 1], "; Size = ", remote.IRState[1, 2]);
                WM2_IR3.Content = String.Concat(" X = ", remote.IRState[2, 0], "; Y = ", remote.IRState[2, 1], "; Size = ", remote.IRState[2, 2]);
                WM2_IR4.Content = String.Concat(" X = ", remote.IRState[3, 0], "; Y = ", remote.IRState[3, 1], "; Size = ", remote.IRState[3, 2]);
                // WM2_Path_Lbl.Content = remote.path.ToString();

                TranslateTransform moveTo1 = new TranslateTransform();
                moveTo1.X = remote.IRState[0, 0]; moveTo1.Y = remote.IRState[0, 1];
                IRSensor21.RenderTransform = moveTo1;
                TranslateTransform moveTo2 = new TranslateTransform();
                moveTo2.X = remote.IRState[1, 0]; moveTo2.Y = remote.IRState[1, 1];
                IRSensor22.RenderTransform = moveTo2;
                TranslateTransform moveTo3 = new TranslateTransform();
                moveTo3.X = remote.IRState[2, 0]; moveTo3.Y = remote.IRState[2, 1];
                IRSensor23.RenderTransform = moveTo3;
                TranslateTransform moveTo4 = new TranslateTransform();
                moveTo4.X = remote.IRState[3, 0]; moveTo4.Y = remote.IRState[3, 1];
                IRSensor24.RenderTransform = moveTo4;

                /*
                Canvas.SetLeft(IRSensor21, remote.IRState[0, 0]); Canvas.SetTop(IRSensor21, remote.IRState[0, 1]);
                Canvas.SetLeft(IRSensor22, remote.IRState[1, 0]); Canvas.SetTop(IRSensor22, remote.IRState[1, 1]);
                Canvas.SetLeft(IRSensor23, remote.IRState[2, 0]); Canvas.SetTop(IRSensor23, remote.IRState[2, 1]);
                Canvas.SetLeft(IRSensor24, remote.IRState[3, 0]); Canvas.SetTop(IRSensor24, remote.IRState[3, 1]);
                Canvas.SetLeft(MidPoint2, remote.IRState[4, 0]); Canvas.SetTop(MidPoint2, remote.IRState[4, 1]);
                wm2_progressbar.Value = remote.battery;
                */

                if (stimulyWindow != null)
                {
                    try
                    {
                        stimulyWindow.moveElipse2ToCoordinate(new System.Windows.Point(remote.IRState[4, 0], remote.IRState[4, 1]));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }



            };
                Dispatcher.BeginInvoke(action);
              
             
            }



            return remote.ToString();

            }

        public void DrawSensor(System.Windows.Media.Color color, double x, double y, int Size)
        {
            Ellipse sensor = new Ellipse();
            sensor.Width = Size + 1;
            sensor.Height = Size + 1;
            sensor.Opacity = 100;
            sensor.Fill = new SolidColorBrush(color);
            Canvas.SetTop(sensor, x);
            //Canvas.SetBottom(sensor , y);
            Canvas.SetLeft(sensor, y);
            IRImage.Children.Add(sensor);
            Console.WriteLine("Point drawn");        
        }


        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            stimulyWindow = new StimulyWindow();
            stimulyWindow.Visibility = System.Windows.Visibility.Visible;
        }

        
    }
}
