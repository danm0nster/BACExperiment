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
        private Timer scrollTimer = new Timer();
        private Timer colorTimer = new Timer();
        private int color = 0;
        private int textSize;
        private int index = 0;
        


        public void setPath(string path) { this.path = path; }
        public string getPath() { return path; }


        public Prompter(int speed, int changeSpeed , int fontSize, string path , int mode)
        {
            try
            { 
                this.path = path;
              
                this.textSize = fontSize;
                InitializeComponent();
                if (mode == 1) {
                    scrollTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
                    scrollTimer.Interval = 100 / speed;
                }
                if (mode == 2)
                {
                    scrollTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent1);
                    scrollTimer.Interval = 100 / speed;
                    colorTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
                    colorTimer.Interval = changeSpeed * 1000 ;
                }

                if(mode == 3)
                {
                    scrollTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent3);
                    scrollTimer.Interval = 600;     
                }

                StreamReader reader = new StreamReader(@path, Encoding.Default, true);
                prompterText.AppendText(reader.ReadToEnd());
                prompterText.FontSize = textSize;
                prompterText.HorizontalAlignment = HorizontalAlignment.Center;

                TextPointer text = prompterText.Document.ContentStart;
                while (text.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
                {
                    text = text.GetNextContextPosition(LogicalDirection.Forward);
                }
                TextPointer startPos = text.GetPositionAtOffset(index);
                TextPointer endPos = text.GetPositionAtOffset(index+10);
                prompterText.Selection.Select(startPos, endPos);
                this.prompterText.SelectionBrush = System.Windows.Media.Brushes.Aqua;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void OnTimedEvent1(object Sender, ElapsedEventArgs args)
        {
            try {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    index++;
                    TextPointer text = prompterText.Document.ContentStart;
                    while (text.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
                    {
                        text = text.GetNextContextPosition(LogicalDirection.Forward);
                    }
                    TextPointer startPos = text.GetPositionAtOffset(index);
                    TextPointer endPos = text.GetPositionAtOffset(index + 10);

                    prompterText.Focus();
                    prompterText.Selection.Select(startPos, endPos);


                    var start = prompterText.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
                    var end = prompterText.Selection.End.GetCharacterRect(LogicalDirection.Forward);
                    prompterText.ScrollToVerticalOffset((start.Top + end.Bottom - prompterText.ViewportHeight) / 2 + prompterText.VerticalOffset);
                }));
            }

            catch(Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void OnTimedEvent2(object Sender, ElapsedEventArgs args)
        {
            try {
                this.Dispatcher.Invoke((Action)(() =>
                    {
                        if (color % 2 == 0)
                            prompterText.SelectionBrush = System.Windows.Media.Brushes.Aqua;
                        else
                            prompterText.SelectionBrush = System.Windows.Media.Brushes.Salmon;

                        color++;
                    }
                    ));
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void OnTimedEvent3(object Sender, ElapsedEventArgs args)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    scroller.Focus();
                    scroller.ScrollToVerticalOffset(scroller.VerticalOffset + prompterText.FontSize/20);
                }
                ));
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
            }
        }




        public void play()
        {
            scrollTimer.Enabled = true;
            colorTimer.Enabled = true;
        }

        public void textSize_Changed(int size)
        {
            prompterText.FontSize = size;
        }

        public void ScrollToSelection(RichTextBox viewer)
        {
            TextPointer t = viewer.Selection.Start;
            FrameworkContentElement e = t.Parent as FrameworkContentElement;
            if (e != null)
                e.BringIntoView();
        }
    }
}
