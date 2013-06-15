using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Character
{
    public class CharacterModel : IMarkable, INotifyPropertyChanged, ISearchable, ITagStickable
    {
        private int _id;
        public int Id
        {
            set
            {
                _id = value;
            }
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// このモデルが現在も有効かどうかを返す。有効でない場合、このモデルを使ってはいけない
        /// </summary>
        public bool IsValid { set; get; }

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

                return new SolidColorBrush(Color.FromArgb(60, col.Color.R,col.Color.G, col.Color.B));
            }
        }

        #region NameProperties

        private String _lastNameRuby;
        public String LastNameRuby
        {
            set
            {
                if (_lastNameRuby != value)
                {
                    _lastNameRuby = value;
                    OnPropertyChanged("LastNameRuby");
                }
            }
            get
            {
                if (_lastNameRuby == null)
                {
                    _lastNameRuby = "";
                }
                return _lastNameRuby;
            }
        }

        private String _firstNameRuby;
        public String FirstNameRuby
        {
            set
            {
                if (_firstNameRuby != value)
                {
                    _firstNameRuby = value;
                    OnPropertyChanged("FirstNameRuby");
                }
            }
            get
            {
                if (_firstNameRuby == null)
                {
                    _firstNameRuby = "";
                }
                return _firstNameRuby;
            }
        }

        private String _middleNameRuby;
        public String MiddleNameRuby
        {
            set
            {
                if (_middleNameRuby != value)
                {
                    _middleNameRuby = value;
                    OnPropertyChanged("MiddleNameRuby");
                }
            }
            get
            {
                if (_middleNameRuby == null)
                {
                    _middleNameRuby = "";
                }
                return _middleNameRuby;
            }
        }

        private String _lastName;
        public String LastName
        {
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged("LastName");
                    OnPropertyChanged("Name");
                    OnPropertyChanged("Symbol");
                }
            }
            get
            {
                if (_lastName == null)
                {
                    _lastName = "";
                }
                return _lastName;
            }
        }

        private String _firstName;
        public String FirstName
        {
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
                    OnPropertyChanged("Name");
                }
            }
            get
            {
                if (_firstName == null)
                {
                    _firstName = "";
                }
                return _firstName;
            }
        }

        private String _middleName;
        public String MiddleName
        {
            set
            {
                if (_middleName != value)
                {
                    _middleName = value;
                    OnPropertyChanged("MiddleName");
                    OnPropertyChanged("Name");
                }
            }
            get
            {
                if (_middleName == null)
                {
                    _middleName = "";
                }
                return _middleName;
            }
        }


        public String Name
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (NameOrder == NameOrderEnum.FamilyPersonelBracketMiddle)
                {
                    if(LastName.Count() != 0)
                    {
                        builder.Append(LastName);
                        if (FirstName.Count() != 0)
                        {
                            builder.Append(" ");
                        }
                    }
                    if (FirstName.Count() != 0)
                    {
                        builder.Append(FirstName);
                    }
                    if (MiddleName.Count() != 0)
                    {
                        builder.Append("(" + MiddleName + ")");
                    }
                    return builder.ToString();
                }

                else if (NameOrder == NameOrderEnum.PersonelMiddleFamilyWithDot)
                {
                    if (FirstName.Count() != 0)
                    {
                        builder.Append(FirstName);
                        if (MiddleName.Count() != 0 || LastName.Count() != 0)
                        {
                            builder.Append("・");
                        }
                    }
                    if (MiddleName.Count() != 0)
                    {
                        builder.Append(MiddleName);
                        if (LastName.Count() != 0)
                        {
                            builder.Append("・");
                        }
                    }
                    if (LastName.Count() != 0)
                    {
                        builder.Append(LastName);
                    }
                    return builder.ToString();
                }
                else
                {
                    if (FirstName.Count() != 0)
                    {
                        builder.Append(FirstName);
                        if (MiddleName.Count() != 0 || LastName.Count() != 0)
                        {
                            builder.Append("＝");
                        }
                    }
                    if (MiddleName.Count() != 0)
                    {
                        builder.Append(MiddleName);
                        if (LastName.Count() != 0)
                        {
                            builder.Append("＝");
                        }
                    }
                    if (LastName.Count() != 0)
                    {
                        builder.Append(LastName);
                    }
                    return builder.ToString();
                }
            }
        }

        private NameOrderEnum _nameOrder;
        public NameOrderEnum NameOrder
        {
            set
            {
                if (_nameOrder != value)
                {
                    _nameOrder = value;
                    OnPropertyChanged("IsShokatsuOrder");
                    OnPropertyChanged("IsBismarckOrder");
                    OnPropertyChanged("IsRichelieuOrder");
                    OnPropertyChanged("Name");
                }
            }
            get
            {
                return _nameOrder;
            }
        }

        public bool IsShokatsuOrder
        {
            set
            {
                if (value == true)
                {
                    NameOrder = NameOrderEnum.FamilyPersonelBracketMiddle;
                }
            }
            get
            {
                return (NameOrder == NameOrderEnum.FamilyPersonelBracketMiddle);
            }
        }

        public bool IsBismarckOrder
        {
            set
            {
                if (value == true)
                {
                    NameOrder = NameOrderEnum.PersonelMiddleFamilyWithDot;
                }
            }
            get
            {
                return (NameOrder == NameOrderEnum.PersonelMiddleFamilyWithDot);
            }
        }

        public bool IsRichelieuOrder
        {
            set
            {
                if (value == true)
                {
                    NameOrder = NameOrderEnum.PersonelMiddleFamilyWithEqual;
                }
            }
            get
            {
                return (NameOrder == NameOrderEnum.PersonelMiddleFamilyWithEqual);
            }
        }

        private String _nickName;
        public String NickName
        {
            set
            {
                if (_nickName != value)
                {
                    _nickName = value;
                    OnPropertyChanged("NickName");
                }
            }
            get
            {
                return _nickName;
            }
        }

        #endregion

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
                    return LastName.Substring(0, Math.Min(LastName.Count(), 3));
                }
                return _symbol;
            }
        }

        private String _age;
        public String Age
        {
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged("Age");
                }
            }
            get
            {
                return _age;
            }
        }

        private String _relationWithHero;
        public String RelationWithHero
        {
            set
            {
                if (_relationWithHero != value)
                {
                    _relationWithHero = value;
                    OnPropertyChanged("RelationWithHero");
                }
            }
            get
            {
                return _relationWithHero;
            }
        }

        private String _species;
        public String Species
        {
            set
            {
                if (_species != value)
                {
                    _species = value;
                    OnPropertyChanged("Species");
                }
            }
            get
            {
                return _species;
            }
        }

        private String _remarks;
        public String Remarks
        {
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
            get
            {
                return _remarks;
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
        

        #region GenderProperties

        private GenderEnum _gender;
        public GenderEnum Gender
        {
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged("IsMale");
                    OnPropertyChanged("IsFemale");
                    OnPropertyChanged("IsNoneGender");
                    OnPropertyChanged("IsHermaphroditic");
                    OnPropertyChanged("IsGenderNotObvious");
                    OnPropertyChanged("IsGenderOther");
                }
            }
            get
            {
                return _gender;
            }
        }
        public bool IsMale
        {
            set
            {
                if (value == true)
                {
                    Gender = GenderEnum.Male;
                }
            }
            get
            {
                return (Gender == GenderEnum.Male);
            }
        }

        public bool IsFemale
        {
            set
            {
                if (value == true)
                {
                    Gender = GenderEnum.Female;
                }
            }
            get
            {
                return (Gender == GenderEnum.Female);
            }
        }

        public bool IsNoneGender
        {
            set
            {
                if (value == true)
                {
                    Gender = GenderEnum.None;
                }
            }
            get
            {
                return (Gender == GenderEnum.None);
            }
        }

        public bool IsHermaphroditic
        {
            set
            {
                if (value == true)
                {
                    Gender = GenderEnum.Hermaphroditic;
                }
            }
            get
            {
                return (Gender == GenderEnum.Hermaphroditic);
            }
        }

        public bool IsGenderNotObvious
        {
            set
            {
                if (value == true)
                {
                    Gender = GenderEnum.NotObvious;
                }
            }
            get
            {
                return (Gender == GenderEnum.NotObvious);
            }
        }

        public bool IsGenderOther
        {
            set
            {
                if (value == true)
                {
                    Gender = GenderEnum.Other;
                }
            }
            get
            {
                return (Gender == GenderEnum.Other);
            }
        }

        #endregion

        public void CopyWithoutId(CharacterModel src)
        {
            this.Age = src.Age;
            this.ColorBrush = src.ColorBrush;
            this.FirstName = src.FirstName;
            this.FirstNameRuby = src.FirstNameRuby;
            this.Gender = src.Gender;
            this.LastName = src.LastName;
            this.LastNameRuby = src.LastNameRuby;
            this.MiddleName = src.MiddleName;
            this.MiddleNameRuby = src.MiddleNameRuby;
            this.NameOrder = src.NameOrder;
            this.NickName = src.NickName;
            this.RelationWithHero = src.RelationWithHero;
            this.Remarks = src.Remarks;
            this.Species = src.Species;
            this.Symbol = src.Symbol;
            this.Tags.Clear();

            foreach (var tagId in src.Tags)
            {
                this.Tags.Add(tagId);
            }
        }        

        public string ToSearchString()
        {
            return this.Name + ","
                + this.FirstNameRuby + ","
                + this.MiddleNameRuby + ","
                + this.LastNameRuby + ","
                + this.NickName + ","
                + CharacterUtil.GetGender(this.Gender) + ","
                + this.RelationWithHero + ","
                + this.Remarks + ","
                + this.Species + ","
                + TagsToString.ToString(_tags, ModelsComposite.TagManager);
        }

        public void SetTagIds(List<int> stickTagList)
        {
            _tags.Clear();

            foreach (var tagId in stickTagList)
            {
                _tags.Add(tagId);
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

        #endregion


        public List<int> GetTagIds()
        {
            return Tags.ToList();
        }
    }
}
