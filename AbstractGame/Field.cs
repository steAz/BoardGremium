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
        public Enum Type { get; set; }

        public Field()
        {
            X = 0;
            Y = 0;
            Type = null;
        }

        public Field(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Type = null;
        }

        public Field(int x, int y, Enum type)
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
