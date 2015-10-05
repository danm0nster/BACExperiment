using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

namespace BACExperiment.Model
{
    class WiimoteSampleAggregator
    {

        // Class that will take the coordiantes provided by the wiiremotes and process them as to dampen any sudden movements of the wrists and such so that they would not be shown on screen 


        public EventHandler<CoordinatesProcessedEventArgs> Processed;
        public event EventHandler Restart = delegate { };
        public Wiimote Wiimote { get; internal set; }
        public int coordinateSet = -1;
        public int NotificationCount { get; set; }
        private float[] AccelState = new float[3];
        private Point[] IRState = new Point[5];
        private int battery;
        int count;

        public void RaiseRestart()
        {
            Restart(this, EventArgs.Empty);
        }

        private void Reset()
        {
            count = 0;
        }

        public async Task ProcessSample(object sender, WiimoteChangedEventArgs e)
        {
            await ProcessSample(sender, e);

            if (e.WiimoteState.IRState.IRSensors[0].Found && e.WiimoteState.IRState.IRSensors[1].Found)
            {
                if (Processed != null)
                {
                    Reset();

                    AccelState[0] = e.WiimoteState.AccelState.RawValues.X;
                    AccelState[1] = e.WiimoteState.AccelState.RawValues.Y;
                    AccelState[2] = e.WiimoteState.AccelState.RawValues.Z;
                    if (e.WiimoteState.IRState.IRSensors[0].Found)
                        IRState[0] = e.WiimoteState.IRState.IRSensors[0].RawPosition;
                    if (e.WiimoteState.IRState.IRSensors[1].Found)
                        IRState[1] = e.WiimoteState.IRState.IRSensors[1].RawPosition;
                    if (e.WiimoteState.IRState.IRSensors[2].Found)
                        IRState[2] = e.WiimoteState.IRState.IRSensors[2].RawPosition;
                    if (e.WiimoteState.IRState.IRSensors[3].Found)
                        IRState[3] = e.WiimoteState.IRState.IRSensors[3].RawPosition;

                    Processed(this, new CoordinatesProcessedEventArgs(AccelState,IRState ));
                }
                }

        }
    }

        public class CoordinatesProcessedEventArgs : EventArgs
        {
            public float[] AccelCoordinates { get; private set; }
            public Point[] IRCoordinates { get; private set; }
            public int battery;

        
            public CoordinatesProcessedEventArgs(float[] AccelState , Point[] IRState)
            {
            AccelCoordinates = AccelState;
            IRCoordinates = IRState;
            }

            
        }

        
}
