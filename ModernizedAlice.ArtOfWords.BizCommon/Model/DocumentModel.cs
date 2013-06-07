using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model
{
    [Serializable()]
    public class DocumentModel
    {
        private string _text;
        public string Text
        {
            set
            {
                if (value != _text)
                {
                    _text = value;
                }
            }
            get
            {
                return _text;
            }
        }
    }
}
