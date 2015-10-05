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
        public event PropertyChangedEventHandler PropertyChanged;

        private int _ID = -1;
        private Point[] _IRCoordinates;
        private float[] _AccelValues;
        private Point _MidPoint = new Point();
        private Point _StimulyMidPoint = new Point();

        public int ID { get { return _ID; } set { _ID = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ID")); } }
        public Point[] IRCoordinates { get { return _IRCoordinates; } set { _IRCoordinates = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IRCoordinates")); } }
        public float[] AccelValues { get { return _AccelValues; } set { _AccelValues = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("AccelValues")); } }
        public Point MidPoint { get { return _MidPoint; } set { } }

        public WiimoteCoordinate()
        {
            _IRCoordinates = new Point[4];
            _AccelValues = new float[3];

        }

        public async Task setCoordinates(CoordinatesProcessedEventArgs e)
        {
            await setCoordinates(e);

            Point[] ir = new Point[4];
            for (int i = 0; i <= 4 ;i++)
            {
                ir[i] = new Point(e.IRCoordinates[i].X  , e.IRCoordinates[i].Y);
            }

            float[] accel = new float[3];
            for (int i = 0; i <= 3; i++)
            {
                accel[i] = e.AccelCoordinates[i];
            }

            IRCoordinates = ir;
            AccelValues = accel;
        }
        
        #region INotifyPropertyChanged implementation
        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged implementation

    }
}
