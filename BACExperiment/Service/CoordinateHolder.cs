using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BACExperiment
{
    class CoordinateHolder
    {
        private static CoordinateHolder instance;

        private Point EllipseCoordinates;
        private Point[] PointerCoordinates ;
        



        public static CoordinateHolder getInstance()
        {
            if (instance == null)
                instance = new CoordinateHolder();
            return instance;
        }

        private CoordinateHolder()
        {
            EllipseCoordinates = new Point();
            PointerCoordinates = new Point[2];
        }

        public void setEllipseCoordinates(double x , double y)
        {
            EllipseCoordinates.X = x;
            EllipseCoordinates.Y = y;

        }
         
        public Point getEllipseCoordinates()
        {
            return EllipseCoordinates;
        }

        public void setPointerCoordinates(int index , Point p)
        {
            PointerCoordinates[index] = p;
        }

        public Point getPointerCoordinates(int index)
        {
            return PointerCoordinates[index];
        }

    }
}
