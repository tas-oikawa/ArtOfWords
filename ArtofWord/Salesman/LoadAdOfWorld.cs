using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ArtOfWords.Salesman
{
    public class LoadAdOfWorld
    {
        private string GetDirectoryPath()
        {
            string assemblyLocation = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\KienaiProject\\ArtOfWords";
            string path = assemblyLocation + "\\AdsOfWorld\\";

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
