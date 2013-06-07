using CommonControls.Model;
using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TimelineControl.Model
{
    public class EventViewModel
    {
        public bool IsSavable()
        {
            if (StartDateTime >= EndDateTime)
            {
                ShowDialogManager.ShowMessageBox("終了時刻には開始時刻よりも後の時刻を設定してください。", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (AppearListViewItems.Count(item => item.IsAppeared == true) == 0)
            {
                var res = ShowDialogManager.ShowMessageBox("このイベントに登録されている人が一人もいません。イベントごと削除しますけどだいじょうぶですか？", "確認",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                return res == MessageBoxResult.Yes;
            }

            return true;
        }

        #region Properties

        private DateTime _startDateTime;

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set
            {
                _startDateTime = value;
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

        private DateTime _endDateTime;

        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set
            {
                _endDateTime = value;
            }
        }

        public DateTime EndDate
        {
            get { return _endDateTime; }
            set
            {
                EndDateTime = new DateTime(value.Year, value.Month, value.Day, _endDateTime.Hour, _endDateTime.Minute, _endDateTime.Second);
            }
        }


        public TimeSpan EndTime
        {
            get { return new TimeSpan(_endDateTime.Hour, _endDateTime.Minute, _endDateTime.Second); }
            set
            {
                EndDateTime = new DateTime(_endDateTime.Year, _endDateTime.Month, _endDateTime.Day, value.Hours, value.Minutes, value.Seconds);
            }
        }

        private String _title;

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private String _detail;

        public String Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        private ObservableCollection<AppearListViewItemModel> _appearListViewItems;

        public ObservableCollection<AppearListViewItemModel> AppearListViewItems
        {
            get { return _appearListViewItems; }
            set { _appearListViewItems = value; }
        }

        #endregion
    }
}
