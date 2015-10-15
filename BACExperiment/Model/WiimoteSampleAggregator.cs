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
        public Wiimote Wiimote { get; set; }
        public int coordinateSet = -1;
        public int NotificationCount { get; set; }
        private float[] AccelState = new float[3];
        private PointF[] IRState = new PointF[4];
        private int battery = 0;
        private PointF MidPoint = new PointF();

        private Stabilizer stabilizer = null;

        private int ProcessCount = 0;
        private int Cap = 50;

        public WiimoteSampleAggregator()
        {
            Wiimote = new Wiimote();
            stabilizer = new Stabilizer();
        }

        public void RaiseRestart()
        {
            Restart(this, EventArgs.Empty);
        }

        private void Reset()
        {
            Cap = 0;
        }

        public async Task<String> ProcessSample(object sender, WiimoteChangedEventArgs e)
        {

            if (ProcessCount >= Cap)
            {
                EventHandler<CoordinatesProcessedEventArgs> handler = Processed;
                if (handler != null)
                {


                    try {
                        AccelState[0] = e.WiimoteState.AccelState.RawValues.X;
                        AccelState[1] = e.WiimoteState.AccelState.RawValues.Y;

                        AccelState[2] = e.WiimoteState.AccelState.RawValues.Z;
                        IRState = stabilizer.Update(e);
              
                        battery = (e.WiimoteState.Battery > 200f ? 200 : (int)e.WiimoteState.Battery);
                        MidPoint = stabilizer.CalculateMidPoint();

                        Notify(new CoordinatesProcessedEventArgs(((Wiimote)sender).HIDDevicePath, AccelState, IRState, battery, MidPoint));
                        Reset();
                    }

                    catch (Exception ex)
                    { Console.WriteLine(ex); }
                }


            }
            ProcessCount++;

            return "Done";

        }
        protected void Notify(CoordinatesProcessedEventArgs e)
        {
            if (this.Processed != null)
            {
                EventHandler<CoordinatesProcessedEventArgs> handler = Processed;
                handler(this, e);
            }
        }
    }

    public class CoordinatesProcessedEventArgs : EventArgs
    {
        public string ID { get; private set; }
        public float[] AccelCoordinates { get; private set; }
        public PointF[] IRCoordinates { get; private set; }
        public PointF MidPoint { get; private set; }
        public int battery;


        public CoordinatesProcessedEventArgs(string ID, float[] AccelState, PointF[] IRState, int battery, PointF MidPoint)
        {
            this.ID = ID;
            AccelCoordinates = AccelState;
            IRCoordinates = IRState;
            this.MidPoint = MidPoint;
            this.battery = battery;
        }
    }

    public class Stabilizer
    {
        PointF[] prev_IRState = new PointF[4];
        PointF[] current_IRState = new PointF[4];
        PointF[] updated_IRState = new PointF[4];
        PointF MidPoint = new PointF();

        public Stabilizer()
        {

        }

        public void Add(WiimoteChangedEventArgs e)
        {
            if (CheckForArrayNull(prev_IRState))
            {
                prev_IRState[0] = e.WiimoteState.IRState.IRSensors[0].Position;
                prev_IRState[1] = e.WiimoteState.IRState.IRSensors[1].Position;
                prev_IRState[2] = e.WiimoteState.IRState.IRSensors[2].Position;
                prev_IRState[3] = e.WiimoteState.IRState.IRSensors[3].Position;
            }

            else
            {
                prev_IRState[0] = e.WiimoteState.IRState.IRSensors[0].Position;
                prev_IRState[1] = e.WiimoteState.IRState.IRSensors[1].Position;
                prev_IRState[2] = e.WiimoteState.IRState.IRSensors[2].Position;
                prev_IRState[3] = e.WiimoteState.IRState.IRSensors[3].Position;
            }
        } 

        public PointF[] Update( WiimoteChangedEventArgs e)
        {
            Add(e);
            PointF delta = Stabilize(e.WiimoteState);

            for(int i = 0; i < 4; i++)
            {
                if (e.WiimoteState.IRState.IRSensors[i].Found)
                    updated_IRState[i] = e.WiimoteState.IRState.IRSensors[i].Position;
                else
                {
                    updated_IRState[i].X = prev_IRState[i].X + delta.X;
                    updated_IRState[i].Y = prev_IRState[i].Y + delta.Y;
                }     
            }

            return updated_IRState;
        }


        public bool CheckForArrayNull(PointF[] array)
        {
            int i = 0;
            foreach (var x in array)
                if (x.X != 0 && x.Y !=0)
                    i++;

            if(i <0)
            return false;
            return true;
        }

        public PointF Stabilize(WiimoteState e)
        {
            PointF delta;
            delta.X = -1;
            delta.Y = -1;


            int increment = 0;

            foreach(var sensor in e.IRState.IRSensors)
            {
                if(sensor.Found)
                {
                    delta.X += sensor.Position.X;
                    delta.Y += sensor.Position.Y;
                    increment++;
                }
            }

            if(delta.X!=-1 && delta.Y!=-1)
            {
                delta.X = delta.X / increment;
                delta.Y = delta.Y / increment;
            }

            return delta;

        }

        public PointF CalculateMidPoint()
        {
            foreach( var sensor in updated_IRState)
            {
                MidPoint.X = MidPoint.X + sensor.X;
                MidPoint.Y = MidPoint.Y + sensor.Y;
            }

            MidPoint.X = MidPoint.X / 4;
            MidPoint.Y = MidPoint.Y / 4;

            return MidPoint;
        }

    }
    
}
