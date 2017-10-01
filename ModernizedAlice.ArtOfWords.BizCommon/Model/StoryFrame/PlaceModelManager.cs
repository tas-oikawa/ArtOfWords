using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Event;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame
{
    public class PlaceModelManager : ModelManager, INotifyPropertyChanged
    {
        public PlaceModelManager()
        {
        }

        public PlaceModel GetNewModel()
        {
            var model = new PlaceModel();
            model.Name = "名前のない場所";
            model.Id = GetUniqueId();
            model.IsValid = true;

            return model;
        }

        public PlaceModel FindPlaceModel(int id)
        {
            var model = FindModel(id);

            if (model == null)
            {
                return null;
            }

            return model as PlaceModel;
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
