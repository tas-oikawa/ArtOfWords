using System;
using System.Linq;
using System.Text;
using WritersBattleField.ViewModel;
using System.Windows;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using CharacterBuildControll.Model;
using StoryFrameBuildControl.Model;
using ItemBuildControl.Model;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ArtOfWords.Salesman;
using ModernizedAlice.ArtOfWords.BizCommon;
using System.Windows.Input;
using ModernizedAlice.ArtOfWords.BizCommon.Util;
using System.IO;
using System.Reflection;
using ArtOfWords.DataGenerator;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using ArtOfWords.ViewModel.FileSelector;
using CommonControls.Util;
using ModernizedAlice.ArtOfWords.BizCommon.ControlUtil;
using ModernizedAlice.PluginLoader;
using ModernizedAlice.IPlugin.ModuleInterface;
using System.Windows.Media;


namespace ArtOfWords.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
#region properties
        private bool _isDirty;
        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    OnPropertyChanged("IsDirty");
                    OnPropertyChanged("Title");
                }
            }
        }

        public string Title
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Art Of Words");
                if (_fileManager != null)
                {
                    if (_fileManager.CurrentFile.Count() > 1)
                    {
                        builder.Append(" [" + _fileManager.CurrentFile + "] ");
                    }
                }
                if (IsDirty)
                {
                    builder.Append("*編集中　CTRL + S でセーブできます");
                }

                return builder.ToString();
            }
        }

        private int _selectedTab;
        public int SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;

                    OnPropertyChanged("SelectedTab");
                    SelectTabChanged();
                }
            }
        }

        private BitmapImage _adsTabImage;
        public BitmapImage AdsTabImage
        {
            get
            {
                return _adsTabImage;
            }
            set
            {
                if (_adsTabImage != value)
                {
                    _adsTabImage = value;
                    OnPropertyChanged("AdsTabImage");
                }
            }
        }

        private string _adsPage;
        public string AdsPage
        {
            get
            {
                return _adsPage;
            }
            set
            {
                if (_adsPage != value)
                {
                    _adsPage = value;
                    _view.GetAdsWebBrowser().Source = new Uri(_adsPage);
                    OnPropertyChanged("AdsPage");
                }
            }
        }

        private bool _isAdsVisibility;
        public bool IsAdsVisibility
        {
            get
            {
                return _isAdsVisibility;
            }
            set
            {
                if (_isAdsVisibility != value)
                {
                    _isAdsVisibility = value;
                    OnPropertyChanged("IsAdsVisibility");
                }
            }
        }

        public string MyFileVersion
        {
            get
            {   
                return "ArtOfWords(Ver." + GetVersion() + ")";
            }
        }

        public string GetVersion()
        {
            //自分自身のAssemblyを取得
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            //バージョンの取得
            var ver = asm.GetName().Version;
            return ver.ToString(3);
        }

