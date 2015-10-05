using BACExperiment.Model;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
namespace BACExperiment
{
    /// <summary>
    /// Interaction logic for StimulyWindow.xaml
    /// </summary>
    public partial class StimulyWindow : Window
    {
        //Links
        private CourseThread course;
        private AnimationQueue queue1;
        private MainWindow mainWindow;
        private coordinateRecorder recorder;
        private CoordinateHolder holder;
        private System.Timers.Timer t;


        //Variables
        private int CourseMode;
        private int CourseSpeed;
        private int CourseComplexity;
        private bool random;
        private bool showTrajectory;
        private Random r = new Random();
       

        // 

  
        private static StimulyWindow instance;

        public int getCourseMode() { return CourseMode; }
        public void setCourseMode(int CourseMode) { this.CourseMode = CourseMode; }
        public int getCourseSpeed() { return CourseSpeed; }
        public void setCourseSpeed(int CourseSpeed) { this.CourseSpeed = CourseSpeed; }
        public int getCourseComplexity() { return CourseComplexity; }
        public void setCourseComplexity(int CourseComplexity) { this.CourseComplexity = CourseComplexity; }
        public bool isRandom() { return random; }
        public void setRandom(bool random) { this.random = random; }
        public bool isShowTrajectory() { return showTrajectory; }
        public void setShowTrajectory(bool value) { this.showTrajectory = value; }
        public static StimulyWindow GetInstance(MainWindow observer) { if (instance == null) { instance = new StimulyWindow(observer); } return instance; }

        
        

        private StimulyWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            course = new CourseThread(this);
            this.queue1 = new AnimationQueue(StimulyEllipse1, Canvas.LeftProperty, Canvas.TopProperty);
            this.mainWindow = mainWindow;
            random = (bool)mainWindow.RandomCheck.IsChecked;
            recorder = coordinateRecorder.getInstance(this);
            holder = CoordinateHolder.GetInstance();

            t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(SendInfo);
            t.Interval += 100;
        }


        public void buildCourseType1()
        {

            // First we check all the info input from the main window such as course speed , complexity etc. If the Random marc is checked then  none of the seetings apply and the course is using random values



            int i = 1;

            List<System.Windows.Point> coordinates = new List<System.Windows.Point>();
            if (CourseComplexity == 0)
            {
                coordinates = course.firstFuntion();
            }
            if (CourseComplexity == 1)
            {
                coordinates = course.secondFunction();
            }
            if (CourseComplexity == 2)
            {
                coordinates = course.thirdFunction();
            }

            double lastX = 0;
            double lastY = 0;
            while (i != coordinates.Count)
            {


                if (Math.Abs(coordinates[i].X - coordinates[i - 1].X) > 3 || Math.Abs(coordinates[i].Y - coordinates[i - 1].Y) > 3)
                {
                    queue1.queueAnimation(new DoubleAnimation(coordinates[i].X - 50, new Duration(TimeSpan.FromMilliseconds(1000 / CourseSpeed / 3))),
                                           new DoubleAnimation(coordinates[i].Y - 50, new Duration(TimeSpan.FromMilliseconds(1000 / CourseSpeed / 3)))
                                           );

                    Line l = new Line();
                    l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

                    if (showTrajectory == true)
                    {
                        //Starts the line drawing from where the first coordinate set is
                        if (lastX == 0 || lastY == 0)
                        {
                            lastX = coordinates[i].X;
                            lastY = coordinates[i].Y;
                        }

                        //Draw the line 
                        l.X1 = lastX;
                        l.X2 = coordinates[i].X;
                        l.Y1 = lastY;
                        l.Y2 = coordinates[i].Y;

                        l.StrokeThickness = 2;
                        StimulyCanvas.Children.Add(l);
                    }

                }

                //Position Ellipse to the begining of the course

                if (i == 1)
                {
                    Canvas.SetLeft(StimulyEllipse1, coordinates[i].X - 50);
                    Canvas.SetTop(StimulyEllipse1, coordinates[i].Y - 50);
                }

                lastX = coordinates[i].X;
                lastY = coordinates[i].Y;
                i++;
            }


        }

        public void startCourse()
        {
            queue1.start();
        }

        public async void movePointer1(int x, int y)
        {
            Action action = () =>
            {
                Point p = makeToCartezian(x, y);
                Canvas.SetLeft(Pointer1, p.X);
                Canvas.SetTop(Pointer1, p.Y);
                Pointer1.Visibility = System.Windows.Visibility.Visible;
            };

            await Dispatcher.BeginInvoke(action);
        }

        public async void movePointer2(int x, int y)
        {
            Action action = () =>
            {
                Point p = makeToCartezian(x, y);
                Canvas.SetLeft(Pointer2, p.X);
                Canvas.SetTop(Pointer2, p.Y);
                Pointer2.Visibility = System.Windows.Visibility.Visible; // Moving the pointer visibility setter here appears to have improved the speed of the guy dramatically as there is no intermediary thread called just to 
                //Show the pointer 
            };

            await Dispatcher.BeginInvoke(action);
        }

        private class AnimationQueue
        {
            private List<DoubleAnimation> animation1;
            private DependencyProperty property1;

            private List<DoubleAnimation> animation2;
            private DependencyProperty property2;

            private int curent;
            private UIElement element;

            public AnimationQueue(UIElement element, DependencyProperty property)
            {
                curent = -1;
                this.element = element;
                animation1 = new List<DoubleAnimation>();
                animation2 = new List<DoubleAnimation>();
                this.property1 = property;
            }

            public AnimationQueue(UIElement element, DependencyProperty property1, DependencyProperty property2)
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
            mainWindow.complexitySlider.IsEnabled = true;
            mainWindow.SpeedSlider.IsEnabled = true;
            mainWindow.TrajectoryCheck.IsEnabled = true;
            mainWindow.StopFullRecording();
            recorder.Stop();
            
            t.Stop();

            instance = null;
        }

        private Point makeToCartezian(int x, int y)
        {
            Point p = new Point();
            p.X = (500 - x);
            p.Y = (500 - y);
            /*
            if (x < 500)
            {
                p.X = 0 - 500 + x;
            }

            else
            {
                p.X = x - 500;
            }

            if (y < 500)
            {
                p.Y = 500 - y;
            }
            else
            {
                p.Y = 0 - (y - 500);
            }
            */
            return p;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void startRecording()
        {
            recorder.Run();
        }

        public void StartSendingInfo()
        {
            t.Start();
        }
      
        private void SendInfo(object sender, ElapsedEventArgs e)
        {
            // When the recording starts , the sending of information will also be sent . The only thing left is to make shure that the two timer threads are synched with the CoordinateHolder class 
            // So that one does not update the numbers , while the

            Action action = () =>
            {
               
                holder.SetEllipseCoordinates(Canvas.GetLeft(StimulyEllipse1), Canvas.GetTop(StimulyEllipse1));
                holder.SetPointerCoordinates(0, new Point(Canvas.GetLeft(Pointer1), Canvas.GetTop(Pointer1)));
                holder.SetPointerCoordinates(1, new Point(Canvas.GetLeft(Pointer2), Canvas.GetTop(Pointer2)));
            };

            Dispatcher.BeginInvoke(action);
        }

        public void ShowPointer1()
        {
            Action action = () =>
            {
                Pointer1.Visibility = System.Windows.Visibility.Visible;
            };

            Dispatcher.BeginInvoke(action);
        }

        public void ShowPointer2()
        {
            Action action = () =>
            {
                Pointer2.Visibility = System.Windows.Visibility.Visible;
            };

            Dispatcher.BeginInvoke(action);
        }

    }


    


    }
