using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using AbstractGame;

namespace BoardGremiumCore.Tablut
{
    [ValueConversion(typeof(Enum), typeof(Brush))]
    public class TablutColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            TablutFieldType type = (TablutFieldType)value;

            switch (type)
            {
                case TablutFieldType.BLACK_PAWN:
                    return new SolidColorBrush(Colors.Black);
                case TablutFieldType.RED_PAWN:
                    return new SolidColorBrush(Colors.Red);
                case TablutFieldType.KING:
                    return new SolidColorBrush(Colors.Cyan);
                case TablutFieldType.EMPTY_FIELD:
                    return new SolidColorBrush(Colors.LightYellow);
            }

            throw new Exception("fail");
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

