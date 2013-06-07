using ArtOfWords.ViewModel.FileSelector;
using CommonControls.Util;
using FileSelector;
using FileSelector.Model;
using Microsoft.Win32;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ArtOfWords.ViewModel
{
    public class FileManager
    {
        private String _currentFile = "";
        public String CurrentFile
        {
            set
            {
                if (_currentFile != value)
                {
                    _currentFile = value;
                }
            }
            get
            {
                return _currentFile;
            }
        }

        public bool SaveFile()
        {
            if (CurrentFile == "")
            {
                return SaveFileWithName();
            }

            return SaveFile(CurrentFile);
        }

        public bool SaveFile(string toFile)
        {
            SaveManager manager = new SaveManager();

            if (manager.Save(toFile) == SaveResult.Succeed)
            {
                CurrentFile = toFile;

                Properties.Settings.Default.SavedFolderPath = Path.GetDirectoryName(CurrentFile);
                Properties.Settings.Default.LastSavedFilePath = CurrentFile;

                FileBoxModelManager.AddRecentlySavedFile(CurrentFile);

                Properties.Settings.Default.Save();

                EventAggregator.OnSaved(this, new SaveEventArgs());

                return true;
            }
            else
            {
                ShowDialogManager.ShowMessageBox("保存に失敗しました。保存先を変えるなどして再チャレンジしてみてください", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return false;
        }

        public bool SaveFileWithName()
        {
            //SaveFileDialogクラスのインスタンスを作成
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = "新しいファイル.kieAow";
            sfd.InitialDirectory = GetFolderPath();
            sfd.Filter = "ArtOfWordsファイル(*.kieAow)|*.kieAow";
            sfd.FilterIndex = 1;

            sfd.Title = "保存先のファイルを選択してください";
            sfd.RestoreDirectory = true;
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;

            if (ShowDialogManager.ShowDialog(sfd) == true)
            {
                return SaveFile(sfd.FileName);
            }

            return false;
        }

        private string GetFolderPath()
        {
            var t = Properties.Settings.Default.SavedFolderPath;
            if (t.Count() == 0 || t == "Default")
            {
                return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            return t;
        }

        public bool OpenFile(IEditor editor)
        {
            FileSelectorControl control = new FileSelectorControl();
            var viewModel = new FileSelectorViewModel(control);

            viewModel.Initialize(NovelsCollectorGenerator.GetRecentlyNovelsCollector(),
                                    NovelsCollectorGenerator.GetSemiAutoBackupNovelsCollector(),
                                    NovelsCollectorGenerator.GetNovelsBoxCollector());

            control.SetViewModel(viewModel);
            //ダイアログを表示する
            if (ShowDialogManager.ShowDialog(control) == true)
            {
                OpenFile(editor, control.FilePath);
                return true;
            }

            return false;
        }

        public bool OpenFile(IEditor editor, String filePath)
        {
            LoadManager manager = new LoadManager(editor);

            if (manager.Load(filePath) == LoadResult.Succeed)
            {
                CurrentFile = filePath;

                string lastSavedFilePath = Properties.Settings.Default.LastSavedFilePath;
                if (lastSavedFilePath != "Default")
                {
                    Properties.Settings.Default.SavedFolderPath = Path.GetDirectoryName(CurrentFile);
                    Properties.Settings.Default.LastSavedFilePath = CurrentFile;
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                ShowDialogManager.ShowMessageBox("ファイルを開けませんでした。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            EventAggregator.OnDataReloaded(this, new DataReloadedEventArgs());

            return true;
        }


        public bool CreateNew(IEditor iEditor)
        {
            ModelsComposite.CreateNew(iEditor);
            CurrentFile = "";

            EventAggregator.OnDataReloaded(this, new DataReloadedEventArgs());

            return true;
        }
    }
}
