using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Item
{
    public class ItemModelManager : ModelManager, INotifyPropertyChanged
    {

        public ItemModelManager()
        {
        }

        public ItemModel GetNewModel()
        {
            var model = new ItemModel();
            model.Name = "名前のないアイテム";
            model.Id = GetUniqueId();
            model.ColorBrush = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));
            model.Symbol = "";
            model.Remarks = "";
            model.Kind = ItemKindEnum.Arm;


            return model;
        }

        public ItemModel FindItemModel(int id)
        {
            var model = FindModel(id);

            if (model == null)
            {
                return null;
            }

            return model as ItemModel;
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
