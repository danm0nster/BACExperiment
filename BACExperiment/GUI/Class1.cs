using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BACExperiment.GUI
{
    public partial class RichTextBoxDerivate : RichTextBox
    {
        private ObservableCollection<TextRange> AdditionalRanges;


        public RichTextBoxDerivate()
        {
         //   AdditionalRanges.CollectionChanged += AddRange;
        }
    }
}
