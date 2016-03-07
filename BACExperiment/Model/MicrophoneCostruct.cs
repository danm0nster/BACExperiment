using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACExperiment.Model
{
    public class MicrophoneConstruct : INotifyPropertyChanged

    {
        #region INotifyPropertyChangedImplementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private WaveIn waveIn = null;
        public MicrophoneSampleAggregator aggregator { get; set; }
        private WaveFileWriter file = null;
        private int sampleCount = 0;
        public bool active = false;

        public bool Active { get { return active;} set { active = value; if (PropertyChanged != null) Notify("Active"); } } 
       

        public int groupBoxIndex { get; set; }

        public override string ToString()
        {
            WaveInCapabilities wav = WaveIn.GetCapabilities(waveIn.DeviceNumber);
            return wav.ProductName;
        }

        public WaveIn get_WaveIn(){ return this.waveIn; }


        /// <summary>
        /// Create MicrophoneConstruct object for the the audio device in memory with the "selectedDevice" index.
        /// Wave format of 44.1 khz , stereo.
        /// </summary>
        /// <param ID="selectedDevice"></param>
        public MicrophoneConstruct(int selectedDevice)
        {
            waveIn = new WaveIn();

            if (WaveIn.DeviceCount > 1)
                waveIn.DeviceNumber = selectedDevice;

            waveIn.WaveFormat = new WaveFormat(44100, 2);

            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveIn_RecordingStoped);

            aggregator = new MicrophoneSampleAggregator();
            aggregator.NotificationCount = 10; // How often we update the volume bar value;
        }

        /// <summary>
        /// Start recording audio from microphone. if "createAudioRecord" was not previouselly called the audio will not be saved. 
        /// </summary>
        public void startListening()
        {
            waveIn.StartRecording();
        }

        /// <summary>
        /// Creates the audio file for the microphone to save the audio.
        /// </summary>
        public void createAudioRecord()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string directoryName = "\\Audio recordings";
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")+"_"+waveIn.DeviceNumber.ToString()+".wav";

            if (!Directory.Exists(string.Concat(Path , directoryName)))
                Directory.CreateDirectory(string.Concat(Path, directoryName));

            file = new WaveFileWriter(Path+directoryName+"\\"+fileName, waveIn.WaveFormat);
        }

        /// <summary>
        /// Stop listening. Closes audio file and disposes of it from memory.
        /// </summary>
        public void stopListening()
        {
            waveIn.StopRecording();
        }

        /// <summary>
        /// Processes the audio signal for the assigned microphone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            active = true;
            if (file != null)
            {
                file.Write(e.Buffer, 0, e.BytesRecorded);
                file.Flush();
            }
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) |
                                        e.Buffer[index + 0]);
                float sample32 = sample / 32768f;

                ProcessSample(sample32);
            }
        }

        public void ProcessSample(float sample)
        {
            sampleCount++;
            if (sampleCount > 800)
            {
                aggregator.Add(sample);
                sampleCount = 0;
            }

        }

        public void waveIn_RecordingStoped(object sender, StoppedEventArgs e)
        {
           
            if (file != null)
            {
                file.Dispose();
                file = null;
            }

        }

    }
}
