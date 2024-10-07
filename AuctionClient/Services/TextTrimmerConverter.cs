using System.Globalization;
using System.Windows.Data;

namespace AuctionClient.Services
{
    public class TextTrimmerConverter : IValueConverter
    {
        public int MaxLength { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text && text.Length > MaxLength)
            {
                return text.Substring(0, MaxLength).Trim() + "...";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
