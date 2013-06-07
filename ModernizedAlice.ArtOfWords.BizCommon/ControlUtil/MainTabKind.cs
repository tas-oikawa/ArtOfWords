using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.BizCommon.ControlUtil
{
    public enum MainTabKind
    {
        WritingTab,
        CharacterTab,
        ItemTab,
        StoryFrameTab,
        TimelineTab,
        FromKienaiTab,
    }

    public static class MainTabKindUtil
    {
        public static int ToInt(MainTabKind kind)
        {
            switch (kind)
            {
                case MainTabKind.WritingTab:
                    return 0;
                case MainTabKind.CharacterTab:
                    return 1;
                case MainTabKind.ItemTab:
                    return 2;
                case MainTabKind.StoryFrameTab:
                    return 3;
                case MainTabKind.TimelineTab:
                    return 4;
                case MainTabKind.FromKienaiTab:
                    return 6;
            }
            return 0;
        }

        public static MainTabKind ToMainTabKind(int kind)
        {
            switch (kind)
            {
                case 0:
                    return MainTabKind.WritingTab;
                case 1:
                    return MainTabKind.CharacterTab;
                case 2:
                    return MainTabKind.ItemTab;
                case 3:
                    return MainTabKind.StoryFrameTab;
                case 4:
                    return MainTabKind.TimelineTab;
                case 6:
                    return MainTabKind.FromKienaiTab;
            }
            return MainTabKind.WritingTab;
        }
    }
}
