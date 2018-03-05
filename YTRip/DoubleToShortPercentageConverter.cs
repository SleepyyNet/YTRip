using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace YTRip
{
    class DoubleToShortPercentageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a double to a string representing the double to 0dp
        /// </summary>
        /// <param name="value">The input value</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">Culture info</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return Math.Round((double)value, 0).ToString();
            } else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
