using BoardGremiumBotDecisions;
using BoardGremiumBotDecisions.Tablut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumBotDecisions.Tablut
{
    public class TablutBoardState : ICloneable
    {
        public Field[,] BoardFields { get; }
        public int Height { get; }
        public int Width { get; }

        public TablutBoardState(int width, int height)
        {
            BoardFields = new Field[height,width];
            this.Height = height;
            this.Width = width;

            for (int i = 0; i != this.Width; ++i)
            {
                for (int j = 0; j != this.Height; ++j)
                {
                    BoardFields[i, j] = new Field(j, i, FieldType.EMPTY_FIELD);
                }
            }
        }

        public int NumberOfPawnsForPlayer(FieldType forPlayer)
        {
            int result = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (!BoardFields[i, j].Type.Equals(FieldType.EMPTY_FIELD))
                    {
                        if(TablutUtils.PawnsInSameTeam(BoardFields[i, j].Type, forPlayer))
                        {
                            if(BoardFields[i, j].Type != FieldType.KING) // counting without king to heuristic
                                result++;
                        }
                    }
                }
            }
            return result;
        }

        public Field GetKingTablutField()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (BoardFields[i, j].Type.Equals(FieldType.KING))
                    {
                        return BoardFields[i, j];
                    }
                }
            }

            return null;
        }

        public int ClosestCornerDistanceFromKing()
        {
            Field kingField = GetKingTablutField();

            int upLeftDistance = DistanceToUpperLeftCorner(kingField);
            int upRightDistance = DistanceToUpperRightCorner(kingField);
            int downLeftDistance = DistanceToDownLeftCorner(kingField);
            int downRightDistance = DistanceToDownRightCorner(kingField);

            int minimum = upLeftDistance;
            if (upRightDistance < minimum)
                minimum = upRightDistance;
            if (downLeftDistance < minimum)
                minimum = downLeftDistance;
            if (downRightDistance < minimum)
                minimum = downRightDistance;

            return minimum;

        }

        private int DistanceToUpperLeftCorner(Field kingField)
        {

            return kingField.X + kingField.Y;
        }

        private int DistanceToUpperRightCorner(Field kingField)
        {
            return (Width - kingField.X - 1) + kingField.Y;
        }

        private int DistanceToDownLeftCorner(Field kingField)
        {
            return kingField.X + (Height - kingField.Y - 1);
        }

        private int DistanceToDownRightCorner(Field kingField)
        {
            return (Width - kingField.X - 1) + (Height - kingField.Y - 1);
        }

        /// <summary>
        /// returns adjecent Field object or null if there is no adjecent field in that direction
        /// </summary>
        /// <returns>adjecent field</returns>
        public Field AdjacentField(Field field, DirectionEnum direction)
        {
            switch(direction)
            {
                case DirectionEnum.UP:
                    {
                        if (field.Y == 0)
                            return null;
                        else return BoardFields[field.Y - 1, field.X];
                    }
                case DirectionEnum.DOWN:
                    {
                        if (field.Y == Height - 1)
                            return null;
                        else return BoardFields[field.Y + 1, field.X];
                    }
                case DirectionEnum.LEFT:
                    {
                        if (field.X == 0)
                            return null;
                        else return BoardFields[field.Y, field.X - 1];
                    }
                case DirectionEnum.RIGHT:
                    {
                        if (field.X == Width - 1)
                            return null;
                        else return BoardFields[field.Y, field.X + 1];
                    }
            }
            return null;
        }

        /// <summary>
        /// implementation of Deep copy of TablutBoardState object
        /// </summary>
        public object Clone()
        {
            TablutBoardState clone = new TablutBoardState(Width, Height);
            for(int i=0; i < Height; i++)
            {
                for(int j=0; j < Width; j++)
                {
                    clone.BoardFields[i, j] = (Field)this.BoardFields[i, j].Clone();
                }
            }
            return clone;
        }

        
    }
}
