using ArtOfWords.Models.FileSelector;
using CommonControls.Util;
using FileSelector.Biz;
using FileSelector.Model;
using FileSelector.Views;
using Microsoft.Win32;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.Services.Manager;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ArtOfWords.MainService
{
    /// <summary>
    /// セーブファイルを扱うサービス
    /// </summary>
    public class SaveFileService
    {
        private String _currentFile = "";

        /// <summary>
        /// 現在開いているファイルのパス
        /// </summary>
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

        /// <summary>
        /// ファイルをセーブする
        /// </summary>
        /// <returns>正否</returns>
        public bool SaveFile()
        {
            // 現在のファイル名が設定されていなければ名前をつけて保存
            if (CurrentFile == "")
            {
                return SaveFileWithName();
            }

            // それ以外は上書き
            return SaveFile(CurrentFile);
        }

        /// <summary>
        /// ファイル名を指定してファイルを保存する
        /// </summary>
        /// <param name="toFile"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 名前をつけて保存
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 保存先フォルダーのパスを取得する
        /// </summary>
        private string GetFolderPath()
        {
            var t = Properties.Settings.Default.SavedFolderPath;

            if (t.Count() == 0 || t == "Default")
            {
                return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            return t;
        }

        /// <summary>
        /// ファイルを開く
        /// </summary>
        /// <param name="editor">テキストを展開するためのIEditorインターフェース</param>
        /// <returns>正否</returns>
        public bool OpenFile(IEditor editor)
        {
            FileSelectorControl control = new FileSelectorControl();
            control.Owner = Application.Current.MainWindow;
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

        /// <summary>
        /// パスを指定してファイルを開く
        /// </summary>
        /// <param name="editor">テキストを展開するためのIEditorインターフェース</param>
        /// <param name="filePath">ファイルを開くパス</param>
        /// <returns>正否</returns>
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

        /// <summary>
        /// 新規作成
        /// </summary>
        /// <param name="iEditor">テキストを取得するためのIEditorインターフェース</param>
        /// <returns>正否だけどとりあえずtrueしか返さない</returns>
        public bool CreateNew(IEditor iEditor)
        {
            ModelsComposite.CreateNew(iEditor);
            CurrentFile = "";

            EventAggregator.OnDataReloaded(this, new DataReloadedEventArgs());

            return true;
        }

        /// <summary>
        /// 引き継いで新規作成
        /// </summary>
        /// <param name="iEditor">テキストを取得するためのIEditorインターフェース</param>
        /// <returns>正否だけどとりあえずtrueしか返さない</returns>
        public bool CreateNewPlus(IEditor iEditor)
        {
            NewFilePlusControl control = new NewFilePlusControl();
            control.Owner = Application.Current.MainWindow;
            var newFilePlusViewModel = NewFilePlusGenerator.GetNewFilePlusViewModel();
            control.DataContext = newFilePlusViewModel;

            //ダイアログを表示する
            if (ShowDialogManager.ShowDialog(control) == true)
            {
                var transferData = NewFilePlusGenerator.GetTransferData(newFilePlusViewModel);

                ModelsComposite.CreateNew(iEditor);
                CurrentFile = "";

                // Transferの過程でイベントが発生しても怒られないように、一旦Newの状態を各画面に通知する
                EventAggregator.OnDataReloaded(this, new DataReloadedEventArgs());

                // 引き継ぎ
                NewFilePlusTransferer.Transfer(transferData);

                // もっかい画面に通知
                EventAggregator.OnDataReloaded(this, new DataReloadedEventArgs());


                return true;
            }

            return true;
        }
    }
}
