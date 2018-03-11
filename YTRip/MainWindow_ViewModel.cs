using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTRip
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        #region ExtractionUrl

        private string _extractionUrl = "https://www.youtube.com";

        /// <summary>
        /// The entered extraction URL
        /// </summary>
        public string ExtractionUrl
        {
            get { return _extractionUrl; }
            set { _extractionUrl = value; }
        }

        #endregion

        #region IsExtractionButtonEnabled

        private bool _isExtractionButtonEnabled;

        /// <summary>
        /// Is the extract button enabled?
        /// </summary>
        public bool IsExtractionButtonEnabled
        {
            get { return _isExtractionButtonEnabled; }
            set
            {
                _isExtractionButtonEnabled = value;
                RaisePropertyChanged("IsExtractionButtonEnabled");
            }
        }

        #endregion

        #region ExtractButtonContent

        private object _extractButtonContent = DOWNLOAD_TEXT_QUERY;

        /// <summary>
        /// The content of the extraction button
        /// </summary>
        public object ExtractButtonContent
        {
            get { return _extractButtonContent; }
            set
            {
                _extractButtonContent = value;
                RaisePropertyChanged("ExtractButtonContent");
            }
        }

        #endregion

        #region DownloadingItems

        private ObservableCollection<DownloadItem> _downloadingItems { get; set; } = new ObservableCollection<DownloadItem>();

        /// <summary>
        /// The items currently downloading
        /// </summary>
        public ObservableCollection<DownloadItem> DownloadingItems
        {
            get { return _downloadingItems; }
            set
            {
                _downloadingItems = value;
                RaisePropertyChanged("DownloadingItems");
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify that a property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Disables the extraction button and sets its content to the passed object
        /// </summary>
        /// <param name="content">The new button content</param>
        private void DisableExtractionButton(object content)
        {
            IsExtractionButtonEnabled = false;
            ExtractButtonContent = content;
        }

        /// <summary>
        /// Enables the extraction button and sets its content to the passed object
        /// </summary>
        /// <param name="disabledText">The new button content</param>
        private void EnableExtractionButton(object content)
        {
            IsExtractionButtonEnabled = false;
            ExtractButtonContent = content;
        }
    }
}
