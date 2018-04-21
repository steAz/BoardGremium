using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    public class BoardState
    {
        public Enum[][] BoardObjects { get;}

        public BoardState()
        {

        }

        public void SetField(int x, int y, Enum fieldState)
        {
            BoardObjects[y][x] = fieldState;
        }
    }
}
