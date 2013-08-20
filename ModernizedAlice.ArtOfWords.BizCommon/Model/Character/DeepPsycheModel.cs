using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Character
{
    /// <summary>
    /// 登場人物の性格分析
    /// </summary>
    public class DeepPsycheModel : NotifyPropertyChangedBase
    {
        private string _mostShockedEvent;

        /// <summary>
        /// 最もショックを受けた出来事
        /// </summary>
        public string MostShockedEvent
        {
            get { return _mostShockedEvent; }
            set 
            {
                if (_mostFeltAngerEvent == value)
                {
                    return;
                }

                _mostShockedEvent = value;
                this.OnPropertyChanged((o) => MostShockedEvent);
            }
        }

        private string _mostFeltHappinessEvent;

        /// <summary>
        /// 最も喜びを感じた出来事
        /// </summary>
        public string MostFeltHappinessEvent
        {
            get { return _mostFeltHappinessEvent; }
            set
            {
                if (_mostFeltHappinessEvent == value)
                {
                    return;
                }

                _mostFeltHappinessEvent = value;
                this.OnPropertyChanged((o) => MostFeltHappinessEvent);
            }
        }

        private string _mostFeltAngerEvent;

        /// <summary>
        /// 最も怒りを感じた出来事
        /// </summary>
        public string MostFeltAngerEvent
        {
            get { return _mostFeltAngerEvent; }
            set
            {
                if (_mostFeltAngerEvent == value)
                {
                    return;
                }

                _mostFeltAngerEvent = value;
                this.OnPropertyChanged((o) => MostFeltAngerEvent);
            }
        }

        private string _ifHeHadHisLifeToLiveOverAgain;

        /// <summary>
        /// 人生をもう一度やり直せるとしたら
        /// </summary>
        public string IfHeHadHisLifeToLiveOverAgain
        {
            get { return _ifHeHadHisLifeToLiveOverAgain; }
            set
            {
                if (_ifHeHadHisLifeToLiveOverAgain == value)
                {
                    return;
                }

                _ifHeHadHisLifeToLiveOverAgain = value;
                this.OnPropertyChanged((o) => IfHeHadHisLifeToLiveOverAgain);
            }
        }

        private string _mostImportantThing;

        /// <summary>
        /// もっとも大事にしていること
        /// </summary>
        public string MostImportantThing
        {
            get { return _mostImportantThing; }
            set
            {
                if (_mostImportantThing == value)
                {
                    return;
                }

                _mostImportantThing = value;
                this.OnPropertyChanged((o) => MostImportantThing);
            }
        }

        private string _theTimeToFallAwayMostImportance;

        /// <summary>
        /// 大事にしているものを捨てる時
        /// </summary>
        public string TheTimeToFallAwayMostImportance
        {
            get { return _theTimeToFallAwayMostImportance; }
            set
            {
                if (_theTimeToFallAwayMostImportance == value)
                {
                    return;
                }

                _theTimeToFallAwayMostImportance = value;
                this.OnPropertyChanged((o) => TheTimeToFallAwayMostImportance);
            }
        }

        private string _mostInfluencedEventForDevelopingCharacter;

        /// <summary>
        /// 性格形成に一番影響を与えた出来事
        /// </summary>
        public string MostInfluencedEventForDevelopingCharacter
        {
            get { return _mostInfluencedEventForDevelopingCharacter; }
            set
            {
                if (_mostInfluencedEventForDevelopingCharacter == value)
                {
                    return;
                }

                _mostInfluencedEventForDevelopingCharacter = value;
                this.OnPropertyChanged((o) => MostInfluencedEventForDevelopingCharacter);
            }
        }

        private string _signatureLine;

        /// <summary>
        /// 代表的なセリフ
        /// </summary>
        public string SignatureLine
        {
            get { return _signatureLine; }
            set
            {
                if (_signatureLine == value)
                {
                    return;
                }

                _signatureLine = value;
                this.OnPropertyChanged((o) => SignatureLine);
            }
        }

        private string _triggerForMurder;

        /// <summary>
        /// 殺人を侵すとしたら
        /// </summary>
        public string TriggerForMurder
        {
            get { return _triggerForMurder; }
            set
            {
                if (_triggerForMurder == value)
                {
                    return;
                }

                _triggerForMurder = value;
                this.OnPropertyChanged((o) => TriggerForMurder);
            }
        }

        private string _trickForCharmUp;

        /// <summary>
        /// 惚れさせるための手段
        /// </summary>
        public string TrickForCharmUp
        {
            get { return _trickForCharmUp; }
            set
            {
                if (_trickForCharmUp == value)
                {
                    return;
                }

                _trickForCharmUp = value;
                this.OnPropertyChanged((o) => TrickForCharmUp);
            }
        }

        private string _wayForOverthrow;

        /// <summary>
        /// 屈服させる手段
        /// </summary>
        public string WayForOverthrow
        {
            get { return _wayForOverthrow; }
            set
            {
                if (_wayForOverthrow == value)
                {
                    return;
                }

                _wayForOverthrow = value;
                this.OnPropertyChanged((o) => WayForOverthrow);
            }
        }

        private string _wayForHeal;

        /// <summary>
        /// 立ち直らせる手段
        /// </summary>
        public string WayForHeal
        {
            get { return _wayForHeal; }
            set
            {
                if (_wayForHeal == value)
                {
                    return;
                }

                _wayForHeal = value;
                this.OnPropertyChanged((o) => WayForHeal);
            }
        }


        private string _hisDesire;

        /// <summary>
        /// 望んでいる物
        /// </summary>
        public string HisDesire
        {
            get { return _hisDesire; }
            set
            {
                if (_hisDesire == value)
                {
                    return;
                }

                _hisDesire = value;
                this.OnPropertyChanged((o) => HisDesire);
            }
        }

        private string _reasonForNotGetDesireYet;

        /// <summary>
        /// 望みを果たせていない理由
        /// </summary>
        public string ReasonForNotGetDesireYet
        {
            get { return _reasonForNotGetDesireYet; }
            set
            {
                if (_reasonForNotGetDesireYet == value)
                {
                    return;
                }

                _reasonForNotGetDesireYet = value;
                this.OnPropertyChanged((o) => ReasonForNotGetDesireYet);
            }
        }

        private string _hisFears;

        /// <summary>
        /// 恐れていること
        /// </summary>
        public string HisFears
        {
            get { return _hisFears; }
            set
            {
                if (_hisFears == value)
                {
                    return;
                }

                _hisFears = value;
                this.OnPropertyChanged((o) => HisFears);
            }
        }

        private string _reasonForFears;

        /// <summary>
        /// 恐れている理由
        /// </summary>
        public string ReasonForFears
        {
            get { return _reasonForFears; }
            set
            {
                if (_reasonForFears == value)
                {
                    return;
                }
                
                _reasonForFears = value;
                this.OnPropertyChanged((o) => ReasonForFears);
            }
        }


        /// <summary>
        /// 引数のデータをコピーする
        /// </summary>
        /// <param name="src"></param>
        public void Copy(DeepPsycheModel src)
        {
            this.HisDesire = src.HisDesire;
            this.HisFears = src.HisFears;
            this.IfHeHadHisLifeToLiveOverAgain = src.IfHeHadHisLifeToLiveOverAgain;
            this.MostFeltAngerEvent = src.MostFeltAngerEvent;
            this.MostFeltHappinessEvent = src.MostFeltHappinessEvent;
            this.MostImportantThing = src.MostImportantThing;
            this.MostInfluencedEventForDevelopingCharacter = src.MostInfluencedEventForDevelopingCharacter;
            this.MostShockedEvent = src.MostShockedEvent;
            this.ReasonForFears = src.ReasonForFears;
            this.ReasonForNotGetDesireYet = src.ReasonForNotGetDesireYet;
            this.SignatureLine = src.SignatureLine;
            this.TheTimeToFallAwayMostImportance = src.TheTimeToFallAwayMostImportance;
            this.TrickForCharmUp = src.TrickForCharmUp;
            this.TriggerForMurder = src.TriggerForMurder;
            this.WayForHeal = src.WayForHeal;
            this.WayForOverthrow = src.WayForOverthrow;
        }


        public override void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(name);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }
    }
}
