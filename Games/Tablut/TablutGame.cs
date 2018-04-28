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
        public static int BOARD_WIDTH = 9;
        public static int BOARD_HEIGHT = 9;
        public TablutGame(string boardPath, string whitePawnPath, string blackPawnPath, string kingPath) 
            :base(BOARD_WIDTH, BOARD_HEIGHT, boardPath)
        {
            ItemToGraphicsDict.Add(TablutFieldType.WHITE_PAWN, whitePawnPath);
            ItemToGraphicsDict.Add(TablutFieldType.BLACK_PAWN, blackPawnPath);
            ItemToGraphicsDict.Add(TablutFieldType.KING, kingPath);
            ItemToGraphicsDict.Add(TablutFieldType.EMPTY_FIELD, null);
            this.currentBoardState = StartingPosition();

        }
        public override List<BoardState> GetPossibleBoardStates(BoardState initial, PlayerEnum playerType)
        {
            TablutFieldType pawnType;
            //for now we assume player playes white pawns, and bot plays black
            if (playerType == PlayerEnum.BOT_PLAYER)
                pawnType = TablutFieldType.BLACK_PAWN;
            else 
                pawnType = TablutFieldType.WHITE_PAWN;
            List<BoardState> result = new List<BoardState>();
            for (int i = 0; i < BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    if ((TablutFieldType)initial.BoardFields[i, j].Type == pawnType)
                    {
                        result.AddRange(GetPossibleBoardStatesForPawn(initial, initial.BoardFields[i, j]));
                    }
                    if(pawnType == TablutFieldType.WHITE_PAWN)
                    {
                        //we add moves of the king
                        if ((TablutFieldType)initial.BoardFields[i, j].Type == TablutFieldType.KING)
                        {
                            result.AddRange(GetPossibleBoardStatesForPawn(initial, initial.BoardFields[i, j]));
                        }
                    }
                }
            }
            return result;
        }

        private List<BoardState> GetPossibleBoardStatesForPawn(BoardState initial, Field pawn)
        {
            if((TablutFieldType)pawn.Type != TablutFieldType.WHITE_PAWN && 
                (TablutFieldType)pawn.Type != TablutFieldType.BLACK_PAWN &&
                (TablutFieldType)pawn.Type != TablutFieldType.KING)
            {
                throw new ArgumentException("Field passed to GetPossibleBoardStatesForPawn is not pawn type");
            }
            Array directions = Enum.GetValues(typeof(DirectionEnum));
            List<BoardState> result = new List<BoardState>();
            BoardState initialCopy = (BoardState)initial.Clone();
            
            foreach(var direction in directions)
            {
                result.AddRange(AllMovesInDirection(initial, pawn, (DirectionEnum)direction));
            }
            return result;
        }
        //TODO refactor these methods to new class TablutBoardState : BoardState
        private List<BoardState> AllMovesInDirection(BoardState initialBS, Field pawn, DirectionEnum direction)
        {
            List<BoardState> result = new List<BoardState>();
            int currentX, currentY;
            currentX = pawn.X;
            currentY = pawn.Y;
            int maximumPossibleRange = calculateMaximumPossibleRange(initialBS, pawn, direction);
            BoardState currentBoardState;
            Field currentField;
            for(int i = 1; i <= maximumPossibleRange; i++)
            {
                currentBoardState = (BoardState)initialBS.Clone();
                currentField = currentBoardState.BoardFields[pawn.Y, pawn.X];
                movePawn(currentBoardState, currentField, direction, i);
                result.Add(currentBoardState);
            }
            return result;
        }
        /// <summary>
        /// returns maximum number of fields a pawn can go in a direction
        /// </summary>
        private int calculateMaximumPossibleRange(BoardState initial, Field pawn, DirectionEnum direction)
        {
            int result = 0;
            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        for(int i=0; i < pawn.Y; i++)
                        {
                            if ((TablutFieldType)initial.BoardFields[pawn.X, pawn.Y - i - 1].Type == TablutFieldType.EMPTY_FIELD)
                            {
                                result++;
                            }
                            else break;
                        }
                        break;
                    }
                case DirectionEnum.DOWN:
                    {
                        for(int i=0; i < BOARD_HEIGHT - 1 - pawn.Y; i++)
                        {
                            if((TablutFieldType)initial.BoardFields[pawn.X, pawn.Y + i + 1].Type == TablutFieldType.EMPTY_FIELD)
                            {
                                result++;
                            }
                            else break;
                        }
                        break;
                    }
                case DirectionEnum.LEFT:
                    {
                        for(int i=0; i < pawn.X; i++)
                        {
                            if ((TablutFieldType)initial.BoardFields[pawn.X - i - 1, pawn.Y].Type == TablutFieldType.EMPTY_FIELD)
                            {
                                result++;
                            }
                            else break;
                        }
                        break;
                    }
                case DirectionEnum.RIGHT:
                    {
                        for(int i=0; i < BOARD_WIDTH - 1 - pawn.X; i++)
                        {
                            if ((TablutFieldType)initial.BoardFields[pawn.X + i + 1, pawn.Y].Type == TablutFieldType.EMPTY_FIELD)
                            {
                                result++;
                            }
                            else break;
                        }
                        break;
                    }
            }
            return result;
        }
        //TODO this method can be abstract in Game class
        // changes BoardState inside of method
        public void movePawn(BoardState board, Field field, DirectionEnum direction, int numberOfFields)
        {
            int xCoord, yCoord;
            xCoord = field.X;
            yCoord = field.Y;
            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        yCoord -= numberOfFields;
                        if (yCoord < 0) yCoord = 0;
                        break;
                    }
                case DirectionEnum.DOWN:
                    {
                        yCoord += numberOfFields;
                        if (yCoord > board.Height - 1) yCoord = board.Height - 1;
                        break;
                    }
                case DirectionEnum.RIGHT:
                    {
                        xCoord += numberOfFields;
                        if (xCoord > board.Width - 1) xCoord = board.Width - 1;
                        break;
                    }
                case DirectionEnum.LEFT:
                    {
                        xCoord -= numberOfFields;
                        if (xCoord < 0) xCoord = 0;
                        break;
                    }
            }
            board.BoardFields[yCoord, xCoord].Type = field.Type;
            field.Type = TablutFieldType.EMPTY_FIELD;
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
                startingBoardState.BoardFields[4, 7].Type = TablutFieldType.BLACK_PAWN;
                startingBoardState.BoardFields[5, 8].Type = TablutFieldType.BLACK_PAWN;

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
