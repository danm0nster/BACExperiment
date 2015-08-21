﻿using System;
using System.Windows.Controls;
using WiimoteLib;
using System.Collections.Generic;

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
        public string Astatus ;
        public string Bstatus ;
        public string path ;
        private Service observer;
        private string LastAction { get; set; }
        //Status can be seen by anyone , but can only be modified by the owning class. If you want to modify the status it can only be done by the methods in this class.

        private string setAStatus { set { this.Astatus = value; } }
        public string getAStatus { get { return this.Astatus; } }

        private string setBStatus { set { this.Bstatus = value; } }
        public string getBStatus { get { return this.Bstatus; } }


        // Defining containers for the accelerometer , IR positions ; Might not be necessary since the Wiimote is sending info through it's Events to the Service listeners.
        public float[] Accelerometer = new float[3];
        public float[,] IRState = new float[5, 3];
        public int battery { get; set; }

        // Structures to hold the x , y , and sensor size 


        public WiimoteInfo(Service observer) 
        {
            this.mWiimotes = new WiimoteCollection();
            this.observer = observer;
        }



        /// <summary>
        /// Considering how could I pass the IR Sensor coordinates to the GUI for the sensor to be drawn .
        /// I could store the information in a sensor instance each time it changes . Problem is innitioally this method did not take into account the position of each sensor , 
        
        /// </summary>
        /// <param name="irSensor"></param>
        /// <param name="lblNorm"></param>
        /// <param name="color"></param>

        private void UpdateIR(IRSensor irSensor, int index)
        {

            if (irSensor.Found)
            {
                IRState[index, 0] = irSensor.Position.X/4;
                IRState[index, 1] = irSensor.Position.Y/4;
                IRState[index, 2] = irSensor.Size;
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
            mWiimotes[i].Disconnect();
        }

        public void Connect(int i)
        {
            this.mWiimotes.FindAllWiimotes();
            try {
                mWiimotes[i].Connect();
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
            }
        }

        //-1 value in the IRState array indicate that the respective sensor can not be found.
       

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            WiimoteInfo.UpdateWiimoteStateDelegate updateWiimoteStateDelegate = new WiimoteInfo.UpdateWiimoteStateDelegate(this.UpdateWiimoteChanged);
            object[] objArray = new object[] { args };
            
        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            WiimoteState wiimoteState = args.WiimoteState;
            Astatus = wiimoteState.ButtonState.A.ToString();
            Accelerometer[0] = wiimoteState.AccelState.Values.X;
            Accelerometer[1] = wiimoteState.AccelState.Values.Y;
            Accelerometer[2] = wiimoteState.AccelState.Values.Z;
            

            ExtensionType extensionType = wiimoteState.ExtensionType;
            if (extensionType <= (ExtensionType.Nunchuk | ExtensionType.ClassicController | ExtensionType.Guitar | ExtensionType.Drums))
            {
                if ((int)extensionType == -1541406720)
                {
                    Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need nunchuck . Please remove .");
                }
                else if (extensionType <= (ExtensionType.Nunchuk | ExtensionType.ClassicController | ExtensionType.Guitar | ExtensionType.Drums) && extensionType >= (ExtensionType.Nunchuk | ExtensionType.ClassicController))
                {
                    switch ((int)((long)extensionType - (long)(ExtensionType.Nunchuk | ExtensionType.ClassicController)))
                    {
                        case 0:
                            {
                                Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need classic controller . Please remove .");
                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need guitar . Please remove .");
                                break;
                            }
                    }
                }
            }
            else if (extensionType == (ExtensionType.Nunchuk | ExtensionType.BalanceBoard))
            {
              
                
                    Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need a ballanceboard or nunchuck . Please remove .");
                
               
            }
            else if (extensionType == ExtensionType.Drums)
            {
                Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need drums . Please remove .");
            }

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
        }

     
      

        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);



        public void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            // current state information

            Wiimote wm = (Wiimote)sender;
            UpdateWiimoteChanged(args);
            observer.informMainWindow(this);
            
            
            
            //Change If statements from using "==" to use " Equals " method


            /*if (wm.ID.Equals(wiimote1_info.getWiimoteID()))
                observer.OnNext(wiimote1_info);
            else if (wm.ID.Equals(wiimote2_info.getWiimoteID()))
                observer.OnNext(wiimote2_info);
            */
            Console.WriteLine(String.Concat(args.WiimoteState.AccelState.Values.ToString()));

        }





    }
}   