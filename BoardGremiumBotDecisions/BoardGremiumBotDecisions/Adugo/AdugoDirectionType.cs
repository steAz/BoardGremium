using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions.Adugo
{
    public enum AdugoDirectionType
    {
        ALL_DIRECTIONS, 
        UP_UPLEFT_LEFT, 
        UP_UPRIGHT_RIGHT, 
        UP_LEFT_RIGHT, 
        UP_DOWN_LEFT_RIGHT, 
        UP_DOWN_LEFT,
        UP_DOWN_RIGHT,
        DOWN_DOWNLEFT_LEFT,
        DOWN_DOWNRIGHT_RIGHT,
        DOWN_LEFT_RIGHT,
        DOWN_UPLEFT_LEFT, 
        DOWN_UPRIGHT_RIGHT,
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
