using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumCore.Tablut
{
    public class TablutBoardState : ICloneable
    {
        public List<List<Field>> BoardFields { get; set; }
        public int Height { get; }
        public int Width { get; }

        public TablutBoardState(int width, int height)
        {
            BoardFields = new List<List<Field>>();
            this.Height = height;
            this.Width = width;

            for (int i = 0; i != this.Width; ++i)
            {
                BoardFields.Add(new List<Field>());

                for (int j = 0; j != this.Height; ++j)
                {
                    BoardFields[i].Add(new Field(j, i));
                }
            }
        }

        /// <summary>
        /// returns adjecent Field object or null if there is no adjecent field in that direction
        /// </summary>
        /// <returns>adjecent field</returns>
        public Field AdjecentField(Field field, DirectionEnum direction)
        {
            switch(direction)
            {
                case DirectionEnum.UP:
                    {
                        if (field.Y == 0)
                            return null;
                        else return BoardFields[field.Y - 1][field.X];
                    }
                case DirectionEnum.DOWN:
                    {
                        if (field.Y == Height - 1)
                            return null;
                        else return BoardFields[field.Y + 1][field.X];
                    }
                case DirectionEnum.LEFT:
                    {
                        if (field.X == 0)
                            return null;
                        else return BoardFields[field.Y][field.X - 1];
                    }
                case DirectionEnum.RIGHT:
                    {
                        if (field.X == Width - 1)
                            return null;
                        else return BoardFields[field.Y][field.X + 1];
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
            for(int i=0; i < Width; i++)
            {
                for(int j=0; j < Height; j++)
                {
                    clone.BoardFields[i][j] = (Field)this.BoardFields[i][j].Clone();
                }
            }
            return clone;
        }

        
    }
}
