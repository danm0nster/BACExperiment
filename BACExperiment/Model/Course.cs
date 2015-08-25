using System;
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

        public void firstFuntion()
        {
            double x, y;
            int a = 5;
            int b = 4;
            int A = 1000; // X Range in which the sfere will run around in
            int B = 1000; // Y Range in which the sfere will run around in
            int d = (int)(Math.PI / 2*100);
            double t;
            for (int i = 0; i <= 2000; i++)
            {
               
                t = 4 * System.Math.PI / 2000 * (double)i;
                x = A * System.Math.Sin(a * t + d)  +500 ;
                y = B * System.Math.Sin(b * t) +500 ;
                window.animateEllipse1To(x,y,1000);
                
            }
           

        }

        public float secondFunction(int x , int y)
        {
            return 0;

        }

        public float thirdFunction(int x , int y)
        {
            return 0;
        }

        public static bool Stop()
        {
            running = false;
            return running;
        }

        public void moveSphereDiagonallyTest()
        {
            for(int i = 0; i <=500; i++)
            {
                window.animateEllipse1To(i,i,1);
            }
        }
    }
}
