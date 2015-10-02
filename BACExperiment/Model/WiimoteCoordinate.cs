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
        private int _ID = -1;
        private List<Point> _IRCoordinates = new List<Point>();
        private List<float> _AccelValues = new List<float>();
        private Point _MidPoint = new Point();
        private Point _StimulyMidPoint = new Point();

        public int ID { get { return _ID; } set { _ID = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ID")); } }
        public List<Point> IRCoordinates { get { return _IRCoordinates; } set { _IRCoordinates = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IRCoordinates")); } }
        public List<float> AccelValues { get { return _AccelValues; } set { _AccelValues = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("AccelValues")); } }
        public Point MidPoint { get { return _MidPoint; } set { } }

        public WiimoteCoordinate()
        {

        }

        public async Task setCoordinates(CoordinatesProcessedEventArgs e)
        {
            await setCoordinates(e);
            foreach (var var in e.IRCoordinates)
                IRCoordinates.Add(new Point(var.X, var.Y));

            foreach (var var in e.AccelCoordinates)
                AccelValues.Add(var);       
        }
        
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
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
