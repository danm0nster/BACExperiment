using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WiimoteLib;

namespace BACExperiment.Model
{

    /*Instead of having the wii remotes in a normal class , I am considering the possibility to have the remotes stored in threads which will run continuouselly. The threads then call the informMainWindow
    method from service . which then calls the OnNext method in the Main Window. For that the informMethod in service should me deffined as a critical region since the two threads should not be in that
    method at the same time but to alternate the ose of it between them .
    */
    class WiimoteThread
    {

        private Thread thread = new Thread(Run());
        private Wiimote remote = new Wiimote();
        private Service observer;
        private static bool running;


        public WiimoteThread(Service service, Wiimote remote)
        {
            this.observer = service;
            this.remote = remote;
        }

        private static ThreadStart Run()
        {
            while (running)
            {
              

            }

            return null;
        }

        public void Connect(int i)
        {

            try
            {
                remote.Connect();
                remote.SetReportType(InputReport.IRAccel, true);
                if (i == 0)
                    remote.SetLEDs(true, false, false, false);
                if (i == 1)
                    remote.SetLEDs(false, true, false, false);
                remote.WiimoteChanged += wm_WiimoteChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Disconnect(int i)
        {
            running = false;
            remote.SetLEDs(false, false, false, false);
            remote.Disconnect();
        }

        public void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            // current state information

            Wiimote wm = (Wiimote)sender;
            UpdateWiimoteChanged(args);
            observer.informMainWindow(this, (Wiimote)sender);
            //Change If statements from using "==" to use " Equals " method


            /*if (wm.ID.Equals(wiimote1_info.getWiimoteID()))
                observer.OnNext(wiimote1_info);
            else if (wm.ID.Equals(wiimote2_info.getWiimoteID()))
                observer.OnNext(wiimote2_info);
            */
            Console.WriteLine(String.Concat(args.WiimoteState.AccelState.Values.ToString()));

        }

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            WiimoteThread.UpdateWiimoteStateDelegate updateWiimoteStateDelegate = new WiimoteThread.UpdateWiimoteStateDelegate(this.UpdateWiimoteChanged);
            object[] objArray = new object[] { args };

        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            WiimoteState wiimoteState = args.WiimoteState;

          
        }

        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
    }
}
