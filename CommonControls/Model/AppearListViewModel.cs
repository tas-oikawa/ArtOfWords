using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CommonControls.Model
{
    public class AppearListViewModel
    {
        private string _displayOrNoDisplayHeader = "表示する/しない";
        public string DisplayOrNoDisplayHeader
        {
            set
            {
                _displayOrNoDisplayHeader = value;
            }
            get
            {
                return _displayOrNoDisplayHeader;
            }
        }

        private ObservableCollection<AppearListViewItemModel> _dataList;
        public ObservableCollection<AppearListViewItemModel> DataList
        {
            set
            {
                _dataList = value;
            }
            get
            {
                return _dataList;
            }
        }

        public AppearListViewModel()
        {
        }
    }
}
