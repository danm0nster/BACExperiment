using BACExperiment.GUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        Storyboard storyBoard;


        public MainWindow()
        {
            InitializeComponent();
            service = Service.getInstance(this);
            ModeSelect.Items.Add(new ComboboxItem("Ellipse" , "Ellipse"));
            ModeSelect.Items.Add(new ComboboxItem("Course", "Course"));
            ModeSelect.Items.Add(new ComboboxItem("Pipe", "Pipe"));
            storyBoard = new Storyboard();
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
            if (wm.ID.Equals(remote.mWiimotes[0].ID))
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
                    wm1_progressbar.Value = wm.WiimoteState.Battery;


                // Have to find a way to delay execution of this since in the first ever call there are no . Did not delay execution , instead made if statement in animate
                // sensor that tests for NaN value .




                try {
                    if (Double.IsNaN(wm.WiimoteState.IRState.IRSensors[0].RawPosition.X) || Double.IsNaN(wm.WiimoteState.IRState.IRSensors[0].RawPosition.X))
                    {
                        DoubleAnimation animMoveX = new DoubleAnimation();
                        animMoveX.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
                        animMoveX.To = wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10;
                        Storyboard.SetTarget(animMoveX, IRSensor11);
                        Storyboard.SetTargetProperty(animMoveX, new PropertyPath(Canvas.LeftProperty));

                        DoubleAnimation animMoveY = new DoubleAnimation();
                        animMoveY.Duration = new Duration(TimeSpan.FromMilliseconds(10));
                        animMoveY.To = wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y / 10;
                        Storyboard.SetTarget(animMoveY, IRSensor11);
                        Storyboard.SetTargetProperty(animMoveY, new PropertyPath(Canvas.TopProperty));

                        storyBoard.Children.Add(animMoveX);
                        storyBoard.Children.Add(animMoveY);

                        storyBoard.Begin();
                    }
                   
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }           

            };

                Dispatcher.BeginInvoke(action);             
            }
            else if (wm.ID.Equals(remote.mWiimotes[1].ID))
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

                AnimateSensor(wm.WiimoteState.IRState.IRSensors[0].RawPosition.X, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y, IRSensor11);
                AnimateSensor(wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10, wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y / 10, IRSensor12);
                AnimateSensor(wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10, wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y / 10, IRSensor13);
                AnimateSensor(wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10, wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y / 10, IRSensor14);

            };
                Dispatcher.BeginInvoke(action);
              
             
            }



            return remote.ToString();

            }

        //Method freezes the whole GUI .
        public void AnimateSensor(double x, double y, Ellipse sensor)
        {
            if (Double.IsNaN(x) || Double.IsNaN(y))
            {
                DoubleAnimation animX = new DoubleAnimation();
                animX.To = x;
                animX.Duration = new Duration(TimeSpan.FromMilliseconds(10));
                DoubleAnimation animY = new DoubleAnimation();
                animY.To = y;
                animY.Duration = new Duration(TimeSpan.FromMilliseconds(10));
               

                sensor.BeginAnimation(Canvas.LeftProperty, animX);
                sensor.BeginAnimation(Canvas.TopProperty, animY);

                Console.WriteLine("Sensor animated");
             
            }
            else
                Console.WriteLine("x or y is NaN");
        }
        


        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
           
          
            StartBtn.IsEnabled = false;
            complexitySlider.IsEnabled = false;
            SpeedSlider.IsEnabled = false;
            stimulyWindow.startCourse();
            
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            stimulyWindow = StimulyWindow.getInstance(this);
            stimulyWindow.Visibility = System.Windows.Visibility.Visible;
            stimulyWindow.setCourseComplexity((int)complexitySlider.Value);
            stimulyWindow.setCourseSpeed((int)SpeedSlider.Value);
            // stimulyWindow.setCourseMode((int)ModeSelect.SelectedValue);
            StartBtn.IsEnabled = true;
            stimulyWindow.setShowTrajectory((bool)TrajectoryCheck.IsChecked);
            TrajectoryCheck.IsEnabled = false;

            stimulyWindow.buildCourseType1();
            this.Cursor = Cursors.Arrow;
            
        }

        public void enableStartBtn()
        {
            StartBtn.IsEnabled = true;
        }


        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }


            public ComboboxItem(string Text , string Value)
            {
                this.Text = Text;
                this.Value = Value;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        

        private void prompterOpen_Click(object sender, RoutedEventArgs e)
        {
            Prompter prompter = new Prompter();
            prompter.Visibility = System.Windows.Visibility.Visible ;
         }
    }
}
