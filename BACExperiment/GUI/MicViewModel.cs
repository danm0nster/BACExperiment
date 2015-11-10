using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections;
using NAudio.Wave;
using BACExperiment.Model;
using System.Collections.ObjectModel;

namespace BACExperiment
{
    public class MicViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChangedImplementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private ObservableCollection<MicrophoneConstruct> _mics;
        private float _currentInputlevel1 = 0 ;
        private float _currentInputlevel2 = 0 ;


        public ObservableCollection<MicrophoneConstruct> Mics { get { return _mics; } set { _mics = value; if (PropertyChanged != null) { Notify("Mics"); }; } }
        public float CurrentInputLevel1 { get { return _currentInputlevel1*100; } set { _currentInputlevel1 = value; if (PropertyChanged != null) { Notify("CurrentInputLevel1"); }; }  }
        public float CurrentInputLevel2 { get { return _currentInputlevel2*100; } set { _currentInputlevel2 = value; if (PropertyChanged != null) { Notify("CurrentInputLevel2"); }; }  }

        public MicViewModel()
        {
            _mics = new ObservableCollection<MicrophoneConstruct>();
        }

        private void FromWaveInToMicItem(List<MicrophoneConstruct> list)
        {

            List<MicItem> mics = new List<MicItem>();
            foreach(MicrophoneConstruct item in list)
            {
                mics.Add(new MicItem(item));
            }
        }

        public void FillList(List<MicrophoneConstruct> list)
        {
            Mics.Clear();
            foreach(MicrophoneConstruct item in list)
            {
                Mics.Add(item);
            }
        }

        public class MicItem : ComboBoxItem
        {
            private MicrophoneConstruct mic;

            public MicItem()
            {

            }

            public MicItem( MicrophoneConstruct mic)
            {
                this.mic = mic;
            }
        }


    }
}
