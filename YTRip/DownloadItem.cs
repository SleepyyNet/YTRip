using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YTRip
{
    /// <summary>
    /// The base class for a downloadable item
    /// </summary>
    public abstract class DownloadItem : INotifyPropertyChanged
    {
        #region DType

        private DownloadType _dType;

        public DownloadType DType
        {
            get { return _dType; }
            set
            {
                _dType = value;
                RaisePropertyChanged("DType");
            }
        }

        #endregion

        #region ItemName

        private string _itemName;

        /// <summary>
        /// The name of the downloading item
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                RaisePropertyChanged("ItemName");
            }
        }

        #endregion

        #region DownloadProgress

        private string _downloadProgress;

        /// <summary>
        /// The progress of the download
        /// </summary>
        public string DownloadProgress
        {
            get { return _downloadProgress; }
            set
            {
                _downloadProgress = value;
                RaisePropertyChanged("DownloadProgress");
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

        /// <summary>
        /// Sanitises a string in preparation for turning it into a filename
        /// </summary>
        /// <param name="fileName">The string to sanitise</param>
        /// <returns>The sanitised string</returns>
        public static string PathCleaner(string fileName)
        {
            return String.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        }

        /// <summary>
        /// Converts a double to a string representing the double to 0dp
        /// </summary>
        /// <param name="value">The <see cref="double"/> value to return as a short percentage</param>
        /// <returns></returns>
        public string DoubleToShortPercentage(double value)
        {
            return Math.Round(value, 0).ToString();
        }
    }
}
