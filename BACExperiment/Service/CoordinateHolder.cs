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

        private Point _ellipseCoordinates;
        private Point[] PointerCoordinates;
        private Double[,] AccelValues;





        public static CoordinateHolder GetInstance() => instance ?? (instance = new CoordinateHolder());

        private CoordinateHolder()
        {
            _ellipseCoordinates = new Point();
            PointerCoordinates = new Point[2];
            AccelValues = new Double[2, 3];
        }

        public void SetEllipseCoordinates(double x, double y)
        {
            _ellipseCoordinates.X = x;
            _ellipseCoordinates.Y = y;

        }

        public void SetAccel(Double[,] accelValues)
        {

            AccelValues = accelValues;

        }


        public double[,] getAccelValues()
        {
            return AccelValues;
        }

        public Point GetEllipseCoordinates()
        {
            return _ellipseCoordinates;
        }

        public void SetPointerCoordinates(int index, Point p)
        {
            PointerCoordinates[index] = p;
        }

        public Point GetPointerCoordinates(int index)
        {
            return PointerCoordinates[index];
        }

    }
}