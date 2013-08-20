using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public static class ViewModelEx
    {
        public static void OnPropertyChanged<TObj, TProp>(this TObj self, Expression<Func<TObj, TProp>> e)
            where TObj : NotifyPropertyChangedBase
        {
            var name = ((MemberExpression)e.Body).Member.Name;
            self.OnPropertyChanged(name);
        }
    }
}
