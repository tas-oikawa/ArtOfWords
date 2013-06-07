using Editor4ArtOfWords.Model;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Editor4ArtOfWords
{
    /// <summary>
    /// AvalonEditorControl.xaml の相互作用ロジック
    /// </summary>
    public partial class AvalonEditorControl : UserControl
    {
        AvalonEditorViewModel _model;

        public AvalonEditorControl(AvalonEditorViewModel model)
        {
            _model = model;
            InitializeComponent();

            this.DataContext = _model;

            Editor.TextArea.TextView.ScrollOffsetChanged += TextView_ScrollOffsetChanged;

            Editor.Document.Changed += Document_Changed;
            Editor.Options.InheritWordWrapIndentation = false;
            InputMethod.SetIsInputMethodEnabled(Editor, true);
        }

        void Document_Changed(object sender, DocumentChangeEventArgs e)
        {
            var arg = e as DocumentChangeEventArgs;

            _model.OnTextChanged(arg.OffsetChangeMap);
        }

        private void TextView_ScrollOffsetChanged(object sender, EventArgs e)
        {
            _model.OnScrollOffsetChanged();
        }

        private void Editor_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _model.ResetFocus();
        }

        private void _menuCut_Click(object sender, RoutedEventArgs e)
        {
            Editor.Cut();
        }

        private void _menuCopy_Click(object sender, RoutedEventArgs e)
        {
            Editor.Copy();
        }

        private void _menuPaste_Click(object sender, RoutedEventArgs e)
        {
            Editor.Paste();
        }

        private void _menuSelectAll_Click(object sender, RoutedEventArgs e)
        {
            Editor.SelectAll();
        }

        private void ExecuteSearch(object sender, ExecutedRoutedEventArgs e)
        {
            String select = Editor.SelectedText;
            _model.OnTextSearchOccured(select);
        }
    }
}
