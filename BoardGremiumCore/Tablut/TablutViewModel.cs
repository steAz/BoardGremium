using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumCore.Tablut
{
    public class TablutViewModel
    {
        public BoardState MyBoardState { get; set; }
        public TablutFieldType PlayerPawn { get; set; }
        public string GameName { get; set; }

        public TablutViewModel() { MyBoardState = new BoardState(TablutUtils.BOARD_WIDTH, TablutUtils.BOARD_HEIGHT);  }

        public TablutViewModel(BoardState bs, TablutFieldType playerPawn, string gameName)
        {
            MyBoardState = bs;
            this.PlayerPawn = playerPawn;
            this.GameName = gameName;
        }

        public List<List<Field>> MyBoardFields
        {
            get
            {
                return MyBoardState.BoardFields;
            }
        }

        //returns true if game is over
        public void Clicked(Field field)
        {
            if (TablutUtils.IsTargetSameType(PlayerPawn, (TablutFieldType)field.Type))
            {
                MoveWindow mw = new MoveWindow();
                mw.ShowDialog();
            }
        }

        private void MakePlayerMove(Field selectedField, DirectionEnum selectedDirection, int selectedNumOfFields)
        {
            var message = "move p " + selectedField.X.ToString() + " " + selectedField.Y.ToString()
                            + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString();
            //client.SendPostMove(message);

            //server callback message
            bool isRightMove;
            //ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);
            //bot move message
            //if (isRightMove) ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);

            //MainGrid.Children.Clear();
            //DisplayBoard();
        }
    }
}
