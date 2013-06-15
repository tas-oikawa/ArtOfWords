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

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Item
{
    public class ItemModel : IMarkable, INotifyPropertyChanged, ISearchable
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


        private ItemKindEnum _kind;
        public ItemKindEnum Kind
        {
            set
            {
                if (_kind != value)
                {
                    _kind = value;

                    OnPropertyChanged("IsArms");
                    OnPropertyChanged("IsGuards");
                    OnPropertyChanged("IsTool");
                    OnPropertyChanged("IsTechniques");
                    OnPropertyChanged("IsInfo");
                    OnPropertyChanged("IsIdeology");
                    OnPropertyChanged("IsOrgs");
                    OnPropertyChanged("IsPlace");
                    OnPropertyChanged("IsTheory");
                    OnPropertyChanged("IsTimeline");
                    OnPropertyChanged("IsMislead");
                    OnPropertyChanged("IsState");
                    OnPropertyChanged("IsOralInstruction");
                    OnPropertyChanged("IsTaoYuanming");
                    OnPropertyChanged("IsJingxi");
                    OnPropertyChanged("IsPocketBell");
                    OnPropertyChanged("IsRomance");
                    OnPropertyChanged("IsEtc");
                }
            }
            get
            {
                return _kind;
            }
        }

        public bool IsArms
        {
            set
            {
                if (value == true)
                {
                    Kind = ItemKindEnum.Arm;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Arm);
            }
        }
        public bool IsGuards
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Protecter;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Protecter);
            }
        }
        public bool IsTool
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Tool;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Tool);
            }
        }
        public bool IsTechniques
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Technique;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Technique);
            }
        }
        public bool IsInfo
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Info;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Info);
            }
        }
        public bool IsIdeology
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Ideology;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Ideology);
            }
        }
        public bool IsOrgs
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Organization;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Organization);
            }
        }
        public bool IsPlace
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Place;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Place);
            }
        }
        public bool IsTheory
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Theory;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Theory);
            }
        }
        public bool IsTimeline
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Timeline;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Timeline);
            }
        }
        public bool IsMislead
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Mislead;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Mislead);
            }
        }
        public bool IsState
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Status;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Status);
            }
        }
        public bool IsOralInstruction
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.OralInstruction;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.OralInstruction);
            }
        }
        public bool IsTaoYuanming
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.TaoYuanming;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.TaoYuanming);
            }
        }
        public bool IsJingxi
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Jingxi;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Jingxi);
            }
        }
        public bool IsPocketBell
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.PocketBell;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.PocketBell);
            }
        }
        public bool IsRomance
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Romance;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Romance);
            }
        }
        public bool IsEtc
        {
            set
            {
                if(value == true)
                {
                    Kind = ItemKindEnum.Etc;
                }
            }
            get
            {
                return (Kind == ItemKindEnum.Etc);
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
        

        /// <summary>
        /// このモデルが現在も有効かどうかを返す。有効でない場合、このモデルを使ってはいけない
        /// </summary>
        public bool IsValid { set; get; }

        public string ToSearchString()
        {
            return this.Name + ","
                + ItemUtil.GetItem(this.Kind) + ","
                + this.Remarks + ","
                + TagsToString.ToString(_tags, ModelsComposite.TagManager);
        }

        public void CopyWithoutId(ItemModel src)
        {
            this.ColorBrush = src.ColorBrush;
            this.Kind = src.Kind;
            this.Name = src.Name;
            this.Remarks = src.Remarks;
            this.Symbol = src.Symbol;
            this.Tags.Clear();

            foreach (var tagId in src.Tags)
            {
                this.Tags.Add(tagId);
            }
        }

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
