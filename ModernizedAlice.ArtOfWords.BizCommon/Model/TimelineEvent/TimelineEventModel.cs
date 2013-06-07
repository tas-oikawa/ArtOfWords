using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent
{
    public class TimelineEventModel
    {
        #region Properties

        private int _id;

        public int Id
        {
            get { return _id; }
            set 
            { 
                _id = value; 
            }
        }


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

        private List<int> _participants;

        public List<int> Participants
        {
            get { return _participants; }
            set { _participants = value; }
        }
        #endregion
    }
}
