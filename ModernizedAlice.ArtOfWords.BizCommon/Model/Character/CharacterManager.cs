using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Event;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Character
{
    public class CharacterManager : ModelManager, INotifyPropertyChanged
    {
        public CharacterManager()
        {
            EventAggregator.TagModelModifiedHandler += EventAggregator_TagModelModifiedHandler;
        }

        /// <summary>
        /// タグが削除されたときの処理
        /// </summary>
        /// <param name="arg">タグ変更イベントの引数</param>
        private void OnTagRemoved(Event.TagModelModifiedEventArgs arg)
        {
            foreach (var model in ModelCollection)
            {
                var charaModel = model as CharacterModel;
                if (charaModel.Tags.Contains(arg.ModifiedTag.Id))
                {
                    charaModel.Tags.Remove(arg.ModifiedTag.Id);
                }
            }
        }
        
        /// <summary>
        /// タグが変更されたときにイベントハンドラー
        /// </summary>
        /// <param name="sender">イベント送り元</param>
        /// <param name="arg">タグ変更イベントの引数</param>
        private void EventAggregator_TagModelModifiedHandler(object sender, Event.TagModelModifiedEventArgs arg)
        {
            if (arg.Kind == Event.TagModelModifiedKind.Deleted)
            {
                OnTagRemoved(arg);
            }
        }

        public CharacterModel GetNewModel()
        {
            var model = new CharacterModel();
            model.Age = "";
            model.FirstName = "";
            model.FirstNameRuby = "";
            model.Gender = GenderEnum.Male;
            model.Id = GetUniqueId();
            model.LastName = "名前のない人";
            model.LastNameRuby = "";
            model.ColorBrush = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));
            model.MiddleName = "";
            model.MiddleNameRuby = "";
            model.NameOrder = NameOrderEnum.FamilyPersonelBracketMiddle;
            model.NickName = "";
            model.RelationWithHero = "";
            model.Remarks = "";
            model.Species = "";
            model.Symbol = "";

            return model;
        }
        public CharacterModel FindCharacter(int id)
        {
            var model = base.FindModel(id);

            if (model == null)
            {
                return null;
            }

            return model as CharacterModel;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
