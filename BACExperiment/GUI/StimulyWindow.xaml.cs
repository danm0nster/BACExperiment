using BACExperiment.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BACExperiment
{
    /// <summary>
    /// Interaction logic for StimulyWindow.xaml
    /// </summary>
    public partial class StimulyWindow : Window
    {
        private CourseThread course;

        private AnimationQueue queue1;
        private MainWindow mainWindow;

        private int CourseMode;
        private int CourseSpeed;
        private int CourseComplexity;
        private bool random;
        private Random r = new Random();

        public int getCourseMode() { return CourseMode; }
        public void setCourseMode(int CourseMode) { this.CourseMode = CourseMode; }
        public int getCourseSpeed() { return CourseSpeed; }
        public void setCourseSpeed(int CourseSpeed) { this.CourseSpeed = CourseSpeed; }
        public int getCourseComplexity() { return CourseComplexity; }
        public void setCourseComplexity(int CourseComplexity) { this.CourseComplexity = CourseComplexity};
        public bool isRandom() { return random; }
        public void setRandom(bool random) { this.random = random; }

        public StimulyWindow(MainWindow mainWindow )
        {
            InitializeComponent();
            course = new CourseThread(this);
            this.queue1 = new AnimationQueue(StimulyEllipse1 , Canvas.TopProperty , Canvas.LeftProperty);
            this.mainWindow = mainWindow;
            random = (bool)mainWindow.RandomCheck.IsChecked ;
        }


        public void StartCourse()
        {

            // First we check all the info input from the main window such as course speed , complexity etc. If the Random marc is checked then  none of the seetings apply and the course is using random values

            if (isRandom())
            {
                setCourseMode(r.Next(3));
                setCourseSpeed(r.Next(10));
                setCourseComplexity(r.Next(3));

            }


            int i = 0;

            List<Point> coordinates = course.thirdFunction();
            while (coordinates.Count != i)
            {
                queue1.queueAnimation(new DoubleAnimation(coordinates[i].X, new Duration(TimeSpan.FromMilliseconds(100))),
                                       new DoubleAnimation(coordinates[i].Y, new Duration(TimeSpan.FromMilliseconds(100)))
                                     );
                i++;
            }
            queue1.start();

        }        

        private class AnimationQueue
        {
            private List<DoubleAnimation> animation1 ;
            private DependencyProperty property1;
          
            private List<DoubleAnimation> animation2;
            private DependencyProperty property2;
            
            private int curent;
            private UIElement element;

            public AnimationQueue(UIElement element , DependencyProperty property )
            {
                curent = -1;
                this.element = element;
                animation1 = new List<DoubleAnimation>();
                animation2 = new List<DoubleAnimation>();
                this.property1 = property;
            }

            public AnimationQueue(UIElement element, DependencyProperty property1 , DependencyProperty property2)
            {
                curent = -1;
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

           

            public void start(int i)
            {
                if (animation1.Count != animation2.Count)
                    Console.WriteLine("Animation Queue not equal");
                else
                {
                    if (i == animation1.Count)
                        Console.WriteLine("Animation finished");
                    else if (i <= animation1.Count && i <= animation2.Count)
                    {
                        element.BeginAnimation(property1, animation1[i]);
                        element.BeginAnimation(property2, animation2[i]);
                    }; 
                }
            }

            public void start()
            {
                curent = 0;
                element.BeginAnimation(property1, animation1[curent]);
                element.BeginAnimation(property2, animation2[curent]);
                             
            }
                
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.enableStartBtn();
        }
    }


    

   
    
}
