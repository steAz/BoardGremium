using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Tablut
{
    public class TablutMove
    {
        public int X { get; }
        public int Y { get; }
        public int NumOfFields { get; }
        public DirectionEnum Direction { get; }
        public Field ChosenField { get; }

        public TablutMove(int x, int y, DirectionEnum direction, int numOfFields, TablutGameState gameState)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
            this.NumOfFields = numOfFields;
            this.ChosenField = gameState.game.currentBoardState.BoardFields[X, Y];
        }
    }
}