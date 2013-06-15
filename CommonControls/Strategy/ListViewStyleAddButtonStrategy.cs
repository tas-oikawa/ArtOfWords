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
    public class ListViewStyleAddButtonStrategy : IDeletableLabelAddButtonStrategy
    {
        public void ExecuteOnAdd(ObservableCollection<AppearListViewItemModel> dataList)
        {
            CommonLightBox dialog = new CommonLightBox();

            AppearListViewControl chWindow = new AppearListViewControl();

            dialog.Owner = Application.Current.MainWindow;
            dialog.BindUIElement(chWindow);

            var listViewModel = new AppearListViewModel()
            {
                DataList = dataList,
                DisplayOrNoDisplayHeader = "登場する/しない"
            };

            chWindow.DataContext = listViewModel;
            ShowDialogManager.ShowDialog(dialog);

        }
    }
}
