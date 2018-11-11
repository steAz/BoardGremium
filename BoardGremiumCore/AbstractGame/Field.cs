using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BoardGremiumCore.Adugo;

namespace AbstractGame
{
    /// <summary>
    /// class representing single field on board
    /// </summary>
    public class Field :  DependencyObject, ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public FieldType Type
        {
            get { return (FieldType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
                    DependencyProperty.Register("Type", typeof(FieldType), typeof(Field), null);

        public static readonly DependencyProperty DirectionTypeProperty =
            DependencyProperty.Register("DirectionType", typeof(AdugoDirectionType), typeof(Field), null);

        public Field()
        {
            X = 0;
            Y = 0;
            Type = FieldType.EMPTY_FIELD;
        }

        public Field(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Type = FieldType.EMPTY_FIELD;
        }

        public Field(int x, int y, FieldType type)
        {
            this.X = x;
            this.Y = y;
            this.Type = type;
        }

        //TODO throwing exception when value is below 0? (out of board)
        public void AddToX(int value)
        {
            X += value;
        }

        public void AddToY(int valueToAdd)
        {
            Y += valueToAdd;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
