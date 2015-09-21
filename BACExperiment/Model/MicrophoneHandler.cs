using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.IO;
using System.Diagnostics;

namespace BACExperiment.Model
{
    class MicrophoneHandler
    {

      
        private List<MicrophoneConstruct> microphones = new List<MicrophoneConstruct>();
      
        private Service observer;

        public MicrophoneHandler(Service observer)
        {
            this.observer = observer;
            AudioSessionLogDirectoryPresent(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ToString()));
        }

        public List<WaveInCapabilities> MicrophoneList()
        {
            int WaveInDevices = WaveIn.DeviceCount;
            List<WaveInCapabilities> ToReturn = new List<WaveInCapabilities>();
            for (int WaveInDevice = 0; WaveInDevice < WaveInDevices; WaveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(WaveInDevice);
                ToReturn.Add(deviceInfo);

            }

            return ToReturn;
        }


        public void ListenToMicrophone(int selectedDevice , int groupBoxIndex)
        {
            MicrophoneConstruct mic = new MicrophoneConstruct(selectedDevice, groupBoxIndex);
            mic.aggregator.MaximumCalculated += new EventHandler<MaxSampleEventArgs>(MaximimumCalculated);

            
            if ((microphones.Count>2))
            {
                mic.Listen();
                microphones[groupBoxIndex - 1] = mic;
            }
            else
            {
                mic.Listen();
                microphones.Add(mic);
               
            }
            
        }

        private void MaximimumCalculated(object sender, MaxSampleEventArgs e)
        {
            //Figure ot to which structure the sending aggregator belongs to so that we can update the group box of that specific Micrcophone

            int index = 0;
            foreach (var x in microphones)
                if ( x.aggregator == (SampleAggregator)sender)
                {
                    index = x.groupBoxIndex;
                }
            observer.UpdateVolumeBar( index , Math.Max(e.MaxSample, Math.Abs(e.MinSample)));
        }

        
        public void AudioSessionLogDirectoryPresent(string directoryPath)
        {
            if (!Directory.Exists(directoryPath + "\\Audio Recordings"))
            {
                Directory.CreateDirectory(directoryPath + "\\Audio Recordings");
            }

        }

       

        public static void ConvertToMp3(string lameExePath, string waveFile, string mp3File)
        {
            Process converter = Process.Start(lameExePath, "-V2 \"" + waveFile
                                     + "\" \"" + mp3File + "\"");
            converter.WaitForExit();
        }

        internal void StartRecording(int i)
        {
            microphones[i-1].Record();
        }

        internal void StopRecording(int i)
        {
            microphones[i-1].Stop();
        }
    }
}
