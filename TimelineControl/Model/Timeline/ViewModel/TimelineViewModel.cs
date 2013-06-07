using CommonControls;
using CommonControls.Model;
using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TimelineControl.Model
{
    public class TimelineViewModel : INotifyPropertyChanged
    {
        private Timeline _view;
        private bool _isInitialized = false;

        public TimelineViewModel(Timeline control)
        {
            _view = control;
        }

        public void Initialize()
        {
            _view.DataContext = this;
            DrawAll();

            _isInitialized = true;
        }

        #region util系処理
        private string DateTimeString(DateTime dateTime)
        {
            switch (_timeSpanSelect)
            {
                case TimespanEnum.Day1:
                case TimespanEnum.Day2:
                case TimespanEnum.Day4:
                    return dateTime.ToString("dd日 HH時");
                case TimespanEnum.Hour1:
                case TimespanEnum.Hour3:
                case TimespanEnum.Hour6:
                case TimespanEnum.Hour12:
                    return dateTime.ToString("HH時mm分");
                case TimespanEnum.Min15:
                    return dateTime.ToString("mm分ss秒");
                case TimespanEnum.Month1:
                case TimespanEnum.Month2:
                case TimespanEnum.Month4:
                case TimespanEnum.Month6:
                    return dateTime.ToString("MM月dd日");
                case TimespanEnum.Week1:
                case TimespanEnum.Week2:
                    return dateTime.ToString("dd日 HH時");
                case TimespanEnum.Year1:
                    return dateTime.ToString("yy年MM月");
            }

            return dateTime.ToString("HH:mm:ss");
        }

        private void DrawAll()
        {
            _view.TimelinePanel.Children.Clear();
            GenerateBorders();
            GenerateScale();
            GenerateHeader();
        }

        public void GenerateScale()
        {
            _view.TimescalePanel.Children.Clear();

            var timeCollection = TimeRangeDivider.Divide(new TimeRange()
            {
                StartDateTime = _startDateTime,
                EndDateTime = GetEndDateTime()
            });

            TimelineGenerator gen =
                new TimelineGenerator(null, timeCollection, ScaleWidth, (double)0, CanvasHeight);

            gen.GenerateScale(_view.TimescalePanel);
        }

        public void GenerateHeader()
        {
            _view.TimelineHeaderPanel.Children.Clear();

            TimelineHeaderGenerator gen = new TimelineHeaderGenerator(_dataSource);
            gen.Generate(_view.TimelineHeaderPanel);
        }
        
        public void GenerateBorders()
        {
            _view.TimelinePanel.Children.Clear();

            ResetDataCanvasWidth();

            var timeCollection = TimeRangeDivider.Divide(new TimeRange()
            {
                StartDateTime = _startDateTime,
                EndDateTime = GetEndDateTime()
            });

            TimelineGenerator gen =
                new TimelineGenerator(_dataSource, timeCollection, DataCellWidth, (double)0, CanvasHeight);

            gen.GenerateBorders(_view.TimelinePanel);
            gen.GenerateEvents(_view.TimelinePanel, _eventModelManager);
        }
        
        private void ResetDataCanvasWidth()
        {
            DataCanvasWidth = 0;
            foreach (var data in _dataSource)
            {
                if (data.IsDisplayed)
                {
                    DataCanvasWidth += data.Width;
                }
            }
            DataCanvasWidth += 20;
        }

        public DateTime GetPrevDateTime()
        {
            try
            {
                switch (_timeSpanSelect)
                {
                    case TimespanEnum.Day1:
                        return _startDateTime.AddHours(-12);
                    case TimespanEnum.Day2:
                        return _startDateTime.AddDays(-1);
                    case TimespanEnum.Day4:
                        return _startDateTime.AddDays(-2);
                    case TimespanEnum.Hour1:
                        return _startDateTime.AddMinutes(-30);
                    case TimespanEnum.Hour3:
                        return _startDateTime.AddHours(-1).AddMinutes(-30);
                    case TimespanEnum.Hour6:
                        return _startDateTime.AddHours(-3);
                    case TimespanEnum.Hour12:
                        return _startDateTime.AddHours(-6);
                    case TimespanEnum.Min15:
                        return _startDateTime.AddMinutes(-7);
                    case TimespanEnum.Month1:
                        return _startDateTime.AddDays(-15);
                    case TimespanEnum.Month2:
                        return _startDateTime.AddMonths(-1);
                    case TimespanEnum.Month4:
                        return _startDateTime.AddMonths(-2);
                    case TimespanEnum.Month6:
                        return _startDateTime.AddMonths(-3);
                    case TimespanEnum.Week1:
                        return _startDateTime.AddDays(-4);
                    case TimespanEnum.Week2:
                        return _startDateTime.AddDays(-7);
                    case TimespanEnum.Year1:
                        return _startDateTime.AddDays(-183);
                }
                return _startDateTime.AddYears(-1);
            }
            catch (ArgumentOutOfRangeException)
            {
                ShowDialogManager.ShowMessageBox("開始日時をもっと大きな値にして下さい", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return _startDateTime;
            }
        }

        public DateTime GetNextDateTime()
        {
            try
            {
                switch (_timeSpanSelect)
                {
                    case TimespanEnum.Day1:
                        return _startDateTime.AddHours(12);
                    case TimespanEnum.Day2:
                        return _startDateTime.AddDays(1);
                    case TimespanEnum.Day4:
                        return _startDateTime.AddDays(2);
                    case TimespanEnum.Hour1:
                        return _startDateTime.AddMinutes(30);
                    case TimespanEnum.Hour3:
                        return _startDateTime.AddHours(1).AddMinutes(30);
                    case TimespanEnum.Hour6:
                        return _startDateTime.AddHours(3);
                    case TimespanEnum.Hour12:
                        return _startDateTime.AddHours(6);
                    case TimespanEnum.Min15:
                        return _startDateTime.AddMinutes(7);
                    case TimespanEnum.Month1:
                        return _startDateTime.AddDays(15);
                    case TimespanEnum.Month2:
                        return _startDateTime.AddMonths(1);
                    case TimespanEnum.Month4:
                        return _startDateTime.AddMonths(2);
                    case TimespanEnum.Month6:
                        return _startDateTime.AddMonths(3);
                    case TimespanEnum.Week1:
                        return _startDateTime.AddDays(4);
                    case TimespanEnum.Week2:
                        return _startDateTime.AddDays(7);
                    case TimespanEnum.Year1:
                        return _startDateTime.AddDays(183);
                }
                return _startDateTime.AddYears(1);
            }
            catch (ArgumentOutOfRangeException)
            {
                ShowDialogManager.ShowMessageBox("開始日時をもっと小さな値にして下さい", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return _startDateTime;
            }
        }

        public DateTime GetEndDateTime()
        {
            try
            {
                switch (_timeSpanSelect)
                {
                    case TimespanEnum.Day1:
                        return _startDateTime.AddDays(1);
                    case TimespanEnum.Day2:
                        return _startDateTime.AddDays(2);
                    case TimespanEnum.Day4:
                        return _startDateTime.AddDays(4);
                    case TimespanEnum.Hour1:
                        return _startDateTime.AddHours(1);
                    case TimespanEnum.Hour3:
                        return _startDateTime.AddHours(3);
                    case TimespanEnum.Hour6:
                        return _startDateTime.AddHours(6);
                    case TimespanEnum.Hour12:
                        return _startDateTime.AddHours(12);
                    case TimespanEnum.Min15:
                        return _startDateTime.AddMinutes(15);
                    case TimespanEnum.Month1:
                        return _startDateTime.AddMonths(1);
                    case TimespanEnum.Month2:
                        return _startDateTime.AddMonths(2);
                    case TimespanEnum.Month4:
                        return _startDateTime.AddMonths(4);
                    case TimespanEnum.Month6:
                        return _startDateTime.AddMonths(6);
                    case TimespanEnum.Week1:
                        return _startDateTime.AddDays(7);
                    case TimespanEnum.Week2:
                        return _startDateTime.AddDays(14);
                    case TimespanEnum.Year1:
                        return _startDateTime.AddYears(1);
                }
                return _startDateTime.AddYears(1);
            }
            catch (ArgumentOutOfRangeException )
            {
                ShowDialogManager.ShowMessageBox("開始日時をもっと小さな値にして下さい", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return _startDateTime;
            }
        }

        private void SlideToDetail()
        {
            switch (TimeSpanSelect)
            {
                case TimespanEnum.Year1:
                    TimeSpanSelect = TimespanEnum.Month6;
                    break;
                case TimespanEnum.Month6:
                    TimeSpanSelect = TimespanEnum.Month4;
                    break;
                case TimespanEnum.Month4:
                    TimeSpanSelect = TimespanEnum.Month2;
                    break;
                case TimespanEnum.Month2:
                    TimeSpanSelect = TimespanEnum.Month1;
                    break;
                case TimespanEnum.Month1:
                    TimeSpanSelect = TimespanEnum.Week2;
                    break;
                case TimespanEnum.Week2:
                    TimeSpanSelect = TimespanEnum.Week1;
                    break;
                case TimespanEnum.Week1:
                    TimeSpanSelect = TimespanEnum.Day4;
                    break;
                case TimespanEnum.Day4:
                    TimeSpanSelect = TimespanEnum.Day2;
                    break;
                case TimespanEnum.Day2:
                    TimeSpanSelect = TimespanEnum.Day1;
                    break;
                case TimespanEnum.Day1:
                    TimeSpanSelect = TimespanEnum.Hour12;
                    break;
                case TimespanEnum.Hour12:
                    TimeSpanSelect = TimespanEnum.Hour6;
                    break;
                case TimespanEnum.Hour6:
                    TimeSpanSelect = TimespanEnum.Hour3;
                    break;
                case TimespanEnum.Hour3:
                    TimeSpanSelect = TimespanEnum.Hour1;
                    break;
                case TimespanEnum.Hour1:
                    TimeSpanSelect = TimespanEnum.Min15;
                    break;
            }
        }

        private void SlideToRough()
        {
            switch (TimeSpanSelect)
            {
                case TimespanEnum.Year1:
                    break;
                case TimespanEnum.Month6:
                    TimeSpanSelect = TimespanEnum.Year1;
                    break;
                case TimespanEnum.Month4:
                    TimeSpanSelect = TimespanEnum.Month6;
                    break;
                case TimespanEnum.Month2:
                    TimeSpanSelect = TimespanEnum.Month4;
                    break;
                case TimespanEnum.Month1:
                    TimeSpanSelect = TimespanEnum.Month2;
                    break;
                case TimespanEnum.Week2:
                    TimeSpanSelect = TimespanEnum.Month1;
                    break;
                case TimespanEnum.Week1:
                    TimeSpanSelect = TimespanEnum.Week2;
                    break;
                case TimespanEnum.Day4:
                    TimeSpanSelect = TimespanEnum.Week1;
                    break;
                case TimespanEnum.Day2:
                    TimeSpanSelect = TimespanEnum.Day4;
                    break;
                case TimespanEnum.Day1:
                    TimeSpanSelect = TimespanEnum.Day2;
                    break;
                case TimespanEnum.Hour12:
                    TimeSpanSelect = TimespanEnum.Day1;
                    break;
                case TimespanEnum.Hour6:
                    TimeSpanSelect = TimespanEnum.Hour12;
                    break;
                case TimespanEnum.Hour3:
                    TimeSpanSelect = TimespanEnum.Hour6;
                    break;
                case TimespanEnum.Hour1:
                    TimeSpanSelect = TimespanEnum.Hour3;
                    break;
                case TimespanEnum.Min15:
                    TimeSpanSelect = TimespanEnum.Hour1;
                    break;
            }
        }

        public ObservableCollection<AppearListViewItemModel> GenerateAppearListViewItemModel(TimeBorderViewModel borderModel)
        {
            ObservableCollection<AppearListViewItemModel> modelList = new ObservableCollection<AppearListViewItemModel>();

            foreach(var item in _dataSource)
            {
                if (item.IsUnbound)
                {
                    continue;
                }

                if(borderModel.SourceObject.Id == item.Id)
                {
                    modelList.Add(new AppearListViewItemModel(item.HeaderName, true,"登場する", "登場しない", item) { BackgroundBrush = item.DrawBrush });
                }
                else
                {
                    modelList.Add(new AppearListViewItemModel(item.HeaderName, false,"登場する", "登場しない", item) { BackgroundBrush = item.DrawBrush });
                }
            }
            return modelList;
        }

        public ObservableCollection<AppearListViewItemModel> GenerateAppearListViewItemModel(EventModel eventModel)
        {
            ObservableCollection<AppearListViewItemModel> modelList = new ObservableCollection<AppearListViewItemModel>();

            foreach (var item in _dataSource)
            {
                if (item.IsUnbound)
                {
                    continue;
                }

                if (eventModel.Participants.Contains(item.Id))
                {
                    modelList.Add(new AppearListViewItemModel(item.HeaderName, true, "登場する", "登場しない", item) { BackgroundBrush = item.DrawBrush });
                }
                else
                {
                    modelList.Add(new AppearListViewItemModel(item.HeaderName, false, "登場する", "登場しない", item) { BackgroundBrush = item.DrawBrush });
                }
            }
            return modelList;
        }


        public List<int> GetParticipants(ICollection<AppearListViewItemModel> apModels)
        {
            List<int> ids = new List<int>();

            foreach (var ap in apModels)
            {
                if (ap.IsAppeared)
                {
                    var axis = ap.ParentObject as TimelineAxis;
                    ids.Add(axis.Id);
                }
            }
            return ids;
        }

        #endregion

        #region event処理

        public void ShowDetail(DateTime startTime)
        {
            StartDateTime = startTime;
            SlideToDetail();
        }

        public void ShowRough(DateTime startTime)
        {
            SlideToRough();
            StartDateTime = startTime.AddTicks((StartDateTime.Ticks - GetEndDateTime().Ticks) / 2);
        }


        public void ShowEventOnVacantCell(TimeBorderViewModel borderModel)
        {
            EventViewModel evtModel = new EventViewModel()
            {
                StartDateTime = borderModel.StartDateTime,
                EndDateTime = borderModel.EndDateTime,
                AppearListViewItems = GenerateAppearListViewItemModel(borderModel)
            };

            CommonLightBox lightBox = new CommonLightBox();
            EventRegister register = new EventRegister();

            register.DataContext = evtModel;

            lightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveCancel;
            lightBox.BindUIElement(register);
            lightBox.Owner = Application.Current.MainWindow;
            lightBox.OnSaveAndQuit += register.OnSaveAndQuit;

            if (ShowDialogManager.ShowDialog(lightBox) == true)
            {
                var participants = GetParticipants(evtModel.AppearListViewItems);
                // 参加者がいないイベント作っても意味ないよね
                if (participants.Count() == 0)
                {
                    return;
                }

                var addEvent = new EventModel()
                    {
                        StartDateTime = evtModel.StartDateTime,
                        EndDateTime = evtModel.EndDateTime,
                        Title = evtModel.Title,
                        Detail = evtModel.Detail,
                        Participants = participants,
                    };
                _eventModelManager.Add(addEvent);
                _view.OnEventChanged(_view, new Timeline.EventChangedArgs(){kind = Timeline.EventChangedKind.Add, model = addEvent});
                Initialize();
            }
        }

        public void ShowExistEvent(EventModel eventModel)
        {
            EventViewModel evtModel = new EventViewModel()
            {
                StartDateTime = eventModel.StartDateTime,
                EndDateTime = eventModel.EndDateTime,
                Detail = eventModel.Detail,
                Title = eventModel.Title,
                AppearListViewItems = GenerateAppearListViewItemModel(eventModel)
            };

            CommonLightBox lightBox = new CommonLightBox();
            EventRegister register = new EventRegister();

            register.DataContext = evtModel;

            lightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveCancel;
            lightBox.BindUIElement(register);
            lightBox.Owner = Application.Current.MainWindow;

            if (ShowDialogManager.ShowDialog(lightBox) == true)
            {
                _eventModelManager.Remove(eventModel);

                var participants = GetParticipants(evtModel.AppearListViewItems);
                // 参加者がいないイベント作っても意味ないよね
                if (participants.Count() == 0)
                {
                    _view.OnEventChanged(_view, new Timeline.EventChangedArgs() { kind = Timeline.EventChangedKind.Delete, model = eventModel });
                }
                else
                {
                    var addEvent = new EventModel()
                            {
                                StartDateTime = evtModel.StartDateTime,
                                EndDateTime = evtModel.EndDateTime,
                                Title = evtModel.Title,
                                Detail = evtModel.Detail,
                                Participants = GetParticipants(evtModel.AppearListViewItems),
                                SourceObject = eventModel.SourceObject,
                            };
                    _eventModelManager.Add(addEvent);
                    _view.OnEventChanged(_view, new Timeline.EventChangedArgs() { kind = Timeline.EventChangedKind.Modify, model = addEvent });
                }
                Initialize();
            }
        }

        public void DeleteEvent(EventBorderViewModel eventViewModel)
        {
            _eventModelManager.Remove(eventViewModel.Parent);
            _view.OnEventChanged(_view, new Timeline.EventChangedArgs() { kind = Timeline.EventChangedKind.Delete, model = eventViewModel.Parent });

            Initialize();
        }

        public void GoNextRange()
        {
            StartDateTime = GetNextDateTime();
        }

        public void GoPreviousRange()
        {
            StartDateTime = GetPrevDateTime();
        }

        public void ShowTimelineAxisEvents(TimelineAxis axis)
        {
            EventModelManager manager = _eventModelManager;

            var list = manager.GetEventModel(axis.Id);
            var eventListViewerViewModel = new EventViewerViewModel() { DataList = new List<EventViewerItemViewModel>() };

            foreach (var item in list)
            {
                eventListViewerViewModel.DataList.Add(new EventViewerItemViewModel()
                {
                    StartDateTime = item.StartDateTime,
                    EndDateTime = item.EndDateTime,
                    Name = item.Title,
                });
            }

            CommonLightBox lightBox = new CommonLightBox();
            EventListViewer viewer = new EventListViewer();

            viewer.DataContext = eventListViewerViewModel;

            lightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.CancelOnly;
            lightBox.BindUIElement(viewer);
            lightBox.Owner = Application.Current.MainWindow;

            if (ShowDialogManager.ShowDialog(lightBox) == true)
            {
                StartDateTime = viewer.JumpDateTime;
            }
        }

        #endregion

        #region Properties

        public static int Height
        {
            get
            {
                return 800;
            }
        }

        private double _dataCellWidth;

        public double DataCellWidth
        {
            get { return _dataCellWidth; }
            set { _dataCellWidth = value; }
        }

        #region TimeSpan
        private TimespanEnum _timeSpanSelect;
        public TimespanEnum TimeSpanSelect
        {
            set
            {
                if (value != _timeSpanSelect)
                {
                    _timeSpanSelect = value;
                    OnPropertyChanged("IsSpanSelectMin15");
                    OnPropertyChanged("IsSpanSelectHour1");
                    OnPropertyChanged("IsSpanSelectHour3");
                    OnPropertyChanged("IsSpanSelectHour6");
                    OnPropertyChanged("IsSpanSelectHour12");
                    OnPropertyChanged("IsSpanSelectDay1");
                    OnPropertyChanged("IsSpanSelectDay2");
                    OnPropertyChanged("IsSpanSelectDay4");
                    OnPropertyChanged("IsSpanSelectWeek1");
                    OnPropertyChanged("IsSpanSelectWeek2");
                    OnPropertyChanged("IsSpanSelectMonth1");
                    OnPropertyChanged("IsSpanSelectMonth2");
                    OnPropertyChanged("IsSpanSelectMonth4");
                    OnPropertyChanged("IsSpanSelectMonth6");
                    OnPropertyChanged("IsSpanSelectYear1");
                    OnPropertyChanged("UpperButtonText");
                    OnPropertyChanged("LowerButtonText");

                    if (_isInitialized)
                    {
                        DrawAll();
                    }
                }
            }
            get
            {
                return _timeSpanSelect;
            }
        }

        public bool IsSpanSelectMin15
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Min15;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Min15;
            }
        }

        public bool IsSpanSelectHour1
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Hour1;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Hour1;
            }
        }

        public bool IsSpanSelectHour3
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Hour3;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Hour3;
            }
        }

        public bool IsSpanSelectHour6
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Hour6;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Hour6;
            }
        }

        public bool IsSpanSelectHour12
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Hour12;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Hour12;
            }
        }

        public bool IsSpanSelectDay1
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Day1;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Day1;
            }
        }
        public bool IsSpanSelectDay2
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Day2;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Day2;
            }
        }
        public bool IsSpanSelectDay4
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Day4;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Day4;
            }
        }
        public bool IsSpanSelectWeek1
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Week1;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Week1;
            }
        }

        public bool IsSpanSelectWeek2
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Week2;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Week2;
            }
        }
        public bool IsSpanSelectMonth1
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Month1;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Month1;
            }
        }

        public bool IsSpanSelectMonth2
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Month2;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Month2;
            }
        }

        public bool IsSpanSelectMonth4
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Month4;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Month4;
            }
        }
        public bool IsSpanSelectMonth6
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Month6;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Month6;
            }
        }


        public bool IsSpanSelectYear1
        {
            set
            {
                if (value == true)
                {
                    TimeSpanSelect = TimespanEnum.Year1;
                }
            }
            get
            {
                return _timeSpanSelect == TimespanEnum.Year1;
            }
        }
