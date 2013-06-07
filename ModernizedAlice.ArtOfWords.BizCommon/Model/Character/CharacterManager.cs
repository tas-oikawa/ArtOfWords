using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Character
{
    public class CharacterManager : ModelManager, INotifyPropertyChanged
    {
        public CharacterManager()
        {
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
