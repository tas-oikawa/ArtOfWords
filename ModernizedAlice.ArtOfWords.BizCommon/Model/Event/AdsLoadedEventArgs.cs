using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Event
{
    public class AdsLoadedEventArgs : EventArgs
    {
        public BitmapImage AdsTabImage;
        public string AdsPageUrl;
    }
}
