using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    public class BoardState
    {
        public Enum[,] BoardObjects { get;}
        public int Height { get; }
        public int Width { get; }

        public BoardState(int width, int height)
        {
            BoardObjects = new Enum[height,width];
            this.Height = height;
            this.Width = width;
        }

        public void SetField(int x, int y, Enum fieldState)
        {
            BoardObjects[y,x] = fieldState;
        }
    }
}
