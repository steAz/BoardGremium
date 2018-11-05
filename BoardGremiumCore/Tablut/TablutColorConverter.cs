﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using AbstractGame;

namespace BoardGremiumCore.Tablut
{
    [ValueConversion(typeof(FieldType), typeof(Brush))]
    public class TablutColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            FieldType type = (FieldType)value;

            switch (type)
            {
                case FieldType.BLACK_PAWN:
                    return new SolidColorBrush(Colors.Black);
                case FieldType.RED_PAWN:
                    return new SolidColorBrush(Colors.Red);
                case FieldType.KING:
                    return new SolidColorBrush(Colors.Cyan);
                case FieldType.EMPTY_FIELD:
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

