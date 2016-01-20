using BACExperiment.GUI;
using BACExperiment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
namespace BACExperiment
{
    /// <summary>
    /// Interaction logic for StimulyWindow.xaml
    /// </summary>
    public partial class MovementWindow : Window
    {

        #region INotifyPropertyChangedImplementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        //Links
        private Course course;
        private AnimationQueue queue1;
        private coordinateRecorder recorder;
        private CoordinateHolder holder;
        private System.Timers.Timer t;
        private System.Timers.Timer checkPointTimer;
        private MainWindow mainWindow;
        private MovementWindowViewModel model = MovementWindowViewModel.GetInstance();
        private System.Windows.Media.Brush lineBrush;

    

        //Variables
        private int CourseMode;
        private int CourseSpeed;
        private int CourseComplexity = -1;
        private bool random;      
        private int currentCheckPoint = 0;
        private List<System.Windows.Point> coordinates = new List<Point>();
        private List<System.Windows.Point> checkPoints = new List<Point>();
        private int StrokeThickness;
        private SolidColorBrush color1;
        private SolidColorBrush color2;
        private bool built = false;


        private delegate void  CourseGenerate() ;
        private CourseGenerate generate ;

        public int getCourseMode() { return CourseMode; }
        public void setCourseMode(int CourseMode) { this.CourseMode = CourseMode; }
        public int getCourseSpeed() { return CourseSpeed; }
        public void setCourseSpeed(int CourseSpeed) { this.CourseSpeed = CourseSpeed; }
        public int getCourseComplexity() { return CourseComplexity; }
        public void setCourseComplexity(int CourseComplexity) { this.CourseComplexity = CourseComplexity; }
        public bool isRandom() { return random; }
        public void setRandom(bool random) { this.random = random; }
      
        public MovementWindow instance { get; set; }


        public MovementWindow(MainWindow mainWindow , string mode , int complexity , int Speed, System.Windows.Media.Color color1 , System.Windows.Media.Color color2 , System.Windows.Media.Color color3 )
        {
     
            InitializeComponent();
            
            Pointer1.Stroke = new SolidColorBrush(color1);
            this.color1 = new SolidColorBrush(color1);
            Pointer1.DataContext = model;
            Pointer2.Stroke = new SolidColorBrush(color2);
            this.color2 = new SolidColorBrush(color2);
            StimulyEllipse1.Stroke = this.color1;
            CheckPointEllipse.Stroke = this.color2;
            Pointer2.DataContext = model;
            lineBrush = new SolidColorBrush(color3);
            CourseSpeed = Speed;
            
           
            this.queue1 = new AnimationQueue(StimulyEllipse1, Canvas.LeftProperty, Canvas.TopProperty);
            this.mainWindow = mainWindow;
            recorder = coordinateRecorder.getInstance(this);
            holder = CoordinateHolder.GetInstance();

            t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(SendInfo);
            t.Interval += 100;
 

            this.SetBinding(Window.WidthProperty, new Binding("RezolutionX") { Source = model, Mode = BindingMode.OneWayToSource });
            this.SetBinding(Window.HeightProperty, new Binding("RezolutionY") { Source = model, Mode = BindingMode.OneWayToSource });

            if (mode == "Asynchronous")
            {
                generate = Asynchronous;
                checkPointTimer = new System.Timers.Timer();
                checkPointTimer.Interval = 1000.0 / CourseSpeed * 50;
                checkPointTimer.Elapsed += new ElapsedEventHandler(ShowNextCheckPoint);
                queue1.checkPointHandler += new EventHandler(ShowNextCheckPoint);
                StimulyEllipse1.Visibility = Visibility.Visible;
                CheckPointEllipse.Visibility = Visibility.Visible;
            }
            if (mode == "Synchronous")
            {
                generate = Synchronous;
                StimulyEllipse1.Visibility = Visibility.Visible;
            }
            CourseComplexity = complexity;
          
        }

        public MovementWindow(MainWindow mainWindow , System.Windows.Media.Color color1, System.Windows.Media.Color color2, System.Windows.Media.Color color3 , int StrokeThickness) 
        {
            InitializeComponent();
            Pointer1.Stroke = new SolidColorBrush(color1);
            Pointer1.DataContext = model;
            Pointer2.Stroke = new SolidColorBrush(color2);
            Pointer2.DataContext = model;
            lineBrush = new SolidColorBrush(color3);
            this.StrokeThickness = StrokeThickness;

            this.mainWindow = mainWindow;
            recorder = coordinateRecorder.getInstance(this);
            holder = CoordinateHolder.GetInstance();
            this.queue1 = new AnimationQueue(StimulyEllipse1, Canvas.LeftProperty, Canvas.TopProperty);
            t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(SendInfo);
            t.Interval += 100;

            this.SetBinding(Window.WidthProperty, new Binding("RezolutionX") { Source = model, Mode = BindingMode.OneWayToSource });
            this.SetBinding(Window.HeightProperty, new Binding("RezolutionY") { Source = model, Mode = BindingMode.OneWayToSource });

            generate = Self_Paced;
            CourseComplexity = 0;
            StimulyEllipse1.Visibility = Visibility.Visible;
            
        }
        private void ShowNextCheckPoint(object sender, EventArgs e)
         {
                Action action = () =>
                {
                    CheckPoint();
                };
                this.Dispatcher.Invoke(action);
                
            
        }

