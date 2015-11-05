﻿using System;
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

      
        private List<MicrophoneConstruct> microphones = new List<MicrophoneConstruct>();
        private UnsignedMixerControl volumeControl1;
        private UnsignedMixerControl volumeControl2;
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


        public bool ListenToMicrophone(int selectedDevice , int groupBoxIndex)
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

            TryGetVolumeControl(selectedDevice , groupBoxIndex);
            return mic.Active();
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

        internal void StartRecording(WaveInCapabilities i)
        {
           foreach(MicrophoneConstruct mic in microphones)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(mic.get_WaveIn().DeviceNumber);

                if(deviceInfo.Equals(i))
                {
                    mic.Record();
                }
            }
        }

        internal void StopRecording(WaveInCapabilities i)
        {
            foreach (MicrophoneConstruct mic in microphones)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(mic.get_WaveIn().DeviceNumber);

                if (deviceInfo.Equals(i))
                {
                    mic.Stop();
                }
            }
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
