using CommonControls;
using CommonControls.Model;
using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TimelineControl.Model;

namespace TimelineControl
{
    /// <summary>
    /// Interaction logic for TimelineControl.xaml
    /// </summary>
    public partial class Timeline : UserControl
    {
        #region Properties
        private DateTime _startDateTime = DateTime.Today;
        private TimespanEnum _timespanEnum = TimespanEnum.Hour3;
        private ICollection<TimelineAxis> _dataSource = new List<TimelineAxis>() { };
        private EventModelManager _eventModelManager = new EventModelManager();

        private double _dataCellWidth = 180;


        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set 
            { 
                _startDateTime = value;
                if (_timelineViewModel != null)
                {
                    _timelineViewModel.StartDateTime = value;
                }
            }
        }
        public TimespanEnum TimespanEnum
        {
            get { return _timespanEnum; }
            set 
            {
                _timespanEnum = value;
                if (_timelineViewModel != null)
                {
                    _timelineViewModel.TimeSpanSelect = value;
                }
            }
        }
        public ICollection<TimelineAxis> DataSource
        {
            get { return _dataSource; }
            set 
            { 
                _dataSource = value;
                if (_timelineViewModel != null)
                {
                    _timelineViewModel.DataSource = value;
                }
            }
        }

        public EventModelManager EventModelManager
        {
            get { return _eventModelManager; }
            set
            {
                _eventModelManager = value;
                if (_timelineViewModel != null)
                {
                    _timelineViewModel.EventModelManager = value;
                }
            }
        }

        #endregion

        #region Events

        public enum EventChangedKind
        {
            Add,
            Modify,
            Delete
        }

        public class EventChangedArgs : EventArgs
        {
            public EventChangedKind kind;
            public EventModel model;
        }

        public delegate void OnEventChangedHandler(object sender, EventChangedArgs e);

        public event OnEventChangedHandler EventChangedRised;

        public void OnEventChanged(object sender, EventChangedArgs e)
        {
            if (EventChangedRised != null)
            {
                EventChangedRised(sender, e);
            }
        }

        #endregion

        private TimelineViewModel _timelineViewModel;
        public Timeline()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged_1(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                TimescalePanelScroller.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private void ScrollViewer_ScrollChanged_2(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                TimelineHeaderPanelScroller.ScrollToHorizontalOffset(e.HorizontalOffset);
                TimelinePanelScroller.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        private void TimescalePanelScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                TimelinePanelScroller.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (_timelineViewModel == null)
            {
                _timelineViewModel = new TimelineViewModel(this)
                {
                    TimeSpanSelect = _timespanEnum,
                    StartDateTime = _startDateTime,
                    DataSource = _dataSource,
                    DataCellWidth = _dataCellWidth,
                    EventModelManager = _eventModelManager,
                };

            }
            _timelineViewModel.Initialize();
        }

        public void OnScaleBorderLeftClicked(object sender, MouseButtonEventArgs e)
        {
            var context = (sender as Border).DataContext as TimeBorderViewModel;

            _timelineViewModel.ShowDetail(context.StartDateTime);
        }

        public void OnScaleBorderRightClicked(object sender, MouseButtonEventArgs e)
        {
            var context = (sender as Border).DataContext as TimeBorderViewModel;

            _timelineViewModel.ShowRough(context.StartDateTime);
        }

        public void OnHeaderBorderLeftClicked(object sender, MouseButtonEventArgs e)
        {
            var context = (sender as Border).DataContext as TimelineAxis;

            _timelineViewModel.ShowTimelineAxisEvents(context);
        }

        public void OnDataBorderLeftClicked(object sender, MouseButtonEventArgs e)
        {
            var context = (sender as Border).DataContext as TimeBorderViewModel;
            if (context != null)
            {
                _timelineViewModel.ShowEventOnVacantCell(context);
            }
            else
            {
                var eventContext = (sender as Border).DataContext as EventBorderViewModel;
                _timelineViewModel.ShowExistEvent(eventContext.Parent);
            }
        }

        public void OnUnboundDataBorderLeftClicked(object sender, MouseButtonEventArgs e)
        {
        }

        private void UpperButton_Click(object sender, RoutedEventArgs e)
        {
            _timelineViewModel.GoPreviousRange();
        }

        private void LowerButton_Click(object sender, RoutedEventArgs e)
        {
            _timelineViewModel.GoNextRange();
        }

        private void DeleteEventItem(EventBorderViewModel delModel)
        {
            if (delModel != null)
            {
                if (ShowDialogManager.ShowMessageBox("このイベントを削除してもいいですか？\n（選択した人物に関わらず同じイベントは全部消えます）", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes)
                {
                    _timelineViewModel.DeleteEvent(delModel);
                }
            }
        }

        public void OnButtonDeletePushed(object sender, MouseButtonEventArgs e)
        {
            var eventContext = VisualTreeHelper.FindAncestor<Border>(sender as DependencyObject).DataContext as EventBorderViewModel;
            DeleteEventItem(eventContext);
            e.Handled = true;
        }

        private void DeleteEventItem_MenuEvent(object sender, RoutedEventArgs e)
        {
            var eventContext = VisualTreeHelper.FindAncestor<Border>(sender as DependencyObject).DataContext as EventBorderViewModel;
            DeleteEventItem(eventContext);
            e.Handled = true;
        }

        private ObservableCollection<AppearListViewItemModel> GetAppearList()
        {
            ObservableCollection<AppearListViewItemModel> newList = new ObservableCollection<AppearListViewItemModel>();

            foreach (var data in DataSource)
            {
                newList.Add(new AppearListViewItemModel(data.HeaderName, data.IsDisplayed, "表示する", "表示しない", data));
            }
            return newList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CommonLightBox dialog = new CommonLightBox();

            AppearListViewControl chWindow = new AppearListViewControl();

            dialog.Owner = Application.Current.MainWindow;
            dialog.BindUIElement(chWindow);

            var appearList = GetAppearList();
            var listViewModel = new AppearListViewModel()
            {
                DataList = appearList,
                DisplayOrNoDisplayHeader = "表示する/しない",
            };
            
            chWindow.DataContext = listViewModel;
            ShowDialogManager.ShowDialog(dialog);

            foreach (var item in listViewModel.DataList)
            {
                var data = item.ParentObject as TimelineAxis;
                data.IsDisplayed = item.IsAppeared;
            }

            _timelineViewModel.Initialize();
        }
    }
}
