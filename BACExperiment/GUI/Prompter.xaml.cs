﻿using System;
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
using System.Threading;

namespace BACExperiment.GUI
{
    /// <summary>
    /// Interaction logic for Prompter.xaml
    /// </summary>
    /// 


    public partial class Prompter : Window
    {

        private string path = "C:/Users/labclient/Documents/Visual Studio 2015/Projects/BACExperiment/BACExperiment/resources/prompterText.txt";
        private System.Timers.Timer traversallTimer = new System.Timers.Timer();
        private System.Timers.Timer colorTimer = new System.Timers.Timer();
        private System.Timers.Timer scrollTimer = new System.Timers.Timer();
        private System.Timers.Timer switchTimer = new System.Timers.Timer();
        private int textSize;
        private int index = 0;
        private System.Windows.Media.Brush brush1 = System.Windows.Media.Brushes.White;
        private System.Windows.Media.Brush brush2 = System.Windows.Media.Brushes.White;
        private System.Windows.Media.Brush brush3 = System.Windows.Media.Brushes.White;
        private int direction = 0;


        public void setPath(string path) { this.path = path; }
        public string getPath() { return path; }

        public Prompter(int fontSize, string path, int _ScrollDowndSpeed )
        {
            try
            {
                this.path = path;
                this.textSize = fontSize;
                InitializeComponent();

                scrollTimer.Elapsed += new ElapsedEventHandler(ScrollDownEvent);
                scrollTimer.Interval = 600 * _ScrollDowndSpeed;

                StreamReader reader = new StreamReader(@path, Encoding.Default, true);
                prompterText.AppendText(reader.ReadToEnd());
                prompterText.FontSize = textSize;
                prompterText.HorizontalAlignment = HorizontalAlignment.Center;

                TextPointer text = prompterText.Document.ContentStart;
                while (text.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
                {
                    text = text.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Prompter(int fontSize , string path , int _TraversalSpeed , System.Windows.Media.Color color1)
        {
            try
            {
                this.path = path;
                this.brush1 = new SolidColorBrush(color1);
                this.textSize = fontSize;

                InitializeComponent();

                traversallTimer.Elapsed += new ElapsedEventHandler(TraversalEvent);
                traversallTimer.Interval = 1000 / _TraversalSpeed;

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
                TextPointer endPos = text.GetPositionAtOffset(index + 30);
                prompterText.Selection.Select(startPos, endPos);
                prompterText.SelectionBrush = brush1;
               

            }

            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public Prompter(int fontSize, string path, int _TraversalSpeed, int _TurnDuration, int _SwitchDuration , System.Windows.Media.Color color1 , System.Windows.Media.Color color2 , System.Windows.Media.Color color3)
        {
            try
            {
                this.path = path;
                this.brush1 = new SolidColorBrush(color1);
                this.brush2 = new SolidColorBrush(color2);
                this.brush3 = new SolidColorBrush(color3);
                this.textSize = fontSize;

                InitializeComponent();

                traversallTimer.Elapsed += new ElapsedEventHandler(TraversalEvent);
                traversallTimer.Interval = 1000 / _TraversalSpeed;
                colorTimer.Elapsed += new ElapsedEventHandler(ColorSwitchEvent);
                colorTimer.Interval = 1000 * _TurnDuration;
                switchTimer.Elapsed += new ElapsedEventHandler(ColorSwitchEvent2);
                switchTimer.Interval = 1000 * _SwitchDuration;

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
                TextPointer endPos = text.GetPositionAtOffset(index + 30);
                prompterText.Selection.Select(startPos, endPos);
                prompterText.SelectionBrush = brush1;

            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void ColorSwitchEvent2(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                   {
                       if (direction == 0)
                       {
                           prompterText.Focus();
                           prompterText.SelectionBrush = brush3;
                       }
                       if (direction == 1)
                       {
                           prompterText.Focus();
                           prompterText.SelectionBrush = brush1;
                       }
                       switchTimer.Stop();
                   }));
             

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TraversalEvent(object Sender, ElapsedEventArgs args)
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
                    TextPointer endPos = text.GetPositionAtOffset(index + 30);

                    prompterText.Focus();
                    prompterText.Selection.Select(startPos, endPos);


                    var start = prompterText.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
                    var end = prompterText.Selection.End.GetCharacterRect(LogicalDirection.Forward);
                    scroller.ScrollToVerticalOffset(text.GetOffsetToPosition(endPos));
                }));
            }

            catch(Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void ColorSwitchEvent(object Sender, ElapsedEventArgs args)
        {
            try {
                this.Dispatcher.Invoke((Action)(() =>
                    {
                        if (prompterText.SelectionBrush.Equals(brush1))
                        {
                            direction = 0;
                            prompterText.Focus();
                            prompterText.SelectionBrush = brush2;
                            switchTimer.Start();
                            
                        }
                        else
                        if(prompterText.SelectionBrush.Equals(brush3))
                        {
                            direction = 1;
                            prompterText.Focus();
                            prompterText.SelectionBrush = brush2;
                            switchTimer.Start();
                        }
                    }
                    ));
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void ScrollDownEvent(object Sender, ElapsedEventArgs args)
        {
            try
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
            traversallTimer.Enabled = true;
            colorTimer.Enabled = true;
        }

        public void textSize_Changed(int size)
        {
            prompterText.FontSize = size;
        }

        
    }
}
