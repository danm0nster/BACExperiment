using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

// Service class with most major functionality 

namespace BACExperiment
{
    /* In this service class , I plan on programming the functionality of the programm 
    that does not find place in the GUI files . As a general idea , here is where the 
    audio , video , and cursor position recordin will be programmed . 
       In this class there will also be the code that generates the Stimuly for the 
    users to follow and also probably the colision detection .
       Everything that is not a native method of the xaml.cs files will be stored here.
    */
   public class Service 
    { 
        /* 

        I'm not too happy about the current project architecture look. There is a Wii remote class and a graphical interface but the Wii remote class contains elements 
        of gui and also the gui contains instances of the WiimoteStatus classes which are in the model . This is not very efficient and organized architectural whise.
        What i will do now is add the Wii remote instances here in the service and will give the mainframe a instance of the service since it will need the complicated methods anyway .
        I will use the singleton pattern also for the service so we are shure that there will be no way of opening multiple services at once and running the risk of having different sources of input .

        */

        // Instance of the service to return for the singleton

        private int count = 0;
        // So this gives the service acces to the Wiimote methods to retrieve info .
        public WiimoteInfo wiimote1_info { get; set; } 
        public WiimoteInfo wiimote2_info { get; set; }

        private MainWindow observer;

        public Service (MainWindow observer)
        {
       
            this.observer = observer;
            wiimote1_info = new WiimoteInfo(this);
            wiimote2_info = new WiimoteInfo(this);

        }


        /// <summary>
        /// WiimoteInfo is initialized with a wiimote and 
        /// </summary>
        /// <param name="info"></param>

        public void SetUpWiimoteInfo(WiimoteInfo info)
        {
            /* This method is intended to make setting up a WiimoteInfo class as easy as possible so that even if we don`t need it now , the software will be easely moddified
            ToString handle more than wiiremote.
            */
            Console.WriteLine("Entered method");

            //If statement to make shure that we have max 2 wii remotes 
            if (count == 0 || count == 1)
            {
                Console.WriteLine("Entered if");
               

                //Each Wiimote object has a event Handler List called WiimoteChanged , so we can add the same method to two different wee remotes

                try
                {
                    //info.mWiimote.WiimoteChanged += wm_WiimoteChanged; // Add change event to it
                    Console.WriteLine("Added listener");

                    
                    if (count == 0)
                    {
                        info.Connect(count);
                        Console.WriteLine("Connected wm1");
                    }
                    if (count == 1)
                    {
                        info.Connect(count);
                        Console.WriteLine("Connected wm2");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                count++;

                Console.WriteLine(string.Concat("Wiimote ", count, " has been created"));

              
            }

            else
            {
                Console.WriteLine(string.Concat("There are currently", count, "wii remotes enabled. Blease disconnect 1 and try again."));
            }

        }

        public void DisconnectWiimoteFromInfo(WiimoteInfo info)
        {   int count = info.count;
            info.Disconnect(count);
            Console.WriteLine(string.Concat("Wiimote", count, " has been disconnected"));
            count--;
        }


        public void informMainWindow(WiimoteInfo sender)
        {
            observer.OnNext(sender);
        }

        /*
        private void InitializeWiimote1()
        {

            // Initialize the wiimotes
            // Not shure if the connection between the wii remote and the wiimoteinfo should be done before or after connecting the wii remote.

            wiimote1 = new Wiimote();
            Console.WriteLine("Wiimote 1 has been initiated");
            Console_Textbox_WM1.AppendText("Wiimote 1 has been initiated\n");
            //wiimote1_info.Wiimote = wiimote1;
            Console.WriteLine("Wiimote 1 has connected to it's container;");
            Console_Textbox_WM1.AppendText("Wiimote 1 has been initiated\n");


            //After initializing the wii remote we add the event change listener to each one of them
            wm1.WiimoteChanged += wm1_WiimoteChanged;
            Console.WriteLine("Wiimote 1 now has change listener;");
            Console_Textbox_WM1.AppendText("Wiimote 1 now has change listener\n");



            try
            {
                wiimote1.Connect();
                wiimote1.SetReportType(InputReport.IRAccel, true);
            }
            catch (WiimoteException ex)
            {
                Console.WriteLine(ex.ToString());
                Console_Textbox_WM1.AppendText(ex.ToString() + "\n");
            }
            /*
        We have the possibility with the WiimoteLib to add extentions to our programm such as
        the wii drum kit , wii nunchucks etc. for our experiment it is not currently neccesary. 
        



        }


         Method was innitially in MainWindow 

        public void InitializeWiimote2()
        {

            // Initialize the wiimotes
            // Not shure if the connection between the wii remote and the wiimoteinfo should be done before or after connecting the wii remote.


            wm2 = new Wiimote();
            Console.WriteLine("Wiimote 2 initialized;");
            wm2_info.Wiimote = wm2;
            Console.WriteLine("Wiimote 2 has connected to it's container;");

            //After initializing the wii remote we add the event change listener to each one of them

            wm2.WiimoteChanged += wm2_WiimoteChanged;
            Console.WriteLine("Wiimote 2 now has change listener;");

            wm2.Connect();
            wm2.SetReportType(InputReport.IRAccel, true);
            
            //We have the possibility with the WiimoteLib to add extentions to our programm such as
            //the wii drum kit , wii nunchucks etc. for our experiment it is not currently neccesary. 
            



        }

        
         Not completelly shure if we need 2 methods for the change event of the wii remote . After implementing the observer pattern , will test out to see if
         it is possible to work with only one method.
        *///Possibly obsolete code
       




      
    }

   
}
