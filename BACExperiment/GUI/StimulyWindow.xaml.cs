using BACExperiment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace BACExperiment
{
    /// <summary>
    /// Interaction logic for StimulyWindow.xaml
    /// </summary>
    public partial class StimulyWindow : Window
    {

        private TranslateTransform moveTo;
        private CourseThread course ;
        private DoubleAnimation Xanimation = new DoubleAnimation();
        private DoubleAnimation Yanimation = new DoubleAnimation();

        int Ellipse_1_X = 0;
        int Ellipse_1_Y = 0;

        public StimulyWindow()
        {
            InitializeComponent();
            course = new CourseThread(this);
        }


        public void StartCourse()
        {
            course.firstFuntion();
        }

        public void StopCourse()
        {

        }

        public void animateEllipse1To( double X ,double Y, int sec)
        {
            
            Xanimation.From=Ellipse_1_X;
            Xanimation.To = X;
            Xanimation.Duration = new Duration(TimeSpan.FromMilliseconds(sec));

            Yanimation.From = Ellipse_1_Y;
            Yanimation.To = Y;
            Yanimation.Duration = new Duration(TimeSpan.FromMilliseconds(sec));

            TranslateTransform rt = new TranslateTransform();
            StimulyEllipse1.RenderTransform = rt;
            rt.BeginAnimation(TranslateTransform.XProperty,  Xanimation);
            rt.BeginAnimation(TranslateTransform.YProperty,  Yanimation);
            this.Ellipse_1_X = (int)X;
            this.Ellipse_1_Y = (int)Y;
        }

        public void moveElipse1ToCoordinate(Point point)
        {

                moveTo = new TranslateTransform();
                moveTo.X = (int)point.X + 500;  // Add 500 px so that center of the canvas will be treated as the center of the carthezian plane .
                moveTo.Y = (int)point.Y + 500;
                StimulyEllipse1.RenderTransform = moveTo;

        }

            

        public void moveElipse2ToCoordinate(Point point)
        {
            moveTo.X = point.X;
            moveTo.Y = point.Y;
            StimulyEllipse2.RenderTransform = moveTo;
        }
    }
}
