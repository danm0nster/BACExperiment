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
using WiimoteLib;

namespace BACExperiment.GUI
{
    /// <summary>
    /// Interaction logic for WiiremoteWindow.xaml
    /// </summary>
    public partial class WiiremoteWindow : Window
    {

        Wiimote wm;

        public WiiremoteWindow()
        {
            InitializeComponent();

        }

        private void WM1_Detect_Click(object sender, RoutedEventArgs e)
        {
            wm = new Wiimote();
            wm.Connect();
            wm.WiimoteChanged += wm_WiimoteChanged;
            wm.SetReportType(InputReport.IRAccel, true);
            wm.SetLEDs(1);
        }

        private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            Action action = () =>
            {


                WM1Status_Lbl.Content = wm.WiimoteState.ButtonState.A.ToString();
                WM1_Accel_X.Content = wm.WiimoteState.AccelState.Values.X.ToString();
                WM1_Accel_Y.Content = wm.WiimoteState.AccelState.Values.Y.ToString();
                WM1_Accel_Z.Content = wm.WiimoteState.AccelState.Values.Z.ToString();
                WM1_IR1.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[0].Size + 1);
                WM1_IR2.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[1].Size + 1);
                WM1_IR3.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[2].Size + 1);
                WM1_IR4.Content = String.Concat(" X = ", wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10, "; Y = ", wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y / 10, "; Size = ", wm.WiimoteState.IRState.IRSensors[3].Size + 1);
                wm1_progressbar.Value = wm.WiimoteState.Battery;


                // Have to find a way to delay execution of this since in the first ever call there are no . Did not delay execution , instead made if statement in animate
                // sensor that tests for NaN value .



                if (wm.WiimoteState.IRState.IRSensors[0].Found)
                {
                    IRSensor11.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor11, wm.WiimoteState.IRState.IRSensors[0].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor11, wm.WiimoteState.IRState.IRSensors[0].RawPosition.Y / 5);
                    IRSensor11.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor11.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    IRSensor12.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor12, wm.WiimoteState.IRState.IRSensors[1].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor12, wm.WiimoteState.IRState.IRSensors[1].RawPosition.Y / 5);
                    IRSensor12.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor12.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[2].Found)
                {
                    IRSensor13.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor13, wm.WiimoteState.IRState.IRSensors[2].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor13, wm.WiimoteState.IRState.IRSensors[2].RawPosition.Y / 5);
                    IRSensor13.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor13.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[3].Found)
                {
                    IRSensor14.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(IRSensor14, wm.WiimoteState.IRState.IRSensors[3].RawPosition.X / 10 * 3);
                    Canvas.SetTop(IRSensor14, wm.WiimoteState.IRState.IRSensors[3].RawPosition.Y / 5);
                    IRSensor14.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    IRSensor14.Visibility = System.Windows.Visibility.Hidden;
                }
                if (wm.WiimoteState.IRState.IRSensors[0].Found && wm.WiimoteState.IRState.IRSensors[1].Found)
                {
                    MidPoint1.Visibility = System.Windows.Visibility.Visible;
                    Canvas.SetRight(MidPoint1, wm.WiimoteState.IRState.RawMidpoint.X / 10 * 3);
                    Canvas.SetTop(MidPoint1, wm.WiimoteState.IRState.RawMidpoint.Y / 5);
                    MidPoint1.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    MidPoint1.Visibility = System.Windows.Visibility.Hidden;
                }
            };
            try
            {
                Dispatcher.Invoke(action);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void WM1_Disconect_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
