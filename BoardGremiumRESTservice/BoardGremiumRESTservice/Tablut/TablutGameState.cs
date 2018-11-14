using BoardGremiumRESTservice.Tablut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Tablut
{
    public class TablutGameState
    {
        //private BoardState CurrentBoardState { get; }
        public Game game { get; }

        public TablutGameState(FieldType playerPawn)
        {
            game = new TablutGame(playerPawn);
        }

        public TablutGameState(FieldType playerPawn, BoardState bs)
        {
            game = new TablutGame(playerPawn);
            game.currentBoardState = bs;
        }

        public Boolean IsChosenMoveValid(TablutMove move, PlayerEnum currentPlayer)
        {

            Field field = game.currentBoardState.BoardFields[move.X,move.Y];
            FieldType currentFieldType;
            if(currentPlayer.Equals(PlayerEnum.HUMAN_PLAYER))
            {
                currentFieldType = (FieldType)game.HumanPlayerFieldType;// change HUmanPlayerType to CreatorFieldType ?? - so we can play bot-bot or human-human
            }else
            {
                currentFieldType = (FieldType)game.BotPlayerFieldType;
            }
            if (move.NumOfFields <= 0)
                return false;
            else if (move.NumOfFields > game.CalculateMaximumPossibleRange(game.currentBoardState, field, move.Direction))
            {
                return false;
            } 
            else if (!PawnsInSameTeam(currentFieldType, (FieldType)field.FieldType))
            {
                return false;
            }
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
                    if (!fields[i, j].FieldType.Equals(FieldType.EMPTY_FIELD))
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
        public Field GetMissingPawnForPlayer(BoardState oldBS, BoardState newBS, PlayerEnum player)
        {
            FieldType takenType;
            if (player.Equals(PlayerEnum.HUMAN_PLAYER))
            {
                takenType = (FieldType)game.HumanPlayerFieldType;
            }
            else
            {
                takenType = (FieldType)game.BotPlayerFieldType;
            }
            for (int i = 0; i < game.BoardHeight; i++)
            {
                for (int j = 0; j < game.BoardWidth; j++)
                {
                    if (oldBS.BoardFields[i, j].FieldType.Equals(takenType) && !newBS.BoardFields[i, j].FieldType.Equals(takenType))
                    {
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
        public bool PawnsInSameTeam(FieldType t1, FieldType t2)
        {
            if ((t1.Equals(t2)) ||
               (t1.Equals(FieldType.RED_PAWN) && t2.Equals(FieldType.KING)) ||
               (t1.Equals(FieldType.KING) && t2.Equals(FieldType.RED_PAWN)))
            {
                return true;
            }
            else return false;
        }

        public DirectionEnum GetDirectionFromMove(Field source, Field destination)
        {
            if (source.X > destination.X)
                return DirectionEnum.LEFT;
            if (source.X < destination.X)
                return DirectionEnum.RIGHT;
            if (source.Y > destination.Y)
                return DirectionEnum.UP;
            else return DirectionEnum.DOWN;
        }

        public int GetNumOfFieldsFromMove(Field source, Field destination, DirectionEnum direction)
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