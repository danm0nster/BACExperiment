using BACExperiment.GUI;
using BACExperiment.Model;
using NAudio.Mixer;
using NAudio.Wave;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WiimoteLib;
using Xceed.Wpf.Toolkit;

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
       

        public MainWindow()
        {
           
            
            InitializeComponent();
            
               
            

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


            service = Service.getInstance(this);
            ModeSelect.Items.Add(new ComboboxItem("Ellipse", "Ellipse"));
            ModeSelect.Items.Add(new ComboboxItem("Course", "Course"));
            ModeSelect.Items.Add(new ComboboxItem("Pipe", "Pipe"));
            Mic1_VolumeBar.DataContext = this;
            Mic2_VolumeBar.DataContext = this;

            service.getMicrophoneList();
            WM1_groupbox.DataContext = service.wm1_data_context;
            WM2_groupbox.DataContext = service.wm2_data_context;


            Mic1_VolumeBar.DataContext = service.mic_data_context;
            Microphone1_ComboBox.ItemsSource = service.mic_data_context.Mics;
            Mic2_VolumeBar.DataContext = service.mic_data_context;
            Microphone2_ComboBox.ItemsSource = service.mic_data_context.Mics;
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }


            public ComboboxItem(string Text, string Value)
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
        }



        #region MainCourseOfActionCode
      

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                stimulyWindow = StimulyWindow.GetInstance(this);
                stimulyWindow.Visibility = System.Windows.Visibility.Visible;
               
                // stimulyWindow.setCourseMode((int)ModeSelect.SelectedValue);
               
                stimulyWindow.Show();
            
                this.Cursor = Cursors.Arrow;
                GenerateCourseBtn.IsEnabled = true;
                OpenBtn.IsEnabled = false;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void GenerateCourse_Click(object sender, RoutedEventArgs e)
        {
            stimulyWindow.setCourseComplexity((int)complexitySlider.Value);
            stimulyWindow.setCourseSpeed((int)SpeedSlider.Value);
            stimulyWindow.setShowTrajectory((bool)TrajectoryCheck.IsChecked);
            stimulyWindow.ResizeMode = ResizeMode.NoResize;
            TrajectoryCheck.IsEnabled = false;
            stimulyWindow.buildCourseType1();
            StartBtn.IsEnabled = true;
            GenerateCourseBtn.IsEnabled = false;
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

            StartFullRecording();

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
                if (Sync_RBtn.IsChecked == true)
                {
                    try
                    {
                        System.Windows.Media.Color color1 = (System.Windows.Media.Color)Color1.SelectedColor;
                        prompter = new Prompter((int)TextSizeSlider.Value, pathTxt.Text, (int)SyncTraversalSpeed_Slider.Value, color1);
                    }
                    catch (Exception ex)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                    }
                }
                if (Async_RBtn.IsChecked == true)
                {
                    try
                    {
                        System.Windows.Media.Color color1 = (System.Windows.Media.Color)Color1.SelectedColor;
                        System.Windows.Media.Color color2 = (System.Windows.Media.Color)Color2.SelectedColor;
                        System.Windows.Media.Color color3 = (System.Windows.Media.Color)Color3.SelectedColor;
                        prompter = new Prompter((int)TextSizeSlider.Value, pathTxt.Text, (int)AsyncTraversalSpeed_Slider.Value,(int)Turn_duration_Slider.Value, (int)Switch_Frequency_Slider.Value, color1, color2, color3);
                    }
                    catch(Exception ex)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                    }
                }

                if (SelfPaced_RBtn.IsChecked == true)
                {
                    try
                    {
                        prompter = new Prompter((int)TextSizeSlider.Value, pathTxt.Text, (int)prompterSpeed.Value);
                    }
                    catch(Exception ex)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                    }
                }
                prompter.Visibility = System.Windows.Visibility.Visible;
                prompterPlay.IsEnabled = true;

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

        private void RandomizeBtn_Click(object sender, RoutedEventArgs e)
        {
            Sync_RBtn.IsChecked = false;
            Async_RBtn.IsChecked = false;
            SelfPaced_RBtn.IsChecked = false;

            Random r = new Random();
            int i = r.Next(3);

            if (i == 1)
                Sync_RBtn.IsChecked = true;
            if (i == 2)
                Async_RBtn.IsChecked = true;
            if (i == 0)
                SelfPaced_RBtn.IsChecked = true;
        }

        private void TextSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (prompter != null)
                prompter.textSize_Changed((int)TextSizeSlider.Value);
        }

        private void Sync_RBtn_Checked(object sender, RoutedEventArgs e)
        {
            prompterOpen.IsEnabled = true;
            Color1.IsEnabled = true;
            Color2.IsEnabled = false;
            Color3.IsEnabled = false;
        }

        private void Async_RBtn_Checked(object sender, RoutedEventArgs e)
        {
            prompterOpen.IsEnabled = true;
            Color1.IsEnabled = true;
            Color2.IsEnabled = true;
            Color3.IsEnabled = true;
        }
        private void SelfPaced_RBtn_Checked(object sender, RoutedEventArgs e)
        {
            prompterOpen.IsEnabled = true;
            Color1.IsEnabled = false;
            Color2.IsEnabled = false;
            Color3.IsEnabled = false;
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

        private void WM2_Disconect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                service.DisconnectWiimoteFromInfo(1);
                Console.WriteLine("Wiimote 2 has been disconected ;");
                WM2_Detect.IsEnabled = true;
                WM2_Disconect.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
            Console_TextBox.Inlines.Add(string.Format("Found {0} wiimotes", service.GetRemoteCount()));
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

        MicrophoneConstruct mic1PrevVal = null;
        MicrophoneConstruct mic2PrevVal = null;

        private void Microphone1_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (((ComboBox)sender).SelectedIndex > -1)
                {
                    if (mic1PrevVal != null) service.stopRecording(mic1PrevVal);

                    bool active = service.ListenToMicrophone(((ComboBox)sender).SelectedIndex, 1);
                    Mic1_Rec.IsEnabled = true;
                    mic1PrevVal = (MicrophoneConstruct)((ComboBox)sender).SelectedValue;
                }

            }
            catch (Exception Ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(Ex.Message);
                Microphone1_ComboBox.SelectedIndex = -1;
            }
        }

        private void Microphone2_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((ComboBox)sender).SelectedIndex > -1)
                {
                    if (mic2PrevVal != null) service.stopRecording(mic2PrevVal);
                    bool active = service.ListenToMicrophone(((ComboBox)sender).SelectedIndex, 2);
                    Mic2_Rec.IsEnabled = true;
                }

                mic2PrevVal = (MicrophoneConstruct)((ComboBox)sender).SelectedValue;
            }
            catch (Exception Ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(Ex.Message);
                Microphone2_ComboBox.SelectedIndex = -1;
            }
        }




        private void Mic1_Rec_Click(object sender, RoutedEventArgs e)
        {

            MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone1_ComboBox.SelectedItem);
            service.startRecording(mic);
            Mic1_Rec.IsEnabled = false;
            Mic1_Stop.IsEnabled = true;
        }

        private void Mic2_Rec_Click(object sender, RoutedEventArgs e)
        {
            MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone2_ComboBox.SelectedItem);
            service.startRecording(mic);
            Mic2_Rec.IsEnabled = false;
            Mic2_Stop.IsEnabled = true;
        }



        private void Mic1_Stop_Click(object sender, RoutedEventArgs e)
        {
            MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone1_ComboBox.SelectedItem);
            service.stopRecording(mic);
            Mic1_Rec.IsEnabled = true;
            Mic1_Stop.IsEnabled = false;
        }


        private void Mic2_Stop_Click(object sender, RoutedEventArgs e)
        {
            MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone1_ComboBox.SelectedItem);
            service.stopRecording(mic);
            Mic2_Rec.IsEnabled = true;
            Mic2_Stop.IsEnabled = false;
        }

        private void StartFullRecording()
        {
            if (Microphone1_ComboBox.SelectedIndex != -1)
            {
                MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone1_ComboBox.SelectedItem);
                service.startRecording(mic);
                Mic1_Rec.IsEnabled = false;
                Mic1_Stop.IsEnabled = true;
            }
            if (Microphone2_ComboBox.SelectedIndex != -1)
            {
                MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone2_ComboBox.SelectedItem);
                service.startRecording(mic);
                Mic2_Rec.IsEnabled = false;
                Mic2_Stop.IsEnabled = true;
            }
        }

        public void StopFullRecording()
        {
            if (Microphone1_ComboBox.SelectedIndex != -1)
            {
                MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone1_ComboBox.SelectedItem);
                service.stopRecording(mic);
                Mic1_Rec.IsEnabled = true;
                Mic1_Stop.IsEnabled = false;
            }
            if (Microphone2_ComboBox.SelectedIndex != -1)
            {
                MicrophoneConstruct mic = (MicrophoneConstruct)(Microphone2_ComboBox.SelectedItem);
                service.stopRecording(mic);
                Mic2_Rec.IsEnabled = true;
                Mic2_Stop.IsEnabled = false;
            }
        }

        private void Microphone_refresh_btn_Click(object sender, RoutedEventArgs e)
        { /*
            Microphone1_ComboBox.Items.Clear();
            Microphone2_ComboBox.Items.Clear();
            foreach (var mic in service.getMicrophoneList())
            {
                Microphone1_ComboBox.Items.Add(mic);
                Microphone2_ComboBox.Items.Add(mic);
            }
            Microphone1_ComboBox.DisplayMemberPath = "ProductName";
            Microphone2_ComboBox.DisplayMemberPath = "ProductName";
         */
            service.getMicrophoneList();
        }


        private void VolumeSlider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            service.setVolume((int)e.NewValue, 1);
        }

        private void VolumeSlider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            service.setVolume((int)e.NewValue, 2);

            #endregion


        }

        private void SelfPaced_RBtn_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
