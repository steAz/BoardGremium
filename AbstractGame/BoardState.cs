using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    public class BoardState : ICloneable
    {
        public Field[,] BoardFields { get; }
        public int Height { get; }
        public int Width { get; }

        public BoardState(int width, int height)
        {
            BoardFields = new Field[height,width];
            this.Height = height;
            this.Width = width;
        }

        /// <summary>
        /// implementation of Deep copy of BoardState object
        /// </summary>
        public object Clone()
        {
            BoardState clone = new BoardState(Width, Height);
            for(int i=0; i < Height; i++)
            {
                for(int j=0; j < Width; j++)
                {
                    clone.BoardFields[i, j] = (Field)this.BoardFields[i, j].Clone();
                }
            }
            return clone;
        }

        
    }
}
