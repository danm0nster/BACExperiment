    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BACExperiment.Model
{
    class WiimoteCoordinate 
    {
       

        private string _ID = "";
        private Point[] _IRCoordinates;
        private Double[] _AccelValues;
        private Point _MidPoint = new Point();
        private int _battery=0;

        public string ID { get { return _ID; } set { _ID = value; if (PropertyChanged != null) Notify("ID"); } }
        public Point[] IRCoordinates { get { return _IRCoordinates; } set { _IRCoordinates = value; if (PropertyChanged != null) Notify("IRCoordinates"); } }
        public Double[] AccelValues { get { return _AccelValues; } set { _AccelValues = value; if (PropertyChanged != null) Notify("AccelValues"); } }
        public Point MidPoint { get { return _MidPoint; } set { _MidPoint = value; if (PropertyChanged != null) Notify("MidPoint"); } }
        public int Battery { get { return _battery; } set { _battery = value; if (PropertyChanged != null) Notify("MidPoint"); } }

        public WiimoteCoordinate()
        {
            _IRCoordinates = new Point[4];
            _AccelValues = new Double[3];
        }

        public void setCoordinates(CoordinatesProcessedEventArgs e)
        {
            
            
            Point[] ir = new Point[4];
            for (int i = 0; i < 4 ;i++)
            {
                ir[i] = new Point(e.IRCoordinates[i].X  , e.IRCoordinates[i].Y);
            }

            double[] accel = new double[3];
            for (int i = 0; i < 3; i++)
            {
                accel[i] = e.AccelCoordinates[i];
            }
            MidPoint = new Point(e.MidPoint.X , e.MidPoint.Y);
            Battery = e.battery;
            ID = e.ID;
        
            IRCoordinates = ir;
            AccelValues = accel;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged implementation

    }
}
