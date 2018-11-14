using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice
{
    /// <summary>
    /// class representing single field on board
    /// </summary>
    public class Field : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Enum FieldType { get; set; }

        public Field()
        {
            X = 0;
            Y = 0;
            FieldType = null;
        }

        public Field(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.FieldType = null;
        }

        public Field(int x, int y, Enum type)
        {
            this.X = x;
            this.Y = y;
            this.FieldType = type;
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