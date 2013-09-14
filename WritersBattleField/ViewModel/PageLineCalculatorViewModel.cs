using ModernizedAlice.ArtOfWords.BizCommon.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WritersBattleField.ViewModel
{
    public class PageLineCalculatorViewModel : INotifyPropertyChanged
    {
        private DocumentModel _documentModel;
        private NovelSettingModel _novelSettingModel;

        public int CharactersPerLineCount
        {
            get { return _novelSettingModel.CharactersPerLineCount; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }

                if (_novelSettingModel.CharactersPerLineCount != value)
                {
                    _novelSettingModel.CharactersPerLineCount = value;
                    OnPropertyChanged("CharactersPerLineCount");
                };
            }
        }

        public int LineCountPerPage
        {
            get { return _novelSettingModel.LineCountPerPage; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }

                if (_novelSettingModel.LineCountPerPage != value)
                {
                    _novelSettingModel.LineCountPerPage = value;
                    OnPropertyChanged("LineCountPerPage");
                };
            }
        }

        private int _lineCount;

        public int LineCount
        {
            get { return _lineCount; }
        }

        private int _pageCount;

        public int PageCount
        {
            get { return _pageCount; }
        }

        private string _byteCount;
        public string ByteCount
        {
            get { return _byteCount; }
        }

        public string AllCharactersCount
        {
            get
            {
                int count = 0;
                foreach (var txt in _documentModel.Text)
                {
                    if (txt == '\n')
                    {
                        continue;
                    }
                    count++;
                }

                return "(" +  count.ToString("#,##0") + "文字)";
            }
        }

        private bool _isCalculated = false;

        public bool IsCalculated
        {
            get { return _isCalculated; }
            set 
            {
                if (_isCalculated != value)
                {
                    _isCalculated = value;

                    OnPropertyChanged("IsCalculated");
                }
            }
        }

        public PageLineCalculatorViewModel(NovelSettingModel novelSettingModel, DocumentModel documentModel)
        {
            _novelSettingModel = novelSettingModel;
            _documentModel = documentModel;
        }

        /// <summary>
        /// 行数とページ数を計算する
        /// </summary>
        public void CalculateLineAndPages()
        {
            _lineCount = 0;
            
            int currentCharacterNum = 0;

            foreach (var chara in _documentModel.Text)
            {
                if(chara == '\n')
                {
                    currentCharacterNum = 0;
                    _lineCount++;
                }

                currentCharacterNum++;

                if (currentCharacterNum > CharactersPerLineCount)
                {
                    currentCharacterNum = 0;
                    _lineCount++;
                }
                
            }

            if (LineCountPerPage > 0)
            {
                _pageCount = (_lineCount / LineCountPerPage) + 1;
            }

            CalculateByteCount();

            IsCalculated = true;

            OnPropertyChanged("LineCount");
            OnPropertyChanged("PageCount");
        }

        /// <summary>
        /// バイトサイズを計算する
        /// </summary>
        private void CalculateByteCount()
        {
            int byteSize = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(_documentModel.Text);

            _byteCount = (byteSize / 1024) + "KB(" + byteSize + "バイト)";
            OnPropertyChanged("ByteCount");
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