        private void CheckPoint()
        {
            if ( currentCheckPoint == 0 || ( Canvas.GetLeft(StimulyEllipse1) == Canvas.GetLeft(CheckPointEllipse) && Canvas.GetTop(StimulyEllipse1)== Canvas.GetTop(CheckPointEllipse)))
            {
                
                Canvas.SetLeft(CheckPointEllipse, checkPoints[currentCheckPoint].X);
                Canvas.SetTop(CheckPointEllipse, checkPoints[currentCheckPoint].Y);

                if (currentCheckPoint != 0)
                {
                    if (StimulyEllipse1.Stroke == color2)
                    {
                        CheckPointEllipse.Stroke = color2;
                        StimulyEllipse1.Stroke = color1;
                    }
                    else
                    {
                        CheckPointEllipse.Stroke = color1;
                        StimulyEllipse1.Stroke = color2;
                    }
                }

                currentCheckPoint++;

            }
           
        }


       
        public void buildCourse()
        {
            
            course = new Course(this);
            try {
                if (CourseComplexity == 0)
                    coordinates = course.simpleFunction();
                if (CourseComplexity == 1)
                    coordinates = course.firstFunction();
                if (CourseComplexity == 2)
                    coordinates = course.secondFunction();
                if (CourseComplexity == 3)
                    coordinates = course.thirdFunction();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (generate == null)
            { MessageBox.Show(" Please select a course setting from the main window before attempting to run ."); }
            else
            {
                if(CourseComplexity == -1 )
                { MessageBox.Show(" Please select a course complexity . If the problem persists please contact our developer team of one."); }
                else
                {
                    generate();
                    built = true;
                }
            }
        }

        private void Synchronous()
        {
          
            int i = 1;
            double lastX = 0;
            double lastY = 0;
            while (i != coordinates.Count)
            {


                if (Math.Abs(coordinates[i].X - coordinates[i - 1].X) > 3 || Math.Abs(coordinates[i].Y - coordinates[i - 1].Y) > 3)
                {
                   this.queue1.queueAnimation(new DoubleAnimation(coordinates[i].X - 50, new Duration(TimeSpan.FromMilliseconds(1000 / CourseSpeed ))),
                                           new DoubleAnimation(coordinates[i].Y - 50, new Duration(TimeSpan.FromMilliseconds(1000 / CourseSpeed )))
                                           );
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

        private void Self_Paced()
        {

            int i = 1;
            double lastX = 0;
            double lastY = 0;
            while (i != coordinates.Count)
            {
                Line l = new Line();
                l.Stroke = lineBrush;

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

                l.StrokeThickness = StrokeThickness;

                StimulyReferencePoint.Children.Add(l);


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
        private void Asynchronous()
        {
            int agregator = 0;
            int i = 1;
            double lastX = 0;
            double lastY = 0;
            while (i != coordinates.Count)
            {


                if (Math.Abs(coordinates[i].X - coordinates[i - 1].X) > 3 || Math.Abs(coordinates[i].Y - coordinates[i - 1].Y) > 3)
                {
                    this.queue1.queueAnimation(new DoubleAnimation(coordinates[i].X - 50, new Duration(TimeSpan.FromMilliseconds(1000.00 / CourseSpeed))),
                                            new DoubleAnimation(coordinates[i].Y - 50, new Duration(TimeSpan.FromMilliseconds(1000.00 / CourseSpeed)))
                                            );
                    if (agregator == 50)
                    {
                        checkPoints.Add(new Point(coordinates[i].X - 50, coordinates[i].Y - 50));
                        agregator = 0;
                    }
                    agregator++;

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
            Canvas.SetLeft(StimulyEllipse1, coordinates[0].X);
            Canvas.SetTop(StimulyEllipse1, coordinates[0].Y);
        }
        
        public void startCourse()
        {
            if(course == null) buildCourse();
            if (queue1.NotEmpty) queue1.start();
            else
                StimulyEllipse1.Visibility = System.Windows.Visibility.Hidden;

            if (checkPointTimer != null && built)
            {
                checkPointTimer.Start();
                ShowNextCheckPoint(null , null);
            }
        }

        private class AnimationQueue
        {
            public EventHandler checkPointHandler;

            private List<DoubleAnimation> animation1;
            private DependencyProperty property1;

            private List<DoubleAnimation> animation2;
            private DependencyProperty property2;

            private int curent;
            private UIElement element;
            public bool NotEmpty = false;
            public int Count=0;
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
                NotEmpty = true;

                animation1.Completed += (s,e) =>
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
                        if (checkPointHandler != null)
                            checkPointHandler(this , new EventArgs());
                    }
                    Count++;
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
                        curent++;
                        
                    };
                }
            }

            public void start()
            {
                curent = 0;
                element.BeginAnimation(property1, animation1[curent]);
                element.BeginAnimation(property2, animation2[curent]);

            }

            public void Clear()
            {
                animation1.Clear();
                animation2.Clear();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            mainWindow.OpenBtn.IsEnabled = true;
            mainWindow.complexitySlider.IsEnabled = true;
            mainWindow.SpeedSlider.IsEnabled = true;           
            mainWindow.StopFullRecording();
            recorder.Stop();
            if(t.Enabled)
            t.Stop();
            if(checkPointTimer != null )
            checkPointTimer.Stop();
           
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
            
            Action action = () =>
            {

                holder.SetEllipseCoordinates(Canvas.GetLeft(StimulyEllipse1), Canvas.GetTop(StimulyEllipse1));
                holder.SetPointerCoordinates(0, new Point(Canvas.GetLeft(Pointer1), Canvas.GetTop(Pointer1)));
                holder.SetPointerCoordinates(1, new Point(Canvas.GetLeft(Pointer2), Canvas.GetTop(Pointer2)));
                holder.SetAccel(model.GetAccelValues());
            };

            Dispatcher.BeginInvoke(action);
        }

        public void Reset()
        {
            StimulyReferencePoint.Children.Clear();
            queue1.Clear();
        }
    }

    
   
}
