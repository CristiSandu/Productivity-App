using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Productivity_App.Converters
{
    public class ColorBreakConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var startTime = (DateTime)value;
                DateTime endTime = startTime.AddMinutes(10);
               
                if (DateTime.Compare(DateTime.Now, startTime) >= 0 && DateTime.Compare(DateTime.Now, endTime) < 0)
                {
                    Application.Current.Resources.TryGetValue("MagicMint", out var outColor);
                    return outColor;
                }else if  (DateTime.Compare(DateTime.Now, startTime) < 0)
                {
                    Application.Current.Resources.TryGetValue("BabyBlue", out var outColor);
                    return outColor;
                } else
                {
                    Application.Current.Resources.TryGetValue("JazzberryJam", out var outColor);
                    return outColor;
                }
            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
