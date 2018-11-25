using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumBotDecisions.Adugo
{
    public class AdugoBoardState : ICloneable
    {
        public AdugoField[,] BoardFields { get; }
        public int Height { get; }
        public int Width { get; }

        public AdugoBoardState(int width, int height)
        {
            BoardFields = new AdugoField[height, width];
            this.Height = height;
            this.Width = width;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    BoardFields[y, x] = new AdugoField(x, y, FieldType.EMPTY_FIELD, AdugoDirectionType.NONE);
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
                        return field.Y == 0 ? null : BoardFields[field.Y - 1, field.X];
                    }
                case DirectionEnum.DOWN:
                    {
                        return field.Y == Height - 1 ? null : BoardFields[field.Y + 1, field.X];
                    }
                case DirectionEnum.LEFT:
                    {
                        return field.X == 0 ? null : BoardFields[field.Y, field.X - 1];
                    }
                case DirectionEnum.RIGHT:
                    {
                        return field.X == Width - 1 ? null : BoardFields[field.Y, field.X + 1];
                    }
                case DirectionEnum.UPLEFT:
                    {
                        return (field.Y == 0) || (field.X == 0) ? null : BoardFields[field.Y - 1, field.X - 1];
                    }
                case DirectionEnum.UPRIGHT:
                    {
                        return (field.Y == 0) || (field.X == Width - 1) ? null : BoardFields[field.Y - 1, field.X + 1];
                    }
                case DirectionEnum.DOWNRIGHT:
                    {
                        return (field.Y == Height - 1) || (field.X == Width - 1) ? null : BoardFields[field.Y + 1, field.X + 1];
                    }
                case DirectionEnum.DOWNLEFT:
                    {
                        return (field.Y == Height - 1) || (field.X == 0) ? null : BoardFields[field.Y + 1, field.X - 1];
                    }
            }
            return null;
        }

        /// <summary>
        /// implementation of Deep copy of AdugoBoardState object
        /// </summary>
        public object Clone()
        {
            AdugoBoardState clone = new AdugoBoardState(Width, Height);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    clone.BoardFields[i, j] = (AdugoField)this.BoardFields[i, j].Clone();
                }
            }
            return clone;
        }

        public int NumberOfDogsOnBoard()
        {
            return BoardFields.Cast<AdugoField>().Count(field => field.Type.Equals(FieldType.DOG_PAWN));
        }

        public int NumOfEmptyFieldsNextToJaguar()
        {
            foreach (var field in BoardFields)
            {
                if (field.Type.Equals(FieldType.JAGUAR_PAWN))
                {
                    return this.GetNumOfEmptyFieldsNextToField(field);
                }
            }
            throw new ArgumentException("Jaguar needs to be on te Board");
        }

        public int GetNumOfEmptyFieldsNextToField(AdugoField field)
        {
            var directions = AdugoUtils.GetPossibleDirectionsFromDirectionType(field);

            var numOfEmptyFields = 0;
            foreach (var direction in directions)
            {
                var adjacentField = this.AdjecentField(field, direction);

                if (adjacentField.Type.Equals(FieldType.EMPTY_FIELD))
                {
                    numOfEmptyFields++;
                }
            }

            return numOfEmptyFields;
        }



    }
}
