using BACExperiment.GUI;
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
     

        //Volume bar variables

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

            WM1_groupbox.DataContext = service.wm1_data_context;
            WM2_groupbox.DataContext = service.wm2_data_context;

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
                if (Sync_RBtn.IsChecked == true)
                    prompter = new Prompter((int)prompterSpeed.Value, (int)Switch_Frequency_Slider.Value, (int)TextSizeSlider.Value, pathTxt.Text , 1);
                if(Async_RBtn.IsChecked == true)
                prompter = new Prompter((int)prompterSpeed.Value,(int)Switch_Frequency_Slider.Value , (int)TextSizeSlider.Value, pathTxt.Text, 2);
                prompter.Visibility = System.Windows.Visibility.Visible;
                prompterPlay.IsEnabled = true;
               // prompterPause.IsEnabled = true;
               // prompterStop.IsEnabled = true;
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

        

        private void TextSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (prompter != null)
                prompter.textSize_Changed((int)TextSizeSlider.Value);
        }

        private void Sync_RBtn_Checked(object sender, RoutedEventArgs e)
        {
            prompterOpen.IsEnabled = true;
        }

        private void Async_RBtn_Checked(object sender, RoutedEventArgs e)
        {
            prompterOpen.IsEnabled = true;
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
                Mic1_VolumeBar.Value = v1*100;
            else if (v1 == 2)
                Mic2_VolumeBar.Value = v2*100;
            else
            {
                Console.WriteLine(" Volume Bar not available for the current index.");
            }
        }
 

        private void Mic1_Rec_Click(object sender, RoutedEventArgs e)
        {
            service.startRecording((WaveInCapabilities)Microphone1_ComboBox.SelectedValue);
            Mic1_Rec.IsEnabled = false;
            Mic1_Stop.IsEnabled = true;
        }

        private void Mic2_Rec_Click(object sender, RoutedEventArgs e)
        {
            service.startRecording((WaveInCapabilities)Microphone2_ComboBox.SelectedValue);
            Mic2_Rec.IsEnabled = false;
            Mic2_Stop.IsEnabled = true;
        }



        private void Mic1_Stop_Click(object sender, RoutedEventArgs e)
        {
            service.stopRecording((WaveInCapabilities)Microphone1_ComboBox.SelectedValue);
            Mic1_Rec.IsEnabled = true;
            Mic1_Stop.IsEnabled = false;
        }
        

        private void Mic2_Stop_Click(object sender, RoutedEventArgs e)
        {
            service.stopRecording((WaveInCapabilities)Microphone2_ComboBox.SelectedValue);
            Mic2_Rec.IsEnabled = true;
            Mic2_Stop.IsEnabled = false;
        }

        private void StartFullRecording()
        {
            if (Microphone1_ComboBox.SelectedIndex != -1)
            {
                service.startRecording((WaveInCapabilities)Microphone1_ComboBox.SelectedValue);
                Mic1_Rec.IsEnabled = false;
                Mic1_Stop.IsEnabled = true;
            }
            if (Microphone2_ComboBox.SelectedIndex != -1)
            {
                service.startRecording((WaveInCapabilities)Microphone2_ComboBox.SelectedValue);
                Mic2_Rec.IsEnabled = false;
                Mic2_Stop.IsEnabled = true;
            }
        }

        public void StopFullRecording()
        {
            if (Microphone1_ComboBox.SelectedIndex != -1)
            {
                service.stopRecording((WaveInCapabilities)Microphone1_ComboBox.SelectedValue);
                Mic1_Rec.IsEnabled = true;
                Mic1_Stop.IsEnabled = false;
            }
            if (Microphone2_ComboBox.SelectedIndex != -1)
            {
                service.stopRecording((WaveInCapabilities)Microphone2_ComboBox.SelectedValue);
                Mic2_Rec.IsEnabled = true;
                Mic2_Stop.IsEnabled = false;
            }
        }

        private void Microphone_refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            Microphone1_ComboBox.Items.Clear();
            Microphone2_ComboBox.Items.Clear();
            foreach (var mic in service.getMicrophoneList())
            {
                Microphone1_ComboBox.Items.Add(mic);
                Microphone2_ComboBox.Items.Add(mic);
            }
            Microphone1_ComboBox.DisplayMemberPath = "ProductName";
            Microphone2_ComboBox.DisplayMemberPath = "ProductName";
        }


        private void VolumeSlider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            service.setVolume((int)e.NewValue , 1);
        }

        private void VolumeSlider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            service.setVolume((int)e.NewValue , 2);  
        }









        #endregion

        
    }
}
