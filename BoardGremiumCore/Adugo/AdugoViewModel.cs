using AbstractGame;
using BoardGremiumCore.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Adugo
{
    public class AdugoViewModel
    {
        public BoardState MyBoardState { get; set; }
        public GameInfos GameInfos { get; set; }

        public List<List<Field>> MyBoardFields
        {
            get
            {
                return MyBoardState.BoardFields;
            }
        }

        public AdugoViewModel() { MyBoardState = new BoardState(AdugoUtils.BOARD_WIDTH, AdugoUtils.BOARD_HEIGHT); }

        public AdugoViewModel(BoardState bs)
        {
            MyBoardState = bs;
        }
    }
}
