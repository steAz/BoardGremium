using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice
{
    public class TablutGameState
    {
        //private BoardState CurrentBoardState { get; }
        public Game game { get; }

        public TablutGameState(TablutFieldType playerPawn)
        {
            game = new TablutGame("", "", "", "", playerPawn);
        }

        public TablutGameState(TablutFieldType playerPawn, BoardState bs)
        {
            game = new TablutGame("", "", "", "", playerPawn);
            game.currentBoardState = bs;
        }

        public Boolean IsChosenMoveValid(int numOfFields, DirectionEnum direction, Field field)
        {
            //Field field = game.currentBoardState.BoardFields[yCoord,xCoord];
            if (numOfFields <= 0)
                return false;
            else if (numOfFields > game.CalculateMaximumPossibleRange(game.currentBoardState, field, direction))
                return false;
            else if (((TablutFieldType)game.HumanPlayerFieldType == TablutFieldType.BLACK_PAWN && (TablutFieldType)field.Type != TablutFieldType.BLACK_PAWN)
                || ((TablutFieldType)game.HumanPlayerFieldType == TablutFieldType.RED_PAWN && (TablutFieldType)field.Type == TablutFieldType.BLACK_PAWN))
                return false;

            return true;
        }

        public int NumberOfPawnsOnBS(BoardState bs)
        {
            int result = 0;
            Field[,] fields = bs.BoardFields;
            for (int i = 0; i < game.BoardHeight; i++)
            {
                for (int j = 0; j < game.BoardWidth; j++)
                {
                    if (!fields[i, j].Type.Equals(TablutFieldType.EMPTY_FIELD))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// returns field on which there was pawn taken off for player
        /// </summary>
        public Field getMissingPawnForPlayer(BoardState oldBS, BoardState newBS, PlayerEnum player)
        {
            TablutFieldType takenType;
            if (player.Equals(PlayerEnum.HUMAN_PLAYER))
            {
                takenType = (TablutFieldType)game.HumanPlayerFieldType;
            }
            else
            {
                takenType = (TablutFieldType)game.BotPlayerFieldType;
            }
            for (int i = 0; i < game.BoardHeight; i++)
            {
                for (int j = 0; j < game.BoardWidth; j++)
                {
                    if (oldBS.BoardFields[i, j].Type.Equals(takenType) && !newBS.BoardFields[i, j].Type.Equals(takenType))
                    {
                        Console.WriteLine("Returns field coords: " + i + ";" + j);
                        return newBS.BoardFields[i, j];
                    }
                }
            }
            return null;
        }

        public Field GetFieldForCoords(int x, int y)
        {
            return game.currentBoardState.BoardFields[y, x];
        }

        /// <summary>
        /// checks if 2 pawns are in the same team, e.g white pawn and king -> true white pawn and black pawn -> false
        /// </summary>
        public bool pawnsInSameTeam(TablutFieldType t1, TablutFieldType t2)
        {
            if ((t1.Equals(t2)) ||
               (t1.Equals(TablutFieldType.RED_PAWN) && t2.Equals(TablutFieldType.KING)) ||
               (t1.Equals(TablutFieldType.KING) && t2.Equals(TablutFieldType.RED_PAWN)))
            {
                return true;
            }
            else return false;
        }

        public DirectionEnum getDirectionFromMove(Field source, Field destination)
        {
            if (source.X > destination.X)
                return DirectionEnum.LEFT;
            if (source.X < destination.X)
                return DirectionEnum.RIGHT;
            if (source.Y > destination.Y)
                return DirectionEnum.UP;
            else return DirectionEnum.DOWN;
        }

        public int getNumOfFieldsFromMove(Field source, Field destination, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        return source.Y - destination.Y;
                    }
                case DirectionEnum.DOWN:
                    {
                        return destination.Y - source.Y;
                    }
                case DirectionEnum.LEFT:
                    {
                        return source.X - destination.X;
                    }
                case DirectionEnum.RIGHT:
                    {
                        return destination.X - source.X;
                    }
            }
            //won't happen
            return 0;
        }
    }
}