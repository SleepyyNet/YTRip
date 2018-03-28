using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace YTRip
{
    class StringToDoubleConverter : IValueConverter
    {
        /// <summary>
        /// Converts a string to a double
        /// </summary>
        /// <param name="value">The input value</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">Culture info</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                double convertedValue;
                //Try to convert the string
                double.TryParse(value as string, out convertedValue);

                return convertedValue;
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
