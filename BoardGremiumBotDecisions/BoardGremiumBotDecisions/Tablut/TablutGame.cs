using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions.Tablut
{
    public class TablutGame : Game
    {
        public static int BOARD_WIDTH = 9;
        public static int BOARD_HEIGHT = 9;
        public TablutGame(string boardPath, string whitePawnPath, string blackPawnPath, string kingPath, FieldType humanPawn)
            : base(BOARD_WIDTH, BOARD_HEIGHT, boardPath)
        {
            this.CurrentTablutBoardState = StartingPosition();
            //for now hardcoded white for player, black for bot
            this.HumanPlayerFieldType = humanPawn;
            if (humanPawn.Equals(FieldType.RED_PAWN))
                this.BotPlayerFieldType = FieldType.BLACK_PAWN;
            else
                this.BotPlayerFieldType = FieldType.RED_PAWN;

        }
        public override List<TablutBoardState> GetPossibleBoardStates(TablutBoardState initial, FieldType playerFieldType)
        {
            List<TablutBoardState> result = new List<TablutBoardState>();
            for (int i = 0; i < BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    if ((FieldType)initial.BoardFields[i, j].Type == playerFieldType)
                    {
                        result.AddRange(GetPossibleBoardStatesForPawn(initial, initial.BoardFields[i, j]));
                    }
                    if (playerFieldType == FieldType.RED_PAWN)
                    {
                        //we add moves of the king
                        if ((FieldType)initial.BoardFields[i, j].Type == FieldType.KING)
                        {
                            result.AddRange(GetPossibleBoardStatesForPawn(initial, initial.BoardFields[i, j]));
                        }
                    }
                }
            }
            return result;
        }

        private List<TablutBoardState> GetPossibleBoardStatesForPawn(TablutBoardState initial, Field pawn)
        {
            if ((FieldType)pawn.Type != FieldType.RED_PAWN &&
                (FieldType)pawn.Type != FieldType.BLACK_PAWN &&
                (FieldType)pawn.Type != FieldType.KING)
            {
                throw new ArgumentException("Field passed to GetPossibleBoardStatesForPawn is not pawn type");
            }
            Array directions = Enum.GetValues(typeof(DirectionEnum));
            List<TablutBoardState> result = new List<TablutBoardState>();

            foreach (var direction in directions)
            {
                result.AddRange(AllMovesInDirection(initial, pawn, (DirectionEnum)direction));
            }
            return result;
        }
        //TODO refactor these methods to new class TablutBoardState : TablutBoardState
        private List<TablutBoardState> AllMovesInDirection(TablutBoardState initialBS, Field pawn, DirectionEnum direction)
        {
            List<TablutBoardState> result = new List<TablutBoardState>();
            int maximumPossibleRange = CalculateMaximumPossibleRange(initialBS, pawn, direction);
            TablutBoardState currentTablutBoardState;
            Field currentField;
            for (int i = 1; i <= maximumPossibleRange; i++)
            {
                currentTablutBoardState = (TablutBoardState)initialBS.Clone();
                currentField = currentTablutBoardState.BoardFields[pawn.Y, pawn.X];
                MovePawn(currentTablutBoardState, currentField, direction, i);
                result.Add(currentTablutBoardState);
            }
            return result;
        }
        /// <summary>
        /// returns maximum number of fields a pawn can go in a direction
        /// </summary>
        public override int CalculateMaximumPossibleRange(TablutBoardState initial, Field pawn, DirectionEnum direction)
        {
            int result = 0;
            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        for (int i = 0; i < pawn.Y; i++)
                        {
                            if (((FieldType)initial.BoardFields[pawn.Y - i - 1, pawn.X].Type).Equals(FieldType.EMPTY_FIELD))
                            {
                                if (pawn.Type == FieldType.KING)
                                    result++;
                                else if (!IsCornerCoords(pawn.Y - i - 1, pawn.X))
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
                            if (((FieldType)initial.BoardFields[pawn.Y + i + 1, pawn.X].Type).Equals(FieldType.EMPTY_FIELD))
                            {
                                if (pawn.Type == FieldType.KING)
                                    result++;
                                else if (!IsCornerCoords(pawn.Y + i + 1, pawn.X))
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
                            if (((FieldType)initial.BoardFields[pawn.Y, pawn.X - i - 1].Type).Equals(FieldType.EMPTY_FIELD))
                            {
                                if (pawn.Type == FieldType.KING)
                                    result++;
                                else if (!IsCornerCoords(pawn.Y, pawn.X - i - 1))
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
                            if (((FieldType)initial.BoardFields[pawn.Y, pawn.X + i + 1].Type).Equals(FieldType.EMPTY_FIELD))
                            {
                                if (pawn.Type == FieldType.KING)
                                    result++;
                                else if (!IsCornerCoords(pawn.Y, pawn.X + i + 1))
                                    result++;
                            }
                            else break;
                        }
                        break;
                    }
            }
            return result;
        }

        private bool IsCornerCoords(int x, int y)
        {
            if ((x == 0 && y == 0)
                || (x == 0 && y == (BoardWidth - 1))
                || (x == (BoardHeight - 1) && y == 0)
                || (x == (BoardHeight - 1) && y == (BoardWidth - 1)))
            {
                return true;
            }
            else
                return false;
        }
        //TODO this method can be abstract in Game class
        // changes TablutBoardState inside of method
        public override void MovePawn(TablutBoardState tablutBoard, Field field, DirectionEnum direction, int numberOfFields)
        {
            int xCoord, yCoord;
            xCoord = field.X;
            yCoord = field.Y;
            FieldType type = FieldType.KING;
            if (((FieldType)field.Type).Equals(FieldType.BLACK_PAWN))
                type = FieldType.BLACK_PAWN;
            else if (((FieldType)field.Type).Equals(FieldType.RED_PAWN))
                type = FieldType.RED_PAWN;
            else if (((FieldType)field.Type).Equals(FieldType.KING))
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
                        if (yCoord > tablutBoard.Height - 1) yCoord = tablutBoard.Height - 1;
                        break;
                    }
                case DirectionEnum.RIGHT:
                    {
                        xCoord += numberOfFields;
                        if (xCoord > tablutBoard.Width - 1) xCoord = tablutBoard.Width - 1;
                        break;
                    }
                case DirectionEnum.LEFT:
                    {
                        xCoord -= numberOfFields;
                        if (xCoord < 0) xCoord = 0;
                        break;
                    }
            }
            tablutBoard.BoardFields[yCoord, xCoord].Type = type;
            field.Type = FieldType.EMPTY_FIELD;
            TakeEnemyPawns(tablutBoard, tablutBoard.BoardFields[yCoord, xCoord]);
        }
        /// <summary>
        /// method called after performing move on boardstate, then this method is taking enemy pawns off the tablutBoard
        ///if it's necessary
        /// </summary>
        private void TakeEnemyPawns(TablutBoardState bs, Field targetField)
        {
            TakePawnAtDirection(bs, targetField, DirectionEnum.UP);
            TakePawnAtDirection(bs, targetField, DirectionEnum.DOWN);
            TakePawnAtDirection(bs, targetField, DirectionEnum.LEFT);
            TakePawnAtDirection(bs, targetField, DirectionEnum.RIGHT);
        }
        /// <summary>
        /// Checks if pawn adjecent to our at direction has to be taken and eventually takes it
        /// </summary>
        private void TakePawnAtDirection(TablutBoardState bs, Field ourField, DirectionEnum direction)
        {
            FieldType enemyPawnType;
            if ((FieldType)ourField.Type == FieldType.BLACK_PAWN)
            {
                //just white pawn because taking off the king is checked in checking
                //end of the game, which is called before taking pawns off
                enemyPawnType = FieldType.RED_PAWN;
            }
            else
            {
                //King cannot capture
                if ((FieldType)ourField.Type == FieldType.KING)
                    return;
                else enemyPawnType = FieldType.BLACK_PAWN;
            }
            Field adjecentField = bs.AdjacentField(ourField, direction);
            Field nextField;
            if (adjecentField != null && (FieldType)adjecentField.Type == enemyPawnType)
            {
                nextField = bs.AdjacentField(adjecentField, direction);
                if (enemyPawnType == FieldType.RED_PAWN)
                {
                    if (nextField != null && (FieldType)nextField.Type == FieldType.BLACK_PAWN)
                    {
                        //take off if not a king
                        if ((FieldType)adjecentField.Type == FieldType.RED_PAWN)
                        {
                            adjecentField.Type = FieldType.EMPTY_FIELD;
                        }
                    }
                }
                else
                {
                    if (nextField != null && (FieldType)nextField.Type == FieldType.RED_PAWN)
                    {
                        adjecentField.Type = FieldType.EMPTY_FIELD;
                    }
                }
            }
        }

        public override bool IsGameWon(TablutBoardState bs, PlayerEnum forPlayer)
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
        private bool HasWhitePlayerWon(TablutBoardState bs)
        {
            Field[,] boardFields = bs.BoardFields;
            if ((FieldType)boardFields[0, 0].Type == FieldType.KING
                || (FieldType)boardFields[0, BoardWidth - 1].Type == FieldType.KING
                || (FieldType)boardFields[BoardHeight - 1, 0].Type == FieldType.KING
                || (FieldType)boardFields[BoardHeight - 1, BoardWidth - 1].Type == FieldType.KING)
            {
                return true;
            }
            else
                return false;
        }

        private bool HasBlackPlayerWon(TablutBoardState bs)
        {
            Field kingField = bs.BoardFields[4, 4];
            bool kingFound = false;
            for (int i = 0; i < bs.Height; i++)
            {
                for (int j = 0; j < bs.Width; j++)
                {
                    if ((FieldType)bs.BoardFields[i, j].Type == FieldType.KING)
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
            adjecentUp = bs.AdjacentField(kingField, DirectionEnum.UP);
            adjecentDown = bs.AdjacentField(kingField, DirectionEnum.DOWN);
            adjecentLeft = bs.AdjacentField(kingField, DirectionEnum.LEFT);
            adjecentRight = bs.AdjacentField(kingField, DirectionEnum.RIGHT);
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
            if ((FieldType)f.Type == FieldType.BLACK_PAWN) return true;
            return false;
        }
        public override TablutBoardState StartingPosition()
        {
            TablutBoardState startingTablutBoardState = new TablutBoardState(BoardWidth, BoardHeight);
            for (int i = 0; i < startingTablutBoardState.Height; i++)
            {
                for (int j = 0; j < startingTablutBoardState.Width; j++)
                {
                    //startingTablutBoardState.SetField(j, i, TablutFieldType.EMPTY_FIELD);
                    startingTablutBoardState.BoardFields[i, j] = new Field(j, i, FieldType.EMPTY_FIELD);
                }
            }
            //black pawns
            startingTablutBoardState.BoardFields[3, 0].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[4, 0].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[5, 0].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[4, 1].Type = FieldType.BLACK_PAWN;

            startingTablutBoardState.BoardFields[3, 8].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[4, 8].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[4, 7].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[5, 8].Type = FieldType.BLACK_PAWN;

            startingTablutBoardState.BoardFields[0, 3].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[0, 4].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[1, 4].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[0, 5].Type = FieldType.BLACK_PAWN;

            startingTablutBoardState.BoardFields[8, 3].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[8, 4].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[7, 4].Type = FieldType.BLACK_PAWN;
            startingTablutBoardState.BoardFields[8, 5].Type = FieldType.BLACK_PAWN;
            //white pawns

            startingTablutBoardState.BoardFields[4, 2].Type = FieldType.RED_PAWN;
            startingTablutBoardState.BoardFields[4, 3].Type = FieldType.RED_PAWN;
            startingTablutBoardState.BoardFields[4, 5].Type = FieldType.RED_PAWN;
            startingTablutBoardState.BoardFields[4, 6].Type = FieldType.RED_PAWN;

            startingTablutBoardState.BoardFields[2, 4].Type = FieldType.RED_PAWN;
            startingTablutBoardState.BoardFields[3, 4].Type = FieldType.RED_PAWN;
            startingTablutBoardState.BoardFields[5, 4].Type = FieldType.RED_PAWN;
            startingTablutBoardState.BoardFields[6, 4].Type = FieldType.RED_PAWN;
            //king
            startingTablutBoardState.BoardFields[4, 4].Type = FieldType.KING;
            return startingTablutBoardState;
        }
    }
}
