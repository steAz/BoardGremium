﻿using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoardGremiumCore.Adugo
{ 
    class AdugoField : Field
    {
        public int X { get; set; }
        public int Y { get; set; }

        public AdugoDirectionType DirectionType
        {
            get { return (AdugoDirectionType)GetValue(DirectionTypeProperty); }
            set { SetValue(DirectionTypeProperty, value); }
        }

        public AdugoField(int x, int y, FieldType type, AdugoDirectionType directionType)
        {
            this.X = x;
            this.Y = y;
            this.Type = type;
            this.DirectionType = directionType;
        }
    }
}