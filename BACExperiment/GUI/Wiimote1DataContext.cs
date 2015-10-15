using BACExperiment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACExperiment
{
   public class Wiimote1DataContext :INotifyPropertyChanged
    {

      
 
        private string _AxCoord;
        private string _AyCoord;
        private string _AzCoord;
        private double _IR1_x;
        private double _IR1_y;
        private double _IR2_x;
        private double _IR2_y;
        private double _IR3_x;
        private double _IR3_y;
        private double _IR4_x;
        private double _IR4_y;
        private double _MidPoint_x;
        private double _MidPoint_y;
        private int _battery;
        private string _ID;

        private double _scaleX;
        private double _scaley;

        public string AxCoord { get { return _AxCoord; } set { _AxCoord = value; if (PropertyChanged != null) Notify("AxCoord"); } }
        public string AyCoord { get { return _AyCoord; } set { _AyCoord = value; if (PropertyChanged != null) Notify("AyCoord"); } }
        public string AzCoord { get { return _AzCoord; } set { _AzCoord = value; if (PropertyChanged != null) Notify("AzCoord"); } }
        public double IR1_x { get { return _IR1_x; } set { _IR1_x = value; if (PropertyChanged != null) Notify("IR1_x"); } }
        public double IR1_y { get { return _IR1_y; } set { _IR1_y = value; if (PropertyChanged != null) Notify("IR1_y"); } }
        public double IR2_x { get { return _IR2_x; } set { _IR2_x = value; if (PropertyChanged != null) Notify("IR2_x"); } }
        public double IR2_y { get { return _IR2_y; } set { _IR2_y = value; if (PropertyChanged != null) Notify("IR2_y"); } }
        public double IR3_x { get { return _IR3_x; } set { _IR3_x = value; if (PropertyChanged != null) Notify("IR3_x"); } }
        public double IR3_y { get { return _IR3_y; } set { _IR3_y = value; if (PropertyChanged != null) Notify("IR3_y"); } }
        public double IR4_x { get { return _IR4_x; } set { _IR4_x = value; if (PropertyChanged != null) Notify("IR4_x"); } }
        public double IR4_y { get { return _IR4_y; } set { _IR4_y = value; if (PropertyChanged != null) Notify("IR4_y"); } }
        public double MidPoint_x { get { return _MidPoint_x; } set { _MidPoint_x = value; if (PropertyChanged != null) Notify("MidPoint_x"); } }
        public double MidPoint_y { get { return _MidPoint_y; } set { _MidPoint_y = value; if (PropertyChanged != null) Notify("MidPoint_y"); } }
        public int Battery { get { return _battery; } set { _battery = value; if (PropertyChanged != null) Notify("Battery"); } }
        public string ID { get { return _ID; } set { _ID = value; if (PropertyChanged != null) Notify("ID"); } }
        public double ScaleX { get { return _scaleX; } set { _scaleX = value; if (PropertyChanged != null) Notify("ScaleX"); } }
        public double ScaleY { get { return _scaley; } set { _scaley = value; if (PropertyChanged != null) Notify("ScaleY"); } } 
        
        public Wiimote1DataContext()
        {
            ScaleX = 300.0;
            ScaleY = 200.0;


        }

        public void Update(object sender , PropertyChangedEventArgs e)
        {
            WiimoteCoordinate wm = (WiimoteCoordinate)sender;
            AxCoord = wm.AccelValues[0].ToString();
            AyCoord = wm.AccelValues[1].ToString();
            AzCoord = wm.AccelValues[2].ToString();
            Battery = wm.Battery;

            IR1_x = wm.IRCoordinates[0].X *ScaleX;
            IR1_y = wm.IRCoordinates[0].Y *ScaleY;
            IR2_x = wm.IRCoordinates[1].X *ScaleX;
            IR2_y = wm.IRCoordinates[1].Y *ScaleY;
            IR3_x = wm.IRCoordinates[2].X *ScaleX;
            IR3_y = wm.IRCoordinates[2].Y *ScaleY;
            IR4_x = wm.IRCoordinates[3].X *ScaleX;
            IR4_y = wm.IRCoordinates[3].Y *ScaleY;
            MidPoint_x = wm.MidPoint.X;
            MidPoint_y = wm.MidPoint.Y;
            ID = wm.ID;


        }





        #region NotifyEvent     
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify ( string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));

        }
        #endregion
    }
}
