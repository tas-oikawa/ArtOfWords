using CommonControls.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonControls
{
    /// <summary>
    /// DeletableLabelStackPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class DeletableLabelStackPanel : UserControl
    {
        public DeletableLabelStackPanel()
        {
            InitializeComponent();
        }

        #region Properties

        private DeletableLabelStackPanelViewModel _model;

        private ObservableCollection<AppearListViewItemModel> _dataList = new ObservableCollection<AppearListViewItemModel>();

        public ObservableCollection<AppearListViewItemModel> DataList
        {
            get { return _dataList; }
            set 
            {
                _dataList = value;
                if (_model != null)
                {
                    _model.DataList = value;
                }
            }
        }

        private string _noItemMessage;

        public string NoItemMessage
        {
            get { return _noItemMessage; }
            set 
            { 
                _noItemMessage = value;

                if (_model != null)
                {
                    _model.NoItemErrorMessage = value;
                }
            }
        }

        public delegate void OnModelIsAppearedChangedEventHandler(object sender, OnModelIsAppearedChangedEventArgs e);
        public event OnModelIsAppearedChangedEventHandler OnModelIsAppearedChangedEvent;

        public void OnModelChanged(object sender)
        {
            if (OnModelIsAppearedChangedEvent != null)
            {
                OnModelIsAppearedChangedEvent(sender, new OnModelIsAppearedChangedEventArgs() { IsAppeared = (sender as AppearListViewItemModel).IsAppeared });
            }
        }

        #endregion

        private void DeletableLabelStackPanel_Loaded_1(object sender, RoutedEventArgs e)
        {
        }

        public void Initialize()
        {
            _model = new DeletableLabelStackPanelViewModel(this)
            {
                DataList = this.DataList,
                NoItemErrorMessage = this.NoItemMessage,
            };

            _model.Initialize();
        }
    }
}
