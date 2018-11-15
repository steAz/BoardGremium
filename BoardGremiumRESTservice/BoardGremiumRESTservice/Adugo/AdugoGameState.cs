using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoardGremiumRESTservice.Utils;

namespace BoardGremiumRESTservice.Adugo
{
    public class AdugoGameState
    {
        public static int BoardWidth = 5;
        public static int BoardHeight = 7;
        public AdugoGame Game { get; }

        public AdugoGameState(FieldType playerPawn)
        {
            Game = new AdugoGame(playerPawn, BoardWidth, BoardHeight);
        }

        public AdugoGameState(FieldType playerPawn, AdugoBoardState bs)
        {
            Game = new AdugoGame(playerPawn, BoardWidth, BoardHeight)
            {
                CurrentBoardState = bs
            };
        }

        public bool IsChosenMoveValid(AdugoMove move, PlayerEnum currentPlayer)
        {
            FieldType currentFieldType;
            if (currentPlayer.Equals(PlayerEnum.HUMAN_PLAYER))
            {
                currentFieldType = (FieldType)Game.HumanPlayerFieldType;
            }
            else
            {
                currentFieldType = (FieldType)Game.BotPlayerFieldType;
            }

            if (!move.ChosenField.DirectionType.ToString().Contains(move.Direction.ToString())) // isf it's not possible to move in this direction from this place
            {
                return false;
            }


            var fieldToMove = Game.CurrentBoardState.AdjecentField(move.ChosenField, move.Direction);
            switch (currentFieldType)
            {
                case FieldType.JAGUAR_PAWN when fieldToMove.Type.Equals(FieldType.DOG_PAWN):
                {
                    var adjacentFieldToMove = Game.CurrentBoardState.AdjecentField(fieldToMove, move.Direction);
                    return adjacentFieldToMove.Type.Equals(FieldType.EMPTY_FIELD); // return true if after dog pawn there will be empty field (jaguar will eat dog)
                }
                case FieldType.DOG_PAWN when (fieldToMove.Type.Equals(FieldType.JAGUAR_PAWN) || fieldToMove.Type.Equals(FieldType.DOG_PAWN)):
                    return false; // DOG cannot move on JAGUAR or another DOG
                default:
                {
                    if (fieldToMove.Type.Equals(FieldType.LOCKED_FIELD))
                    {
                        return false;
                    }

                    break;
                }
            }

            return true;
        }
    }
}