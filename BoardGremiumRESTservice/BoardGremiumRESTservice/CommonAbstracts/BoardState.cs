using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice
{
    public class BoardState : ICloneable
    {
        public Field[,] BoardFields { get; }
        public int Height { get; }
        public int Width { get; }

        public BoardState(int width, int height)
        {
            BoardFields = new Field[height, width];
            this.Height = height;
            this.Width = width;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    BoardFields[j,i] = new Field(j, i, TablutFieldType.EMPTY_FIELD);
                }
            }
        }
        /// <summary>
        /// returns adjecent Field object or null if there is no adjecent field in that direction
        /// </summary>
        /// <returns>adjecent field</returns>
        public Field AdjecentField(Field field, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        if (field.Y == 0)
                            return null;
                        else return BoardFields[field.X, field.Y - 1];
                    }
                case DirectionEnum.DOWN:
                    {
                        if (field.Y == Height - 1)
                            return null;
                        else return BoardFields[field.X, field.Y + 1];
                    }
                case DirectionEnum.LEFT:
                    {
                        if (field.X == 0)
                            return null;
                        else return BoardFields[field.X - 1, field.Y];
                    }
                case DirectionEnum.RIGHT:
                    {
                        if (field.X == Width - 1)
                            return null;
                        else return BoardFields[field.X + 1, field.Y];
                    }
            }
            return null;
        }

        /// <summary>
        /// implementation of Deep copy of BoardState object
        /// </summary>
        public object Clone()
        {
            BoardState clone = new BoardState(Width, Height);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    clone.BoardFields[i, j] = (Field)this.BoardFields[i, j].Clone();
                }
            }
            return clone;
        }


    }
}