using BACExperiment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BACExperiment.GUI
{
    class StimulyWindowViewModel : INotifyPropertyChanged
    {
        //Links
        //Variables
        

        // Pointer dataContext
        private double _pointer1x;
        private double _pointer1y;
        private double _pointer1z;
        private double _pointer2x;
        private double _pointer2y;
        private double _pointer2z;
        private double _rezolutionX;
        private double _rezolutionY;

        public double  RezolutionX
        {
            get { return _rezolutionX; }
            set { if(value != _rezolutionX) { _rezolutionX = value; if (PropertyChanged != null) { Notify("RezolutionX"); } } }
        }
        public double RezolutionY
        {
            get { return _rezolutionY; }
            set { if (value != _rezolutionY) { _rezolutionY = value; if (PropertyChanged != null) { Notify("RezolutionY"); } } }
        }

        public double Pointer1X
        {
            get { return _pointer1x; }
            set { _pointer1x =RezolutionX/2-ScaleXToResolution(value); if (PropertyChanged != null) { Notify("Pointer1X"); } }
        }
            public double Pointer1Y
        {
            get { return _pointer1y; }
            set { _pointer1y =RezolutionY/2-ScaleYToResolution(value); if (PropertyChanged != null) { Notify("Pointer1Y"); } }
        }
        public double Pointe1Z
        {
            get { return _pointer1z; }
            set { _pointer1z = value; if (PropertyChanged != null) { Notify("Pointer1Z"); } }
        }
        public double Pointer2X
        {
            get { return _pointer2x; }
            set { _pointer2x = RezolutionX/2-ScaleXToResolution(value); if (PropertyChanged != null) { Notify("Pointer2X"); } }
        }
            public double Pointer2Y
        {
            get { return _pointer2y; }
            set { _pointer2y = RezolutionY/2-ScaleYToResolution(value); if (PropertyChanged != null) { Notify("Pointer2Y"); } }
        }
        public double Pointe2Z
        {
            get { return _pointer2z; }
            set { _pointer2z = value; if (PropertyChanged != null) { Notify("Pointer2Z"); } }
        }

        private static StimulyWindowViewModel instance;

        public event PropertyChangedEventHandler PropertyChanged;

        public static StimulyWindowViewModel GetInstance() { if (instance == null) { instance = new StimulyWindowViewModel(); } return instance; }




        private StimulyWindowViewModel()
        {
            _pointer1x = 0;
            _pointer1y = 50;
            _pointer2x = 0;
            _pointer2y = -50;

        }

        private int ScaleXToResolution(double x )
        {
            int toReturn = (int)(x * (RezolutionX*1.5));
            return toReturn;
        }

        private int ScaleYToResolution(double y)
        {
            int toReturn = (int)(y * (RezolutionY*1.5));
            return toReturn;
        }


        #region INotifyPropertyChangedImplementation

        protected void Notify(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        #endregion




    }
}
