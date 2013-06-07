using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimelineControl.Model;

namespace ArtOfWords.DataGenerator
{
    public class TimelineModelConverter
    {
        public static TimelineEventModel ConvertAsNew(EventModel evt)
        {
            var newModel = ModelsComposite.TimelineEventModelManager.GetNewModel();

            newModel.Title = evt.Title;
            newModel.StartDateTime = evt.StartDateTime;
            newModel.EndDateTime = evt.EndDateTime;
            newModel.Detail = evt.Detail;
            newModel.Participants = evt.Participants;

            return newModel;
        }

        public static void ConvertAsModify(EventModel src, TimelineEventModel target)
        {
            target.Title = src.Title;
            target.StartDateTime = src.StartDateTime;
            target.EndDateTime = src.EndDateTime;
            target.Detail = src.Detail;
            target.Participants = src.Participants;
        }
    }
}