#endregion

        private DateTime _startDateTime;

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set 
            { 
                _startDateTime = value;
                OnPropertyChanged("StartDateTime");
                OnPropertyChanged("StartDate");
                OnPropertyChanged("StartTime");
                OnPropertyChanged("UpperButtonText");
                OnPropertyChanged("LowerButtonText");
                if (_isInitialized)
                {
                    DrawAll();
                }
            }
        }

        public DateTime StartDate
        {
            get { return _startDateTime; }
            set
            {
                StartDateTime = new DateTime(value.Year, value.Month, value.Day, _startDateTime.Hour, _startDateTime.Minute, _startDateTime.Second);
            }
        }


        public TimeSpan StartTime
        {
            get { return new TimeSpan(_startDateTime.Hour, _startDateTime.Minute, _startDateTime.Second); }
            set
            {
                StartDateTime = new DateTime(_startDateTime.Year, _startDateTime.Month, _startDateTime.Day, value.Hours, value.Minutes, value.Seconds);
            }
        }

        public string UpperButtonText
        {
            get
            {
                return "▲" + DateTimeString(StartDateTime) + "より前へ";
            }
        }

        public string LowerButtonText
        {
            get
            {
                return "▼" +  DateTimeString(GetEndDateTime()) + "より後へ";
            }
        }

        private ICollection<TimelineAxis> _dataSource;
        public ICollection<TimelineAxis> DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        private EventModelManager _eventModelManager;

        public EventModelManager EventModelManager
        {
            get { return _eventModelManager; }
            set { _eventModelManager = value; }
        }

        #endregion

        #region 定義
        public double ScaleWidth 
        {
            get
            {
                return 160;
            }
        }
        public double CanvasHeight
        {
            get
            {
                return 1400.0;
            }
        }
        private double _dataCanvasWidth;
        public double DataCanvasWidth
        {
            get
            {
                return _dataCanvasWidth;
            }
            set
            {
                if (value != _dataCanvasWidth)
                {
                    _dataCanvasWidth = value;
                    OnPropertyChanged("DataCanvasWidth");
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
