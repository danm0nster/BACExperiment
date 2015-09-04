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
        private int textSize;
        private int speed;

        public void setPath(string path) { this.path = path; }
        public string getPath() { return path; }


        public Prompter(int speed , int fontSize , string path)
        {
            this.path = path;
            this.speed = speed;
            this.textSize = fontSize;
            InitializeComponent();
            myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            myTimer.Interval += 100*speed;

          


            try
            {
                text = System.IO.File.ReadAllText(path);
                
                prompterText.Text = File.ReadAllText(@path);
                prompterText.FontSize = textSize;
                prompterText.TextAlignment = TextAlignment.Left;
                prompterText.TextWrapping = TextWrapping.Wrap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void OnTimedEvent(object Sender, ElapsedEventArgs args)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                scroller.ScrollToVerticalOffset(scrollPosition);
                scrollPosition += 1;
            }));
        }

        public void play()
        {
            myTimer.Enabled = true;

            if (myTimer.Interval == 0)
                myTimer.Interval = speed;
        }

        public void pause()
        {
            myTimer.Interval += 0;
        }

        public void stop()
        {
            throw new NotImplementedException();
        }

        public void textSize_Changed(int size)
        {
            prompterText.FontSize = size;
        }

        public void speed_Changed(int speed)
        {
            this.speed = speed; 
            myTimer.Interval += 0;
            myTimer.Interval += 100 * speed;
        }
    }
}