#endregion

        private PluginLoader _pluginLoader;
        private FileManager _fileManager;


        public MainWindowView _view;

        public WritersBattleFieldViewModel _writersBattleFieldViewModel;
        public CharacterBuildViewModel _characterBuildControlViewModel;
        public ItemBuildControlViewModel _itemBuildControlViewModel;
        public StoryFrameBuildControlViewModel _storyFrameBuildControlViewModel;

        public MainWindowViewModel(MainWindowView view)
        {
            IsAdsVisibility = false;
            _selectedTab = 0;

            _view = view;

            _writersBattleFieldViewModel = new WritersBattleFieldViewModel();
            _characterBuildControlViewModel = new CharacterBuildViewModel();
            _storyFrameBuildControlViewModel = new StoryFrameBuildControlViewModel();
            _itemBuildControlViewModel = new ItemBuildControlViewModel();

            _fileManager = new FileManager();

            _view.BindData(this);
            PluginLoad();

            //TestBizModel.PrepareForTest();

            // WindowShowイベントに登録
            EventAggregator.ShowEventRised += OnShowWindowEvent;
            EventAggregator.AdsLoaded += OnAdsLoaded;
            EventAggregator.DataReloaded += OnDataReloaded;
            EventAggregator.ModelValueChanged += OnModelChanged;
            EventAggregator.SaveSucceeded += OnSaved;
            EventAggregator.TryClose += OnTryClose;
            EventAggregator.TrySave += OnTrySave;
            EventAggregator.TryOpen += OnTryOpen;
            EventAggregator.TryCreateNew += OnTryCreateNew;
            EventAggregator.ChangeTabOccuredHandler += OnChangeTabOccured;
            EventAggregator.FontSettingChangedHandler += EventAggregator_FontSettingChangedHandler;

            _view.GetTimelineControl().EventChangedRised += OnTimelineEventChanged;
            
        }

        void EventAggregator_FontSettingChangedHandler(object sender, int dummy)
        {
            SetTextStyle();
        }

        private void SetTextStyle()
        {
            var iEditor = _pluginLoader.GetContainer().GetExportedValue<IEditor>();

            TextStyle style = new TextStyle();
            style.FontFamily = Properties.Settings.Default.TextBoxFontFamily;
            style.FontSize = Properties.Settings.Default.TextBoxFontSize;

            style.Background = new SolidColorBrush(Properties.Settings.Default.TextBoxBackgroundColor);
            style.Foreground = new SolidColorBrush(Properties.Settings.Default.TextBoxFontColor);

            iEditor.SetTextStyle(style);
        }

        private void PluginLoad()
        {
            _pluginLoader = new PluginLoader();
            _pluginLoader.Load();

            SetTextStyle();
            _view.GetWritersBattleFieldView().SetEditor(_pluginLoader.GetContainer().GetExportedValue<IEditor>());
        }

        private bool Initialized = false;
        public void InitializeViews()
        {
            _view.BindData(this);
            _writersBattleFieldViewModel.Initialize(_view.GetWritersBattleFieldView());
            _characterBuildControlViewModel.Initialize(_view.characterBuildControl1.Content as CharacterBuildControll.CharacterBuilder);
            _storyFrameBuildControlViewModel.Initialize(_view.storyFrameBuildControll.Content as StoryFrameBuildControl.StoryFrameBuildControll);
            _itemBuildControlViewModel.Initialize(_view.itemBuildControl.Content as ItemBuildControl.ItemBuildControl);

            LoadAdOfWorld words = new LoadAdOfWorld();
            words.Load(GetVersion());

            if (!Initialized)
            {
                Initialized = true;
                OnFirstInitialized();
            }
        }

        #region Events
        public void OnFirstInitialized()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() == 2)
            {
                var iEditor = _pluginLoader.GetContainer().GetExportedValue<IEditor>();
                _fileManager.OpenFile(iEditor, args[1]);
            }
            else
            {
                string filePath = Properties.Settings.Default.LastSavedFilePath;
                if (filePath == "Default")
                {
                    filePath = "Novels\\マニュアル.kieAow";
                }

                if (!Path.IsPathRooted(filePath))
                {
                    Assembly myAssembly = Assembly.GetEntryAssembly();
                    string path = myAssembly.Location;
                    filePath = Path.Combine(Path.GetDirectoryName(path), filePath);
                }
                if (File.Exists(filePath))
                {
                    var iEditor = _pluginLoader.GetContainer().GetExportedValue<IEditor>();
                    _fileManager.OpenFile(iEditor, filePath);
                }
            }
            FileBoxModelManager.Load();
        }

        private void OnShowWindowEvent(object sender, ShowWindowEventArgs e)
        {
        }

        private void OnAdsLoaded(object sender, AdsLoadedEventArgs e)
        {
            AdsTabImage = e.AdsTabImage;
            AdsPage = e.AdsPageUrl;
            IsAdsVisibility = true;
        }

        private void OnDataReloaded(object sender, DataReloadedEventArgs e)
        {
            InitializeViews();
            IsDirty = false;
            OnPropertyChanged("Title");
        }

        private void OnModelChanged(object sender, ModelValueChangedEventArgs e)
        {
            IsDirty = true;
        }

        private void OnSaved(object sender, SaveEventArgs e)
        {
            IsDirty = false;
            OnPropertyChanged("Title");
        }

        private void OnTryCreateNew(object sender, TryCreateEventArgs arg)
        {
            if (IsDirty)
            {
                if (AskSaving() == AskSaveResult.Cancel)
                {
                    return;
                }
            }
            _fileManager.CreateNew(arg.iEditor);
        }

        public void OnWindowActivated()
        {
            _writersBattleFieldViewModel.OnWindowActivated();
        }

        private bool IsMainWindowDisplayable()
        {
            if (ShowDialogManager.ShowingDialogCount == 0)
            {
                return true;
            }
            return false;
        }

        private void OnChangeTabOccured(object sender, ChangeTabEventArg arg)
        {
            if (!IsMainWindowDisplayable())
            {
                Application.Current.MainWindow.Activate();
                return;
            }

            SelectedTab = MainTabKindUtil.ToInt(arg.ChangeTo);
            EventAggregator.OnSelectObjectForceOccured(sender, new SelectObjectForceEventArgs() { Model = arg.SubObject });
        }

        private enum AskSaveResult
        {
            CanProcess,
            Cancel,
        }

        private AskSaveResult AskSaving()
        {
            MessageBoxResult res = ShowDialogManager.ShowMessageBox("そのまえに、データを保存しますか？（「いいえ」を押すと変更内容は破棄されます）", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.Cancel)
            {
                return AskSaveResult.Cancel;
            }

            if (res == MessageBoxResult.No)
            {
                return AskSaveResult.CanProcess;
            }

            // 一旦フォーカスを外します。
            var prevFocus = FocusManager.GetFocusedElement(_view);
            FocusManager.SetFocusedElement(_view, _view);

            // 溜まってるイベントは全部処理させる
            WPFUtil.DoEvents();


            _writersBattleFieldViewModel.SetTextToModelsComposite();

            if (_fileManager.SaveFile() == false)
            {
                return AskSaving();
            }

            return AskSaveResult.CanProcess;
        }

        private void OnTryClose(object sender, TryCloseEventArgs e)
        {
            TryClose();
        }

        public bool TryClose()
        {
            if (IsDirty)
            {
                if (AskSaving() == AskSaveResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }

        private void OnTrySave(object sender, TrySaveOccuredEventArgs e)
        {
            // 一旦フォーカスを外します。
            var prevFocus = FocusManager.GetFocusedElement(_view);
            FocusManager.SetFocusedElement(_view, _view);

            // 溜まってるイベントは全部処理させる
            WPFUtil.DoEvents();

            _writersBattleFieldViewModel.SetTextToModelsComposite();
            if (e.SaveKind == SaveKind.SaveWithName)
            {
                _fileManager.SaveFileWithName();
            }
            else
            {
                _fileManager.SaveFile();
            }

            FocusManager.SetFocusedElement(_view, prevFocus);
        }

        private void OnTryOpen(object sender, TryOpenEventArgs data)
        {
            if (IsDirty)
            {
                if (AskSaving() == AskSaveResult.Cancel)
                {
                    return;
                }
            }
            _fileManager.OpenFile(data.iEditor);
        }

        #endregion

        public void SelectTabChanged()
        {
            switch (MainTabKindUtil.ToMainTabKind(_selectedTab))
            {
                case MainTabKind.WritingTab:
                    _writersBattleFieldViewModel.DoShowMarkReviewer = false;
                    _writersBattleFieldViewModel.Mode = WritersBattleField.ViewModel.ViewMode.Writing;
                    break;

                case MainTabKind.CharacterTab:
                    _characterBuildControlViewModel.Initialize(_view.characterBuildControl1.Content as CharacterBuildControll.CharacterBuilder);
                    break;

                case MainTabKind.ItemTab:
                    _itemBuildControlViewModel.Initialize(_view.itemBuildControl.Content as ItemBuildControl.ItemBuildControl);
                    break;

                case MainTabKind.StoryFrameTab:
                    _storyFrameBuildControlViewModel.Initialize(_view.storyFrameBuildControll.Content as StoryFrameBuildControl.StoryFrameBuildControll);
                    break;

                case MainTabKind.TimelineTab:
                    ShowTimelineViewer();
                    break;

                case MainTabKind.FromKienaiTab:
                    break;
            }
        }

        private DateTime GetStartDateTime()
        {
            DateTime borderDate = new DateTime(0001, 01, 01);

            foreach (var model in ModelsComposite.StoryFrameModelManager.ModelCollection)
            {
                var story = model as StoryFrameModel;
                if (borderDate < story.StartDateTime)
                {
                    return story.StartDateTime;
                }
            }

            return DateTime.Today;
        }

        private bool isFirstTime2ShowTimeline = true;
        public void ShowTimelineViewer()
        {
            // Axisを作る
            _view.GetTimelineControl().DataSource = TimeAxisGenerator.Generate();
            if (isFirstTime2ShowTimeline)
            {
                _view.GetTimelineControl().TimespanEnum = TimelineControl.Model.TimespanEnum.Hour3;
                _view.GetTimelineControl().StartDateTime = GetStartDateTime();

                isFirstTime2ShowTimeline = false;
            }
            _view.GetTimelineControl().EventModelManager = EventModelManagerGenerator.Generate();
            
        }

        public void OnTimelineEventChanged(object sender, TimelineControl.Timeline.EventChangedArgs arg)
        {
            if (arg.kind == TimelineControl.Timeline.EventChangedKind.Add)
            {
                var addData = TimelineModelConverter.ConvertAsNew(arg.model);
                ModelsComposite.TimelineEventModelManager.AddModel(addData);
                arg.model.SourceObject = addData;
            }
            else if (arg.kind == TimelineControl.Timeline.EventChangedKind.Modify)
            {
                TimelineModelConverter.ConvertAsModify(arg.model, arg.model.SourceObject as TimelineEventModel);
                EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            }
            else
            {
                ModelsComposite.TimelineEventModelManager.RemoveModel(arg.model.SourceObject as TimelineEventModel);
            }
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
