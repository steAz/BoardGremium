using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AbstractGame;

namespace BotDecisions
{
    class Program
    {

        static void Main(string[] args)
        {
            GameState gameState = null;
            var client = new BotClient("127.0.0.1", 13000);

            var buffer = new byte[1024];
            while (true)
            {
                if(client.stream.DataAvailable)
                {
                    var i = client.stream.Read(buffer, 0, buffer.Length);
                    var data = Encoding.ASCII.GetString(buffer, 0, i);
                    Console.WriteLine(data);

                    if (data.StartsWith("start"))
                    {
                        var botFieldTypeText = data.Split(' ')[1];
                        gameState = new GameState(botFieldTypeText); // sent message looks like start Black/Red, so we get second parameter=[1] of array
                    }
                    else if(data.StartsWith("enemy"))
                    {                  
                        //Changing BoardState after player's move
                        var arrayOfData = data.Split(' ');
                        var directionText = arrayOfData[3];
                        var direction = GetDirectionFromCode(directionText);
                        var playerMovedX = Int32.Parse(arrayOfData[1]);
                        var playerMovedY = Int32.Parse(arrayOfData[2]);
                        var playerMovedField = gameState.CurrentBoardState.BoardFields[playerMovedY, playerMovedX];
                        var numOfFields = Int32.Parse(arrayOfData[4]);
                        gameState.ChangeBoardStateAfterMove(direction, playerMovedField, numOfFields);

                        if (data.Contains("taken"))
                        {
                            var botTakenX = Int32.Parse(arrayOfData[6]);
                            var botTakenY = Int32.Parse(arrayOfData[7]);
                            gameState.CurrentBoardState.BoardFields[botTakenY, botTakenX].Type = TablutFieldType.EMPTY_FIELD;
                        }
                    }
                    else if(data.Contains("possibleMoves"))
                    {
                        var arrayOfMoves = data.Split('|');
                        Random gen = new Random();
                        int randomMoveNumber = gen.Next(1, arrayOfMoves.Length);
                        var message = "move b " + arrayOfMoves[randomMoveNumber].Substring(0, 7); // move b x y direction numOfFields 

                        //Changing BoardState after bot's move
                        var arrayOfMessage = message.Split(' ');
                        var directionText = arrayOfMessage[4];
                        var botMovedDirection = GetDirectionFromCode(directionText);
                        var botMovedX = Int32.Parse(arrayOfMessage[2]);
                        var botMovedY = Int32.Parse(arrayOfMessage[3]);
                        var botMovedField = gameState.CurrentBoardState.BoardFields[botMovedY, botMovedX];
                        var numOfFields = Int32.Parse(arrayOfMessage[5]);
                        gameState.ChangeBoardStateAfterMove(botMovedDirection, botMovedField, numOfFields);

                        Console.WriteLine("Send message to server: " + message);
                        client.SendMessage(message);
                    }
                    else if(data.StartsWith("ok taken"))
                    {
                        var arrayOfData = data.Split(' ');
                        var playerTakenX = Int32.Parse(arrayOfData[2]);
                        var playerTakenY = Int32.Parse(arrayOfData[3]);
                        gameState.CurrentBoardState.BoardFields[playerTakenY, playerTakenX].Type = TablutFieldType.EMPTY_FIELD;
                    }
                }
            }

        }

        static private DirectionEnum GetDirectionFromCode(string directionCode)
        {
            if (directionCode.Equals("L"))
            {
                return DirectionEnum.LEFT;
            }
            else if (directionCode.Equals("R"))
            {
                return DirectionEnum.RIGHT;
            }
            else if (directionCode.Equals("U"))
            {
                return DirectionEnum.UP;
            }
            else
            {
                return DirectionEnum.DOWN;
            }
        }
    }
}
