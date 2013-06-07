using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Item
{
    public static class ItemUtil
    {
        public static string GetItem(ItemKindEnum data)
        {
            switch (data)
            {
                case ItemKindEnum.Arm:
                    return "武器・凶器";
                case ItemKindEnum.Protecter:
                    return "防具・衣装・服";
                case ItemKindEnum.Tool:
                    return "道具・機器・食糧";
                case ItemKindEnum.Technique:
                    return "魔法・能力・技能";
                case ItemKindEnum.Info:
                    return "情報";
                case ItemKindEnum.Ideology:
                    return "思想・価値感";
                case ItemKindEnum.Organization:
                    return "組織";
                case ItemKindEnum.Place:
                    return "建物・場所";
                case ItemKindEnum.Theory:
                    return "現象・理論・法則";
                case ItemKindEnum.Timeline:
                    return "タイムライン";
                case ItemKindEnum.Mislead:
                    return "罠・ミスリード";
                case ItemKindEnum.Status:
                    return "状況・状態";
                case ItemKindEnum.OralInstruction:
                    return "書物・口伝";
                case ItemKindEnum.TaoYuanming:
                    return "音楽・詩歌";
                case ItemKindEnum.Jingxi:
                    return "映像・演劇・舞";
                case ItemKindEnum.PocketBell:
                    return "メール・伝達手段";
                case ItemKindEnum.Romance:
                    return "経験・過去・歴史";
                case ItemKindEnum.Etc:
                    return "その他";

            }
            return "";
        }
    }
}
