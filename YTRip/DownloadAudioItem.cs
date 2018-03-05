using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YTRip
{
    public class DownloadAudioItem : DownloadItem
    {
        /// <summary>
        /// Downloads the video for us
        /// </summary>
        public AudioDownloader Downloader;

        public DownloadAudioItem()
        {

        }

        public DownloadAudioItem(DownloadType type, string name, AudioDownloader downloader)
        {
            DType = type;
            ItemName = name;
            Downloader = downloader;

            StartDownload();
        }

        /// <summary>
        /// Starts the download process
        /// </summary>
        /// <param name="downloader"></param>
        private async void StartDownload()
        {
            // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
            // because the download will take much longer than the audio extraction.
            Downloader.DownloadProgressChanged += (sender, args) => DownloadProgress = args.ProgressPercentage * 0.85;
            Downloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

            //Start the download
            await Task.Run(() => DownloadVideo());
        }

        private void DownloadVideo()
        {
            Downloader.Execute();
        }
    }
}
