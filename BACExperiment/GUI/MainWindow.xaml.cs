using BACExperiment.GUI;
using BACExperiment.Model;
using NAudio.Mixer;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
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
    public partial class MainWindow : Window , INotifyPropertyChanged
    {

        private Service service;
        private MovementWindow stimulyWindow;
        private ReadingWIndow prompter;
        public event PropertyChangedEventHandler PropertyChanged;

        #region INotifyPropertyChanged Members

        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion


        private ObservableCollection<Window> _sequenceList;

        public MainWindow()
        {


            InitializeComponent();

            _sequenceList = new ObservableCollection<Window>();
            SequenceListView.DataContext = _sequenceList;

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


            service = Service.getInstance(this);
            ModeSelect.Items.Add(new ComboboxItem("Synchronous", "Synchronous"));
            ModeSelect.Items.Add(new ComboboxItem("Asynchronous", "Asynchronous"));
            ModeSelect.Items.Add(new ComboboxItem("Self-Paced", "Self-Paced"));
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
        


        #region MovementSettingCode

        private void ModeSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0) // Synchronous
            {
                AsynchronousGroupBox.IsEnabled = false;
                SelfpacedCombobox.IsEnabled = false;
                SynchronousGroupBox.IsEnabled = true;
            }
            if (((ComboBox)sender).SelectedIndex == 1) // Asynchronous
            {
                AsynchronousGroupBox.IsEnabled = true;
                SelfpacedCombobox.IsEnabled = false;
                SynchronousGroupBox.IsEnabled = false;
            }
            if (((ComboBox)sender).SelectedIndex == 2) // Self-Paced
            {
                AsynchronousGroupBox.IsEnabled = false;
                SelfpacedCombobox.IsEnabled = true;
                SynchronousGroupBox.IsEnabled = false;
            }
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                System.Windows.Media.Color color1 = (System.Windows.Media.Color)Subject1.SelectedColor;
                System.Windows.Media.Color color2 = (System.Windows.Media.Color)Subject2.SelectedColor;
                System.Windows.Media.Color color3 = (System.Windows.Media.Color)CourseColorPicker.SelectedColor;

                if (ModeSelect.SelectedIndex == -1)
                {
                    System.Windows.MessageBox.Show("Please select a course mode before attempting to run the experiment");
                }
                else
                {
                    if (((ComboboxItem)ModeSelect.SelectedItem).Value == "Synchronous")
                    {
                        stimulyWindow = new MovementWindow(this, "Synchronous", (int)complexitySlider.Value, (int)SpeedSlider.Value, color1, color2, color3);
                    }
                    else
                    if (((ComboboxItem)ModeSelect.SelectedItem).Value == "Asynchronous")
                    {
                        stimulyWindow = new MovementWindow(this, "Asynchronous", (int)complexitySlider2.Value, (int)SpeedSlider2.Value, color1, color2, color3);
                    }
                    else
                    if (((ComboboxItem)ModeSelect.SelectedItem).Value == "Self-Paced")
                    {
                        stimulyWindow = new MovementWindow(this, color1, color2, color3, (int)LineThicknessPicker.Value);
                    }

                    stimulyWindow.Show();


                    GenerateCourseBtn.IsEnabled = true;
                    OpenBtn.IsEnabled = false;
                    CloseBtn.IsEnabled = true;
                }

                
            }

            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            this.Cursor = Cursors.Arrow;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            stimulyWindow.Close();
            CloseBtn.IsEnabled = false;
        }

        private void GenerateCourse_Click(object sender, RoutedEventArgs e)
        {
           
            stimulyWindow.ResizeMode = ResizeMode.NoResize;
            
            stimulyWindow.buildCourse();
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

        private void AddToSequence_Click(object sender, RoutedEventArgs e)
        {
            MovementWindow window = null;
            try
            {
            System.Windows.Media.Color color1 = (System.Windows.Media.Color)Subject1.SelectedColor;
            System.Windows.Media.Color color2 = (System.Windows.Media.Color)Subject2.SelectedColor;
            System.Windows.Media.Color color3 = (System.Windows.Media.Color)CourseColorPicker.SelectedColor;

            if (ModeSelect.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Please select a course mode before attempting to run the experiment");
            }
            else
            {

                if (((ComboboxItem)ModeSelect.SelectedItem).Value == "Synchronous")
                {
                    window = new MovementWindow(this, "Synchronous", (int)complexitySlider.Value, (int)SpeedSlider.Value, color1, color2, color3);
                }
                else
                    if (((ComboboxItem)ModeSelect.SelectedItem).Value == "Asynchronous")
                {
                    window = new MovementWindow(this, "Asynchronous", (int)complexitySlider2.Value, (int)SpeedSlider2.Value, color1, color2, color3);
                }
                else
                    if (((ComboboxItem)ModeSelect.SelectedItem).Value == "Self-Paced")
                {
                    window = new MovementWindow(this, color1, color2, color3, (int)LineThicknessPicker.Value);
                }
            }

          
                if (window != null)
                    _sequenceList.Add(window);
            }
            catch(Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString());
            }
            
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
                        prompter = new ReadingWIndow((int)TextSizeSlider.Value, pathTxt.Text, (int)SyncTraversalSpeed_Slider.Value, color1);
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
                        prompter = new ReadingWIndow((int)TextSizeSlider.Value, pathTxt.Text, (int)AsyncTraversalSpeed_Slider.Value, (int)Turn_duration_Slider.Value, (int)Switch_Frequency_Slider.Value, color1, color2, color3);
                    }
                    catch (Exception ex)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                    }
                }

                if (SelfPaced_RBtn.IsChecked == true)
                {
                    try
                    {
                        prompter = new ReadingWIndow((int)TextSizeSlider.Value, pathTxt.Text, (int)prompterSpeed.Value);
                    }
                    catch (Exception ex)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                    }
                }
                prompter.Visibility = System.Windows.Visibility.Visible;
                prompterPlay.IsEnabled = true;

            }

            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("No file specified in path");
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
        {

            service.getMicrophoneList();
        }


        private void VolumeSlider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            service.setVolume((int)e.NewValue, 1);
        }

        private void VolumeSlider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            service.setVolume((int)e.NewValue, 2);
        }
            #endregion

#region SequenceManagement

            public ObservableCollection<Window> SequenceList
        {
            get { return _sequenceList; }
            set
            {
                _sequenceList = value;
                if(PropertyChanged!=null)
                Notify("SequenceList");
            }
        }

           private void SequenceList_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if(PropertyChanged != null)
            Notify("SequenceList");
        }
#endregion


        private void Port_Ping_btn_Click(object sender, RoutedEventArgs e)
        {
            service.PingExperimentStart();
        }

        private void Port_Phase1_btn_Click(object sender, RoutedEventArgs e)
        {
            service.PingFirstPhase();
        }

        private void Port_Phase2_btn_Click(object sender, RoutedEventArgs e)
        {
            service.PingSecondPhase();
        }

      
       
    }
}
