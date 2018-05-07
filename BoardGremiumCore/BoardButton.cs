using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AbstractGame;

namespace BoardGremiumCore
{
    public class BoardButton : Button
    {
        public Field RepresentedField { get; set; }

       public BoardButton() : base()
        {

        }
    }
}
