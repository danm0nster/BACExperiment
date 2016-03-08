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
        private PointF[] IRState = new PointF[5];
        private int battery = 0;

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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<String> ProcessSample(object sender, WiimoteChangedEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
                        battery = (e.WiimoteState.Battery > 100f ? 100 : (int)e.WiimoteState.Battery);
                        
                        Notify(new CoordinatesProcessedEventArgs(((Wiimote)sender).HIDDevicePath, AccelState, IRState, battery, IRState[4]));
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


    /// <summary>
    /// The stabilizer object is responsible with processing the coordinates recieved from the Wiimote class. 
    /// It uses the coordinates recieved and intervenes if a coordinate set is found missing. This can occour when
    /// the infra red camera has troubles detecting the IR-LED's.
    /// </summary>
    public class Stabilizer
    {
        // Made the Point Array of 5 values so that the mid point is stored and returned with the point simultanousely 
        PointF[] prev_IRState = new PointF[5];
        PointF[] current_IRState = new PointF[5];
        PointF[] updated_IRState = new PointF[5];
        public PointF MidPoint = new PointF();

        public Stabilizer()
        {

        }

        /*
        Processes data values and ads them to containing lists.
        */
        public void Add(WiimoteChangedEventArgs e)
        {
            if (e.WiimoteState.IRState.IRSensors[0].Found)
                current_IRState[0] = e.WiimoteState.IRState.IRSensors[0].Position;
            else { current_IRState[0].X = float.NaN; current_IRState[0].Y = float.NaN; }

            if (e.WiimoteState.IRState.IRSensors[1].Found)
                current_IRState[1] = e.WiimoteState.IRState.IRSensors[1].Position;
            else { current_IRState[1].X = float.NaN; current_IRState[1].Y = float.NaN; }

            if (e.WiimoteState.IRState.IRSensors[2].Found)
                current_IRState[2] = e.WiimoteState.IRState.IRSensors[2].Position;
            else { current_IRState[2].X = float.NaN; current_IRState[2].Y = float.NaN; }

            if (e.WiimoteState.IRState.IRSensors[3].Found)
                current_IRState[3] = e.WiimoteState.IRState.IRSensors[3].Position;
            else { current_IRState[3].X = float.NaN; current_IRState[3].Y = float.NaN; }

        }

        /// <summary>
        /// Updates value lists with latest data recieved. If a data set is missing, the previouselly available data for that sesor is used together with a diferential calculation of
        /// the latest recieved data. 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PointF[] Update( WiimoteChangedEventArgs e)
        {
            Add(e);
            PointF delta = Stabilize(e.WiimoteState);
            Boolean all4 = true;

            bool ir1 = false;
            bool ir2 = false;

            for(int i = 0; i < 4; i++)
            {
                if (float.IsNaN(current_IRState[i].X) || float.IsNaN(current_IRState[i].Y))
                {
                    updated_IRState[i].X = prev_IRState[i].X + delta.X;
                    updated_IRState[i].Y = prev_IRState[i].Y + delta.Y;
                    if (i == 0) ir1 = false;
                    if (i == 1) ir2 = false;
                   
                }
                else
                {
                    updated_IRState[i].X = current_IRState[i].X;
                    updated_IRState[i].Y = current_IRState[i].Y;

                    if (i == 0) ir1 = true;
                    if (i == 1) ir2 = true;
                }

                all4 = all4 && e.WiimoteState.IRState.IRSensors[i].Found;
            }

            if (ir1 == true && ir2 == true)
            {
                updated_IRState[4] = e.WiimoteState.IRState.Midpoint;
            }
            else
            {
                float x = prev_IRState[4].X + delta.X;
                float y = prev_IRState[4].Y + delta.Y;

                if (!float.IsNaN(x) && !float.IsNaN(y))
                {
                    updated_IRState[4].X = x;
                    updated_IRState[4].Y = y;
                }

            }

            if (float.IsNaN(updated_IRState[4].X) || float.IsNaN(updated_IRState[4].Y))
            {
                prev_IRState[0] = updated_IRState[0];
                prev_IRState[1] = updated_IRState[1];
                prev_IRState[2] = updated_IRState[2];
                prev_IRState[3] = updated_IRState[3];
            }
            else
            {
                prev_IRState = updated_IRState;
            }
            return updated_IRState;
        }


        public bool CheckForArrayNull(PointF[] array)
        {
           
            foreach (var x in array)
                if (x.X != 0 && x.Y != 0)
                    return false;
            return true;
        }


        /// <summary>
        /// Calculates difference between the curent coordinates and the previous coordinates recieved from the remote.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PointF Stabilize(WiimoteState e)
        {
            PointF AvgDelta;
            AvgDelta.X = 0;
            AvgDelta.Y = 0;
            PointF[] delta = new PointF[4];

            int increment = 0;

            for(int i = 0; i < 4; i++)
            {
                if (e.IRState.IRSensors[i].Found)
                {
                    delta[i].X = e.IRState.IRSensors[i].Position.X - prev_IRState[i].X;
                    delta[i].Y = e.IRState.IRSensors[i].Position.Y - prev_IRState[i].Y;
                }
                else
                {
                    delta[i].X = float.NaN;
                    delta[i].Y = float.NaN;
                }
            }

            foreach (var D in delta)
            {
                if (!float.IsNaN(D.X) && !float.IsNaN(D.Y))
                {
                    AvgDelta.X += D.X;
                    AvgDelta.Y += D.Y;
                    increment++;
                }
            }

            AvgDelta.X = AvgDelta.X / increment;
            AvgDelta.Y = AvgDelta.Y / increment;
            
            return AvgDelta;

        }


        public PointF CalculateMidPoint()
        {
            PointF point = new PointF(); 

            for(int i=0; i < 4; i++)
            {
                point.X = point.X + updated_IRState[i].X;
                point.Y = point.Y + updated_IRState[i].Y;
            }

            point.X = point.X / 4;
            point.Y = point.Y / 4;

            return point;
        }

    }
    
}
