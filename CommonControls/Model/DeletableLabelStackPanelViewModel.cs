using CommonControls;
using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonControls.Model
{
    public class DeletableLabelStackPanelViewModel
    {
        private DeletableLabelStackPanel _view;

        private ObservableCollection<AppearListViewItemModel> _dataList;
        public ObservableCollection<AppearListViewItemModel> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }

        public string NoItemErrorMessage;


        public DeletableLabelStackPanelViewModel(DeletableLabelStackPanel view)
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.LabelStackPabel.Children.Clear();

            if (_dataList.Count == 0)
            {
                PutNoItemTextBlock();
            }
            else
            {
                PutAddItemNewButton();
            }

            foreach (var item in _dataList)
            {
                if (item.IsAppeared)
                {
                    PutItemButton(item);
                }
            }
        }

        private void PutNoItemTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = NoItemErrorMessage;
            textBlock.Foreground = Brushes.OrangeRed;
            _view.LabelStackPabel.Children.Add(textBlock);
        }

        private void PutAddItemNewButton()
        {
            Button button = new Button();

            button.Content = " + ";
            button.Background = Brushes.AliceBlue;
            button.Click += addItembutton_Click;
            button.Margin = new Thickness(5, 5, 5, 5);
            _view.LabelStackPabel.Children.Add(button);
        }

        public void PutItemButton(AppearListViewItemModel model)
        {
            DeletableLabelControl deleteLabelControl = new DeletableLabelControl();

            deleteLabelControl.DataContext = model;
            deleteLabelControl.OnDeletePushed += OnDeleteItemButtonPushedHandler;

            _view.LabelStackPabel.Children.Add(deleteLabelControl);
        }

        private void addItembutton_Click(object sender, RoutedEventArgs e)
        {
            CommonLightBox dialog = new CommonLightBox();

            AppearListViewControl chWindow = new AppearListViewControl();

            dialog.Owner = Application.Current.MainWindow;
            dialog.BindUIElement(chWindow);

            var listViewModel = new AppearListViewModel()
            {
                DataList = _dataList,
                DisplayOrNoDisplayHeader = "登場する/しない"
            };

            chWindow.DataContext = listViewModel;
            ShowDialogManager.ShowDialog(dialog);

            foreach (var item in DataList)
            {
                if (item.IsAppearedChanged)
                {
                    _view.OnModelChanged(item);
                    item.IsAppearedChanged = false;
                }
            }

            Initialize();
        }


        public void OnDeleteItemButtonPushedHandler(object sender, RoutedEventArgs e)
        {
            if (ShowDialogManager.ShowMessageBox("削除してもいいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            var model = (AppearListViewItemModel)((sender as Control).DataContext);

            model.IsAppeared = false;
            _view.OnModelChanged(model);

            Initialize();
        }

    }
}
