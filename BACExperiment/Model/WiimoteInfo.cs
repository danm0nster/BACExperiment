using System;
using System.Windows.Controls;
using WiimoteLib;
using System.Collections.Generic;
using System.Timers;
using System.Threading.Tasks;

namespace BACExperiment
{
    /*
    This class I have initially copied from the WiimoteLib 1.7 tester application for familiarizing with myself with the way the wiimote library
    interacts with the guy . For BACExperiment however , I will now modify the class so that it contains what we need for the experiment
     

    */
    public class WiimoteInfo

    {
        
        public WiimoteCollection mWiimotes { get; set; }
        public int count { get; set; }  
        private Service observer;
        private bool searched;

        // Defining containers for the accelerometer , IR positions ; Might not be necessary since the Wiimote is sending info through it's Events to the Service listeners.
        public float[] Accelerometer = new float[3];
        public float[,] IRState = new float[5, 3];
        public int battery { get; set; }
        public float maxX;
        public float maxY;
        public bool IRActive = false;

        // Structures to hold the x , y , and sensor size 


        public WiimoteInfo(Service observer) 
        {
            this.mWiimotes = new WiimoteCollection();
            this.observer = observer;
        }


        private void UpdateIR(IRSensor irSensor, int index)
        {

            if (irSensor.Found)
            {
                IRState[index, 0] = irSensor.RawPosition.X/4;
                IRState[index, 1] = irSensor.RawPosition.Y/4;
                IRState[index, 2] = irSensor.Size;

                if (irSensor.RawPosition.X / 4 > maxX)
                    maxX = irSensor.RawPosition.X / 4;
                if (irSensor.RawPosition.Y / 4 > maxY)
                    maxY = irSensor.RawPosition.Y / 4;
            }
            else
            {
                IRState[index, 0] = -1;
                IRState[index, 1] = -1;
                IRState[index, 2] = -1;

            }
        }

        private void UpdateIR(Point irSensor, int index)
        {

            if (irSensor.X != 0 && irSensor.Y !=0)
            {
                IRState[index, 0] = irSensor.X / 4;
                IRState[index, 1] = irSensor.Y / 4;
                IRState[index, 2] = 5;
            }
        }

        // Accesor to the Disconnect method inside the Wiimote
        public void Disconnect(int i)
        {
            mWiimotes[i].SetLEDs(false, false, false, false);
            mWiimotes[i].Disconnect();
            count--;
        }

        public void Connect(int i)
        {
            try {
                mWiimotes[i].Connect();  //first attempt throws null reference exception
                mWiimotes[i].SetReportType(InputReport.IRAccel, true);
                
                if (i == 0)
                    mWiimotes[i].SetLEDs(true, false, false, false);
                if (i == 1)
                    mWiimotes[i].SetLEDs(false, true, false, false);
                mWiimotes[i].WiimoteChanged += wm_WiimoteChanged;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        //-1 value in the IRState array indicate that the respective sensor can not be found.

        public void SearchForWiimotes()
        {
            if (!searched)
            {
                try
                {
                    mWiimotes.FindAllWiimotes();
                    searched = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Console.WriteLine("Already Searched");
            }
        }

        internal void DisconnectAll()
        {
            foreach (var wm in mWiimotes)
            {
                wm.Disconnect();
            }
        }

        public void ConnectAll()
        {
            for(int i = 0 ; i <= mWiimotes.Count-1 ; i++ )
            {
                mWiimotes[i].Connect();
                mWiimotes[i].SetReportType(InputReport.IRAccel, true );
                mWiimotes[i].SetLEDs(i+1);
            }
        }

        private async Task UpdateWiimoteChanged(WiimoteChangedEventArgs args , object sender)
        {
            WiimoteState wiimoteState = args.WiimoteState;
            Accelerometer[0] = wiimoteState.AccelState.Values.X;
            Accelerometer[1] = wiimoteState.AccelState.Values.Y;
            Accelerometer[2] = wiimoteState.AccelState.Values.Z;
            
          
            this.UpdateIR(wiimoteState.IRState.IRSensors[0], 0);
            
            this.UpdateIR(wiimoteState.IRState.IRSensors[1], 1);
            this.UpdateIR(wiimoteState.IRState.IRSensors[2], 2);
            this.UpdateIR(wiimoteState.IRState.IRSensors[3], 3);

            if(wiimoteState.IRState.IRSensors[0].Found && wiimoteState.IRState.IRSensors[1].Found)
            {
                this.UpdateIR(wiimoteState.IRState.RawMidpoint, 4);

            }


            // This if statement makes shure that there are at least 2 IR sensors detected so that the Mid point can be drawn .
            // If we do not have at least 2 IR sensors we run the rist of dividing  0 when we try to determine the middle point.
            // The wii remote can see max 4 sensors but can run with two . For that reasons , if there are not at least two sensors detected ,
            // the values of x and y will be changed to -1 . That way we can make a check whitouth running into a NullPointerException or 
            if (wiimoteState.IRState.IRSensors[0].Found && wiimoteState.IRState.IRSensors[1].Found)
            {
                Console.WriteLine("Found 2 sensors");
            }

            // this.battery = (wiimoteState.Battery > 100f ? 100 : (int)wiimoteState.Battery);
            this.battery = (wiimoteState.Battery > 200f ? 200 : (int)wiimoteState.Battery);
            //this.path = string.Concat("Device Path: ", wiimoteState.HIDDevicePath);

            await observer.informMainWindow(this, (Wiimote)sender );
        }

     
      

        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);

        public async void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {

            await UpdateWiimoteChanged(args, sender);

            Console.WriteLine(String.Concat(args.WiimoteState.AccelState.Values.ToString()));
        }
    }
}   