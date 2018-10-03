using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    /// <summary>
    /// class representing single field on board
    /// </summary>
    public class Field : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TablutFieldType Type { get; set; }

        public Field()
        {
            X = 0;
            Y = 0;
            Type = TablutFieldType.EMPTY_FIELD;
        }

        public Field(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Type = TablutFieldType.EMPTY_FIELD;
        }

        public Field(int x, int y, TablutFieldType type)
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
