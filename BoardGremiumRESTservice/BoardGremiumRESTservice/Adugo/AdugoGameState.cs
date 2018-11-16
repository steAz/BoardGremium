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

        public bool IsChosenMoveValid(AdugoMove move, PlayerEnum currentPlayer, out AdugoField fieldToBeat, out AdugoField fieldToMove)
        {
            fieldToBeat = null;
            fieldToMove = null;
            FieldType currentFieldType;
            if (currentPlayer.Equals(PlayerEnum.HUMAN_PLAYER))
            {
                currentFieldType = (FieldType)Game.HumanPlayerFieldType;
            }
            else
            {
                currentFieldType = (FieldType)Game.BotPlayerFieldType;
            }

            if (!move.ChosenField.DirectionType.ToString().Contains(move.Direction.ToString()) &&
                !move.ChosenField.DirectionType.Equals(AdugoDirectionType.ALL_DIRECTIONS)) // if it's not possible to move in this direction from this place
            {
                return false;
            }

            var helpfulField = Game.CurrentBoardState.AdjecentField(move.ChosenField, move.Direction);
            if (helpfulField == null || !helpfulField.Type.Equals(FieldType.EMPTY_FIELD))
            {
                return false;
            }
            switch (currentFieldType)
            {
                case FieldType.JAGUAR_PAWN when helpfulField.Type.Equals(FieldType.DOG_PAWN):
                {
                    var adjacentToHelpfulField = Game.CurrentBoardState.AdjecentField(fieldToMove, move.Direction);
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
    }
}