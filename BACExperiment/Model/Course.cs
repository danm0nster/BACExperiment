using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BACExperiment.Model
{
    class CourseThread 
    {

        private Thread course = new Thread(Run());
        private Random random = new Random();
        private int courseNumber;
        private static bool running = true; 

        public CourseThread()
        {
            courseNumber = random.Next(3); 
            
        }

        private static ThreadStart Run()
        {
            while(running)
            {

            }

            return null;
        }

        private static bool Stop()
        {
            running = false;
            return running;
        }
    }

    class Sphere
    {
    }

}
