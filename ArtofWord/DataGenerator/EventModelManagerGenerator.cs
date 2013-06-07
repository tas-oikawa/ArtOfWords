using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimelineControl.Model;

namespace ArtOfWords.DataGenerator
{
    public class EventModelManagerGenerator
    {
        public static EventModel GenerateEventModel(StoryFrameModel story)
        {
            return new EventModel()
            {
                Title = story.Name,
                StartDateTime = story.StartDateTime,
                EndDateTime = story.EndDateTime,
                Detail = story.Remarks,
                SourceObject = story,
                Participants = new List<int>(){CommonDefinitions.StoryFrameEventId}
            };
        }

        public static EventModel GenerateEventModel(TimelineEventModel timeline)
        {
            return new EventModel()
            {
                Title = timeline.Title,
                StartDateTime = timeline.StartDateTime,
                EndDateTime = timeline.EndDateTime,
                Detail = timeline.Detail,
                SourceObject = timeline,
                Participants = new List<int>(timeline.Participants)
            };
        }

        public static EventModelManager Generate()
        {
            EventModelManager manager = new EventModelManager();

            foreach (var model in ModelsComposite.StoryFrameModelManager.ModelCollection)
            {
                manager.Add(GenerateEventModel(model as StoryFrameModel));
            }

            foreach (var model in ModelsComposite.TimelineEventModelManager.ModelCollection)
            {
                manager.Add(GenerateEventModel(model));
            }

            return manager;
        }
    }
}
