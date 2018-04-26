using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    public class BoardState
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
    }
}
