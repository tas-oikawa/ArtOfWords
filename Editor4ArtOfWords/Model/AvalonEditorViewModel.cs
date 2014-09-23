using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor4ArtOfWords.Model
{
    [Export(typeof(IEditor))]
    public class AvalonEditorViewModel : IEditor, INotifyPropertyChanged
    {
        private AvalonEditorControl _view;
        private TextStyle _textStyle = new TextStyle();

        public TextStyle TextStyle
        {
            set 
            {
                if (value != _textStyle)
                {
                    _textStyle = value;
                    _textStyle.NotifyAll();
                    OnPropertyChanged("TextStyle");
                }
            }
            get
            {
                return _textStyle;
            }

        }

        public AvalonEditorViewModel()
        {
            _view = new AvalonEditorControl(this);
        }

        public int GetHeadIndexOfVisibleText()
        {
            var textView = _view.Editor.TextArea.TextView;

            TextViewPosition? start = textView.GetPosition(new Point(0, 0) + textView.ScrollOffset);

            return (start != null) ? _view.Editor.Document.GetOffset(start.Value.Location) : _view.Editor.Document.TextLength;
        }

        public int GetTailIndexOfVisibleText()
        {
            var textView = _view.Editor.TextArea.TextView;

            TextViewPosition? end = textView.GetPosition(new Point(textView.ActualWidth, textView.ActualHeight) + textView.ScrollOffset);

            return (end != null) ? _view.Editor.Document.GetOffset(end.Value.Location) : _view.Editor.Document.TextLength;
        }

        public int GetTailIndexOfLineByIndex(int index)
        {
            var line = _view.Editor.Document.GetLineByOffset(index);

            if (line.LineNumber < 0)
            {
                return -1;
            }

            var visualLine = _view.Editor.TextArea.TextView.GetVisualLine(line.LineNumber);

            var lineBreak = visualLine.Document.Text.IndexOf('\n', index);

            if (lineBreak < visualLine.StartOffset)
            {
                return visualLine.StartOffset + visualLine.TextLines[0].Length - 1;
            }

            int minStartOffset = Math.Max(index, visualLine.StartOffset);

            return minStartOffset + Math.Min(visualLine.TextLines[0].Length - 1, lineBreak - index);
        }

        public int GetLineIndexFromCharacterIndex(int index)
        {
            return _view.Editor.Document.GetLineByOffset(index).LineNumber;
        }

        public void LineUp()
        {
            _view.Editor.LineUp();
        }

        public void LineDown()
        {
            _view.Editor.LineDown();
        }

        public List<Rect> GetRectByCharIndex(int headIndex, int tailIndex)
        {
            var seg = new TextSegment()
            {
                StartOffset = headIndex,
                EndOffset = tailIndex + 1,
            };

            _view.Editor.TextArea.TextView.EnsureVisualLines();

            var list = new List<Rect>();
            foreach (Rect r in BackgroundGeometryBuilder.GetRectsForSegment(_view.Editor.TextArea.TextView, seg))
            {
                list.Add(r);
            }

            return list;
        }

        public Rect GetRectByCharIndex(int index)
        {
            return GetRectByCharIndex(index, index).First();
        }

        public int GetIndexFromPosition(Point point)
        {
            var position = _view.Editor.TextArea.TextView.GetPosition(point + _view.Editor.TextArea.TextView.ScrollOffset);
            if (position == null)
            {
                return -1;
            }

            return _view.Editor.Document.GetOffset(position.Value.Location);
        }

        private bool DoNotFireEvent;

        public void SetText(string text)
        {
            DoNotFireEvent = true;
            _view.Editor.Text = text;
            DoNotFireEvent = false;
        }

        public string GetText()
        {
            return _view.Editor.Text;
        }

        public void SetTextStyle(TextStyle style)
        {
            TextStyle = style;
        }

        public Control GetControl()
        {
            return _view;
        }

        public void Select(int head, int tail)
        {
            _view.Editor.Select(head, tail);
            var line = _view.Editor.TextArea.Document.GetLocation(head);

            _view.Editor.ScrollTo(line.Line, line.Column);
        }

        public void Focus()
        {
            _view.Editor.Focus();
        }


        public void Cut()
        {
            _view.Editor.Cut();
        }

        public void Paste()
        {
            _view.Editor.Paste();
        }

        public int FindWord(string word)
        {
            return _view.Editor.Document.Text.IndexOf(word);
        }

        public event ScrollOffsetChangedEventHandler ScrollOffsetChanged;

        public void OnScrollOffsetChanged()
        {
            if (ScrollOffsetChanged != null)
            {
                ScrollOffsetChanged(this);
            }
        }

        public void OnModeChanged()
        {
            _view.Editor.Select(0, 0);
        }

        public void ResetFocus()
        {
            // 一旦フォーカスが外れると、IMEが無効になる問題への対処療法
            _view.DummyTextBox.Focus();
            InputMethod.SetPreferredImeState(_view.Editor, InputMethodState.On);
            InputMethod.SetPreferredImeConversionMode(_view.Editor, ImeConversionModeValues.FullShape);
        }

        public void OnWindowActivated()
        {
            ResetFocus();
        }

        public event ModernizedAlice.IPlugin.ModuleInterface.TextChangedEventHandler TextChanged;

        public void OnTextChanged(OffsetChangeMap map)
        {
            if (DoNotFireEvent)
            {
                return;
            }

            if (TextChanged != null)
            {
                List<MyTextChange> textChangeList = new List<MyTextChange>();

                foreach (var change in map)
                {
                    textChangeList.Add(new MyTextChange()
                    {
                        AddedLength = change.InsertionLength,
                        Offset = change.Offset,
                        RemovedLength = change.RemovalLength,
                    });
                }

                TextChanged(this,
                    new ModernizedAlice.IPlugin.ModuleInterface.TextChangedEventArgs() {Source = _view, Changes = textChangeList });
            }
        }

        public event TextSearchEventHandler TextSearchOccured;
        public void OnTextSearchOccured(String initWord)
        {
            if (TextSearchOccured != null)
            {
                TextSearchOccured(this, new TextSearchEventArgs() { SearchWord = initWord });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
