using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using System.Drawing;

namespace Games
{
    public class TablutGame : Game
    {
        public TablutGame(string boardPath, string whitePawnPath, string blackPawnPath, string kingPath) :base(9,9,boardPath)
        {
            ItemToGraphicsDict.Add(TablutFieldType.WHITE_PAWN, whitePawnPath);
            ItemToGraphicsDict.Add(TablutFieldType.BLACK_PAWN, blackPawnPath);
            ItemToGraphicsDict.Add(TablutFieldType.KING, kingPath);
            ItemToGraphicsDict.Add(TablutFieldType.EMPTY_FIELD, null);

        }
        public override List<BoardState> GetPossibleBoardStates(BoardState initial)
        {
            throw new NotImplementedException();
        }

        public override bool IsGameOver()
        {
            throw new NotImplementedException();
        }

        public override BoardState StartingPosition()
        {
            throw new NotImplementedException();
        }
    }
}
