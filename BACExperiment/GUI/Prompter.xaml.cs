using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BACExperiment.GUI
{
    /// <summary>
    /// Interaction logic for Prompter.xaml
    /// </summary>
    /// 

   
    public partial class Prompter : Window
    {

        private string path = "C:/Users/labclient/Documents/Visual Studio 2015/Projects/BACExperiment/BACExperiment/resources/prompterText.txt";
        private string text;
        private Timer myTimer = new Timer();
        private Double scrollPosition = 0;

        public void setPath(string path) { this.path = path; }
        public string getPath() { return path; }


        public Prompter()
        {
            InitializeComponent();
            myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            myTimer.Interval += 100;
            myTimer.Enabled = true;


            try
            {
                text = System.IO.File.ReadAllText(path);
                
                prompterText.Text = text;
                prompterText.TextAlignment = TextAlignment.Left;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void OnTimedEvent(object Sender, ElapsedEventArgs args)
        {
            start();
        }

        private void start()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                scroller.ScrollToVerticalOffset(scrollPosition);
                scrollPosition += 1;
            }));
            
        }

        private void stop()
        {
            throw new NotImplementedException();
        }
    }
}
