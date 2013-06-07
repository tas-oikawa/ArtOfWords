using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ArtOfWords.Salesman
{
    public class AdsDownloader
    {
        private enum DownloadState
        {
            None,
            XmlDownloading,
            ImageDownloading,
        }

        #region Properties
        private DownloadState _state = DownloadState.None;

        private readonly string XmlFilePath = "http://www.kienai.com/adsofworld/Ads.xml";

        public string DestinationDirectoryPath = "";

        private AdsXmlFormat _recentDownloadedXmlFormat;
        private AdsXmlFormatManager _xmlFormatManager;

        #endregion

        #region Event
        public delegate void AdsLoadedEventHandler(object sender, AdsLoadedEventArgs e);

        public event AdsLoadedEventHandler AdsLoaded;

        public void OnAdsLoadedEvent(object sender, AdsLoadedEventArgs e)
        {
            if (AdsLoaded != null)
            {
                AdsLoaded(sender, e);
            }
        }
        #endregion

        public AdsDownloader(String DestPath, AdsXmlFormatManager formatManager)
        {
            DestinationDirectoryPath = DestPath;
            _xmlFormatManager = formatManager;
        }

        private string GetDownloadedXmlPath()
        {
            return DestinationDirectoryPath + "\\" + "Ads.xml";
        }

        private string GetDownloadedImagePath()
        {
            return DestinationDirectoryPath + "\\" + "Ads.png";
        }

        private void AsyncLoadFile(string sourceFilePath, string destinationPath)
        {
            try
            {
                //ダウンロード基のURL
                Uri u = new Uri(sourceFilePath);
                WebClient downloadClient = new System.Net.WebClient();
                downloadClient.DownloadFileCompleted +=
                    new System.ComponentModel.AsyncCompletedEventHandler(
                        downloadClient_DownloadFileCompleted);
                //非同期ダウンロードを開始する
                downloadClient.DownloadFileAsync(u, destinationPath);
            }
            catch (Exception)
            {
            }
        }

        public void StartLoad()
        {
            _state = DownloadState.None;
            DoNextStep();
        }

        private void StartXmlDownloading()
        {
            _state = DownloadState.XmlDownloading;
            AsyncLoadFile(XmlFilePath, GetDownloadedXmlPath());
        }

        private void StartImageDownloading()
        {
            _state = DownloadState.ImageDownloading;
            try
            {
                _recentDownloadedXmlFormat = _xmlFormatManager.GetXmlFormat(GetDownloadedXmlPath());
                if (_recentDownloadedXmlFormat == null)
                {
                    return;
                }

                AsyncLoadFile(_recentDownloadedXmlFormat.DownloadFile, GetDownloadedImagePath());
            }
            catch (Exception )
            {
            }
        }

        private void FinishDownloading()
        {
            try
            {
                OnAdsLoadedEvent(this, new AdsLoadedEventArgs()
                {
                    AdsTabImage = new BitmapImage(new Uri(GetDownloadedImagePath())),
                    AdsPageUrl = _recentDownloadedXmlFormat.AdsPageURL
                });
            }
            catch (Exception)
            {
            }

            _state = DownloadState.None;
        }

        private void DoNextStep()
        {
            switch (_state)
            {
                case DownloadState.None:
                    StartXmlDownloading();
                    break;
                case DownloadState.XmlDownloading:
                    StartImageDownloading();
                    break;
                case DownloadState.ImageDownloading:
                    FinishDownloading();
                    break;
            }
        }

        private void downloadClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DoNextStep();
        }
    }
}
