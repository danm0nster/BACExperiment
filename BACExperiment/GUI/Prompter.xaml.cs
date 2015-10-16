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
using System.Windows.Media.Animation;

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
        private Double scrollPosition = 1;
        private int textSize;
        private int speed;
        
    
        public void setPath(string path) { this.path = path; }
        public string getPath() { return path; }


        public Prompter(int speed , int fontSize , string path)
        {
            try
            {

                this.path = path;
                this.speed = speed;
                this.textSize = fontSize;
                InitializeComponent();
                myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
                myTimer.Interval = 100/speed;

                text = System.IO.File.ReadAllText(path);
                prompterText.AppendText(File.ReadAllText(@path));
                prompterText.FontSize = textSize;
                prompterText.HorizontalAlignment = HorizontalAlignment.Center;
                prompterText.Focus();

                prompterText.SelectionStart = 0;
                prompterText.SelectionLength = 20;
                prompterText.SelectionBrush = System.Windows.Media.Brushes.Blue;

              

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void OnTimedEvent2(object Sender, ElapsedEventArgs args)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                prompterText.Focus();
                prompterText.SelectionStart++;
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

       

        private class AnimationQueue
        {
            private List<DoubleAnimation> animation1;
            private DependencyProperty property1;

            private List<DoubleAnimation> animation2;
            private DependencyProperty property2;

           // private int curent;
            private UIElement element;

            public AnimationQueue(UIElement element, DependencyProperty property)
            {
           //   curent = -1;
                this.element = element;
                animation1 = new List<DoubleAnimation>();
                animation2 = new List<DoubleAnimation>();
                this.property1 = property;
            }

            public AnimationQueue(UIElement element, DependencyProperty property1, DependencyProperty property2)
            {
           //   curent = -1;
                this.element = element;
                animation1 = new List<DoubleAnimation>();
                animation2 = new List<DoubleAnimation>();
                this.property1 = property1;
                this.property2 = property2;
            }

            public void queueAnimation(DoubleAnimation animation1, DoubleAnimation animation2)
            {

                this.animation1.Add(animation1);
                this.animation2.Add(animation2);
                animation1.Completed += (s, e) =>
                {
                    if (this.animation1.Contains(animation1))
                    {
                        int index1 = this.animation1.IndexOf(animation1);
                        if (index1 + 1 < this.animation1.Count)
                        {
                            element.BeginAnimation(property1, this.animation1[index1 + 1]);
                        }
                    }
                };
                animation2.Completed += (s, e) =>
                {
                    if (this.animation2.Contains(animation2))
                    {
                        int index2 = this.animation2.IndexOf(animation2);
                        if (index2 + 1 < this.animation2.Count)
                        {
                            element.BeginAnimation(property2, this.animation2[index2 + 1]);

                        }
                    }
                };



            }
        }
    }
}
