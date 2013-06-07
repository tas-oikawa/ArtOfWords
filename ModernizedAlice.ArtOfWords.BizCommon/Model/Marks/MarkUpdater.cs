using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class MarkUpdater
    {
        public static void UpdateMarkings(MyTextChange change)
        {
            var deleteMarks = new List<Mark>();
            var marks = ModelsComposite.MarkManager.GetMarks();

            foreach (var mark in marks)
            {
                //　☆☆☆
                // 　 　　　|<--->|　
                // なら興味なし
                if (mark.TailCharIndex < change.Offset)
                {
                    continue;
                }

                //    ☆☆☆
                // |<-------->|　
                // なら削除
                if (mark.HeadCharIndex >= change.Offset && mark.TailCharIndex < change.Offset + change.RemovedLength)
                {
                    deleteMarks.Add(mark);
                    continue;
                }

                //　　　　　☆☆☆
                // |<--->|　
                // なら単純に加算するだけ
                if (mark.HeadCharIndex >= change.Offset + change.RemovedLength)
                {
                    mark.HeadCharIndex += change.AddedLength - change.RemovedLength;
                    mark.TailCharIndex += change.AddedLength - change.RemovedLength;

                    continue;
                }

                //    　　　　☆☆☆
                // 　|<-------->|　
                // なら先頭を削る
                if (mark.HeadCharIndex > change.Offset && mark.HeadCharIndex <= change.Offset + change.RemovedLength)
                {
                    mark.HeadCharIndex = change.Offset + change.AddedLength;
                    mark.TailCharIndex += change.AddedLength - change.RemovedLength;
                    continue;
                }


                //    ☆☆☆☆☆☆☆☆☆
                // 　　　|<-------->|　
                // なら末尾をその分変動させる
                if (mark.HeadCharIndex <= change.Offset && mark.TailCharIndex >= change.Offset + change.RemovedLength)
                {
                    mark.TailCharIndex += change.AddedLength - change.RemovedLength;
                    continue;
                }

                //    ☆☆☆
                // 　　　|<-------->|　
                // なら末尾を伸ばす
                if (mark.HeadCharIndex <= change.Offset && mark.TailCharIndex >= change.Offset)
                {
                    mark.TailCharIndex = change.Offset + change.AddedLength - 1;
                    continue;
                }
            }

            foreach (var del in deleteMarks)
            {
                ModelsComposite.MarkManager.DeleteMark(del);
            }
        }

        public static void UpdateMarkings(ModernizedAlice.IPlugin.ModuleInterface.TextChangedEventArgs args)
        {
            EventAggregator.OnModelDataChanged(args.Source, new ModelValueChangedEventArgs());
            foreach (var arg in args.Changes)
            {
                UpdateMarkings(arg);
            }
        }
    }
}
