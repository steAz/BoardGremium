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
            this.currentBoardState = StartingPosition();

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
            BoardState startingBoardState = new BoardState(BoardWidth,BoardHeight);
            for(int i=0; i < startingBoardState.Height; i++)
            {
                for(int j=0; j < startingBoardState.Width; j++)
                {
                    startingBoardState.SetField(j, i, TablutFieldType.EMPTY_FIELD);
                }
                //black pawns
                startingBoardState.SetField(3, 0, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(4, 0, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(5, 0, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(4, 1, TablutFieldType.BLACK_PAWN);

                startingBoardState.SetField(3, 8, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(4, 8, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(5, 8, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(4, 7, TablutFieldType.BLACK_PAWN);

                startingBoardState.SetField(0, 3, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(0, 4, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(1, 4, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(0, 5, TablutFieldType.BLACK_PAWN);

                startingBoardState.SetField(8, 3, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(8, 4, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(7, 4, TablutFieldType.BLACK_PAWN);
                startingBoardState.SetField(8, 5, TablutFieldType.BLACK_PAWN);

                //white pawns
                startingBoardState.SetField(4, 2, TablutFieldType.WHITE_PAWN);
                startingBoardState.SetField(4, 3, TablutFieldType.WHITE_PAWN);
                startingBoardState.SetField(4, 5, TablutFieldType.WHITE_PAWN);
                startingBoardState.SetField(4, 6, TablutFieldType.WHITE_PAWN);

                startingBoardState.SetField(2, 4, TablutFieldType.WHITE_PAWN);
                startingBoardState.SetField(3, 4, TablutFieldType.WHITE_PAWN);
                startingBoardState.SetField(5, 4, TablutFieldType.WHITE_PAWN);
                startingBoardState.SetField(6, 4, TablutFieldType.WHITE_PAWN);
                //king
                startingBoardState.SetField(4, 4, TablutFieldType.KING);
            }
            return startingBoardState;
        }
    }
}
