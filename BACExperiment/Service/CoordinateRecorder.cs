using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace BACExperiment
{
    //NEED TO DEBUG
    class coordinateRecorder
    {
       

        private System.Timers.Timer timer;
        private String directoryPath;// file path for the current sessionLogs
        private String logPath;
        private Stopwatch watch;
       
        private static coordinateRecorder instance;

        private CoordinateHolder holder;


        public static coordinateRecorder getInstance(StimulyWindow observee)
        {
            if (instance == null)
                instance = new coordinateRecorder();

            return instance;
        }

        private coordinateRecorder()
        {
            holder = CoordinateHolder.GetInstance();

            timer = new System.Timers.Timer();
            directoryPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            watch = new Stopwatch();

            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
            logPath = System.IO.Path.Combine(SessionLogDirectoryPresent(), fileName);
        }

        public void Run()
        {
            timer.Interval = 175; // setting the frequency at which the thread will register the current coordinates in the sessionLog
            timer.Elapsed += new ElapsedEventHandler(Record);

            timer.Enabled = true;
            watch.Start();
        }

        public void Stop()
        {
            timer.Enabled = false;
            watch.Stop();

        }


        //Test if there is a session File in the current assembly execution path . If not create a new one entirely.
        public string SessionLogDirectoryPresent()
        {
            if (!Directory.Exists(directoryPath + "\\Session Logs"))
            {
                Directory.CreateDirectory(directoryPath + "\\Session Logs");
            }

            return directoryPath + "\\Session Logs";

        }


        public void Record(object sender, ElapsedEventArgs args)
        {
          
            // First attemtp always give exception that current file is being used . Look into that because you can not run 2 times for 1 file since the name is given by the time of the creation which is always different


          

            DateTime t = args.SignalTime; // Take the time the tick was done
            Point ellipseCoordiante = holder.GetEllipseCoordinates();  // Parse coordinates from StimulyWindow to service and then to thread where they will be recorded into the log file
            Point controller1 = holder.GetPointerCoordinates(0);
            Point controller2 = holder.GetPointerCoordinates(1);
            double[,] accel = holder.getAccelValues();
            string toWrite = string.Concat(watch.Elapsed.ToString(), " ", ellipseCoordiante.ToString(), " ", controller1.ToString(), " ", controller2.ToString() , 
                accel[0,0], accel[0, 1], accel[0, 2], accel[1, 0], accel[1, 1], accel[1, 2]  );



            using (StreamWriter s = new StreamWriter(logPath, true))
            {
                s.WriteLine(toWrite);
                s.Close();
            }
              

            
           


        }


    }
}
