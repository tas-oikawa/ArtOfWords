using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonControls.Util
{
    public class ShowDialogManager
    {
        public static int ShowingDialogCount = 0;

        public static bool? ShowDialog(Window window)
        {
            ShowingDialogCount++;
             var result = window.ShowDialog();
            ShowingDialogCount--;

            return result;
        }

        public static bool? ShowDialog(CommonDialog window)
        {
            ShowingDialogCount++;
            var result = window.ShowDialog();
            ShowingDialogCount--;

            return result;
        }

        public static MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            ShowingDialogCount++;
            var result = MessageBox.Show(text, caption, button, image);
            ShowingDialogCount--;

            return result;
        }
    }
}
