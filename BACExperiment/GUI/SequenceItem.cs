using System.Windows.Media;

namespace BACExperiment
{
    class SequenceItem
    {

        private string stimuly;
        private string mode;
           
        public SequenceItem()
        {

        }           
    }

    class MovementSequenceItem: SequenceItem
    {
        private System.Windows.Media.Color color1_movement;
        private System.Windows.Media.Color color2_movement;
        private System.Windows.Media.Color color3_movement;

        private int complexity1;
        private int speed1;

        private int complexity2;
        private int speed2;

        private int linethickness;

        public MovementSequenceItem(Color color1, Color color2, Color color3, int complexity1, int speed1, int complexity2, int speed2, int linethickness)
        {

        }

    }

    class ReadingSequenceItem: SequenceItem
    {
        private System.Windows.Media.Color color1_reading;
        private System.Windows.Media.Color color2_reading;
        private System.Windows.Media.Color color3_reading;


        private int textSize;
        private string path;

        private int scrollSpeed;

        private int traversalSpeed_Async;
        private int turnDuration;
        private int switchDuration;

        private int traversalSpeed_Sync;

    }
}