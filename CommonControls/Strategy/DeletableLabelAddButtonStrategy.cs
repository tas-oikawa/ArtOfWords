using CommonControls.Model;
using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace CommonControls.Strategy
{
    public interface IDeletableLabelAddButtonStrategy
    {
        void ExecuteOnAdd(ObservableCollection<AppearListViewItemModel> dataList);
    }
}
