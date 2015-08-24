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

namespace BACExperiment
{
    /// <summary>
    /// Interaction logic for StimulyWindow.xaml
    /// </summary>
    public partial class StimulyWindow : Window
    {

        TranslateTransform moveTo = new TranslateTransform();
        public StimulyWindow()
        {
            InitializeComponent();
        }

        public void moveElipse1ToCoordinate(Point point)
        {
            moveTo.X = point.X;
            moveTo.Y = point.Y;
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
