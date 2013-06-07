using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace ModernizedAlice.ArtOfWords.BizCommon.Util
{
    public static class WPFUtil
    {
        public static void DoEvents()
        {
            DoEvents(DispatcherPriority.Background);
        }

        public static void DoEvents(DispatcherPriority targetPriority)
        {
            DispatcherFrame nestedFrame = new DispatcherFrame();
            DispatcherOperation exitOperation
                = Dispatcher.CurrentDispatcher.BeginInvoke(targetPriority,
                                                           new DispatcherOperationCallback(obj => ((DispatcherFrame)obj).Continue = false),
                                                           nestedFrame);

            try { Dispatcher.PushFrame(nestedFrame); }
            catch { }

            if (exitOperation.Status != DispatcherOperationStatus.Completed) exitOperation.Abort();
        }

    }
}
