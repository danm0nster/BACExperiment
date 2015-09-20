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
            try
            {

                this.path = path;
                this.speed = speed;
                this.textSize = fontSize;
                InitializeComponent();
                myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                myTimer.Interval += 100*speed;

                text = System.IO.File.ReadAllText(path);
                prompterText.AppendText(File.ReadAllText(@path));
                prompterText.FontSize = textSize;
                prompterText.HorizontalAlignment = HorizontalAlignment.Center;
               

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

        class WordPointer
        {
            

            private int startPoz = 0;
            private int stopPoz = 0;
            private RichTextBox textBox;
            private bool started;
            private string word;

            private string text;
            public WordPointer(RichTextBox textBox)
            {
                this.textBox = textBox;
                text = textBox.Selection.Text;
            }


            public void startWordPointer()
            {
                while(stopPoz!=text.Length)
                {
                    findNextWord();
                    colorWord();
                }

            }


            private void findNextWord()
            {
               
                if (started) //If we already found a word we move stopPoz ahead
                    stopPoz++;

               
                startPoz = stopPoz; //Make the start pozition catch up to the stopPoz
                bool found = false;

                while (!found) // Search for the next space character . There will be the end of the word
                { 
                    stopPoz++;
                if (text[stopPoz].Equals(" ")) 
                    {
                        found = true; // Congratulations we have hound a word.
                       
                    }
                   
                }
            }

            private void colorWord()
            {

                TextRange rangeOfText1 = new TextRange(textBox.Document.ContentEnd, textBox.Document.ContentEnd);
                
                rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
        }
    }
}
