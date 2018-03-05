using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YTRip
{
    class DownloadVideoItem : DownloadItem
    {
        public DownloadVideoItem()
        {

        }

        public DownloadVideoItem(DownloadType type, string name, VideoDownloader downloader)
        {
            DType = type;
            ItemName = name;
            Downloader = downloader;

            StartDownload();
        }

        /// <summary>
        /// Downloads the video for us
        /// </summary>
        public VideoDownloader Downloader;

        /// <summary>
        /// Starts the download process
        /// </summary>
        /// <param name="downloader"></param>
        private async void StartDownload()
        {
            Downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
            await Task.Run(() => DownloadVideo());
        }

        private void DownloadVideo()
        {
            Downloader.Execute();
        }

        /// <summary>
        /// Invoked when the download percentage changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Downloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            DownloadProgress = e.ProgressPercentage;
        }
    }
}
