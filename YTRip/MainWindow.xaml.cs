using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using YoutubeExtractor;

namespace YTRip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string DOWNLOAD_TEXT_QUERY = "Download";
        const string WAIT_TEXT = "Please wait...";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Invoked when BtnExtract is clicked
        /// Starts the extraction process of a video or playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnExtract_Click(object sender, RoutedEventArgs e)
        {
            //The list of videos available for download
            IEnumerable<VideoInfo> videoList = null;

            //Prevent another video being downloaded
            DisableExtractionButton(WAIT_TEXT);
            
            //Get the video info
            videoList = await Task.Run(() => ResolveURL(ExtractionUrl));
            //Check that the return was not null
            if (videoList == null)
            {
                MessageBox.Show("The URL entered is not a valid YouTube video URL!");
                EnableExtractionButton(DOWNLOAD_TEXT_QUERY);
                return;
            }

            //The user download preferences selector
            WndwInfoSelector infoSelector = null;
            //The video to download
            VideoInfo video = null;

            while (true)
            {
                //Get the user preferences
                infoSelector = GetUserDownloadPreferences(videoList);

                //If we are to download video, else download audio
                if (infoSelector.SelectedDownloadType == DownloadType.Video)
                {
                    //Try to find the first element
                    try
                    {
                        //Get the first video with the selected resolution and format and highest bitrate
                        video = videoList.Where(info => info.Resolution == infoSelector.SelectedVideoResolution
                        && info.VideoExtension == infoSelector.SelectedVideoExtension && info.AudioBitrate == infoSelector.SelectedAudioBitrate)
                                         .First();
                        break;
                    }
                    catch
                    {
                        MessageBox.Show("We couldn't find a video with the stats that you selected!");
                        continue;
                    }
                } else
                {
                    try
                    {
                        //Get the first video with the selected resolution and format and highest bitrate
                        video = videoList.Where(info => info.AudioBitrate == infoSelector.SelectedAudioBitrate
                        && info.AudioExtension == infoSelector.SelectedAudioExtension && info.CanExtractAudio)
                                         .First();
                        break;
                    } catch
                    {
                        MessageBox.Show("We couldn't find an audio track with the stats that you selected!");
                        continue;
                    }
                }
            }

            //If the video has an encrypted signature, decode it
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            //Get the download folder location
            string downloadLocation = ShowFolderSelectionDialog();

            if (infoSelector.SelectedDownloadType == DownloadType.Video)
            {
                //Create the downloader
                VideoDownloader downloader = new VideoDownloader(video, Path.Combine(downloadLocation, DownloadItem.PathCleaner(video.Title) + video.VideoExtension));

                //Create a new DownloadVideo item
                DownloadVideoItem item = new DownloadVideoItem(DownloadType.Video, video.Title, downloader);

                //Add a new DownloadVideoItem
                DownloadingItems.Add(item);

                EnableExtractionButton(DOWNLOAD_TEXT_QUERY);
            } else
            {
                //Create the audio downloader
                AudioDownloader downloader = new AudioDownloader(video, Path.Combine(downloadLocation, DownloadItem.PathCleaner(video.Title) + video.AudioExtension));

                //Create a new DownloadAudio item
                DownloadAudioItem item = new DownloadAudioItem(DownloadType.Audio, video.Title, downloader);

                //Add the new DownloadAudioItem
                DownloadingItems.Add(item);

                EnableExtractionButton(DOWNLOAD_TEXT_QUERY);

            }
        }

        /// <summary>
        /// Creates an InfoSelector and returns the users download preferences
        /// </summary>
        /// <param name="videoList">The list of videos available</param>
        /// <returns>The user's preferences</returns>
        private WndwInfoSelector GetUserDownloadPreferences(IEnumerable<VideoInfo> videoList)
        {
            //Create a new info selection window
            WndwInfoSelector infoSelector = new WndwInfoSelector
            {
                VideoInfo = videoList
            };

            //Show the info selection dialog
            infoSelector.ShowDialog();

            //Return if the selection made was cancelled
            if (infoSelector.SelectionCancelled)
            {
                return null;
            } else
            {
                return infoSelector;
            }
        }

        /// <summary>
        /// Resolves a YouTube URL. Used asyncronously
        /// </summary>
        /// <param name="URL">The URL to resolve</param>
        /// <returns>The list of video files available for download</returns>
        private IEnumerable<VideoInfo> ResolveURL(string URL)
        {
            try
            {
                return DownloadUrlResolver.GetDownloadUrls(ExtractionUrl);
            } catch {
                return null;
            }
        }

        /// <summary>
        /// Shows a folder selection dialog
        /// </summary>
        /// <returns></returns>
        private string ShowFolderSelectionDialog()
        {
            //Create a new file dialog
            CommonOpenFileDialog cd = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                Title = "Choose download location"
            };

            //Show a file dialog
            CommonFileDialogResult result = cd.ShowDialog();

            string selectedLocation;

            //Check that the user has selected a folder
            if (result == CommonFileDialogResult.Ok)
            {
                //Set the download location
                selectedLocation =  cd.FileName;
            }
            else
            {
                selectedLocation =  null;
            }

            //Close the resource
            cd.Dispose();

            return selectedLocation;
        }
    }
}
