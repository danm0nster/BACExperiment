using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WiimoteLib;

namespace WiimoteTest
{
    public class WiimoteInfo : UserControl
    {
        private IContainer components;

        public GroupBox groupBox8;

        public CheckedListBox clbButtons;

        public Label lblTriggerR;

        public Label lblTriggerL;

        public Label lblIR3;

        public Label lblIR4;

        public Label lblCCJoy2;

        public Label lblCCJoy1;

        public GroupBox groupBox5;

        public Label lblIR3Raw;

        public Label lblIR1Raw;

        public Label lblIR4Raw;

        public Label lblIR2Raw;

        public Label lblIR1;

        public Label lblIR2;

        public CheckBox chkFound3;

        public CheckBox chkFound4;

        public CheckBox chkFound1;

        public CheckBox chkFound2;

        public PictureBox pbIR;

        public Label lblGuitarWhammy;

        public GroupBox groupBox7;

        public Label lblGuitarJoy;

        public CheckedListBox clbGuitarButtons;

        public GroupBox groupBox6;

        public CheckedListBox clbCCButtons;

        public GroupBox groupBox4;

        public ProgressBar pbBattery;

        public Label lblBattery;

        public GroupBox groupBox3;

        public CheckBox chkLED2;

        public CheckBox chkLED4;

        public CheckBox chkLED3;

        public CheckBox chkLED1;

        public CheckBox chkRumble;

        public CheckBox chkZ;

        public CheckBox chkC;

        public Label lblChuk;

        public GroupBox groupBox2;

        public Label lblChukJoy;

        public GroupBox groupBox1;

        public Label lblAccel;

        public CheckBox chkExtension;

        private GroupBox groupBox9;

        private CheckBox chkLbs;

        private Label lblBBBR;

        private Label lblBBTR;

        private Label lblBBBL;

        private Label lblBBTotal;

        private Label lblBBTL;

        private Label lblCOG;

        private Label lblDevicePath;

        public CheckedListBox clbTouchbar;

        public Label lblGuitarType;

        private GroupBox groupBox10;

        public CheckedListBox clbDrums;

        public Label lblDrumJoy;

        private ListBox lbDrumVelocity;

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

        public WiimoteInfo()
        {
            this.InitializeComponent();
            this.g = Graphics.FromImage(this.b);
        }

        public WiimoteInfo(Wiimote wm) : this()
        {
            this.mWiimote = wm;
        }

        private void chkLED_CheckedChanged(object sender, EventArgs e)
        {
            this.mWiimote.SetLEDs(this.chkLED1.Checked, this.chkLED2.Checked, this.chkLED3.Checked, this.chkLED4.Checked);
        }

