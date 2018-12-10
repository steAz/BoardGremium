using AbstractGame;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace BoardGremiumBotDecisions.Adugo
{
    public class AdugoGame
    {
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public AdugoBoardState CurrentBoardState { get; set; }
        public PlayerEnum CurrentPlayer { get; set; }
        public FieldType HumanPlayerFieldType { get; set; } //e.g dogPawn/jaguarPawn
        public FieldType BotPlayerFieldType { get; set; }


        public AdugoGame(FieldType humanPawn, int boardWidth, int boardHeight)
        {
            this.BoardHeight = boardHeight;
            this.BoardWidth = boardWidth;
            this.CurrentBoardState = StartingPosition();
            this.HumanPlayerFieldType = humanPawn;
            if (humanPawn.Equals(FieldType.JAGUAR_PAWN))
                this.BotPlayerFieldType = FieldType.DOG_PAWN;
            else if (humanPawn.Equals(FieldType.DOG_PAWN))
                this.BotPlayerFieldType = FieldType.JAGUAR_PAWN;

        }

        public AdugoBoardState StartingPosition()
        {
            var startingBoardState = new AdugoBoardState(BoardWidth, BoardHeight);
            for (var y = 0; y < startingBoardState.Height; y++)
            {
                for (var x = 0; x < startingBoardState.Width; x++)
                {
                    startingBoardState.BoardFields[y, x] = new AdugoField(x, y, FieldType.EMPTY_FIELD, AdugoDirectionType.ALL_DIRECTIONS);
                }
            }
            ////locked pawns
            startingBoardState.BoardFields[5, 0].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[5, 0].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[6, 0].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[6, 0].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[5, 4].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[5, 4].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[6, 4].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[6, 4].DirectionType = AdugoDirectionType.NONE;

            ////jaguar pawn
            startingBoardState.BoardFields[2, 2].Type = FieldType.JAGUAR_PAWN;
            startingBoardState.BoardFields[2, 2].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;

            ////dog pawns
            startingBoardState.BoardFields[0, 0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 0].DirectionType = AdugoDirectionType.DOWN_DOWNRIGHT_RIGHT;
            startingBoardState.BoardFields[1, 0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 0].DirectionType = AdugoDirectionType.UP_DOWN_RIGHT;

            startingBoardState.BoardFields[2, 0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 0].DirectionType = AdugoDirectionType.UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT;
            startingBoardState.BoardFields[0, 1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 1].DirectionType = AdugoDirectionType.DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[1, 1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 1].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[2, 1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 1].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0, 2].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 2].DirectionType = AdugoDirectionType.DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT;
            startingBoardState.BoardFields[1, 2].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0, 3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 3].DirectionType = AdugoDirectionType.DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[1, 3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 3].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[2, 3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 3].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0, 4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 4].DirectionType = AdugoDirectionType.DOWN_DOWNLEFT_LEFT;
            startingBoardState.BoardFields[1, 4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 4].DirectionType = AdugoDirectionType.UP_DOWN_LEFT;
            startingBoardState.BoardFields[2, 4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 4].DirectionType = AdugoDirectionType.UP_DOWN_UPLEFT_DOWNLEFT_LEFT;


            //empty pawns
            startingBoardState.BoardFields[3, 0].DirectionType = AdugoDirectionType.UP_DOWN_RIGHT;
            startingBoardState.BoardFields[4, 0].DirectionType = AdugoDirectionType.UP_UPRIGHT_RIGHT;
            startingBoardState.BoardFields[3, 1].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[4, 1].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[3, 2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[4, 2].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[3, 3].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[4, 3].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[3, 4].DirectionType = AdugoDirectionType.UP_DOWN_LEFT;
            startingBoardState.BoardFields[4, 4].DirectionType = AdugoDirectionType.UP_UPLEFT_LEFT;
            startingBoardState.BoardFields[5, 2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[6, 2].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[5, 1].DirectionType = AdugoDirectionType.UPRIGHT_RIGHT;
            startingBoardState.BoardFields[5, 3].DirectionType = AdugoDirectionType.UPLEFT_LEFT;
            startingBoardState.BoardFields[6, 1].DirectionType = AdugoDirectionType.UP_RIGHT;
            startingBoardState.BoardFields[6, 3].DirectionType = AdugoDirectionType.UP_LEFT;
            return startingBoardState;
        }

        public void MovePawnAndBeatIfNecessary(AdugoBoardState board, Field field, DirectionEnum direction, AdugoField fieldToBeat, AdugoField fieldToMove)
        {
            var xCoordToMove = fieldToMove.X;
            var yCoordToMove = fieldToMove.Y;
            board.BoardFields[yCoordToMove, xCoordToMove].Type = field.Type; // pawn is on new field
            field.Type = FieldType.EMPTY_FIELD; // old field is empty
            if (fieldToBeat != null) // it wont go through it if ChosenFieldType is Dog
            {
                var xCoordToBeat = fieldToBeat.X;
                var yCoordToBeat = fieldToBeat.Y;
                board.BoardFields[yCoordToBeat, xCoordToBeat].Type = FieldType.EMPTY_FIELD; // beaten dog pawn is an empty field now
            }
        }

        public List<AdugoBoardState> GetPossibleAdugoBoardStates(AdugoBoardState initial, FieldType playerFieldType)
        {
            var result = new List<AdugoBoardState>();
            for (var y = 0; y < BoardHeight; y++)
            {
                for (var x = 0; x < BoardWidth; x++)
                {
                    if (initial.BoardFields[y, x].Type == playerFieldType)
                    {
                        result.AddRange(GetPossibleAdugoBoardStatesForPawn(initial, initial.BoardFields[y, x]));
                    }
                }
            }

            return result;
        }

        private List<AdugoBoardState> GetPossibleAdugoBoardStatesForPawn(AdugoBoardState initial, AdugoField pawn)
        {
            if ((FieldType)pawn.Type != FieldType.JAGUAR_PAWN &&
                (FieldType)pawn.Type != FieldType.DOG_PAWN)
            {
                throw new ArgumentException("Field passed to GetPossibleAdugoBoardStatesForPawn is not pawn type");
            }
            var directions = Enum.GetValues(typeof(DirectionEnum));
            var result = new List<AdugoBoardState>();

            foreach (var direction in directions)
            {
                var oneResult = MoveInOneDirection(initial, pawn, (DirectionEnum) direction);
                if (oneResult != null)
                {
                    result.Add(oneResult);
                }
            }

            return result;
        }
        
        private AdugoBoardState MoveInOneDirection(AdugoBoardState initialBS, AdugoField pawn, DirectionEnum direction)
        {
            AdugoBoardState currentBoardState = null;
            if (IsChosenMoveValid(initialBS, pawn, direction, out var fieldToBeat, out var fieldToMove))
            {
                currentBoardState = (AdugoBoardState)initialBS.Clone();
                var currentField = currentBoardState.BoardFields[pawn.Y, pawn.X];
                MovePawnAndBeatIfNecessary(currentBoardState, currentField, direction, fieldToBeat, fieldToMove);
            }

            return currentBoardState;
        }
        
        private static bool IsChosenMoveValid(AdugoBoardState currentBoardState, AdugoField chosenField, DirectionEnum direction, out AdugoField fieldToBeat, out AdugoField fieldToMove)
        {

            fieldToBeat = null;
            fieldToMove = null;

            if (!chosenField.DirectionType.Equals(AdugoDirectionType.ALL_DIRECTIONS)) // if it's not possible to move in this direction from this place
            {
                var directionParams = chosenField.DirectionType.ToString().Split('_');
                if (!directionParams.Contains(direction.ToString()))
                {
                    return false;
                }
            }

            var helpfulField = currentBoardState.AdjecentField(chosenField, direction);
            if (helpfulField == null) return false;

            switch (chosenField.Type)
            {
                case FieldType.JAGUAR_PAWN when helpfulField.Type.Equals(FieldType.DOG_PAWN):
                    {
                        var directionsFromHelpfulField =
                            AdugoUtils.GetPossibleDirectionsFromDirectionType(helpfulField);
                        if (!directionsFromHelpfulField.Contains(direction))
                        {
                            return false;
                        }
                        var adjacentToHelpfulField = currentBoardState.AdjecentField(helpfulField, direction);
                        if (adjacentToHelpfulField == null || !adjacentToHelpfulField.Type.Equals(FieldType.EMPTY_FIELD))
                            return false;
                        fieldToBeat = helpfulField; // Jaguar will beat dog
                        fieldToMove = adjacentToHelpfulField; // Jaguar will go to empty field after dog
                        return true;

                    }
                case FieldType.DOG_PAWN when (helpfulField.Type.Equals(FieldType.JAGUAR_PAWN) ||
                                              helpfulField.Type.Equals(FieldType.DOG_PAWN) ||
                                              helpfulField.Type.Equals(FieldType.LOCKED_FIELD)):
                    return false; // DOG cannot move on JAGUAR or another DOG
                default:
                    {
                        if (helpfulField.Type.Equals(FieldType.LOCKED_FIELD))
                        {
                            return false;
                        }

                        break;
                    }
            }

            fieldToMove = helpfulField;
            return true;
        }

        public bool IsGameWon(AdugoBoardState bs, PlayerEnum forPlayer)
        {
            if (forPlayer == PlayerEnum.HUMAN_PLAYER)
            {
                if ((FieldType)HumanPlayerFieldType == FieldType.JAGUAR_PAWN)
                    return HasJaguarPlayerWon(bs);
                return HasDogPlayerWon(bs);
            }
            else
            {
                if ((FieldType)BotPlayerFieldType == FieldType.JAGUAR_PAWN)
                    return HasJaguarPlayerWon(bs);
                return HasDogPlayerWon(bs);
            }
        }
        private bool HasJaguarPlayerWon(AdugoBoardState bs)
        {
            var numOfDogsOnBoard = bs.NumberOfDogsOnBoard();

            return numOfDogsOnBoard <= AdugoUtils.INITIAL_DOG_PAWNS_NUMBER - 5;
        }

        private bool HasDogPlayerWon(AdugoBoardState bs)
        {
            var jaguarField = bs.BoardFields[4, 4];
            bool jaguarFound = false;
            for (int y = 0; y < bs.Height; y++)
            {
                for (int x = 0; x < bs.Width; x++)
                {
                    if ((FieldType)bs.BoardFields[y, x].Type == FieldType.JAGUAR_PAWN)
                    {
                        jaguarField = bs.BoardFields[y, x];
                        jaguarFound = true;
                        break;
                    }
                }
                if (jaguarFound)
                    break;
            }

            var numOfEmptyFieldsNextToJag = bs.NumOfEmptyFieldsNextToJaguar();
            return numOfEmptyFieldsNextToJag == 0;
        }
    }
}
