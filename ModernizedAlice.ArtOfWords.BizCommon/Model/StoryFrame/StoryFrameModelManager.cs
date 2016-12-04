using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame
{
    public class StoryFrameModelManager : ModelManager, INotifyPropertyChanged
    {

        public StoryFrameModelManager()
        {
        }

        public DateTime GetNewStoryDate()
        {
            for (int i = ModelCollection.Count - 1; i >= 0; i--)
            {
                var story = ModelCollection[i] as StoryFrameModel;

                if (story.EndDateTime > new DateTime(0001, 01, 01))
                {
                    return story.EndDateTime;
                }
            }

            return DateTime.Today;
        }

        public StoryFrameModel GetNewModel()
        {
            var model = new StoryFrameModel();
            model.Name = "名前のないシーン";
            model.Id = GetUniqueId();
            model.ColorBrush = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));
            model.Symbol = "";
            model.Remarks = "";
            model.StartDateTime = GetNewStoryDate();
            model.EndDateTime = new DateTime(model.StartDateTime.Ticks).AddHours(1);
            

            return model;
        }

        public StoryFrameModel FindStoryFrameModel(int id)
        {
            var model = FindModel(id);

            if (model == null)
            {
                return null;
            }

            return model as StoryFrameModel;
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
