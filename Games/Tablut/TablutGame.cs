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

        private List<BoardState> GetPossibleBoardStatesWhitePlayer(BoardState initial)
        {
            throw new NotImplementedException();
        }

        private List<BoardState> GetPossibleBoardStatesBlackPlayer(BoardState initial)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// returns list of possible boardstates got by moving the white pawn
        /// </summary>
        /// <param name="initial"> boardstate from which to return possible moves</param>
        /// <returns> list of possible boardstates got by moving the white pawn</returns>
        private List<BoardState> GetPossibleBoardStatesWhitePawn(BoardState initial)
        {
            throw new NotImplementedException();
        }
        private List<BoardState> GetPossibleBoardStatesBlackPawn(BoardState initial)
        {
            throw new NotImplementedException();
        }

        private bool hasWhitePlayerWon(BoardState currentBs)
        {
            throw new NotImplementedException();
        }

        private bool hasBlackPlayerWon(BoardState currentBs)
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
            for (int i = 0; i < startingBoardState.Height; i++)
            {
                for (int j = 0; j < startingBoardState.Width; j++)
                {
                    //startingBoardState.SetField(j, i, TablutFieldType.EMPTY_FIELD);
                    startingBoardState.BoardFields[i, j] = new Field(j, i, TablutFieldType.EMPTY_FIELD);
                }
            }
                //black pawns
                startingBoardState.BoardFields[3,0].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[4, 0].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[5, 0].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[4, 1].Type = TablutFieldType.BLACK_PAWN;

                startingBoardState.BoardFields[3, 8].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[4, 8].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[5, 7].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[4, 8].Type = TablutFieldType.BLACK_PAWN;

                startingBoardState.BoardFields[0, 3].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[0, 4].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[1, 4].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[0, 5].Type = TablutFieldType.BLACK_PAWN;

                startingBoardState.BoardFields[8, 3].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[8, 4].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[7, 4].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[8, 5].Type = TablutFieldType.BLACK_PAWN;
                //white pawns

                startingBoardState.BoardFields[4, 2].Type = TablutFieldType.WHITE_PAWN;
                startingBoardState.BoardFields[4, 3].Type = TablutFieldType.WHITE_PAWN;
                startingBoardState.BoardFields[4, 5].Type = TablutFieldType.WHITE_PAWN;
                startingBoardState.BoardFields[4, 6].Type = TablutFieldType.WHITE_PAWN;

                startingBoardState.BoardFields[2, 4].Type = TablutFieldType.WHITE_PAWN;
                startingBoardState.BoardFields[3, 4].Type = TablutFieldType.WHITE_PAWN;
                startingBoardState.BoardFields[5, 4].Type = TablutFieldType.WHITE_PAWN;
                startingBoardState.BoardFields[6, 4].Type = TablutFieldType.WHITE_PAWN;
                //king
                startingBoardState.BoardFields[4, 4].Type = TablutFieldType.KING;
                return startingBoardState;
        }
    }
}
