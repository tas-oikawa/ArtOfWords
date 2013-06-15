using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame
{
    public class StoryFrameModel : IMarkable, INotifyPropertyChanged, ISearchable
    {
        private int _id;
        public int Id
        {
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
            get
            {
                return _id;
            }
        }

        private int _placeId;
        public int PlaceId
        {
            set
            {
                if(_placeId != value)
                {
                    _placeId = value;
                    OnPropertyChanged("PlaceId");
                }
            }
            get
            {
                return _placeId;
            }
        }
        
        private string _name;
        public string Name
        {
            set
            {
                if (value != _name)
                {
                    _name = value;

                    OnPropertyChanged("Name");
                    OnPropertyChanged("Symbol");
                }
            }
            get
            {
                if (_name == null)
                {
                    return "";
                }
                return _name;
            }

        }
        
        private string _remarks;
        public string Remarks
        {
            set
            {
                if (value != _remarks)
                {
                    _remarks = value;
                    OnPropertyChanged("Remark");
                }
            }
            get
            {
                return _remarks;
            }
        }

        private String _symbol;
        public String Symbol
        {
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
            get
            {
                if (_symbol == null || _symbol.Count() == 0)
                {
                    return Name.Substring(0, Math.Min(Name.Count(), 3));
                }

                return _symbol;
            }
        }

        private DateTime _startDateTime;

        public DateTime StartDateTime
        {
            set
            {
                if (_startDateTime != value)
                {
                    _startDateTime = value;
                    OnPropertyChanged("StartDateTime");
                }
            }
            get 
            { 
                return _startDateTime; 
            }
        }
        private DateTime _endDateTime;

        public DateTime EndDateTime
        {
            set
            {
                if (_endDateTime != value)
                {
                    _endDateTime = value;
                    OnPropertyChanged("EndDateTime");
                }
            }
            get 
            { 
                return _endDateTime; 
            }
        }

        private ObservableCollection<int> _tags = new ObservableCollection<int>();

        public ObservableCollection<int> Tags
        {
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    OnPropertyChanged("Tags");
                }
            }
            get
            {
                return _tags;
            }
        }
        

        public string ToSearchString()
        {
            return this.Name + ","
                + this.StartDateTime.ToString("yyyy/MM/dd HH:mm:ss") + ","
                + this.EndDateTime.ToString("yyyy/MM/dd HH:mm:ss") + ","
                + ToSearchStringByRelations() + ","
                + TagsToString.ToString(_tags, ModelsComposite.TagManager);
        }

        private string ToSearchStringByRelations()
        {
            StringBuilder builder = new StringBuilder();

            var charaModels = ModelsComposite.CharacterStoryModelManager.FindCharacterStoryRelationModels(this.Id);

            foreach (var charaRelation in charaModels)
            {
                var chara =  ModelsComposite.CharacterManager.FindCharacter(charaRelation.CharacterId);
                if (chara == null)
                {
                    continue;
                }
                builder.Append(chara.Name + "," + charaRelation.Behavior + ",");
            }
            var itemModels = ModelsComposite.ItemStoryModelManager.FindItemStoryRelationModels(this.Id);

            foreach (var itemRelation in itemModels)
            {
                var item = ModelsComposite.ItemModelManager.FindItemModel(itemRelation.ItemId);
                if (item == null)
                {
                    continue;
                }
                builder.Append(item.Name + "," + itemRelation.Behavior + ",");
            }

            return builder.ToString();
        }

        /// <summary>
        /// このモデルが現在も有効かどうかを返す。有効でない場合、このモデルを使ってはいけない
        /// </summary>
        public bool IsValid { set; get; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }


        private Brush _colorBrush { set; get; }
        public Brush ColorBrush
        {
            set
            {
                if (_colorBrush != value)
                {
                    _colorBrush = value;
                    OnPropertyChanged("ColorBrush");
                    OnPropertyChanged("MarkBrush");
                }
            }
            get
            {
                return _colorBrush;
            }
        }

        public Brush MarkBrush
        {
            get
            {
                SolidColorBrush col = _colorBrush as SolidColorBrush;

                return new SolidColorBrush(Color.FromArgb(60, col.Color.R, col.Color.G, col.Color.B));
            }
        }

        #endregion
    }
}
