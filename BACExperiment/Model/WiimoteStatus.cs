using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Controls;
using WiimoteLib;

namespace BACExperiment
{
    /*
    This class I have initialli copied from the WiimoteLib 1.7 tester application for familiarizing with myself with the way the wiimote library
    interacts with the guy . For BACExperiment however , I will now modify the class so that it contains what we need for the experiment
     

    */
    public class WiimoteInfo : UserControl
    {
       

        public System.Drawing.Image pbIR;

        public ProgressBar pbBattery;

        public Label StateLbl;
        public Label PathLbl;

        public Label lblAccelX;
        public Label lblAccelY;
        public Label lblAccelZ;

        public Label lblIR1;
        public Label lblIR2;
        public Label lblIR3;
        public Label lblIR4;

        public TextBox txtBox;
        private Bitmap b = new Bitmap(256, 192, PixelFormat.Format24bppRgb);

        private Graphics g;

        private Wiimote mWiimote;

        public Wiimote Wiimote
        {
            set
            {
                this.mWiimote = value;

            }
        }

        public WiimoteInfo(ProgressBar pbBattery ,
         Label StateLbl, 
         Label PathLbl,  
              
         Label lblAccelX,
         Label lblAccelY,
         Label lblAccelZ,

         Label lblIR1,
         Label lblIR2,
         Label lblIR3,
         Label lblIR4,

         TextBox txtBox  )
        {
            this.StateLbl = StateLbl;
            this.PathLbl = PathLbl;
            this.pbBattery = pbBattery;
            this.lblAccelX = lblAccelX;
            this.lblAccelY = lblAccelY;
            this.lblAccelZ = lblAccelZ;
            this.lblIR1 = lblIR1;
            this.lblIR2 = lblIR2;
            this.lblIR3 = lblIR3;
            this.lblIR4 = lblIR4;
            this.txtBox = txtBox;
        }

       

        public WiimoteInfo(Wiimote wm) 
        {
            this.mWiimote = wm;
        }

        

        private void UpdateIR(IRSensor irSensor, Label lblNorm, Color color)
        {

            if (irSensor.Found)
            {
                lblNorm.Content = string.Concat(irSensor.Position.ToString(), ", ", irSensor.Size);
                this.g.DrawEllipse(new Pen(color), irSensor.RawPosition.X / 4, irSensor.RawPosition.Y / 4, irSensor.Size + 1, irSensor.Size + 1);
            }
        }

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            WiimoteInfo.UpdateWiimoteStateDelegate updateWiimoteStateDelegate = new WiimoteInfo.UpdateWiimoteStateDelegate(this.UpdateWiimoteChanged);
            object[] objArray = new object[] { args };
            //base.BeginInvoke(updateWiimoteStateDelegate, objArray);
        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            WiimoteState wiimoteState = args.WiimoteState;
            this.StateLbl.Content = wiimoteState.ButtonState.A.ToString();
            this.lblAccelX.Content = wiimoteState.AccelState.Values.X.ToString();
            this.lblAccelY.Content = wiimoteState.AccelState.Values.Y.ToString();
            this.lblAccelZ.Content = wiimoteState.AccelState.Values.Z.ToString();


            ExtensionType extensionType = wiimoteState.ExtensionType;
            if (extensionType <= (ExtensionType.Nunchuk | ExtensionType.ClassicController | ExtensionType.Guitar | ExtensionType.Drums))
            {
                if ((int)extensionType == -1541406720)
                {
                    Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need nunchuck . Please remove .");
                }
                else if (extensionType <= (ExtensionType.Nunchuk | ExtensionType.ClassicController | ExtensionType.Guitar | ExtensionType.Drums) && extensionType >= (ExtensionType.Nunchuk | ExtensionType.ClassicController))
                {
                    switch ((int)((long)extensionType - (long)(ExtensionType.Nunchuk | ExtensionType.ClassicController)))
                    {
                        case 0:
                            {
                                Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need classic controller . Please remove .");
                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need guitar . Please remove .");
                                break;
                            }
                    }
                }
            }
            else if (extensionType == (ExtensionType.Nunchuk | ExtensionType.BalanceBoard))
            {
              
                
                    Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need a ballanceboard or nunchuck . Please remove .");
                
               
            }
            else if (extensionType == ExtensionType.Drums)
            {
                Console.WriteLine("WiimoteStatus.updateWiimoteChanged : For the experiment we do not need drums . Please remove .");
            }
            this.g.Clear(Color.Black);
            this.UpdateIR(wiimoteState.IRState.IRSensors[0], this.lblIR1,  Color.Red);
            this.UpdateIR(wiimoteState.IRState.IRSensors[1], this.lblIR2,  Color.Blue);
            this.UpdateIR(wiimoteState.IRState.IRSensors[2], this.lblIR3,  Color.Yellow);
            this.UpdateIR(wiimoteState.IRState.IRSensors[3], this.lblIR4,  Color.Orange);
            if (wiimoteState.IRState.IRSensors[0].Found && wiimoteState.IRState.IRSensors[1].Found)
            {
                this.g.DrawEllipse(new Pen(Color.Green), wiimoteState.IRState.RawMidpoint.X / 4, wiimoteState.IRState.RawMidpoint.Y / 4, 2, 2);
            }
            this.pbIR = this.b;
            this.pbBattery.Value = (wiimoteState.Battery > 200f ? 200 : (int)wiimoteState.Battery);
            this.PathLbl.Content = string.Concat("Device Path: ", this.mWiimote.HIDDevicePath);
        }

        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
    }
}   