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
    public class DownloadItem : INotifyPropertyChanged
    {
        public DownloadItem()
        {

        }

        public DownloadItem(DownloadType type, string name)
        {
            DType = type;
            ItemName = name;
        }

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

        private double _downloadProgress = 0;

        public double DownloadProgress
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
    }
}
