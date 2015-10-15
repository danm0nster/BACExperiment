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
        #endregion

        #region Links
        private StimulyWindowViewModel stimuly_data_context {get; set ;}
        public Wiimote1DataContext wm1_data_context;
        public Wiimote1DataContext wm2_data_context;
         
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
            stimuly_data_context = StimulyWindowViewModel.GetInstance();
            wm1_data_context = new Wiimote1DataContext();
            wm2_data_context = new Wiimote1DataContext();

         
         
        }

        public void TellStimulyWindow(object sender , PropertyChangedEventArgs e)
        {
            if (((WiimoteCoordinate)sender).Equals(WMHandler.coordinateSet[0]))
            {
                // Modify first ellipse 
                stimuly_data_context.Pointer1X = ((WiimoteCoordinate)sender).MidPoint.X;
                stimuly_data_context.Pointer1Y = ((WiimoteCoordinate)sender).MidPoint.Y;
                wm1_data_context.Update(sender , e);

            }

            if (((WiimoteCoordinate)sender).Equals(WMHandler.coordinateSet[1]))
            {
                //Modify second ellipse;
                stimuly_data_context.Pointer2X = ((WiimoteCoordinate)sender).MidPoint.X;
                stimuly_data_context.Pointer2Y = ((WiimoteCoordinate)sender).MidPoint.Y;
                wm2_data_context.Update(sender, e);
            }
        }


        /// <summary>
        /// WIIMOTE Handling
        /// </summary>


        public void DetectWiimotes()
        {
            WMHandler.SearchForWiimotes();
            Console.WriteLine("Searched for wiimotes;");
        }

        public void ConnectWiimoteToInfo(int i)
        {
            WMHandler.Connect(i);
            WMHandler.coordinateSet[i].PropertyChanged += TellStimulyWindow;
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
        public List<WaveInCapabilities> getMicrophoneList()
        {
           return microphones.MicrophoneList();
        }
     
        public void ListenToMicrophone(int index , int groupBoxIndex)
        {
            microphones.ListenToMicrophone(index , groupBoxIndex);
        }

        public void UpdateVolumeBar(int v1, float v2)
        {
            observer.UpdateVolumeBar( v1,  v2);
        }

        public void startRecording(int i)
        {
            microphones.StartRecording(i);
        }

        public void stopRecording(int i)
        {
            microphones.StopRecording(i);
        }
#endregion Microphone

    }
}   


