using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YTRip
{
    /// <summary>
    /// The logic for a downloadable audio item
    /// </summary>
    public class DownloadAudioItem : DownloadItem
    {
        public DownloadAudioItem() { }

        public DownloadAudioItem(DownloadType type, string name, AudioDownloader downloader)
        {
            DType = type;
            ItemName = name;
            Downloader = downloader;

            StartDownload();
        }

        /// <summary>
        /// Downloads the audio for us
        /// </summary>
        public AudioDownloader Downloader;

        /// <summary>
        /// Starts the download process
        /// </summary>
        /// <param name="downloader"></param>
        private async void StartDownload()
        {
            // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
            // because the download will take much longer than the audio extraction.
            Downloader.DownloadProgressChanged += (sender, args) => DownloadProgress = DoubleToShortPercentage(args.ProgressPercentage * 0.85);
            Downloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

            //Start the download
            await Task.Run(() => DownloadAudio());
        }

        /// <summary>
        /// Downloads the audio
        /// </summary>
        private void DownloadAudio()
        {
            //Start the download
            Downloader.Execute();
            //Set the download progress to complete
            DownloadProgress = "Complete";
        }
    }
}
