using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.ViewModels.NewFilePlus
{
    /// <summary>
    /// タイムラインイベントのListViewItem
    /// </summary>
    public class EventTransferItemViewModel : NotifyPropertyChangedBase
    {
        private TimelineEventModel _source;

        /// <summary>
        /// データソース
        /// </summary>
        public TimelineEventModel Source
        {
            get { return _source; }
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get
            {
                return _source.Title;
            }
        }

        private bool _isSelected;

        /// <summary>
        /// 選択状態かどうか
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="org"></param>
        public EventTransferItemViewModel(TimelineEventModel org)
        {
            _source = org;
        }
    }
}
