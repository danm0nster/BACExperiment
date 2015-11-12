using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace BACExperiment.Model
{
    class Course 
    {
        
        //private Thread course = new Thread(Run());
        private Random random = new Random();
        private int courseNumber;
        private StimulyWindow window;


        public Course( StimulyWindow window)
        {
            courseNumber = random.Next(3);
            this.window = window;
            
        }

        

        public List<Point> firstFuntion()
        {
            List<Point> coordinates = new List<Point>();
            double x, y;
            int a = 5;
            int b = 4;
            int A = (int)window.ActualWidth / 3;// X Range in which the sfere will run around in
            int B = (int)window.ActualHeight / 3;  // Y Range in which the sfere will run around in
            double d = Math.PI / 2;
            //float f = 3 / 2;
            float o = 2 / 7;
            double t;
            double lastX = 0;
            double lastY = 0;
            for (int i = 0; i <= 4000; i++)
            {
               
                t = 8* Math.PI / 4000 * (double)i;
                x = Math.Cos(o * t)*A*Math.Sin(a*t+ d) + Math.Sin(o*t)*B*Math.Sin(b*t); //A * System.Math.Sin(a * t + d); //cos(o*t)*A*sin(a*t+d)+sin(o*t)*B*sin(b*t)
                y = -Math.Sin(o * t) * A * Math.Sin(a * t + d) + Math.Cos(o * t) * B * Math.Sin(b * t); // B * System.Math.Sin(b * t); // -sin(o*t)*A*sin(a*t+d)+cos(o*t)*B*sin(b*t)
                if (Math.Abs(lastX - x) > 5 || Math.Abs(lastY - y) > 5)
                {
                    coordinates.Add(new Point(x, y));
                    lastX = x;
                    lastY = y;
                }

            }

            return coordinates;

        }

        public List<Point> secondFunction()
        {
            List<Point> coordinates = new List<Point>();
            double x, y;
            int a = 9;
            int b = 10;
            int A =100; // X Range in which the sfere will run around in
            int B =100; // Y Range in which the sfere will run around in
            double d = Math.PI / 2;
            int g = 2;
            double f = 1.5;
            double t;
            double lastX = 0;
            double lastY = 0;
            for (int i = 0; i <= 4000; i++)
            {

                t = 8 * System.Math.PI / 4000 * i; // angle of the point relative to the origin
                x = A * Math.Exp(-g * Math.Cos(3 * b * t)) * Math.Sin(a * t + d);
                y = B * Math.Exp(-f * Math.Cos(2 * a * t + d)) * Math.Sin(b * t);
                if (Math.Abs(lastX - x) > 5 || Math.Abs(lastY - y) > 5)
                {
                    coordinates.Add(new Point(x, y));
                    lastX = x;
                    lastY = y;
                }

            }

            return coordinates;

        }

        public List<Point> thirdFunction()
        {
            List<Point> coordinates = new List<Point>();
            double x, y;
            int a = 9;
            int b = 10;
            int A = 1; // X Range in which the sfere will run around in
            int B = 1; // Y Range in which the sfere will run around in
            double d = Math.PI / 2 ;
            int g = 2;
            double f = 1.5;
            double t;
            double lastX = 0;
            double lastY = 0;

            for (int i = 0; i <= 4000; i++)
            {

                t = 8 * System.Math.PI / 4000 * (double)i;
                x = A * Math.Exp(-g * Math.Pow(Math.Cos(2.1 * b * t) ,2)) * Math.Sin(a * t + d) ; 
                y = B * Math.Exp(-f * Math.Pow(Math.Cos(1.9 * a * t + d) ,2)) * Math.Sin(b * t) ;
                if (Math.Abs(lastX - x) > 5 || Math.Abs(lastY - y) > 5)
                {
                    coordinates.Add(new Point(x, y));
                    lastX = x;
                    lastY = y;
                }
            }

            return coordinates;
        } 
    }
}
