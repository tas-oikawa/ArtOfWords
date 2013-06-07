using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Character
{
    public static class CharacterUtil
    {
        public static string GetGender(GenderEnum data)
        {
            switch (data)
            {
                case GenderEnum.Female:
                    return "女";
                case GenderEnum.Hermaphroditic:
                    return "両性";
                case GenderEnum.Male:
                    return "男";
                case GenderEnum.None:
                    return "なし";
                case GenderEnum.NotObvious:
                    return "不明";
                case GenderEnum.Other:
                    return "その他";
            }
            return "";
        }
    }
}
