using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Adugo
{
    public class AdugoBoardState : ICloneable
    {
        public List<List<AdugoField>> BoardFields { get; set; }
        public int Height { get; }
        public int Width { get; }

        public AdugoBoardState(int width, int height)
        {
            BoardFields = new List<List<AdugoField>>();
            this.Height = 7;
            this.Width = 5;

            for (int y = 0; y != this.Height; ++y)
            {
                BoardFields.Add(new List<AdugoField>());

                for (int x = 0; x != this.Width; ++x)
                {
                    BoardFields[y].Add(new AdugoField(x, y));
                }
            }
        }
        /// <summary>
        /// returns adjecent Field object or null if there is no adjecent field in that direction
        /// </summary>
        /// <returns>adjecent field</returns>
        public AdugoField AdjecentField(AdugoField field, DirectionEnum direction)
        {
            switch (direction)
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
        /// implementation of Deep copy of BoardState object
        /// </summary>
        public object Clone()
        {
            AdugoBoardState clone = new AdugoBoardState(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    clone.BoardFields[i][j] = (AdugoField)this.BoardFields[i][j].Clone();
                }
            }
            return clone;
        }


    }
}
