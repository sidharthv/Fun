using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CalvinHobbes.Common
{
    public class BoolInverterConverter : IValueConverter
    {
        /// <summary>
        /// Inverts a boolean
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? boolValue = (bool?)value;

            // Return the inverted value of the boolean
            return (boolValue.HasValue && boolValue.Value) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
