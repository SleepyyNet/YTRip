using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YTRip
{
    /// <summary>
    /// The logic for a downloadable video
    /// </summary>
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
            //Update DownloadProgress when required
            Downloader.DownloadProgressChanged += (sender, args) => DownloadProgress = DoubleToShortPercentage(args.ProgressPercentage);
            //Run the downloader thread
            await Task.Run(() => DownloadVideo());
        }

        /// <summary>
        /// Downloads a video
        /// </summary>
        private void DownloadVideo()
        {
            //Start the download
            Downloader.Execute();
            //Set the download progress to complete
            DownloadProgress = "Complete";
        }
    }
}
