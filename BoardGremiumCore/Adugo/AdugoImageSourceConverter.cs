using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static BoardGremiumCore.Adugo.AdugoDirectionType;

namespace BoardGremiumCore.Adugo
{
    [ValueConversion(typeof(AdugoDirectionType), typeof(ImageSource))]
    public class AdugoImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            AdugoDirectionType type = (AdugoDirectionType)value;
            switch (type)
            {
                case ALL_DIRECTIONS:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/all.png", UriKind.Relative));
                case UP_UPLEFT_LEFT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_upleft_left.png", UriKind.Relative));
                case UP_UPRIGHT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_upright_right.png", UriKind.Relative));
                case UP_LEFT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_left_right.png", UriKind.Relative));
                case UP_DOWN_LEFT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_down_left_right.png", UriKind.Relative));
                case UP_DOWN_LEFT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_down_left.png", UriKind.Relative));
                case UP_DOWN_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_down_right.png", UriKind.Relative));
                case DOWN_DOWNLEFT_LEFT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/down_downleft_left.png", UriKind.Relative));
                case DOWN_DOWNRIGHT_RIGHT:
                    return new BitmapImage(new Uri(@".../../Adugo/directionImages/down_downright_right.png", UriKind.Relative));
                case DOWN_LEFT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/down_left_right.png", UriKind.Relative));
                case DOWN_UPLEFT_LEFT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/down_upleft_left.png", UriKind.Relative));
                case DOWN_UPRIGHT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/down_upright_right.png", UriKind.Relative));
                case DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/down_downleft_downright_left_right.png", UriKind.Relative));
                case UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_down_upright_downright_right.png", UriKind.Relative));
                case UP_DOWN_UPLEFT_DOWNLEFT_LEFT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_down_upleft_downleft_left.png", UriKind.Relative));
                case UP_LEFT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_left.png", UriKind.Relative));
                case UP_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/up_right.png", UriKind.Relative));
                case UPLEFT_LEFT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/upleft_left.png", UriKind.Relative));
                case UPRIGHT_RIGHT:
                    return new BitmapImage(new Uri(@"../../Adugo/directionImages/upright_right.png", UriKind.Relative));
                default:
                    return null;
            }

            throw new Exception("fail to convert adugo direction type to image source");
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
