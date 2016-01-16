using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACExperiment.GUI
{
    class Instructions
    {
        public const string ASYNCHRONOUS_READING ="You will now be presented with a text. You will have to read it aloud in"+
            " turns: A will read the lines in blue, B will read the lines in red"+
            " Please, try to keep the reading as fluent as possible, that is, take turns in a way that the reading feels natural"+
            "(not too long or short pauses between lines).";

        public const string SYNCHRONOUS_READING = "You will now be presented with a text. You will have to read it aloud together"+
            " keeping pace with the cursor. Please, try to synchronize with the cursor asaccurately as possible.";

        public const string SELFPACED_READING = "You will now be presented with a text.You will have to read it aloud together."+
            "Please, try to keep the same tempo, that is, to have your reading as synchronized with each other as possible."+
            "Start as soon as you are ready.";

        public const string ASYNCHRONOUS_MOVEMENT = "You will now be presented with a line. You willhave to follow it in turns "+
            "with your cursors: A will have to follow the parts in blue, B those in red. Be as accurate (staying on the line) and fast as possible."+
            "Please, try to keep the movement as fluent as possible, that is, the turn taking should have no gaps and maintain the same speed."+
            "Start as soon as you are ready.";

        public const string SYNCHRONOUS_MOVEMENT = "You will now be presented with a line. You will have to follow it together, keeping pace with the cursor."+
            "Please, try to synchronize with the cursor as accurately as possible. Start as soon as the cursor starts moving.";

        public const string SELFPACED_MOVEMENT = "You will now be presented with a line. You will have to follow it together with your cursor."+
            " Be as accurate (staying on the line) and fast as possible. Please, try to keep the same tempo, that is, to  have your cursors on"+
            " top of each other as  much as possible.Start as soon as you are ready.";

        public const string WII_FAMILIARIZATION = "You will now be given a Wii controller each. Each controller has its own cursor on the screen."+
            "Please take a moment to play with it and get comfortable moving the cursor on the screen.";
    }
}
