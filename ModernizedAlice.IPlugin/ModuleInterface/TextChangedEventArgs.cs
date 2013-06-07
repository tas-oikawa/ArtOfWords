using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ModernizedAlice.IPlugin.ModuleInterface
{
    public class MyTextChange
    {
        public MyTextChange()
        {
        }

        public int AddedLength;
        public int Offset;
        public int RemovedLength;
    }

    public class TextChangedEventArgs
    {
        public object Source;
        public ICollection<MyTextChange> Changes;
    }
}
