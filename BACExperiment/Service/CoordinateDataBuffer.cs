using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BACExperiment
{

    /// <summary>
    /// The CoordinateHolder object is used to contain coordinates recieved from the MovementWindow with the purpose of recording them with the CoordinateRecorder class.
    /// It serves as a small buffer between the MovementWIndow and the CoordinateRecorder class. If we were to store the coordinates directly into the CoordinateRecorder object,
    /// we would have problems with thread concurency over the same resources.
    /// </summary>
    class CoordinateDataBuffer
    {

        private static CoordinateDataBuffer instance;

        private Point _ellipseCoordinates;
        private Point[] PointerCoordinates;
        private Double[,] AccelValues;





        public static CoordinateDataBuffer GetInstance() => instance ?? (instance = new CoordinateDataBuffer());

        private CoordinateDataBuffer()
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

        internal void Clear()
        {
            instance = null;
        }
    }
}