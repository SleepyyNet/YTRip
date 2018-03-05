using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Shapes;
using YoutubeExtractor;

namespace YTRip
{
    /// <summary>
    /// The types of download available to the user
    /// </summary>
    public enum DownloadType
    {
        Video = 0,
        Audio = 1
    }

    /// <summary>
    /// Interaction logic for WndwInfoSelector.xaml
    /// </summary>
    public partial class WndwInfoSelector : Window, INotifyPropertyChanged
    {
        #region VideoInfo

        private IEnumerable<VideoInfo> _videoInfo;

        public IEnumerable<VideoInfo> VideoInfo
        {
            get { return _videoInfo; }
            set
            {
                _videoInfo = value;
                RaisePropertyChanged("VideoInfo");
            }
        }

        #endregion

        #region AvailableVideoResolutions

        private ObservableCollection<int> _availableVideoResolutions = new ObservableCollection<int>();

        //The video resolutions we can download in
        public ObservableCollection<int> AvailableVideoResolutions
        {
            get { return _availableVideoResolutions; }
            set
            {
                _availableVideoResolutions = value;
                RaisePropertyChanged("AvailableVideoResolutions");
            }
        }

        #endregion

        #region AvailableVideoFormats

        private ObservableCollection<string> _availableVideoFormats = new ObservableCollection<string>();

        //The video resolutions we can download in
        public ObservableCollection<string> AvailableVideoFormats
        {
            get { return _availableVideoFormats; }
            set
            {
                _availableVideoFormats = value;
                RaisePropertyChanged("AvailableVideoFormats");
            }
        }

        #endregion

        #region AvailableAudioBitrates

        private ObservableCollection<int> _availableAudioBitrates = new ObservableCollection<int>();

        //The video resolutions we can download in
        public ObservableCollection<int> AvailableAudioBitrates
        {
            get { return _availableAudioBitrates; }
            set
            {
                _availableAudioBitrates = value;
                RaisePropertyChanged("AvailableAudioBitrates");

                //Set the selected video resolution
                if (_availableAudioBitrates.Count > 0)
                {
                    SelectedAudioBitrate = _availableAudioBitrates[0];
                }
            }
        }

        #endregion

        #region AvailableAudioFormats

        private ObservableCollection<string> _availableAudioFormats = new ObservableCollection<string>();

        //The video resolutions we can download in
        public ObservableCollection<string> AvailableAudioFormats
        {
            get { return _availableAudioFormats; }
            set
            {
                _availableAudioFormats = value;
                RaisePropertyChanged("AvailableAudioFormats");

                if (_availableAudioFormats.Count > 0)
                {
                    SelectedAudioExtension = _availableAudioFormats[0];
                }
            }
        }

        #endregion

        #region SelectedVideoResolution

        private int _selectedVideoResolution;

        //The selected video resolution
        public int SelectedVideoResolution
        {
            get { return _selectedVideoResolution; }
            set
            {
                _selectedVideoResolution = value;
                RaisePropertyChanged("SelectedVideoResolution");
            }
        }

        #endregion

        #region SelectedAudioBitrate

        private int _selectedAudioBitrate;

        //The selected video resolution
        public int SelectedAudioBitrate
        {
            get { return _selectedAudioBitrate; }
            set
            {
                _selectedAudioBitrate = value;
                RaisePropertyChanged("SelectedAudioBitrate");
            }
        }

        #endregion

        #region SelectedVideoExtension

        private string _selectedVideoExtension;

        public string SelectedVideoExtension
        {
            get { return _selectedVideoExtension; }
            set
            {
                _selectedVideoExtension = value;
                RaisePropertyChanged("SelectedVideoFormat");
            }
        }

        #endregion

        #region SelectedAudioExtension

        private string _selectedAudioExtension;

        public string SelectedAudioExtension
        {
            get { return _selectedAudioExtension; }
            set
            {
                _selectedAudioExtension = value;
                RaisePropertyChanged("SelectedAudioFormat");
            }
        }

        #endregion

        #region Cancelled

        private bool _selectionCancelled = false;

        /// <summary>
        /// Selection has been cancelled
        /// </summary>
        public bool SelectionCancelled
        {
            get { return _selectionCancelled; }
            private set { _selectionCancelled = value; }
        }

        #endregion

        #region SelectedDownloadType

        private DownloadType _selectedDownloadType = DownloadType.Video;

