using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class InstructionsPopUp : Window
    {
        private System.Timers.Timer t = new System.Timers.Timer();
        public InstructionsPopUp(string instructions)
        {
            InitializeComponent();



            InstructionsLabel.AppendText(instructions);
           
        }

        void OnTimed(object sender, ElapsedEventArgs e)
        {
            t.Stop();

            Action action = () => { this.Close(); };
            Dispatcher.Invoke(action);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            t.Interval = 1000 * 60;
            t.Elapsed += new System.Timers.ElapsedEventHandler(OnTimed);
            t.Start();
        }
    }
}
