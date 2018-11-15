using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Adugo
{
    public enum AdugoDirectionType
    {
        ALL_DIRECTIONS, //black
        UP_UPLEFT_LEFT, //brown
        UP_UPRIGHT_RIGHT, // brown
        UP_LEFT_RIGHT, //blue
        UP_DOWN_LEFT_RIGHT, //purple
        UP_DOWN_LEFT,
        UP_DOWN_RIGHT,
        DOWN_DOWNLEFT_LEFT,
        DOWN_DOWNRIGHT_RIGHT,
        DOWN_LEFT_RIGHT,
        DOWN_UPLEFT_LEFT, //green
        DOWN_UPRIGHT_RIGHT, //green
        DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT,
        UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT,
        UP_DOWN_UPLEFT_DOWNLEFT_LEFT,
        UP_LEFT,
        UP_RIGHT,
        UPRIGHT_RIGHT,
        UPLEFT_LEFT,
        NONE
    }
}