        //The selected download type
        public DownloadType SelectedDownloadType
        {
            get { return _selectedDownloadType; }
            set
            {
                _selectedDownloadType = value;
                RaisePropertyChanged("SelectedDownloadType");
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify the view that a property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public WndwInfoSelector()
        {
            InitializeComponent();
            DataContext = this;
            Owner = Application.Current.MainWindow;
            PropertyChanged += WndwInfoSelector_PropertyChanged;
        }

        /// <summary>
        /// Invoked when <see cref="PropertyChanged"/> is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WndwInfoSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "VideoInfo":
                    //Add all the extensions to the extension list
                    foreach (VideoInfo element in VideoInfo)
                    {
                        if (!AvailableVideoFormats.Contains(element.VideoExtension))
                        {
                            AvailableVideoFormats.Add(element.VideoExtension);
                        }
                    }

                    //Sort the format list
                    AvailableVideoFormats.OrderBy(format => format);
                    SelectedVideoExtension = AvailableVideoFormats[0];

                    //Add all the audio extensions to the extension list
                    foreach (VideoInfo element in VideoInfo)
                    {
                        if (!AvailableAudioFormats.Contains(element.AudioExtension))
                        {
                            AvailableAudioFormats.Add(element.AudioExtension);
                        }
                    }

                    //Sort the list
                    AvailableAudioFormats.OrderBy(format => format);
                    SelectedAudioExtension = AvailableAudioFormats[0];
                    break;

                case "SelectedVideoFormat":
                    //Get all the videos with the selected format
                    IEnumerable<VideoInfo> validVideos = VideoInfo.Where(format => format.VideoExtension == SelectedVideoExtension)
                                                                  .OrderBy(res => res.Resolution)
                                                                  .Distinct();

                    //Clear the current resolutions
                    AvailableVideoResolutions.Clear();

                    //Add the resolutions to the list
                    foreach (VideoInfo element in validVideos)
                    {
                        AvailableVideoResolutions.Add(element.Resolution);
                    }

                    //Select the highest res
                    SelectedVideoResolution = AvailableVideoResolutions.Last();
                    break;

                case "SelectedAudioFormat":
                    //Get all the videos with the selected audio format
                    IEnumerable<VideoInfo> validAudios = VideoInfo.Where(format => format.AudioExtension == SelectedAudioExtension)
                                                                  .OrderBy(bitrate => bitrate.AudioBitrate)
                                                                  .Distinct();

                    //Clear the current bitrates
                    AvailableAudioBitrates.Clear();

                    //Add the bitrates to the list
                    foreach (VideoInfo element in validAudios)
                    {
                        AvailableAudioBitrates.Add(element.AudioBitrate);
                    }

                    //Select the highest bitrate
                    SelectedAudioBitrate = AvailableAudioBitrates.Last();
                    break;
                
                //We need to reset the selectable audio bitrates, as may have been updated when SelectedVideoResolution changes
                case "SelectedDownloadType":
                    //Get all the videos with the selected audio format
                    validAudios = VideoInfo.Where(format => format.AudioExtension == SelectedAudioExtension)
                                                                  .OrderBy(bitrate => bitrate.AudioBitrate)
                                                                  .Distinct();

                    //Clear the current bitrates
                    AvailableAudioBitrates.Clear();

                    //Add the bitrates to the list
                    foreach (VideoInfo element in validAudios)
                    {
                        AvailableAudioBitrates.Add(element.AudioBitrate);
                    }

                    //Select the highest bitrate
                    SelectedAudioBitrate = AvailableAudioBitrates.Last();
                    break;

                //Change the available audio bitrates based on the resolution and format of the video selected
                case "SelectedVideoResolution":
                    //Get all the videos with the selected resolution
                    validAudios = VideoInfo.Where(format => format.Resolution == SelectedVideoResolution && format.VideoExtension == SelectedVideoExtension)
                                                                  .OrderBy(bitrate => bitrate.AudioBitrate)
                                                                  .Distinct();

                    //Clear the current bitrates
                    AvailableAudioBitrates.Clear();

                    //Add the bitrates to the list
                    foreach (VideoInfo element in validAudios)
                    {
                        AvailableAudioBitrates.Add(element.AudioBitrate);
                    }

                    //Select the highest bitrate
                    SelectedAudioBitrate = AvailableAudioBitrates.Last();
                    break;
            }
        }

        /// <summary>
        /// Invoked when <see cref="BtnDownload"/> is clicked
        /// Closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Invoked when a key is pressed within a <see cref="WndwInfoSelector"/> instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                SelectionCancelled = true;
                Close();
            }
        }
    }

    /// <summary>
    /// Converts between <see cref="DownloadType"/> and <see cref="Int32"/>
    /// </summary>
    public class DownloadTypeToInt32 : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="DownloadType"/> value to <see cref="Int32"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DownloadType)
            {
                return (Int32)value;
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an <see cref="Int32"/> value to <see cref="DownloadType"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Int32)
            {
                return (DownloadType)value;
            } else
            {
                return null;
            }
        }
    }
}
