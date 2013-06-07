using Microsoft.Win32;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.IPlugin.ModuleInterface;
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
using WritersBattleField.ViewModel;

namespace WritersBattleField.View
{
    /// <summary>
    /// WritersBattlefield.xaml の相互作用ロジック
    /// </summary>
    public partial class WritersBattleFieldView : UserControl
    {
        private WritersBattleFieldViewModel _model;
        private MarkerViewModel _markerModel { set; get; }
        private IEditor _editor;

        public IEditor Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

     

        public WritersBattleFieldView()
        {
            InitializeComponent();
        }

        private void HookScrollEvent()
        {
            var control = _editor.GetControl();

            _editor.ScrollOffsetChanged += (e) =>
            {
                _model.OnTextRegionChanged();
            };

            control.SizeChanged += (me, evt) =>
            {
                _model.OnTextRegionChanged();
            };
        }

        public void BindModel(WritersBattleFieldViewModel model)
        {
            _model = model;
            _markerModel = _model.MarkerModel;

            _editor.SetText(model.Text);
            
            BaseGrid.DataContext = model;
            // スクロール時のイベントを登録する
            HookScrollEvent();

            _editor.TextChanged += _editor_TextChanged;
            _editor.TextSearchOccured += _editor_TextSearchOccured;
        }

        void _editor_TextSearchOccured(object sender, TextSearchEventArgs arg)
        {
            _model.OnEditor_TextSearchOccured(sender, arg);
        }

        void _editor_TextChanged(object sender, ModernizedAlice.IPlugin.ModuleInterface.TextChangedEventArgs arg)
        {
            MarkUpdater.UpdateMarkings(arg);
        }

        public MarkingLayerView GetMarkingLayerView()
        {
            return markingLayer;
        }

        public void LineDown()
        {
            _editor.LineDown();
        }

        public void LineUp()
        {
            _editor.LineUp();
        }

        public int GetHeadIndexOfVisibleText()
        {
            return _editor.GetHeadIndexOfVisibleText();
        }

        public int GetTailIndexOfVisibleText()
        {
            return _editor.GetTailIndexOfVisibleText();
        }

        public int GetTailIndexOfLineByIndex(int index)
        {
            return _editor.GetTailIndexOfLineByIndex(index);
        }

        public int GetLineIndexFromCharacterIndex(int index)
        {
            return _editor.GetLineIndexFromCharacterIndex(index);
        }

        public List<Rect> GetRectByCharIndex(int headIndex, int tailIndex)
        {
            return _editor.GetRectByCharIndex(headIndex, tailIndex);
        }

        public Rect GetRectByCharIndex(int index)
        {
            return _editor.GetRectByCharIndex(index);
        }

        public int GetIndexFromPosition(Point point)
        {
            return _editor.GetIndexFromPosition(point);
        }

        public Grid GetMarkTabPanel()
        {
            if (_model.Mode == ViewMode.Character)
            {
                return this.TalkPanel;
            }
            else if (_model.Mode == ViewMode.StoryFrame)
            {
                return this.StoryFramePanel;
            }
            return null;
        }

        public void ResetMarkTab()
        {
            var panel = GetMarkTabPanel();

            if (panel == null)
            {
                return;
            }

            var markables = _model.GetMarkablesOnMode();
            _markerModel.MarkableObjects = markables;
            panel.Children.Clear();
            panel.Children.Add(_markerModel.GetMarkingPanel());
        }

        public void SetEditor(IEditor iEditor)
        {
            _editor = iEditor;
            TextRegion.Children.Insert(0, _editor.GetControl());
        }

        public void OnWindowActivated()
        {
            if (_editor != null)
            {
                _editor.OnWindowActivated();
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _markerModel.AllButtonBeUnpushed();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SetTextToModelsComposite();
            EventAggregator.OnTrySave(this, new TrySaveOccuredEventArgs() { SaveKind = SaveKind.SaveOverWrite });
        }

        private void saveWithNameButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SetTextToModelsComposite();
            EventAggregator.OnTrySave(this, new TrySaveOccuredEventArgs() { SaveKind = SaveKind.SaveWithName });
        }

        private void createNewButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SetTextToModelsComposite();
            EventAggregator.OnTryCreateNew(this, new TryCreateEventArgs() { iEditor = _editor });
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SetTextToModelsComposite();
            EventAggregator.OnTryOpen(this, new TryOpenEventArgs() { iEditor = _editor });
        }


    }
}
