using BACExperiment.GUI;
using System;
using System.Threading.Tasks;
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
        private Prompter prompter;
        private Task t1;
        private Task t2;

        public MainWindow()
        {
            InitializeComponent();
            service = Service.getInstance(this);
            ModeSelect.Items.Add(new ComboboxItem("Ellipse" , "Ellipse"));
            ModeSelect.Items.Add(new ComboboxItem("Course", "Course"));
            ModeSelect.Items.Add(new ComboboxItem("Pipe", "Pipe"));
            

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
                WM1_Disconect.IsEnabled = true;
                
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
                WM2_Disconect.IsEnabled = true;
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




        public async Task OnNext(WiimoteInfo remote, Wiimote wm)
        {

            //The update method.
            if (wm.ID.Equals(remote.mWiimotes[0].ID))
            {
                t1 = updateWM1Labels(wm);
                await t1;

            }
            else if (wm.ID.Equals(remote.mWiimotes[1].ID))
            {
                t2 = updateWM2Labels(wm);
                await t2;

            }
        }

        // Thinking if should make a class with all the values of the guy where to store them and then just bind the guy values to that data . 
        public void WriteToRemoteMenu(int index, int message)
        {
            if (index == 1)
                throw NotImplementedException;// append text to console log 1
                if (index == 2)
                    // append text to console log 2
                throw NotImplementedException;
        }
        
        

        
        private async Task updateWM1Labels(Wiimote wm)
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



                if (wm.WiimoteState.IRState.IRSensors[0].Found)
                {
                    IRSensor11.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor11, wm.WiimoteState.IRState.IRSensors[0].RawPosition.X/10 * 3 );
                    Canvas.SetTop(IRSensor11, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y /5 );
                }
                else
                {
                    IRSensor11.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    IRSensor12.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor12, wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor12, wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y / 5);
                }
                else
                {
                    IRSensor12.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[2].Found)
                {
                    IRSensor13.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor13, wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor13, wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y / 5);
                }
                else
                {
                    IRSensor13.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[3].Found)
                {
                    IRSensor14.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor14, wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor14, wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y / 5);
                }
                else
                {
                    IRSensor14.Visibility = System.Windows.Visibility.Hidden;
                }
                if(wm.WiimoteState.IRState.IRSensors[0].Found && wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    MidPoint1.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(MidPoint1, wm.WiimoteState.IRState.RawMidpoint.X / 10 * 3);
                    Canvas.SetTop(MidPoint1, wm.WiimoteState.IRState.RawMidpoint.Y / 5);
                }
                else
                {
                    MidPoint1.Visibility = System.Windows.Visibility.Hidden;
                }
                if (stimulyWindow != null)
                {
                    stimulyWindow.movePointer1(wm.WiimoteState.IRState.RawMidpoint.X, wm.WiimoteState.IRState.RawMidpoint.Y);


                    stimulyWindow.Pointer1.Visibility = System.Windows.Visibility.Visible;
                }
                };
            try
            {
                await Dispatcher.BeginInvoke(action);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

       
        private async Task updateWM2Labels(Wiimote wm)
        {
            Action action = () =>
            {
                
                WM2Status_Lbl.Content = wm.WiimoteState.ButtonState.A.ToString();
                WM2_Accel_X.Content = wm.WiimoteState.AccelState.Values.X.ToString();
                WM2_Accel_Y.Content = wm.WiimoteState.AccelState.Values.Y.ToString();
                WM2_Accel_Z.Content = wm.WiimoteState.AccelState.Values.Z.ToString();
                WM2_IR1.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[0].Size + 1);
                WM2_IR2.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[1].Size + 1);
                WM2_IR3.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[2].Size + 1);
                WM2_IR4.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[3].Size + 1);
                wm2_progressbar.Value = wm.WiimoteState.Battery;


                if (wm.WiimoteState.IRState.IRSensors[0].Found)
                {
                    IRSensor21.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetLeft(IRSensor21, wm.WiimoteState.IRState.IRSensors[0].RawPosition.X);
                    Canvas.SetTop(IRSensor21, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y);
                }
                else
                {
                    IRSensor21.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    IRSensor22.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetLeft(IRSensor22, wm.WiimoteState.IRState.IRSensors[1].RawPosition.X);
                    Canvas.SetTop(IRSensor22, wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y);
                }
                else
                {
                    IRSensor22.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[2].Found)
                {
                    IRSensor23.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetLeft(IRSensor23, wm.WiimoteState.IRState.IRSensors[2].RawPosition.X);
                    Canvas.SetTop(IRSensor23, wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y);
                }
                else
                {
                    IRSensor23.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[3].Found)
                {
                    IRSensor24.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetLeft(IRSensor24, wm.WiimoteState.IRState.IRSensors[3].RawPosition.X);
                    Canvas.SetTop(IRSensor24, wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y);
                }
                else
                {
                    IRSensor24.Visibility = System.Windows.Visibility.Hidden;
                }

                if (wm.WiimoteState.IRState.IRSensors[0].Found && wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    MidPoint2.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetLeft(MidPoint2, wm.WiimoteState.IRState.RawMidpoint.X / 10 * 3);
                    Canvas.SetTop(MidPoint2, wm.WiimoteState.IRState.RawMidpoint.Y / 5);
                }
                else
                {
                    MidPoint2.Visibility = System.Windows.Visibility.Hidden;
                }
            };
            if (stimulyWindow != null)
            {
         
                stimulyWindow.movePointer1(wm.WiimoteState.IRState.RawMidpoint.X, wm.WiimoteState.IRState.RawMidpoint.Y);
                stimulyWindow.Pointer1.Visibility = System.Windows.Visibility.Visible;
            }
            try
            {
                await Dispatcher.BeginInvoke(action);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {     
            StartBtn.IsEnabled = false;
            complexitySlider.IsEnabled = false;
            SpeedSlider.IsEnabled = false;
            ReqFrequencySlider.IsEnabled = false;
            stimulyWindow.startCourse();
            stimulyWindow.StartSendingInfo();
            stimulyWindow.startRecording();

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
            prompter = new Prompter((int)prompterSpeed.Value , (int)TextSizeSlider.Value , pathTxt.Text);
            prompter.Visibility = System.Windows.Visibility.Visible ;
            prompterPlay.IsEnabled = true;
            prompterPause.IsEnabled = true;
            prompterStop.IsEnabled = true;
          
         }

        private void prompterPlay_Click(object sender, RoutedEventArgs e)
        {
            prompter.play();
        }

     

       

        private void ControllerTab_Loaded(object sender, RoutedEventArgs e)
        {
            service.DetectWiimotes();
            WM1_Detect.IsEnabled = true;
            WM1_Detect.IsEnabled = true;
        }

        private void prompterPause_Click(object sender, RoutedEventArgs e)
        {
            prompter.pause();
        }

        private void prompterStop_Click(object sender, RoutedEventArgs e)
        {
            prompter.stop();
        }


        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = pathTxt.Text;


            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                pathTxt.Text = filename;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pathTxt.Text = System.IO.Path.GetDirectoryName(
                             System.Reflection.Assembly.GetExecutingAssembly().Location); 
        }

        private void TextSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (prompter != null)
                prompter.textSize_Changed((int)TextSizeSlider.Value);
        }

        private void ConnectAll_OnClick(object sender, RoutedEventArgs e)
        {
            service.ConnectAllWiimotes();
        }
    }
}
