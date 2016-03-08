using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using WiimoteLib;
using System.IO;
using System.Diagnostics;
using BACExperiment.Model;
using NAudio.Wave;
using System.ComponentModel;
using BACExperiment.GUI;

// Service class with most major functionality 

namespace BACExperiment
{
    /* In this service class , I plan on programming the functionality of the programm 
    that does not find place in the GUI files . As a general idea , here is where the 
    audio , video , and cursor position recordin will be programmed . 
       In this class there will also be the code that generates the Stimuly for the 
    users to follow and also probably the colision detection .
       Everything that is not a native method of the xaml.cs files will be stored here.
    */
    public class Service
    {
        public static Service instance;

     
        #region Handlers
        private MicrophoneHandler microphones { get; set; }
        private WiimoteHandler WMHandler { get; set;}
        private static PortAccessHandler port { get; set; }
        #endregion

        #region Links
        private MovementWindowViewModel stimuly_data_context {get; set ;}
        public WiimoteDataContext wm1_data_context;
        public WiimoteDataContext wm2_data_context;
        public MicrophoneViewModel mic_data_context;
        #endregion



        //Make two threads to test if that improves the GUI responsiveness of the led points. Theoretically it should as there would be way less calls to the OnNext method . Ideally 
        //it would have a frequence of around 60 to 100 fps. 



        //Made service as a Singleton

        public static Service getInstance(MainWindow observer)
        {
            if (instance == null)
                instance = new Service(observer);
            return instance;
        }

        private MainWindow observer;

     
        private Service(MainWindow observer)
        {

            this.observer = observer;
            microphones = new MicrophoneHandler(this);
            WMHandler = new WiimoteHandler();
            stimuly_data_context = MovementWindowViewModel.GetInstance();
            wm1_data_context = new WiimoteDataContext();
            wm2_data_context = new WiimoteDataContext();
            mic_data_context = new MicrophoneViewModel();
            try {
                port = PortAccessHandler.GetIntance();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "A paralel port could not be detected on the given address. Please make sure you have a paralel port on the machine. \r\n"+
                    ex.Message);
                port = null;
            }
         
         
        }


        /// <summary>
        /// Sends info to movement window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SendToMovementWindow(object sender , PropertyChangedEventArgs e)
        {
            if (((WiimoteCoordinate)sender).Equals(WMHandler.coordinateSet[0]))
            {
                // Modify first ellipse 
                stimuly_data_context.Pointer1X = ((WiimoteCoordinate)sender).MidPoint.X;
                stimuly_data_context.Pointer1Y = ((WiimoteCoordinate)sender).MidPoint.Y;
                stimuly_data_context.SetAccel(0, ((WiimoteCoordinate)sender).AccelValues);         
                wm1_data_context.Update(sender , e);

            }

            if (((WiimoteCoordinate)sender).Equals(WMHandler.coordinateSet[1]))
            {
                //Modify second ellipse;
                stimuly_data_context.Pointer2X = ((WiimoteCoordinate)sender).MidPoint.X;
                stimuly_data_context.Pointer2Y = ((WiimoteCoordinate)sender).MidPoint.Y;
                stimuly_data_context.SetAccel(1, ((WiimoteCoordinate)sender).AccelValues);
                wm2_data_context.Update(sender, e);
            }
        }


       
        public void DetectWiimotes()
        {
            WMHandler.retrieveWiimotesToMemory();
            Console.WriteLine("Searched for wiimotes;");
        }

        public void ConnectWiimoteToInfo(int i)
        {
            WMHandler.Connect(i);
            WMHandler.coordinateSet[i].PropertyChanged += SendToMovementWindow;
            Console.WriteLine(string.Concat("Wiimote ", i, " has been connected"));
        }

        public void DisconnectWiimoteFromInfo(int i)
        {
            WMHandler.Disconnect(i);
            Console.WriteLine(string.Concat("Wiimote ", i, " has been disconnected/r/n"));

        }
          
        public void ConnectAllWiimotes()
        {
            WMHandler.ConnectAll();
        }

        public void DisconnectAllWiimotes()
        {
            WMHandler.DisconnectAll();
        }

        public void SendMessage(int index, string message)
        {
            observer.WriteToRemoteMenu(index, message);
        }
        public void SendMessage(string message)
        {
            observer.WriteToRemoteMenu(1, message);
            observer.WriteToRemoteMenu(2, message);
        }

        public int GetRemoteCount()
        {
           return WMHandler.mWiimotes.Count;
        }



        #region Microphone
        public void getMicrophoneList()
        {
            mic_data_context.FillList(microphones.MicrophoneList());
        }
     
        public bool ListenToMicrophone(int index , int groupBoxIndex)
        {
           return microphones.ListenToMicrophone(index , groupBoxIndex);
        }

        public void UpdateVolumeBar(int v1, float v2)
        {
            if (v1 == 1)
                mic_data_context.CurrentInputLevel1 = v2 * 100;
            if (v1 == 2)
                mic_data_context.CurrentInputLevel2 = v2 * 100;
        
            }

        public void startRecording(MicrophoneConstruct i)
        {
            try {
                microphones.StartRecording(i);
                port.PingBiopac();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void stopRecording(MicrophoneConstruct i)
        {
            microphones.StopRecording(i);
        }

        public void setVolume(int value , int i)
        {
            microphones.SetVolume(value , i);
        }
        #endregion Microphone

        #region Port

        public static void WriteToPort(short Data)
        {
            if (port != null)
            {
                port.Write(Data);
            }
        }

        public static void PingExperimentStart()
        {
            for (int i = 0; i <= 2; i++)
            {
                if (port != null)
                {
                    port.Write(255);
                    Thread.Sleep(1000);
                    port.Write(0);
                    Thread.Sleep(100);
                }
            }
        }

        public static void PingStartNewPhase()
        {
            if( port !=null) { 
            port.Write(128);
            Thread.Sleep(1000);
            port.Write(0);
                }
        }
        #endregion

    }
}   


