using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ArtOfWords.Models.Salesman
{
    public class LoadAdOfWorld
    {
        private string GetDirectoryPath()
        {
            string path = CommonDirectoryUtil.GetAdsDirectoryPath();

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            return path;
        }

        public void Load(string version)
        {
            AdsXmlFormatManager manager = new AdsXmlFormatManager(version);

            AdsDownloader downloader = new AdsDownloader(GetDirectoryPath(), manager);
            downloader.AdsLoaded += downloader_AdsLoaded;
            downloader.StartLoad();
        }

        void downloader_AdsLoaded(object sender, AdsLoadedEventArgs arg)
        {
            EventAggregator.OnAdsLoadedEvent(sender, arg);
        }
    }
}
