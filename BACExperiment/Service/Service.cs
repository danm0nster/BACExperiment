using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;
using System.Threading;

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

      
        // So this gives the service acces to the Wiimote methods to retrieve info .
        private WiimoteInfo wiimote1_info { get; set; } 
        public static Service instance;


        //Make two threads to test if that improves the GUI responsiveness of the led points. Theoretically it should as there would be way less calls to the OnNext method . Ideally 
        //it would have a frequence of around 60 to 100 fps. 

     

        //Made service as a Singleton

        public static Service getInstance(MainWindow observer)
        {
            if (instance == null)
                instance = new Service(observer);
            return instance;
        }

        private MainWindow observer;

        private Service (MainWindow observer)
        {
       
            this.observer = observer;
            wiimote1_info = new WiimoteInfo(this);
        }

        public void ConnectWiimoteToInfo(int i)
        {
            wiimote1_info.Connect(i);
            Console.WriteLine(string.Concat("Wiimote", i, " has been connected"));
        }

        public void DisconnectWiimoteFromInfo(int i)
        {   
            wiimote1_info.Disconnect(i);
            Console.WriteLine(string.Concat("Wiimote", i , " has been disconnected"));
            
        }


        public void informMainWindow(WiimoteInfo sender, Wiimote wm)
        {
            try
            {
                observer.OnNext(sender, wm);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
               
            }
        }      
    }

   
}
