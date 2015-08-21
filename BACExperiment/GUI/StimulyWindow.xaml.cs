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
        public StimulyWindow()
        {
            InitializeComponent();
        }

        public void moveElipse1ToCoordinate(Point point)
        {
            Canvas.SetTop(StimulyEllipse1, point.Y);
            Canvas.SetLeft(StimulyEllipse1, point.X);
        }

        public void moveElipse2ToCoordinate(Point point)
        {
            Canvas.SetTop(StimulyEllipse2, point.Y);
            Canvas.SetLeft(StimulyEllipse2, point.X);
        }
    }
}
