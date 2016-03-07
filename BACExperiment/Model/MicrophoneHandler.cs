using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.IO;
using System.Diagnostics;
using NAudio.Mixer;

namespace BACExperiment.Model
{
    class MicrophoneHandler
    {


        private MicrophoneConstruct[] activeMicrophones = new MicrophoneConstruct[2];
        private List<MicrophoneConstruct> microphones = new List<MicrophoneConstruct>();
        private UnsignedMixerControl volumeControl1; 
        private UnsignedMixerControl volumeControl2;
        private Service observer;

        public MicrophoneHandler(Service observer)
        {
            this.observer = observer;
            AudioSessionLogDirectoryPresent(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ToString()));
        }

        public List<MicrophoneConstruct> MicrophoneList()
        {
            
            int WaveInDevices = WaveIn.DeviceCount;
            microphones = new List<MicrophoneConstruct>();
            for (int WaveInDevice = 0; WaveInDevice < WaveInDevices; WaveInDevice++)
            {
                MicrophoneConstruct deviceInfo = new MicrophoneConstruct(WaveInDevice);
                deviceInfo.aggregator.MaximumCalculated += MaximimumCalculated;
                microphones.Add(deviceInfo);
            }

            return microphones;
        }


        public bool ListenToMicrophone(int selectedDevice , int groupBoxIndex)
        {
            MicrophoneConstruct mic = microphones[selectedDevice];

            mic.startListening();
            activeMicrophones[groupBoxIndex-1] = mic;  
            
            TryGetVolumeControl(selectedDevice , groupBoxIndex);
            return mic.Active;
        }

        private void MaximimumCalculated(object sender, MaxSampleEventArgs e)
        {
            int index = 0;
            for ( int i = 0; i<=1; i++)
            {
                if (activeMicrophones[i] != null)
                {
                    if (activeMicrophones[i].aggregator.Equals((MicrophoneSampleAggregator)sender))
                    {
                        index = i + 1;
                    }
                }
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

        internal void StartRecording(MicrophoneConstruct i)
        {
            i.createAudioRecord();
        }

        internal void StopRecording(MicrophoneConstruct i)
        {
            i.stopListening();
        }

        private void TryGetVolumeControl(int deviceNumber , int i)
        {
            int waveInDeviceNumber = deviceNumber;
            var mixerLine = new MixerLine((IntPtr)waveInDeviceNumber,
                                           0, MixerFlags.WaveIn);
            foreach (var control in mixerLine.Controls)
            {
                if (control.ControlType == MixerControlType.Volume)
                {
                    if(i == 1)
                    volumeControl1 = control as UnsignedMixerControl;
                   
                    if(i == 2)
                    volumeControl2 = control as UnsignedMixerControl;

                    break;

                }
            }
        }

        public void SetVolume( int value , int i)
        {
            if (i == 1)
                volumeControl1.Percent = value;

            if (i == 2)
                volumeControl2.Percent = value;
        }
    }
}
