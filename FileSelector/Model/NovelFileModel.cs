using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSelector.Model
{
    public class NovelFileModel : INotifyPropertyChanged
    {
        #region Properties
        private string _fileName;

        private DateTime _updateDateTime;

        private string _filePath;

        public string FileName
        {
            get { return _fileName; }
            set 
            {
                if (value != _fileName)
                {
                    _fileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        public DateTime UpdateDateTime
        {
            get { return _updateDateTime; }
            set
            {
                if (value != _updateDateTime)
                {
                    _updateDateTime = value;
                    OnPropertyChanged("UpdateDateTime");
                }
            }
        }

        public String UpdateDateTimeString
        {
            get
            {
                return _updateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }


        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (value != _filePath)
                {
                    _filePath = value;
                    OnPropertyChanged("FilePath");
                }
            }
        }
        #endregion

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
