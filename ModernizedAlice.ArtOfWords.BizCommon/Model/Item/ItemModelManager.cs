using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Event;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Item
{
    public class ItemModelManager : ModelManager, INotifyPropertyChanged
    {
        public ItemModelManager()
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
                var charaModel = model as ItemModel;
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
