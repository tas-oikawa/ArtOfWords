using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ArtOfWords.EnvironmentSettings
{
    public class ListedFont
    {
        private string _fontName;

        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }
        private string _sampleString;

        public string SampleString
        {
            get { return _sampleString; }
            set { _sampleString = value; }
        }
        private FontFamily _fontFamily;

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }
    }
}
