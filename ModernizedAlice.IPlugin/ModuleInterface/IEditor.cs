using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ModernizedAlice.IPlugin.ModuleInterface
{
    public delegate void ScrollOffsetChangedEventHandler(object sender);
    public delegate void TextChangedEventHandler(object sender, TextChangedEventArgs arg);
    public delegate void TextSearchEventHandler(object sender, TextSearchEventArgs arg);

    public interface IEditor
    {
        event ScrollOffsetChangedEventHandler ScrollOffsetChanged;
        event TextChangedEventHandler TextChanged;
        event TextSearchEventHandler TextSearchOccured;

        int GetHeadIndexOfVisibleText();
        int GetTailIndexOfVisibleText();

        int GetTailIndexOfLineByIndex(int index);

        int GetLineIndexFromCharacterIndex(int index);

        void LineUp();
        void LineDown();

        List<Rect> GetRectByCharIndex(int headIndex, int tailIndex);

        Rect GetRectByCharIndex(int index);

        int GetIndexFromPosition(Point point);

        void SetText(string text);
        string GetText();

        void SetTextStyle(TextStyle style);

        Control GetControl();

        void Select(int head, int tail);
        void Focus();

        void Cut();
        void Paste();

        int FindWord(string word);

        void OnModeChanged();
        void OnWindowActivated();
    }
}
