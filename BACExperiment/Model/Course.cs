using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace BACExperiment.Model
{
    class CourseThread 
    {
        
        //private Thread course = new Thread(Run());
        private Random random = new Random();
        private int courseNumber;
        private static bool running = true;
        private StimulyWindow window;


        public CourseThread( StimulyWindow window)
        {
            courseNumber = random.Next(3);
            this.window = window;
            
        }

        /*
        public static ThreadStart Run()
        {
            while(running)
            {

                // calculate function and rezulted coordinates
                //call the method in Stimuly window to display the coordinates
                //Wait 100 ms so that the elipse can be displayed 

            }

            return null;
        }
        */

        public List<Point> firstFuntion()
        {
            List<Point> coordinates = new List<Point>();
            double x, y;
            int a = 5;
            int b = 4;
            int A = 500; // X Range in which the sfere will run around in
            int B = 500; // Y Range in which the sfere will run around in
            int d = (int)(Math.PI / 2*100);
            double t;
            for (int i = 0; i <= 2000; i++)
            {
               
                t = 4 * System.Math.PI / 2000 * (double)i;
                x = A * System.Math.Sin(a * t + d) ;
                y = B * System.Math.Sin(b * t) ;
                coordinates.Add(new Point(x, y));
                
            }

            return coordinates;

        }

        public List<Point> secondFunction()
        {
            List<Point> coordinates = new List<Point>();
            double x, y;
            int a = 9;
            int b = 10;
            int A = 75; // X Range in which the sfere will run around in
            int B = 75; // Y Range in which the sfere will run around in
            int d = (int)(Math.PI / 2 * 100);
            int g = 2;
            double f = 1.5;
            double t;
            for (int i = 0; i <= 2000; i++)
            {

                t = 4 * System.Math.PI / 2000 * (double)i;
                x = A * Math.Exp(-g * Math.Cos(3 * b * t)) * Math.Sin(a * t + d);
                y = B * Math.Exp(-f * Math.Cos(2 * a * t + d)) * Math.Sin(b * t);
                coordinates.Add(new Point(x, y));

            }

            return coordinates;

        }

        public List<Point> thirdFunction()
        {
            List<Point> coordinates = new List<Point>();
            double x, y;
            int a = 9;
            int b = 10;
            int A = 75; // X Range in which the sfere will run around in
            int B = 75; // Y Range in which the sfere will run around in
            int d = (int)(Math.PI / 2 * 100);
            int g = 2;
            double f = 1.5;
            double t;
            for (int i = 0; i <= 2000; i++)
            {

                t = 4 * System.Math.PI / 2000 * (double)i;
                x = A * Math.Exp(-g * Math.Pow(Math.Cos(2.1 * b * t) ,2) * Math.Sin(a * t + d)) -50; //-50 for the elipse values
                y = B * Math.Exp(-f * Math.Pow(Math.Cos(1.9 * a * t + d) ,2) * Math.Sin(b * t)) -50; 
            coordinates.Add(new Point(x, y));

            }

            return coordinates;
        }

        public static bool Stop()
        {
            running = false;
            return running;
        }

       
        
    }
}
