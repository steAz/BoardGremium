using AbstractGame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BoardGremiumCore.Adugo
{
    [ValueConversion(typeof(FieldType), typeof(Brush))]
    public class AdugoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            FieldType type = (FieldType)value;

            switch (type)
            {
                case FieldType.LOCKED_FIELD:
                    return new SolidColorBrush(Colors.White);
                case FieldType.DOG_PAWN:
                    return new SolidColorBrush(Colors.Red);
                case FieldType.JAGUAR_PAWN:
                    return new SolidColorBrush(Colors.Orange);
                case FieldType.EMPTY_FIELD:
                    return new SolidColorBrush(Colors.Black);
            }

            throw new Exception("fail to convert adugo field type to color");
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
