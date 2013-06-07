using CommonControls.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TimelineControl.Model
{
    public class EventModel
    {
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
        private DateTime _endDateTime;

        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set
            {
                _endDateTime = value;
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _detail;

        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        private List<int> _participants;

        public List<int> Participants
        {
            get { return _participants; }
            set { _participants = value; }
        }

        private object _sourceObject;

        public object SourceObject
        {
            get { return _sourceObject; }
            set { _sourceObject = value; }
        }

        #endregion
    }
}
