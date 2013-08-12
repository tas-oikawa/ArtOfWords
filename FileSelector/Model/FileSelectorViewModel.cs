using CommonControls.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSelector.Model
{
    public class FileSelectorViewModel : INotifyPropertyChanged
    {
        #region Properties
        private FileSelectorControl _view;

        private List<NovelFileModel> _recentlyUsedFiles;
        private List<NovelFileModel> _semiautoSavedFiles;
        private List<NovelFileModel> _novelsBoxFiles;

        private NovelFileModel _selectedRecentlyUsed;
        private NovelFileModel _selectedSemiAutoSaved;
        private NovelFileModel _selectedNovelsBox;

        public List<NovelFileModel> RecentlyUsedFiles
        {
            get { return _recentlyUsedFiles; }
        }

        public List<NovelFileModel> SemiautoSavedFiles
        {
            get { return _semiautoSavedFiles; }
        }

        public List<NovelFileModel> NovelsBoxFiles
        {
            get { return _novelsBoxFiles; }
        }

        public NovelFileModel SelectedRecentlyUsed
        {
            get { return _selectedRecentlyUsed; }
            set 
            {
                if (_selectedRecentlyUsed != value)
                {
                    _selectedRecentlyUsed = value;
                    if (value != null)
                    {
                        SelectedSemiAutoSaved = null;
                        SelectedNovelsBox = null;

                        OpenFilePath = value.FilePath;
                    }
                    OnPropertyChanged("SelectedRecentlyUsed");
                } 
            }
        }

        public NovelFileModel SelectedSemiAutoSaved
        {
            get { return _selectedSemiAutoSaved; }
            set
            {
                if (_selectedSemiAutoSaved != value)
                {
                    _selectedSemiAutoSaved = value;
                    if (value != null)
                    {
                        SelectedRecentlyUsed = null;
                        SelectedNovelsBox = null;

                        OpenFilePath = value.FilePath;
                    }
                    OnPropertyChanged("SelectedSemiAutoSaved");
                }
            }
        }

        public NovelFileModel SelectedNovelsBox
        {
            get { return _selectedNovelsBox; }
            set
            {
                if (_selectedNovelsBox != value)
                {
                    _selectedNovelsBox = value;
                    if (value != null)
                    {
                        SelectedRecentlyUsed = null;
                        SelectedSemiAutoSaved = null;

                        OpenFilePath = value.FilePath;
                    }
                    OnPropertyChanged("SelectedNovelsBox");
                }
            }
        }

        private bool _isOpeningRecentlyOpenedFile = true;

        public bool IsOpeningRecentlyOpenedFile
        {
            get { return _isOpeningRecentlyOpenedFile; }
            set 
            {
                if (value != _isOpeningRecentlyOpenedFile) 
                {
                    if (value)
                    {
                        IsOpeningSemiAutoBackupFile = false;
                        IsOpeningNovelsBoxFile = false;
                    }
                    _isOpeningRecentlyOpenedFile = value;
                    OnPropertyChanged("IsOpeningRecentlyOpenedFile");
                }
            }
        }
        private bool _isOpeningSemiAutoBackupFile = false;

        public bool IsOpeningSemiAutoBackupFile
        {
            get { return _isOpeningSemiAutoBackupFile; }
            set
            {
                if (value != _isOpeningSemiAutoBackupFile)
                {
                    if (value)
                    {
                        IsOpeningRecentlyOpenedFile = false;
                        IsOpeningNovelsBoxFile = false;
                    }
                    _isOpeningSemiAutoBackupFile = value;
                    OnPropertyChanged("IsOpeningSemiAutoBackupFile");
                }
            }
        }
        private bool _isOpeningNovelsBoxFile = false;

        public bool IsOpeningNovelsBoxFile
        {
            get { return _isOpeningNovelsBoxFile; }
            set
            {
                if (value != _isOpeningNovelsBoxFile)
                {
                    if (value)
                    {
                        IsOpeningRecentlyOpenedFile = false;
                        IsOpeningSemiAutoBackupFile = false;
                    }
                    _isOpeningNovelsBoxFile = value;
                    OnPropertyChanged("IsOpeningNovelsBoxFile");
                }
            }
        }

        private string _openFilePath;
        public string OpenFilePath
        {
            get
            {
                return _openFilePath;
            }
            set
            {
                if (value != _openFilePath)
                {
                    _openFilePath = value;
                    OnPropertyChanged("OpenFilePath");
                }
            }
        }

        #endregion

        public FileSelectorViewModel(FileSelectorControl view)
        {
            _view = view;

            _recentlyUsedFiles = new List<NovelFileModel>();
            _semiautoSavedFiles = new List<NovelFileModel>();
            _novelsBoxFiles = new List<NovelFileModel>();
        }

        public virtual void Initialize(INovelsCollector recentlyNovelsCollector,
                                        INovelsCollector semiautoSavedFilesCollector,
                                        INovelsCollector novelsBoxFilesCollector)
        {
            _recentlyUsedFiles = recentlyNovelsCollector.Get();
            _semiautoSavedFiles = semiautoSavedFilesCollector.Get();
            _novelsBoxFiles = novelsBoxFilesCollector.Get();
        }

        private string GetOpenFileName()
        {
            var str = Path.GetFileName(OpenFilePath);

            if (str == null || str.Count() == 0)
            {
                return "Default.kieAow";
            }

            return str;
        }

        private string GetOpenDirectoryName()
        {
            var str = Path.GetDirectoryName(OpenFilePath);

            if (str == null || str.Count() == 0)
            {
                return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);;
            }

            return str;
        }

        public void OpenFileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileName = GetOpenFileName();

            ofd.InitialDirectory = GetOpenDirectoryName();

            ofd.Filter = "ArtOfWordsファイル(*.kieAow)|*.kieAow";
            ofd.FilterIndex = 1;

            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;

            //ダイアログを表示する
            if (ShowDialogManager.ShowDialog(ofd) == true)
            {
                OpenFilePath = ofd.FileName;
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
