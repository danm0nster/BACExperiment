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

        //Volume bar variables

        public MainWindow()
        {
            InitializeComponent();
            MainWindowDataContext dataContext = new MainWindowDataContext();
            pathTxt.DataContext = dataContext;
            service = Service.getInstance(this);
            ModeSelect.Items.Add(new ComboboxItem("Ellipse", "Ellipse"));
            ModeSelect.Items.Add(new ComboboxItem("Course", "Course"));
            ModeSelect.Items.Add(new ComboboxItem("Pipe", "Pipe"));
            Mic1_VolumeBar.DataContext = this;
            Mic2_VolumeBar.DataContext = this;

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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            foreach(var mic in service.getMicrophoneList())
            {
                Microphone1_ComboBox.Items.Add(mic);
                Microphone2_ComboBox.Items.Add(mic);
            }
            Microphone1_ComboBox.DisplayMemberPath = "ProductName";
            Microphone2_ComboBox.DisplayMemberPath = "ProductName";
            
        }



        #region MainCourseOfActionCode
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = false;
            complexitySlider.IsEnabled = false;
            SpeedSlider.IsEnabled = false;
            ReqFrequencySlider.IsEnabled = false;
            stimulyWindow.startCourse();
            stimulyWindow.StartSendingInfo();
            stimulyWindow.startRecording();

            StartFullRecording();

        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                stimulyWindow = StimulyWindow.GetInstance(this);
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

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void enableStartBtn()
        {
            StartBtn.IsEnabled = true;
        }

        #endregion

        #region PrompterMenuCode
        private void prompterOpen_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(pathTxt.Text))
            {
                prompter = new Prompter((int)prompterSpeed.Value, (int)TextSizeSlider.Value, pathTxt.Text);
                prompter.Visibility = System.Windows.Visibility.Visible;
                prompterPlay.IsEnabled = true;
                prompterPause.IsEnabled = true;
                prompterStop.IsEnabled = true;
            }

            else
            {
                PopUp msg = new PopUp("No file at specified path .");
                msg.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void prompterPlay_Click(object sender, RoutedEventArgs e)
        {
            prompter.play();
        }

        private void prompterPause_Click(object sender, RoutedEventArgs e)
        {
            prompter.pause();
        }

        private void prompterStop_Click(object sender, RoutedEventArgs e)
        {
            prompter.stop();
        }

        private void TextSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (prompter != null)
                prompter.textSize_Changed((int)TextSizeSlider.Value);
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
        #endregion

        #region WiiMenuCode

        private void WM1_Detect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                service.ConnectWiimoteToInfo(0);
                WM1_Detect.IsEnabled = false;
                WM1_Disconect.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void WM1_Disconect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                service.DisconnectWiimoteFromInfo(0);
                Console.WriteLine("Wiimote 1 has been disconected ;");
                WM1_Detect.IsEnabled = true;
                WM1_Disconect.IsEnabled = false;
                ClearAllLabels(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void WM2_Detect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                service.ConnectWiimoteToInfo(1);
                WM2_Detect.IsEnabled = false;
                WM2_Disconect.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void WM2_Disconect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                service.DisconnectWiimoteFromInfo(1);
                Console.WriteLine("Wiimote 2 has been disconected ;");
                WM2_Detect.IsEnabled = true;
                WM2_Disconect.IsEnabled = false;
                await ClearAllLabels(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task OnNext(WiimoteInfo remote, Wiimote wm)
        {

            //The update method.
            if (wm.WiimoteState.LEDState.LED1 == true)
            {
                t1 = updateWM1Labels(wm);
                await t1;
            }
            else if (wm.WiimoteState.LEDState.LED2 == true)
            {
                t2 = updateWM2Labels(wm);
                await t2;
            }
        }

        public async Task OnNext2(WiimoteInfo remote, Wiimote wm)
        {
            if (wm.ID.Equals(remote.mWiimotes[1].ID))
            {
                t2 = updateWM2Labels(wm);
                await t2;

            }
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
                    Canvas.SetRight(IRSensor11, wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor11, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y / 5);
                    IRSensor11.Visibility = System.Windows.Visibility.Visible;
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
                    IRSensor12.Visibility = System.Windows.Visibility.Visible;
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
                    IRSensor13.Visibility = System.Windows.Visibility.Visible;
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
                    IRSensor14.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor14.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[0].Found && wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    MidPoint1.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(MidPoint1, wm.WiimoteState.IRState.RawMidpoint.X / 10 * 3);
                    Canvas.SetTop(MidPoint1, wm.WiimoteState.IRState.RawMidpoint.Y / 5);
                    MidPoint1.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    MidPoint1.Visibility = System.Windows.Visibility.Hidden;
                }

                if (stimulyWindow != null)
                {
                    if (wm.WiimoteState.IRState.IRSensors[0].Found && wm.WiimoteState.IRState.IRSensors[1].Found)
                    {
                        stimulyWindow.movePointer1(wm.WiimoteState.IRState.IRSensors[0].RawPosition.X, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y);
                        stimulyWindow.ShowPointer1();
                    }

                }
            };
            try
            {
                Dispatcher.Invoke(action);
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
                    Canvas.SetRight(IRSensor21, wm.WiimoteState.IRState.IRSensors[0].RawPosition.X);
                    Canvas.SetTop(IRSensor21, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y);
                    IRSensor21.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor21.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    IRSensor22.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor22, wm.WiimoteState.IRState.IRSensors[1].RawPosition.X);
                    Canvas.SetTop(IRSensor22, wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y);
                    IRSensor22.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor22.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[2].Found)
                {
                    IRSensor23.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor23, wm.WiimoteState.IRState.IRSensors[2].RawPosition.X);
                    Canvas.SetTop(IRSensor23, wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y);
                    IRSensor23.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor23.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[3].Found)
                {
                    IRSensor24.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor24, wm.WiimoteState.IRState.IRSensors[3].RawPosition.X);
                    Canvas.SetTop(IRSensor24, wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y);
                    IRSensor24.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor24.Visibility = System.Windows.Visibility.Hidden;
                }

                if (wm.WiimoteState.IRState.IRSensors[0].Found && wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    MidPoint2.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(MidPoint2, wm.WiimoteState.IRState.RawMidpoint.X / 10 * 3);
                    Canvas.SetTop(MidPoint2, wm.WiimoteState.IRState.RawMidpoint.Y / 5);
                }
                else
                {
                    MidPoint2.Visibility = System.Windows.Visibility.Hidden;
                }
                if (stimulyWindow != null)
                {
                    if (wm.WiimoteState.IRState.IRSensors[0].Found && wm.WiimoteState.IRState.IRSensors[1].Found)
                    {
                        stimulyWindow.movePointer2(wm.WiimoteState.IRState.IRSensors[0].RawPosition.X, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y);
                        stimulyWindow.ShowPointer2();
                    }

                }
            };

            try
            {
                Dispatcher.Invoke(action);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task ClearAllLabels(int i)
        {
            if (i == 1)
            {
                Action action = () =>
                {


                    WM1Status_Lbl.Content = "";
                    WM1_Accel_X.Content = "";
                    WM1_Accel_Y.Content = "";
                    WM1_Accel_Z.Content = "";
                    WM1_IR1.Content = "";
                    WM1_IR2.Content = "";
                    WM1_IR3.Content = "";
                    WM1_IR4.Content = "";
                    wm1_progressbar.Value = 0;


                    // Have to find a way to delay execution of this since in the first ever call there are no . Did not delay execution , instead made if statement in animate
                    // sensor that tests for NaN value 

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

            else if (i == 2)
            {
                Action action = () =>
                {


                    WM2Status_Lbl.Content = "";
                    WM2_Accel_X.Content = "";
                    WM2_Accel_Y.Content = "";
                    WM2_Accel_Z.Content = "";
                    WM2_IR1.Content = "";
                    WM2_IR2.Content = "";
                    WM2_IR3.Content = "";
                    WM2_IR4.Content = "";
                    wm2_progressbar.Value = 0;


                    // Have to find a way to delay execution of this since in the first ever call there are no . Did not delay execution , instead made if statement in animate
                    // sensor that tests for NaN value 

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
        }

        private void ConnectAll_OnClick(object sender, RoutedEventArgs e)
        {
            service.ConnectAllWiimotes();
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            Console_TextBox.Inlines.Add("Searching for wii remotes.....");
            service.DetectWiimotes();
            WM1_Detect.IsEnabled = true;
            WM2_Detect.IsEnabled = true;
            Console_TextBox.Inlines.Add(string.Format("Found {0} wiimotes ", service.GetWiimoteInfo().count));
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            Console_TextBox.Inlines.Add("Disconecting wii remotes .....");
            service.DisconnectAllWiimotes();
            Console_TextBox.Inlines.Add("Wiiremotes disconected;");
        }

        public void WriteToRemoteMenu(int index, string message)
        {

            Console_TextBox.Inlines.Add(string.Format("Wiimote {0}:{1}; \r\n", index, message));// append text to console log 1


        }

        #endregion

        #region MicrophoneCode
        private void Microphone1_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            service.ListenToMicrophone( ((ComboBox)sender).SelectedIndex , 1);
            Mic1_Rec.IsEnabled = true;
        }

        private void Microphone2_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            service.ListenToMicrophone( ((ComboBox)sender).SelectedIndex , 2);
            Mic2_Rec.IsEnabled = true;
        }


        internal void UpdateVolumeBar( int v1,  float v2)
        {
            if (v1 == 1)
                Mic1_VolumeBar.Value = v2*100;
            else if (v1 == 2)
                Mic2_VolumeBar.Value = v2*100;
            else
            {
                Console.WriteLine(" Volume Bar not available for the current index.");
            }
        }
 

        private void Mic1_Rec_Click(object sender, RoutedEventArgs e)
        {
            service.startRecording(1);
            Mic1_Rec.IsEnabled = false;
            Mic1_Stop.IsEnabled = true;
        }

        private void Mic2_Rec_Click(object sender, RoutedEventArgs e)
        {
            service.startRecording(2);
            Mic2_Rec.IsEnabled = false;
            Mic2_Stop.IsEnabled = true;
        }



        private void Mic1_Stop_Click(object sender, RoutedEventArgs e)
        {
            service.stopRecording(1);
            Mic1_Rec.IsEnabled = true;
            Mic1_Stop.IsEnabled = false;
        }
        

        private void Mic2_Stop_Click(object sender, RoutedEventArgs e)
        {
            service.stopRecording(2);
            Mic2_Rec.IsEnabled = true;
            Mic2_Stop.IsEnabled = false;
        }

        private void StartFullRecording()
        {
            if (Microphone1_ComboBox.SelectedIndex != -1)
            {
                service.startRecording(1);
                Mic1_Rec.IsEnabled = false;
                Mic1_Stop.IsEnabled = true;
            }
            if (Microphone2_ComboBox.SelectedIndex != -1)
            {
                service.startRecording(2);
                Mic2_Rec.IsEnabled = false;
                Mic2_Stop.IsEnabled = true;
            }
        }

        public void StopFullRecording()
        {
            if (Microphone1_ComboBox.SelectedIndex != -1)
            {
                service.stopRecording(1);
                Mic1_Rec.IsEnabled = true;
                Mic1_Stop.IsEnabled = false;
            }
            if (Microphone2_ComboBox.SelectedIndex != -1)
            {
                service.stopRecording(2);
                Mic2_Rec.IsEnabled = true;
                Mic2_Stop.IsEnabled = false;
            }
        }


        #endregion

        private void OpenWiimoteWindow_Click(object sender, RoutedEventArgs e)
        {
            WiiremoteWindow window = new WiiremoteWindow();
            window.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
