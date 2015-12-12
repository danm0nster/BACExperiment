using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

namespace BACExperiment.Model
{
    class WiimoteHandler
    {
        // The handler class creates the wiiremote collection class and the aggregators list for every wiiremote


        public WiimoteCollection mWiimotes;
        public WiimoteSampleAggregator[] aggregators;
        public WiimoteCoordinate[] coordinateSet;
        
        // Defining containers for the accelerometer , IR positions ; Might not be necessary since the Wiimote is sending info through it's Events to the Service listeners.


        // Structures to hold the x , y , and sensor size 


        public WiimoteHandler()
        {
            this.mWiimotes = new WiimoteCollection();
            aggregators = new WiimoteSampleAggregator[2];
            coordinateSet = new WiimoteCoordinate[2];
        }

        // Accesor to the Disconnect method inside the Wiimote

        public void SearchForWiimotes()
        {
            mWiimotes.FindAllWiimotes();
        }

        public void Connect(int i)
        {
            mWiimotes[i].Connect();
            mWiimotes[i].SetReportType(InputReport.IRAccel, true);
            mWiimotes[i].SetLEDs(i+1);
            aggregators[i] = new WiimoteSampleAggregator();
            coordinateSet[i] = new WiimoteCoordinate();
            aggregators[i].Wiimote = mWiimotes[i];
            aggregators[i].Processed += SendToWiimoteCoordinate;
            mWiimotes[i].WiimoteChanged += wm_WiimoteChanged;
            // Possibly find a way to set the sensitivity of the wii remote for the LEDs


        }
        
        public void Disconnect(int i)
        {
            aggregators[i] = null;
            coordinateSet[i] = null;
            mWiimotes[i].SetLEDs(false, false, false, false);
            mWiimotes[i].Disconnect();
            mWiimotes[i].Dispose();
            
            
        }

        internal void DisconnectAll()
        {
            foreach (var wm in mWiimotes)
            {
                try {
                    wm.SetLEDs(0);
                    wm.Disconnect();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void ConnectAll()
        {
            int index = 0;
            foreach (var wm in mWiimotes)
            {
                wm.Connect();
                wm.WiimoteChanged += wm_WiimoteChanged;
                wm.SetReportType(InputReport.IRAccel, true);
                wm.SetLEDs(index + 1);
                aggregators[index].Wiimote = wm;
                index++;
            }

        }

        private async void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            foreach(var aggregator in aggregators)
            {
                if (aggregator != null)
                {
                    if (aggregator.Wiimote == (Wiimote)sender)
                      await aggregator.ProcessSample(sender, e);
                }
            }
        }

        private void SendToWiimoteCoordinate(object sender, CoordinatesProcessedEventArgs e)
        {
            if (((WiimoteSampleAggregator)sender).Wiimote.WiimoteState.LEDState.LED1 == true)
            {
               coordinateSet[0].setCoordinates(e);
            }
            else if(((WiimoteSampleAggregator)sender).Wiimote.WiimoteState.LEDState.LED2 == true)
            { 
                coordinateSet[1].setCoordinates(e);
            }

        }


    }
}
