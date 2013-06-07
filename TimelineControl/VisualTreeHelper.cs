using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TimelineControl
{
    public static class VisualTreeHelper
    {
        public static T FindAncestor<T>(this DependencyObject depObj) where T : class
        {
            var target = depObj;

            try
            {
                do
                {
                    //ビジュアルツリー上の親を探します。
                    //T型のクラスにヒットするまでさかのぼり続けます。
                    target = System.Windows.Media.VisualTreeHelper.GetParent(target);

                } while (target != null && !(target is T));

                return target as T;
            }
            finally
            {
                target = null;
                depObj = null;
            }
        }
    }
}
