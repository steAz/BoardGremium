using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Tablut
{
    public class TablutGame : Game
    {
        public static int BOARD_WIDTH = 9;
        public static int BOARD_HEIGHT = 9;
        public TablutGame(FieldType humanPawn)
            : base(BOARD_WIDTH, BOARD_HEIGHT)
        {
            this.currentBoardState = StartingPosition();
            //for now hardcoded white for player, black for bot
            this.HumanPlayerFieldType = humanPawn;
            if (humanPawn.Equals(FieldType.RED_PAWN))
                this.BotPlayerFieldType = FieldType.BLACK_PAWN;
            else
                this.BotPlayerFieldType = FieldType.RED_PAWN;

        }
        public override List<BoardState> GetPossibleBoardStates(BoardState initial, PlayerEnum playerType)
        {
            FieldType pawnType;
            //for now we assume player playes white pawns, and bot plays black
            if (playerType == PlayerEnum.BOT_PLAYER)
                pawnType = (FieldType)BotPlayerFieldType;
            else
                pawnType = (FieldType)HumanPlayerFieldType;
            List<BoardState> result = new List<BoardState>();
            for (int i = 0; i < BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    if ((FieldType)initial.BoardFields[i, j].FieldType == pawnType)
                    {
                        result.AddRange(GetPossibleBoardStatesForPawn(initial, initial.BoardFields[i, j]));
                    }
                    if (pawnType == FieldType.RED_PAWN)
                    {
                        //we add moves of the king
                        if ((FieldType)initial.BoardFields[i, j].FieldType == FieldType.KING)
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
            if ((FieldType)pawn.FieldType != FieldType.RED_PAWN &&
                (FieldType)pawn.FieldType != FieldType.BLACK_PAWN &&
                (FieldType)pawn.FieldType != FieldType.KING)
            {
                throw new ArgumentException("Field passed to GetPossibleBoardStatesForPawn is not pawn type");
            }
            Array directions = Enum.GetValues(typeof(DirectionEnum));
            List<BoardState> result = new List<BoardState>();
            BoardState initialCopy = (BoardState)initial.Clone();

            foreach (var direction in directions)
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
            int maximumPossibleRange = CalculateMaximumPossibleRange(initialBS, pawn, direction);
            BoardState currentBoardState;
            Field currentField;
            for (int i = 1; i <= maximumPossibleRange; i++)
            {
                currentBoardState = (BoardState)initialBS.Clone();
                currentField = currentBoardState.BoardFields[pawn.Y, pawn.X];
                MovePawn(currentBoardState, currentField, direction, i);
                result.Add(currentBoardState);
            }
            return result;
        }
        /// <summary>
        /// returns maximum number of fields a pawn can go in a direction
        /// </summary>
        public override int CalculateMaximumPossibleRange(BoardState initial, Field pawn, DirectionEnum direction)
        {
            int result = 0;
            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        for (int i = 0; i < pawn.Y; i++)
                        {
                            if (((FieldType)initial.BoardFields[pawn.X, pawn.Y - i - 1].FieldType).Equals(FieldType.EMPTY_FIELD))
                            {
                                result++;
                            }
                            else break;
                        }
                        break;
                    }
                case DirectionEnum.DOWN:
                    {
                        for (int i = 0; i < BOARD_HEIGHT - 1 - pawn.Y; i++)
                        {
                            if (((FieldType)initial.BoardFields[pawn.X, pawn.Y + i + 1].FieldType).Equals(FieldType.EMPTY_FIELD))
                            {
                                result++;
                            }
                            else break;
                        }
                        break;
                    }
                case DirectionEnum.LEFT:
                    {
                        for (int i = 0; i < pawn.X; i++)
                        {
                            if (((FieldType)initial.BoardFields[pawn.X - i - 1, pawn.Y].FieldType).Equals(FieldType.EMPTY_FIELD))
                            {
                                result++;
                            }
                            else break;
                        }
                        break;
                    }
                case DirectionEnum.RIGHT:
                    {
                        for (int i = 0; i < BOARD_WIDTH - 1 - pawn.X; i++)
                        {
                            if (((FieldType)initial.BoardFields[pawn.X + i + 1, pawn.Y].FieldType).Equals(FieldType.EMPTY_FIELD))
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
        public override void MovePawn(BoardState board, Field field, DirectionEnum direction, int numberOfFields)
        {
            int xCoord, yCoord;
            xCoord = field.X;
            yCoord = field.Y;
            FieldType type = FieldType.KING;
            if (((FieldType)field.FieldType).Equals(FieldType.BLACK_PAWN))
                type = FieldType.BLACK_PAWN;
            else if (((FieldType)field.FieldType).Equals(FieldType.RED_PAWN))
                type = FieldType.RED_PAWN;
            else if (((FieldType)field.FieldType).Equals(FieldType.KING))
                type = FieldType.KING;
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
            board.BoardFields[xCoord, yCoord].FieldType = type;
            field.FieldType = FieldType.EMPTY_FIELD;
            TakeEnemyPawns(board, board.BoardFields[xCoord, yCoord]);
        }
        /// <summary>
        /// method called after performing move on boardstate, then this method is taking enemy pawns off the board
        ///if it's necessary
        /// </summary>
        private void TakeEnemyPawns(BoardState bs, Field targetField)
        {
            TakePawnAtDirection(bs, targetField, DirectionEnum.UP);
            TakePawnAtDirection(bs, targetField, DirectionEnum.DOWN);
            TakePawnAtDirection(bs, targetField, DirectionEnum.LEFT);
            TakePawnAtDirection(bs, targetField, DirectionEnum.RIGHT);
        }
        /// <summary>
        /// Checks if pawn adjecent to our at direction has to be taken and eventually takes it
        /// </summary>
        private void TakePawnAtDirection(BoardState bs, Field ourField, DirectionEnum direction)
        {
            FieldType enemyPawnType;
            if ((FieldType)ourField.FieldType == FieldType.BLACK_PAWN)
            {
                //just white pawn because taking off the king is checked in checking
                //end of the game, which is called before taking pawns off
                enemyPawnType = FieldType.RED_PAWN;
            }
            else
            {
                //King cannot capture
                if ((FieldType)ourField.FieldType == FieldType.KING)
                    return;
                else enemyPawnType = FieldType.BLACK_PAWN;
            }
            Field adjecentField = bs.AdjecentField(ourField, direction);
            Field nextField;
            if (adjecentField != null && (FieldType)adjecentField.FieldType == enemyPawnType)
            {
                nextField = bs.AdjecentField(adjecentField, direction);
                if (enemyPawnType == FieldType.RED_PAWN)
                {
                    if (nextField != null && (FieldType)nextField.FieldType == FieldType.BLACK_PAWN)
                    {
                        //take off if not a king
                        if ((FieldType)adjecentField.FieldType == FieldType.RED_PAWN)
                        {
                            adjecentField.FieldType = FieldType.EMPTY_FIELD;
                        }
                    }
                }
                else
                {
                    if (nextField != null && (FieldType)nextField.FieldType == FieldType.RED_PAWN)
                    {
                        adjecentField.FieldType = FieldType.EMPTY_FIELD;
                    }
                }
            }
        }

        public override bool IsGameWon(BoardState bs, PlayerEnum forPlayer)
        {
            if (forPlayer == PlayerEnum.HUMAN_PLAYER)
            {
                if ((FieldType)HumanPlayerFieldType == FieldType.RED_PAWN)
                    return HasWhitePlayerWon(bs);
                else return HasBlackPlayerWon(bs);
            }
            else
            {
                if ((FieldType)BotPlayerFieldType == FieldType.RED_PAWN)
                    return HasWhitePlayerWon(bs);
                else return HasBlackPlayerWon(bs);
            }
        }
        private bool HasWhitePlayerWon(BoardState bs)
        {
            Field[,] boardFields = bs.BoardFields;
            if ((FieldType)boardFields[0, 0].FieldType == FieldType.KING
                || (FieldType)boardFields[0, BoardWidth - 1].FieldType == FieldType.KING
                || (FieldType)boardFields[BoardHeight - 1, 0].FieldType == FieldType.KING
                || (FieldType)boardFields[BoardHeight - 1, BoardWidth - 1].FieldType == FieldType.KING)
            {
                return true;
            }
            else
                return false;
        }

        private bool HasBlackPlayerWon(BoardState bs)
        {
            Field kingField = bs.BoardFields[4, 4];
            bool kingFound = false;
            for (int i = 0; i < bs.Height; i++)
            {
                for (int j = 0; j < bs.Width; j++)
                {
                    if ((FieldType)bs.BoardFields[i, j].FieldType == FieldType.KING)
                    {
                        kingField = bs.BoardFields[i, j];
                        kingFound = true;
                        break;
                    }
                }
                if (kingFound)
                    break;
            }
            Field adjecentUp, adjecentDown, adjecentRight, adjecentLeft;
            adjecentUp = bs.AdjecentField(kingField, DirectionEnum.UP);
            adjecentDown = bs.AdjecentField(kingField, DirectionEnum.DOWN);
            adjecentLeft = bs.AdjecentField(kingField, DirectionEnum.LEFT);
            adjecentRight = bs.AdjecentField(kingField, DirectionEnum.RIGHT);
            if ((adjecentUp == null || isFieldStoppingKing(adjecentUp))
                && (adjecentDown == null || isFieldStoppingKing(adjecentDown))
                && (adjecentRight == null || isFieldStoppingKing(adjecentRight))
                && (adjecentLeft == null || isFieldStoppingKing(adjecentLeft)))
            {
                return true;
            }
            return false;
        }
        //this is to refactor to TablutField class inheriting from Field class
        private bool isFieldStoppingKing(Field f)
        {
            //throne field
            if (f.X == BoardWidth / 2 && f.Y == BoardHeight / 2) return true;
            //black pawn
            if ((FieldType)f.FieldType == FieldType.BLACK_PAWN) return true;
            return false;
        }
        public override BoardState StartingPosition()
        {
            BoardState startingBoardState = new BoardState(BoardWidth, BoardHeight);
            for (int i = 0; i < startingBoardState.Height; i++)
            {
                for (int j = 0; j < startingBoardState.Width; j++)
                {
                    //startingBoardState.SetField(j, i, TablutFieldType.EMPTY_FIELD);
                    startingBoardState.BoardFields[i, j] = new Field(j, i, FieldType.EMPTY_FIELD);
                }
            }
            //black pawns
            startingBoardState.BoardFields[3, 0].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 0].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[5, 0].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 1].FieldType = FieldType.BLACK_PAWN;

            startingBoardState.BoardFields[3, 8].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 8].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 7].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[5, 8].FieldType = FieldType.BLACK_PAWN;

            startingBoardState.BoardFields[0, 3].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[0, 4].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[1, 4].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[0, 5].FieldType = FieldType.BLACK_PAWN;

            startingBoardState.BoardFields[8, 3].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[8, 4].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[7, 4].FieldType = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[8, 5].FieldType = FieldType.BLACK_PAWN;
            //white pawns

            startingBoardState.BoardFields[4, 2].FieldType = FieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 3].FieldType = FieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 5].FieldType = FieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 6].FieldType = FieldType.RED_PAWN;

            startingBoardState.BoardFields[2, 4].FieldType = FieldType.RED_PAWN;
            startingBoardState.BoardFields[3, 4].FieldType = FieldType.RED_PAWN;
            startingBoardState.BoardFields[5, 4].FieldType = FieldType.RED_PAWN;
            startingBoardState.BoardFields[6, 4].FieldType = FieldType.RED_PAWN;
            //king
            startingBoardState.BoardFields[4, 4].FieldType = FieldType.KING;
            return startingBoardState;
        }
    }
}