        private void chkRumble_CheckedChanged(object sender, EventArgs e)
        {
            this.mWiimote.SetRumble(this.chkRumble.Checked);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox8 = new GroupBox();
            this.clbButtons = new CheckedListBox();
            this.lblTriggerR = new Label();
            this.lblTriggerL = new Label();
            this.lblIR3 = new Label();
            this.lblIR4 = new Label();
            this.lblCCJoy2 = new Label();
            this.lblCCJoy1 = new Label();
            this.groupBox5 = new GroupBox();
            this.lblIR3Raw = new Label();
            this.lblIR1Raw = new Label();
            this.lblIR4Raw = new Label();
            this.lblIR2Raw = new Label();
            this.lblIR1 = new Label();
            this.lblIR2 = new Label();
            this.chkFound3 = new CheckBox();
            this.chkFound4 = new CheckBox();
            this.chkFound1 = new CheckBox();
            this.chkFound2 = new CheckBox();
            this.pbIR = new PictureBox();
            this.lblGuitarWhammy = new Label();
            this.groupBox7 = new GroupBox();
            this.clbTouchbar = new CheckedListBox();
            this.lblGuitarType = new Label();
            this.lblGuitarJoy = new Label();
            this.clbGuitarButtons = new CheckedListBox();
            this.groupBox6 = new GroupBox();
            this.clbCCButtons = new CheckedListBox();
            this.groupBox4 = new GroupBox();
            this.pbBattery = new ProgressBar();
            this.lblBattery = new Label();
            this.groupBox3 = new GroupBox();
            this.chkLED2 = new CheckBox();
            this.chkLED4 = new CheckBox();
            this.chkLED3 = new CheckBox();
            this.chkLED1 = new CheckBox();
            this.chkRumble = new CheckBox();
            this.chkZ = new CheckBox();
            this.chkC = new CheckBox();
            this.lblChuk = new Label();
            this.groupBox2 = new GroupBox();
            this.lblChukJoy = new Label();
            this.groupBox1 = new GroupBox();
            this.lblAccel = new Label();
            this.chkExtension = new CheckBox();
            this.groupBox9 = new GroupBox();
            this.lblCOG = new Label();
            this.chkLbs = new CheckBox();
            this.lblBBBR = new Label();
            this.lblBBTR = new Label();
            this.lblBBBL = new Label();
            this.lblBBTotal = new Label();
            this.lblBBTL = new Label();
            this.lblDevicePath = new Label();
            this.groupBox10 = new GroupBox();
            this.lbDrumVelocity = new ListBox();
            this.lblDrumJoy = new Label();
            this.clbDrums = new CheckedListBox();
            this.groupBox8.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((ISupportInitialize)this.pbIR).BeginInit();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            base.SuspendLayout();
            this.groupBox8.Controls.Add(this.clbButtons);
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new Size(72, 220);
            this.groupBox8.TabIndex = 37;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Wiimote";
            this.clbButtons.FormattingEnabled = true;
            CheckedListBox.ObjectCollection items = this.clbButtons.Items;
            object[] objArray = new object[] { "A", "B", "-", "Home", "+", "1", "2", "Up", "Down", "Left", "Right" };
            items.AddRange(objArray);
            this.clbButtons.Location = new System.Drawing.Point(8, 16);
            this.clbButtons.Name = "clbButtons";
            this.clbButtons.Size = new Size(56, 184);
            this.clbButtons.TabIndex = 1;
            this.lblTriggerR.AutoSize = true;
            this.lblTriggerR.Location = new System.Drawing.Point(76, 104);
            this.lblTriggerR.Name = "lblTriggerR";
            this.lblTriggerR.Size = new Size(51, 13);
            this.lblTriggerR.TabIndex = 25;
            this.lblTriggerR.Text = "Trigger R";
            this.lblTriggerL.AutoSize = true;
            this.lblTriggerL.Location = new System.Drawing.Point(76, 88);
            this.lblTriggerL.Name = "lblTriggerL";
            this.lblTriggerL.Size = new Size(49, 13);
            this.lblTriggerL.TabIndex = 24;
            this.lblTriggerL.Text = "Trigger L";
            this.lblIR3.AutoSize = true;
            this.lblIR3.Location = new System.Drawing.Point(8, 48);
            this.lblIR3.Name = "lblIR3";
            this.lblIR3.Size = new Size(24, 13);
            this.lblIR3.TabIndex = 7;
            this.lblIR3.Text = "IR3";
            this.lblIR4.AutoSize = true;
            this.lblIR4.Location = new System.Drawing.Point(8, 64);
            this.lblIR4.Name = "lblIR4";
            this.lblIR4.Size = new Size(24, 13);
            this.lblIR4.TabIndex = 7;
            this.lblIR4.Text = "IR4";
            this.lblCCJoy2.Location = new System.Drawing.Point(76, 52);
            this.lblCCJoy2.Name = "lblCCJoy2";
            this.lblCCJoy2.Size = new Size(108, 32);
            this.lblCCJoy2.TabIndex = 24;
            this.lblCCJoy2.Text = "Right Joystick";
            this.lblCCJoy1.Location = new System.Drawing.Point(76, 16);
            this.lblCCJoy1.Name = "lblCCJoy1";
            this.lblCCJoy1.Size = new Size(108, 32);
            this.lblCCJoy1.TabIndex = 24;
            this.lblCCJoy1.Text = "Left Joystick";
            this.groupBox5.Controls.Add(this.lblIR3Raw);
            this.groupBox5.Controls.Add(this.lblIR1Raw);
            this.groupBox5.Controls.Add(this.lblIR4Raw);
            this.groupBox5.Controls.Add(this.lblIR2Raw);
            this.groupBox5.Controls.Add(this.lblIR3);
            this.groupBox5.Controls.Add(this.lblIR1);
            this.groupBox5.Controls.Add(this.lblIR4);
            this.groupBox5.Controls.Add(this.lblIR2);
            this.groupBox5.Controls.Add(this.chkFound3);
            this.groupBox5.Controls.Add(this.chkFound4);
            this.groupBox5.Controls.Add(this.chkFound1);
            this.groupBox5.Controls.Add(this.chkFound2);
            this.groupBox5.Location = new System.Drawing.Point(184, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(176, 188);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "IR";
            this.lblIR3Raw.AutoSize = true;
            this.lblIR3Raw.Location = new System.Drawing.Point(8, 112);
            this.lblIR3Raw.Name = "lblIR3Raw";
            this.lblIR3Raw.Size = new Size(46, 13);
            this.lblIR3Raw.TabIndex = 10;
            this.lblIR3Raw.Text = "IR3Raw";
            this.lblIR1Raw.AutoSize = true;
            this.lblIR1Raw.Location = new System.Drawing.Point(8, 80);
            this.lblIR1Raw.Name = "lblIR1Raw";
            this.lblIR1Raw.Size = new Size(46, 13);
            this.lblIR1Raw.TabIndex = 10;
            this.lblIR1Raw.Text = "IR1Raw";
            this.lblIR4Raw.AutoSize = true;
            this.lblIR4Raw.Location = new System.Drawing.Point(8, 128);
            this.lblIR4Raw.Name = "lblIR4Raw";
            this.lblIR4Raw.Size = new Size(46, 13);
            this.lblIR4Raw.TabIndex = 9;
            this.lblIR4Raw.Text = "IR4Raw";
            this.lblIR2Raw.AutoSize = true;
            this.lblIR2Raw.Location = new System.Drawing.Point(8, 96);
            this.lblIR2Raw.Name = "lblIR2Raw";
            this.lblIR2Raw.Size = new Size(46, 13);
            this.lblIR2Raw.TabIndex = 9;
            this.lblIR2Raw.Text = "IR2Raw";
            this.lblIR1.AutoSize = true;
            this.lblIR1.Location = new System.Drawing.Point(8, 16);
            this.lblIR1.Name = "lblIR1";
            this.lblIR1.Size = new Size(24, 13);
            this.lblIR1.TabIndex = 7;
            this.lblIR1.Text = "IR1";
            this.lblIR2.AutoSize = true;
            this.lblIR2.Location = new System.Drawing.Point(8, 32);
            this.lblIR2.Name = "lblIR2";
            this.lblIR2.Size = new Size(24, 13);
            this.lblIR2.TabIndex = 7;
            this.lblIR2.Text = "IR2";
            this.chkFound3.AutoSize = true;
            this.chkFound3.Location = new System.Drawing.Point(60, 148);
            this.chkFound3.Name = "chkFound3";
            this.chkFound3.Size = new Size(46, 17);
            this.chkFound3.TabIndex = 8;
            this.chkFound3.Text = "IR 3";
            this.chkFound3.UseVisualStyleBackColor = true;
            this.chkFound4.AutoSize = true;
            this.chkFound4.Location = new System.Drawing.Point(60, 164);
            this.chkFound4.Name = "chkFound4";
            this.chkFound4.Size = new Size(46, 17);
            this.chkFound4.TabIndex = 8;
            this.chkFound4.Text = "IR 4";
            this.chkFound4.UseVisualStyleBackColor = true;
            this.chkFound1.AutoSize = true;
            this.chkFound1.Location = new System.Drawing.Point(8, 148);
            this.chkFound1.Name = "chkFound1";
            this.chkFound1.Size = new Size(46, 17);
            this.chkFound1.TabIndex = 8;
            this.chkFound1.Text = "IR 1";
            this.chkFound1.UseVisualStyleBackColor = true;
            this.chkFound2.AutoSize = true;
            this.chkFound2.Location = new System.Drawing.Point(8, 164);
            this.chkFound2.Name = "chkFound2";
            this.chkFound2.Size = new Size(46, 17);
            this.chkFound2.TabIndex = 8;
            this.chkFound2.Text = "IR 2";
            this.chkFound2.UseVisualStyleBackColor = true;
            this.pbIR.Location = new System.Drawing.Point(4, 248);
            this.pbIR.Name = "pbIR";
            this.pbIR.Size = new Size(256, 192);
            this.pbIR.TabIndex = 28;
            this.pbIR.TabStop = false;
            this.lblGuitarWhammy.AutoSize = true;
            this.lblGuitarWhammy.Location = new System.Drawing.Point(92, 140);
            this.lblGuitarWhammy.Name = "lblGuitarWhammy";
            this.lblGuitarWhammy.Size = new Size(51, 13);
            this.lblGuitarWhammy.TabIndex = 24;
            this.lblGuitarWhammy.Text = "Whammy";
            this.groupBox7.Controls.Add(this.clbTouchbar);
            this.groupBox7.Controls.Add(this.lblGuitarType);
            this.groupBox7.Controls.Add(this.lblGuitarWhammy);
            this.groupBox7.Controls.Add(this.lblGuitarJoy);
            this.groupBox7.Controls.Add(this.clbGuitarButtons);
            this.groupBox7.Location = new System.Drawing.Point(364, 272);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(188, 176);
            this.groupBox7.TabIndex = 36;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Guitar";
            this.clbTouchbar.FormattingEnabled = true;
            CheckedListBox.ObjectCollection objectCollection = this.clbTouchbar.Items;
            object[] objArray1 = new object[] { "Green", "Red", "Yellow", "Blue", "Orange" };
            objectCollection.AddRange(objArray1);
            this.clbTouchbar.Location = new System.Drawing.Point(88, 16);
            this.clbTouchbar.Name = "clbTouchbar";
            this.clbTouchbar.Size = new Size(80, 79);
            this.clbTouchbar.TabIndex = 25;
            this.lblGuitarType.AutoSize = true;
            this.lblGuitarType.Location = new System.Drawing.Point(4, 156);
            this.lblGuitarType.Name = "lblGuitarType";
            this.lblGuitarType.Size = new Size(31, 13);
            this.lblGuitarType.TabIndex = 24;
            this.lblGuitarType.Text = "Type";
            this.lblGuitarJoy.Location = new System.Drawing.Point(92, 104);
            this.lblGuitarJoy.Name = "lblGuitarJoy";
            this.lblGuitarJoy.Size = new Size(92, 32);
            this.lblGuitarJoy.TabIndex = 24;
            this.lblGuitarJoy.Text = "Joystick Values";
            this.clbGuitarButtons.FormattingEnabled = true;
            CheckedListBox.ObjectCollection items1 = this.clbGuitarButtons.Items;
            object[] objArray2 = new object[] { "Green", "Red", "Yellow", "Blue", "Orange", "-", "+", "StrumUp", "StrumDown" };
            items1.AddRange(objArray2);
            this.clbGuitarButtons.Location = new System.Drawing.Point(4, 16);
            this.clbGuitarButtons.Name = "clbGuitarButtons";
            this.clbGuitarButtons.Size = new Size(80, 139);
            this.clbGuitarButtons.TabIndex = 23;
            this.groupBox6.Controls.Add(this.lblTriggerR);
            this.groupBox6.Controls.Add(this.lblTriggerL);
            this.groupBox6.Controls.Add(this.lblCCJoy2);
            this.groupBox6.Controls.Add(this.lblCCJoy1);
            this.groupBox6.Controls.Add(this.clbCCButtons);
            this.groupBox6.Location = new System.Drawing.Point(364, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(188, 268);
            this.groupBox6.TabIndex = 35;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Classic Controller";
            this.clbCCButtons.FormattingEnabled = true;
            CheckedListBox.ObjectCollection objectCollection1 = this.clbCCButtons.Items;
            object[] objArray3 = new object[] { "A", "B", "X", "Y", "-", "Home", "+", "Up", "Down", "Left", "Right", "ZL", "ZR", "LTrigger", "RTrigger" };
            objectCollection1.AddRange(objArray3);
            this.clbCCButtons.Location = new System.Drawing.Point(4, 16);
            this.clbCCButtons.Name = "clbCCButtons";
            this.clbCCButtons.Size = new Size(68, 244);
            this.clbCCButtons.TabIndex = 23;
            this.groupBox4.Controls.Add(this.pbBattery);
            this.groupBox4.Controls.Add(this.lblBattery);
            this.groupBox4.Location = new System.Drawing.Point(184, 188);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(176, 52);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Battery";
            this.pbBattery.Location = new System.Drawing.Point(8, 20);
            this.pbBattery.Maximum = 200;
            this.pbBattery.Name = "pbBattery";
            this.pbBattery.Size = new Size(128, 23);
            this.pbBattery.Step = 1;
            this.pbBattery.TabIndex = 6;
            this.lblBattery.AutoSize = true;
            this.lblBattery.Location = new System.Drawing.Point(140, 24);
            this.lblBattery.Name = "lblBattery";
            this.lblBattery.Size = new Size(35, 13);
            this.lblBattery.TabIndex = 9;
            this.lblBattery.Text = "label1";
            this.groupBox3.Controls.Add(this.chkLED2);
            this.groupBox3.Controls.Add(this.chkLED4);
            this.groupBox3.Controls.Add(this.chkLED3);
            this.groupBox3.Controls.Add(this.chkLED1);
            this.groupBox3.Controls.Add(this.chkRumble);
            this.groupBox3.Location = new System.Drawing.Point(264, 248);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(96, 120);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Outputs";
            this.chkLED2.AutoSize = true;
            this.chkLED2.Location = new System.Drawing.Point(8, 36);
            this.chkLED2.Name = "chkLED2";
            this.chkLED2.Size = new Size(53, 17);
            this.chkLED2.TabIndex = 3;
            this.chkLED2.Text = "LED2";
            this.chkLED2.UseVisualStyleBackColor = true;
            this.chkLED2.CheckedChanged += new EventHandler(this.chkLED_CheckedChanged);
            this.chkLED4.AutoSize = true;
            this.chkLED4.Location = new System.Drawing.Point(8, 76);
            this.chkLED4.Name = "chkLED4";
            this.chkLED4.Size = new Size(53, 17);
            this.chkLED4.TabIndex = 3;
            this.chkLED4.Text = "LED4";
            this.chkLED4.UseVisualStyleBackColor = true;
            this.chkLED4.CheckedChanged += new EventHandler(this.chkLED_CheckedChanged);
            this.chkLED3.AutoSize = true;
            this.chkLED3.Location = new System.Drawing.Point(8, 56);
            this.chkLED3.Name = "chkLED3";
            this.chkLED3.Size = new Size(53, 17);
            this.chkLED3.TabIndex = 3;
            this.chkLED3.Text = "LED3";
            this.chkLED3.UseVisualStyleBackColor = true;
            this.chkLED3.CheckedChanged += new EventHandler(this.chkLED_CheckedChanged);
            this.chkLED1.AutoSize = true;
            this.chkLED1.Location = new System.Drawing.Point(8, 16);
            this.chkLED1.Name = "chkLED1";
            this.chkLED1.Size = new Size(53, 17);
            this.chkLED1.TabIndex = 3;
            this.chkLED1.Text = "LED1";
            this.chkLED1.UseVisualStyleBackColor = true;
            this.chkLED1.CheckedChanged += new EventHandler(this.chkLED_CheckedChanged);
            this.chkRumble.AutoSize = true;
            this.chkRumble.Location = new System.Drawing.Point(8, 96);
            this.chkRumble.Name = "chkRumble";
            this.chkRumble.Size = new Size(62, 17);
            this.chkRumble.TabIndex = 4;
            this.chkRumble.Text = "Rumble";
            this.chkRumble.UseVisualStyleBackColor = true;
            this.chkRumble.CheckedChanged += new EventHandler(this.chkRumble_CheckedChanged);
            this.chkZ.AutoSize = true;
            this.chkZ.Location = new System.Drawing.Point(8, 112);
            this.chkZ.Name = "chkZ";
            this.chkZ.Size = new Size(33, 17);
            this.chkZ.TabIndex = 17;
            this.chkZ.Text = "Z";
            this.chkZ.UseVisualStyleBackColor = true;
            this.chkC.AutoSize = true;
            this.chkC.Location = new System.Drawing.Point(8, 92);
            this.chkC.Name = "chkC";
            this.chkC.Size = new Size(33, 17);
            this.chkC.TabIndex = 17;
            this.chkC.Text = "C";
            this.chkC.UseVisualStyleBackColor = true;
            this.lblChuk.Location = new System.Drawing.Point(8, 20);
            this.lblChuk.Name = "lblChuk";
            this.lblChuk.Size = new Size(92, 40);
            this.lblChuk.TabIndex = 13;
            this.lblChuk.Text = "Accel Values";
            this.groupBox2.Controls.Add(this.chkZ);
            this.groupBox2.Controls.Add(this.chkC);
            this.groupBox2.Controls.Add(this.lblChuk);
            this.groupBox2.Controls.Add(this.lblChukJoy);
            this.groupBox2.Location = new System.Drawing.Point(76, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(104, 136);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Nunchuk";
            this.lblChukJoy.Location = new System.Drawing.Point(8, 64);
            this.lblChukJoy.Name = "lblChukJoy";
            this.lblChukJoy.Size = new Size(92, 28);
            this.lblChukJoy.TabIndex = 16;
            this.lblChukJoy.Text = "Joystick Values";
            this.groupBox1.Controls.Add(this.lblAccel);
            this.groupBox1.Location = new System.Drawing.Point(76, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(104, 72);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wiimote Accel";
            this.lblAccel.Location = new System.Drawing.Point(8, 20);
            this.lblAccel.Name = "lblAccel";
            this.lblAccel.Size = new Size(88, 48);
            this.lblAccel.TabIndex = 2;
            this.lblAccel.Text = "Accel Values";
            this.chkExtension.AutoSize = true;
            this.chkExtension.Location = new System.Drawing.Point(4, 224);
            this.chkExtension.Name = "chkExtension";
            this.chkExtension.Size = new Size(52, 17);
            this.chkExtension.TabIndex = 29;
            this.chkExtension.Text = "None";
            this.chkExtension.UseVisualStyleBackColor = true;
            this.groupBox9.Controls.Add(this.lblCOG);
            this.groupBox9.Controls.Add(this.chkLbs);
            this.groupBox9.Controls.Add(this.lblBBBR);
            this.groupBox9.Controls.Add(this.lblBBTR);
            this.groupBox9.Controls.Add(this.lblBBBL);
            this.groupBox9.Controls.Add(this.lblBBTotal);
            this.groupBox9.Controls.Add(this.lblBBTL);
            this.groupBox9.Location = new System.Drawing.Point(556, 0);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new Size(136, 112);
            this.groupBox9.TabIndex = 38;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Balance Board";
            this.lblCOG.AutoSize = true;
            this.lblCOG.Location = new System.Drawing.Point(8, 92);
            this.lblCOG.Name = "lblCOG";
            this.lblCOG.Size = new Size(30, 13);
            this.lblCOG.TabIndex = 2;
            this.lblCOG.Text = "COG";
            this.chkLbs.AutoSize = true;
            this.chkLbs.Location = new System.Drawing.Point(28, 68);
            this.chkLbs.Name = "chkLbs";
            this.chkLbs.Size = new Size(62, 17);
            this.chkLbs.TabIndex = 1;
            this.chkLbs.Text = "Pounds";
            this.chkLbs.UseVisualStyleBackColor = true;
            this.lblBBBR.AutoSize = true;
            this.lblBBBR.Location = new System.Drawing.Point(76, 48);
            this.lblBBBR.Name = "lblBBBR";
            this.lblBBBR.Size = new Size(22, 13);
            this.lblBBBR.TabIndex = 0;
            this.lblBBBR.Text = "BR";
            this.lblBBTR.AutoSize = true;
            this.lblBBTR.Location = new System.Drawing.Point(76, 16);
            this.lblBBTR.Name = "lblBBTR";
            this.lblBBTR.Size = new Size(22, 13);
            this.lblBBTR.TabIndex = 0;
            this.lblBBTR.Text = "TR";
            this.lblBBBL.AutoSize = true;
            this.lblBBBL.Location = new System.Drawing.Point(8, 48);
            this.lblBBBL.Name = "lblBBBL";
            this.lblBBBL.Size = new Size(20, 13);
            this.lblBBBL.TabIndex = 0;
            this.lblBBBL.Text = "BL";
            this.lblBBTotal.AutoSize = true;
            this.lblBBTotal.Location = new System.Drawing.Point(36, 32);
            this.lblBBTotal.Name = "lblBBTotal";
            this.lblBBTotal.Size = new Size(31, 13);
            this.lblBBTotal.TabIndex = 0;
            this.lblBBTotal.Text = "Total";
            this.lblBBTL.AutoSize = true;
            this.lblBBTL.Location = new System.Drawing.Point(8, 16);
            this.lblBBTL.Name = "lblBBTL";
            this.lblBBTL.Size = new Size(20, 13);
            this.lblBBTL.TabIndex = 0;
            this.lblBBTL.Text = "TL";
            this.lblDevicePath.AutoSize = true;
            this.lblDevicePath.Location = new System.Drawing.Point(8, 444);
            this.lblDevicePath.Name = "lblDevicePath";
            this.lblDevicePath.Size = new Size(63, 13);
            this.lblDevicePath.TabIndex = 39;
            this.lblDevicePath.Text = "DevicePath";
            this.groupBox10.Controls.Add(this.lbDrumVelocity);
            this.groupBox10.Controls.Add(this.lblDrumJoy);
            this.groupBox10.Controls.Add(this.clbDrums);
            this.groupBox10.Location = new System.Drawing.Point(556, 112);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new Size(136, 180);
            this.groupBox10.TabIndex = 40;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Drums";
            this.lbDrumVelocity.FormattingEnabled = true;
            this.lbDrumVelocity.Location = new System.Drawing.Point(68, 16);
            this.lbDrumVelocity.Name = "lbDrumVelocity";
            this.lbDrumVelocity.Size = new Size(56, 121);
            this.lbDrumVelocity.TabIndex = 41;
            this.lblDrumJoy.Location = new System.Drawing.Point(8, 144);
            this.lblDrumJoy.Name = "lblDrumJoy";
            this.lblDrumJoy.Size = new Size(92, 32);
            this.lblDrumJoy.TabIndex = 27;
            this.lblDrumJoy.Text = "Joystick Values";
            this.clbDrums.FormattingEnabled = true;
            CheckedListBox.ObjectCollection items2 = this.clbDrums.Items;
            object[] objArray4 = new object[] { "Red", "Blue", "Green", "Yellow", "Orange", "Pedal", "-", "+" };
            items2.AddRange(objArray4);
            this.clbDrums.Location = new System.Drawing.Point(4, 16);
            this.clbDrums.Name = "clbDrums";
            this.clbDrums.Size = new Size(60, 124);
            this.clbDrums.TabIndex = 26;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox10);
            base.Controls.Add(this.lblDevicePath);
            base.Controls.Add(this.groupBox9);
            base.Controls.Add(this.groupBox8);
            base.Controls.Add(this.groupBox5);
            base.Controls.Add(this.pbIR);
            base.Controls.Add(this.groupBox7);
            base.Controls.Add(this.groupBox6);
            base.Controls.Add(this.groupBox4);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.chkExtension);
            base.Name = "WiimoteInfo";
            base.Size = new Size(696, 464);
            this.groupBox8.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((ISupportInitialize)this.pbIR).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void UpdateExtension(WiimoteExtensionChangedEventArgs args)
        {
            WiimoteInfo.UpdateExtensionChangedDelegate updateExtensionChangedDelegate = new WiimoteInfo.UpdateExtensionChangedDelegate(this.UpdateExtensionChanged);
            object[] objArray = new object[] { args };
            base.BeginInvoke(updateExtensionChangedDelegate, objArray);
        }

        private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
        {
            this.chkExtension.Text = args.ExtensionType.ToString();
            this.chkExtension.Checked = args.Inserted;
        }

        private void UpdateIR(IRSensor irSensor, Label lblNorm, Label lblRaw, CheckBox chkFound, Color color)
        {
            chkFound.Checked = irSensor.Found;
            if (irSensor.Found)
            {
                lblNorm.Text = string.Concat(irSensor.Position.ToString(), ", ", irSensor.Size);
                lblRaw.Text = irSensor.RawPosition.ToString();
                this.g.DrawEllipse(new Pen(color), irSensor.RawPosition.X / 4, irSensor.RawPosition.Y / 4, irSensor.Size + 1, irSensor.Size + 1);
            }
        }

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            WiimoteInfo.UpdateWiimoteStateDelegate updateWiimoteStateDelegate = new WiimoteInfo.UpdateWiimoteStateDelegate(this.UpdateWiimoteChanged);
            object[] objArray = new object[] { args };
            base.BeginInvoke(updateWiimoteStateDelegate, objArray);
        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            WiimoteState wiimoteState = args.WiimoteState;
            this.clbButtons.SetItemChecked(0, wiimoteState.ButtonState.A);
            this.clbButtons.SetItemChecked(1, wiimoteState.ButtonState.B);
            this.clbButtons.SetItemChecked(2, wiimoteState.ButtonState.Minus);
            this.clbButtons.SetItemChecked(3, wiimoteState.ButtonState.Home);
            this.clbButtons.SetItemChecked(4, wiimoteState.ButtonState.Plus);
            this.clbButtons.SetItemChecked(5, wiimoteState.ButtonState.One);
            this.clbButtons.SetItemChecked(6, wiimoteState.ButtonState.Two);
            this.clbButtons.SetItemChecked(7, wiimoteState.ButtonState.Up);
            this.clbButtons.SetItemChecked(8, wiimoteState.ButtonState.Down);
            this.clbButtons.SetItemChecked(9, wiimoteState.ButtonState.Left);
            this.clbButtons.SetItemChecked(10, wiimoteState.ButtonState.Right);
            this.lblAccel.Text = wiimoteState.AccelState.Values.ToString();
            this.chkLED1.Checked = wiimoteState.LEDState.LED1;
            this.chkLED2.Checked = wiimoteState.LEDState.LED2;
            this.chkLED3.Checked = wiimoteState.LEDState.LED3;
            this.chkLED4.Checked = wiimoteState.LEDState.LED4;
            ExtensionType extensionType = wiimoteState.ExtensionType;
            if (extensionType <= (ExtensionType.Nunchuk | ExtensionType.ClassicController | ExtensionType.Guitar | ExtensionType.Drums))
            {
                if ((int)extensionType == -1541406720)
                {
                    this.lblChuk.Text = wiimoteState.NunchukState.AccelState.Values.ToString();
                    this.lblChukJoy.Text = wiimoteState.NunchukState.Joystick.ToString();
                    this.chkC.Checked = wiimoteState.NunchukState.C;
                    this.chkZ.Checked = wiimoteState.NunchukState.Z;
                }
                else if (extensionType <= (ExtensionType.Nunchuk | ExtensionType.ClassicController | ExtensionType.Guitar | ExtensionType.Drums) && extensionType >= (ExtensionType.Nunchuk | ExtensionType.ClassicController))
                {
                    switch ((int)((long)extensionType - (long)(ExtensionType.Nunchuk | ExtensionType.ClassicController)))
                    {
                        case 0:
                            {
                                this.clbCCButtons.SetItemChecked(0, wiimoteState.ClassicControllerState.ButtonState.A);
                                this.clbCCButtons.SetItemChecked(1, wiimoteState.ClassicControllerState.ButtonState.B);
                                this.clbCCButtons.SetItemChecked(2, wiimoteState.ClassicControllerState.ButtonState.X);
                                this.clbCCButtons.SetItemChecked(3, wiimoteState.ClassicControllerState.ButtonState.Y);
                                this.clbCCButtons.SetItemChecked(4, wiimoteState.ClassicControllerState.ButtonState.Minus);
                                this.clbCCButtons.SetItemChecked(5, wiimoteState.ClassicControllerState.ButtonState.Home);
                                this.clbCCButtons.SetItemChecked(6, wiimoteState.ClassicControllerState.ButtonState.Plus);
                                this.clbCCButtons.SetItemChecked(7, wiimoteState.ClassicControllerState.ButtonState.Up);
                                this.clbCCButtons.SetItemChecked(8, wiimoteState.ClassicControllerState.ButtonState.Down);
                                this.clbCCButtons.SetItemChecked(9, wiimoteState.ClassicControllerState.ButtonState.Left);
                                this.clbCCButtons.SetItemChecked(10, wiimoteState.ClassicControllerState.ButtonState.Right);
                                this.clbCCButtons.SetItemChecked(11, wiimoteState.ClassicControllerState.ButtonState.ZL);
                                this.clbCCButtons.SetItemChecked(12, wiimoteState.ClassicControllerState.ButtonState.ZR);
                                this.clbCCButtons.SetItemChecked(13, wiimoteState.ClassicControllerState.ButtonState.TriggerL);
                                this.clbCCButtons.SetItemChecked(14, wiimoteState.ClassicControllerState.ButtonState.TriggerR);
                                this.lblCCJoy1.Text = wiimoteState.ClassicControllerState.JoystickL.ToString();
                                this.lblCCJoy2.Text = wiimoteState.ClassicControllerState.JoystickR.ToString();
                                this.lblTriggerL.Text = wiimoteState.ClassicControllerState.TriggerL.ToString();
                                this.lblTriggerR.Text = wiimoteState.ClassicControllerState.TriggerR.ToString();
                                break;
                            }
                        case 2:
                            {
                                this.clbGuitarButtons.SetItemChecked(0, wiimoteState.GuitarState.FretButtonState.Green);
                                this.clbGuitarButtons.SetItemChecked(1, wiimoteState.GuitarState.FretButtonState.Red);
                                this.clbGuitarButtons.SetItemChecked(2, wiimoteState.GuitarState.FretButtonState.Yellow);
                                this.clbGuitarButtons.SetItemChecked(3, wiimoteState.GuitarState.FretButtonState.Blue);
                                this.clbGuitarButtons.SetItemChecked(4, wiimoteState.GuitarState.FretButtonState.Orange);
                                this.clbGuitarButtons.SetItemChecked(5, wiimoteState.GuitarState.ButtonState.Minus);
                                this.clbGuitarButtons.SetItemChecked(6, wiimoteState.GuitarState.ButtonState.Plus);
                                this.clbGuitarButtons.SetItemChecked(7, wiimoteState.GuitarState.ButtonState.StrumUp);
                                this.clbGuitarButtons.SetItemChecked(8, wiimoteState.GuitarState.ButtonState.StrumDown);
                                this.clbTouchbar.SetItemChecked(0, wiimoteState.GuitarState.TouchbarState.Green);
                                this.clbTouchbar.SetItemChecked(1, wiimoteState.GuitarState.TouchbarState.Red);
                                this.clbTouchbar.SetItemChecked(2, wiimoteState.GuitarState.TouchbarState.Yellow);
                                this.clbTouchbar.SetItemChecked(3, wiimoteState.GuitarState.TouchbarState.Blue);
                                this.clbTouchbar.SetItemChecked(4, wiimoteState.GuitarState.TouchbarState.Orange);
                                this.lblGuitarJoy.Text = wiimoteState.GuitarState.Joystick.ToString();
                                this.lblGuitarWhammy.Text = wiimoteState.GuitarState.WhammyBar.ToString();
                                this.lblGuitarType.Text = wiimoteState.GuitarState.GuitarType.ToString();
                                break;
                            }
                    }
                }
            }
            else if (extensionType == (ExtensionType.Nunchuk | ExtensionType.BalanceBoard))
            {
                if (!this.chkLbs.Checked)
                {
                    this.lblBBTL.Text = wiimoteState.BalanceBoardState.SensorValuesKg.TopLeft.ToString();
                    this.lblBBTR.Text = wiimoteState.BalanceBoardState.SensorValuesKg.TopRight.ToString();
                    this.lblBBBL.Text = wiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft.ToString();
                    this.lblBBBR.Text = wiimoteState.BalanceBoardState.SensorValuesKg.BottomRight.ToString();
                    this.lblBBTotal.Text = wiimoteState.BalanceBoardState.WeightKg.ToString();
                }
                else
                {
                    this.lblBBTL.Text = wiimoteState.BalanceBoardState.SensorValuesLb.TopLeft.ToString();
                    this.lblBBTR.Text = wiimoteState.BalanceBoardState.SensorValuesLb.TopRight.ToString();
                    this.lblBBBL.Text = wiimoteState.BalanceBoardState.SensorValuesLb.BottomLeft.ToString();
                    this.lblBBBR.Text = wiimoteState.BalanceBoardState.SensorValuesLb.BottomRight.ToString();
                    this.lblBBTotal.Text = wiimoteState.BalanceBoardState.WeightLb.ToString();
                }
                this.lblCOG.Text = wiimoteState.BalanceBoardState.CenterOfGravity.ToString();
            }
            else if (extensionType == ExtensionType.Drums)
            {
                this.clbDrums.SetItemChecked(0, wiimoteState.DrumsState.Red);
                this.clbDrums.SetItemChecked(1, wiimoteState.DrumsState.Blue);
                this.clbDrums.SetItemChecked(2, wiimoteState.DrumsState.Green);
                this.clbDrums.SetItemChecked(3, wiimoteState.DrumsState.Yellow);
                this.clbDrums.SetItemChecked(4, wiimoteState.DrumsState.Orange);
                this.clbDrums.SetItemChecked(5, wiimoteState.DrumsState.Pedal);
                this.clbDrums.SetItemChecked(6, wiimoteState.DrumsState.Minus);
                this.clbDrums.SetItemChecked(7, wiimoteState.DrumsState.Plus);
                this.lbDrumVelocity.Items.Clear();
                this.lbDrumVelocity.Items.Add(wiimoteState.DrumsState.RedVelocity);
                this.lbDrumVelocity.Items.Add(wiimoteState.DrumsState.BlueVelocity);
                this.lbDrumVelocity.Items.Add(wiimoteState.DrumsState.GreenVelocity);
                this.lbDrumVelocity.Items.Add(wiimoteState.DrumsState.YellowVelocity);
                this.lbDrumVelocity.Items.Add(wiimoteState.DrumsState.OrangeVelocity);
                this.lbDrumVelocity.Items.Add(wiimoteState.DrumsState.PedalVelocity);
                this.lblDrumJoy.Text = wiimoteState.DrumsState.Joystick.ToString();
            }
            this.g.Clear(Color.Black);
            this.UpdateIR(wiimoteState.IRState.IRSensors[0], this.lblIR1, this.lblIR1Raw, this.chkFound1, Color.Red);
            this.UpdateIR(wiimoteState.IRState.IRSensors[1], this.lblIR2, this.lblIR2Raw, this.chkFound2, Color.Blue);
            this.UpdateIR(wiimoteState.IRState.IRSensors[2], this.lblIR3, this.lblIR3Raw, this.chkFound3, Color.Yellow);
            this.UpdateIR(wiimoteState.IRState.IRSensors[3], this.lblIR4, this.lblIR4Raw, this.chkFound4, Color.Orange);
            if (wiimoteState.IRState.IRSensors[0].Found && wiimoteState.IRState.IRSensors[1].Found)
            {
                this.g.DrawEllipse(new Pen(Color.Green), wiimoteState.IRState.RawMidpoint.X / 4, wiimoteState.IRState.RawMidpoint.Y / 4, 2, 2);
            }
            this.pbIR.Image = this.b;
            this.pbBattery.Value = (wiimoteState.Battery > 200f ? 200 : (int)wiimoteState.Battery);
            this.lblBattery.Text = wiimoteState.Battery.ToString();
            this.lblDevicePath.Text = string.Concat("Device Path: ", this.mWiimote.HIDDevicePath);
        }

        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
    }
}