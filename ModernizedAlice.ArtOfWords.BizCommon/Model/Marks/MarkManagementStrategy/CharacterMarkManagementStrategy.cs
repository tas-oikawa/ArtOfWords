using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks.MarkManagementStrategy
{
    public class CharacterMarkManagementStrategy
    {
        private static int GetLineHead(string text, int index)
        {
            int lineHead = 0;
            // Textのindexの一番前にある改行コードを取得
            for (int curPos = index; curPos >= 0; --curPos)
            {
                if (text[curPos] == '\n')
                {
                    lineHead = curPos + 1;
                    break;
                }
            }

            return lineHead;
        }

        private static int GetLineTail(string text, int index)
        {
            int lineTail = text.Length - 1;
            // Textのindexの後方に向けて改行コードを検査
            for (int curPos = index; curPos < text.Length; ++curPos)
            {
                if (text[curPos] == '\n')
                {
                    lineTail = curPos;
                    break;
                }
            }

            return lineTail;
        }

        private static bool IsBracketHead(char ch)
        {
            if (ch == '「' || ch == '｢' || ch == '"' || ch == '(' || ch == '（')
            {
                return true;
            }

            return false;
        }

        private static char GetTailBracket(char headBracket)
        {
            switch (headBracket)
            {
                case '「':
                    return '」';
                case '｢':
                    return '｣';
                case '"':
                    return '"';
                case '(':
                    return ')';
                case '（':
                    return '）';
            }

            return ' ';
        }

        private static bool HasBracketHead(string text, int headOfLine, int tailOfLine, out int bracketIndex)
        {
            bracketIndex = headOfLine;

            if (headOfLine == tailOfLine)
            {
                return false;
            }

            for (int curPos = headOfLine; curPos <= tailOfLine; ++curPos)
            {
                if (!IsBracketHead(text[curPos]))
                {
                    continue;
                }

                // 見つかったのが検索範囲の末尾なら、先頭カッコ以外の何物でも無いのでそのまま返す
                if (curPos == tailOfLine)
                {
                    bracketIndex = curPos;
                    return true;
                }

                char tailBracket = GetTailBracket(text[curPos]);

                // curPosとtailの間に閉じカッコが無ければ先頭カッコ
                int tmpNum;
                if (!HasBracketTail(text, curPos, tailOfLine - 1, GetTailBracket(text[curPos]), out tmpNum))
                {
                    bracketIndex = curPos;

                    return true;
                }
            }

            return false;
        }

        private static bool HasBracketTail(string text, int headOfLine, int tailOfLine, char tail, out int bracketIndex)
        {
            for (int curPos = headOfLine; curPos <= tailOfLine; ++curPos)
            {
                if (tail == text[curPos])
                {
                    bracketIndex = curPos;
                    return true;
                }
            }

            bracketIndex = tailOfLine;
            return false;
        }

        public static void AddCharacterMark(int head, int tail, CharacterModel model)
        {
            var charaMark = new CharacterMark();
            charaMark.Brush = model.MarkBrush;
            charaMark.CharacterId = model.Id;
            charaMark.HeadCharIndex = head;
            charaMark.TailCharIndex = tail;
            charaMark.Parent = model;

            ModelsComposite.MarkManager.AddMark(charaMark);

        }
        public static void AddCharacterMark(string text, int index, CharacterModel model)
        {
            int headLineIndex = GetLineHead(text, index);
            int tailLineIndex = GetLineTail(text, index);
            int headBracketIndex;
            int tailBracketIndex;

            bool hasHeadBracket = HasBracketHead(text, headLineIndex, index, out headBracketIndex);
            bool hasTailBracket = HasBracketTail(text, headBracketIndex, text.Length - 1, GetTailBracket(text[headBracketIndex]), out tailBracketIndex);

            if (hasHeadBracket && hasTailBracket)
            {
                AddCharacterMark(headBracketIndex, tailBracketIndex, model);
            }
            else
            {
                AddCharacterMark(headLineIndex, tailLineIndex, model);
            }
        }

        public static bool AddMark(string text, int index, IMarkable targetMark)
        {
            AddCharacterMark(text, index, targetMark as CharacterModel);
            return true;
        }
    }
}
