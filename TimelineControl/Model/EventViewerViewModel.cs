using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimelineControl.Model
{
    public class EventViewerItemViewModel
    {
        private string _name;
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }
        public string StartDateTimeString
        {
            get { return _startDateTime.ToString("yyyy/MM/dd HH:mm:ss"); }
        }

        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { _endDateTime = value; }
        }

        public string EndDateTimeString
        {
            get { return _endDateTime.ToString("yyyy/MM/dd HH:mm:ss"); }
        }

    }
    public class EventViewerViewModel
    {
        private List<EventViewerItemViewModel> _dataList;

        public List<EventViewerItemViewModel> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }
    }
}
