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
        private StimulyWindow observee;

        private System.Timers.Timer  timer = new System.Timers.Timer();
        private String directoryPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);// file path for the current sessionLogs
        private String logName = System.DateTime.Today.ToString();
        private String logPath;
        private Stopwatch watch = new Stopwatch();
       
        private static coordinateRecorder instance;

        private CoordinateHolder holder;


        public static coordinateRecorder getInstance(StimulyWindow observee)
        {
            if (instance == null)
                instance = new coordinateRecorder(observee);

            return instance;
        }

        private coordinateRecorder(StimulyWindow observee)
        {
            this.observee = observee;
        }

        public void Run()
        {

            sessionLogDirectoryPresent();
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.txt"); // works but ads x at the end of the file
            logPath = System.IO.Path.Combine(directoryPath, fileName);
     
            timer.Interval = 100; // setting the frequency at which the thread will register the current coordinates in the sessionLog
            timer.Elapsed += new ElapsedEventHandler(record);
            holder = CoordinateHolder.getInstance();

            timer.Enabled = true;
            watch.Start();
        }

        public void Stop()
        {
            timer.Enabled = false;
            watch.Stop();

        }


        //Test if there is a session File in the current assembly execution path . If not create a new one entirely.
        public void sessionLogDirectoryPresent()
        {
            if (!Directory.Exists(directoryPath + "\\Session Logs"))
            {
                Directory.CreateDirectory(directoryPath + "\\Session Logs");
            }

        }


        public void record(object sender, ElapsedEventArgs args)
        {

            // Writes the file but puts every info in a new line , also does not format the session name properly 
            // Writes a string every time but now it 
            // First attemtp always give exception that current file is being used . Look into that because you can not run 2 times for 1 file since the name is given by the time of the creation which is always different


            if (!File.Exists(logPath))
                File.Create(logPath);

            DateTime t = args.SignalTime; // Take the time the tick was done
            Point ellipseCoordiante = holder.getEllipseCoordinates();  // Parse coordinates from StimulyWindow to service and then to thread where they will be recorded into the log file
            Point controller1 = holder.getPointerCoordinates(0);
            Point controller2 = holder.getPointerCoordinates(1);
            string toWrite = string.Concat( t.Ticks.ToString(), " ", watch.Elapsed.ToString(), " ", ellipseCoordiante.ToString(), " ", controller1.ToString(), " ", controller2.ToString() );


            using (FileStream f = new FileStream(logPath, FileMode.Append, FileAccess.Write))
            using (StreamWriter s = new StreamWriter(f))
                s.WriteLine(toWrite.ToString());

            
           


        }


    }
}
