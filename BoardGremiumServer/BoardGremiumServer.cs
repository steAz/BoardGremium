using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Games;
using AbstractGame;

namespace BoardGremiumServer
{
    public class BoardGremiumServer
    {
        private string ServerIp { get; }
        private int ServerPort { get; }
        private TcpListener Listener1;
        private byte[] BufferPlayer;
        private byte[] BufferBot;
        private GameState GameState;
        private NetworkStream playerStream;
        private NetworkStream botStream;

        public BoardGremiumServer(string ServerIp, int ServerPort)
        {
            //GameState = new GameState();
            this.ServerIp = ServerIp;
            this.ServerPort = ServerPort;
            IPAddress serverAddress = IPAddress.Parse(ServerIp);
            Listener1 = new TcpListener(serverAddress, ServerPort);
            //Listener2 = new TcpListener(serverAddress, ServerPort);
        }

        public void startListening()
        {
            Listener1.Start();
            //Listener2.Start();
            BufferPlayer = new byte[256];
            BufferBot = new byte[256];
            ListeningLoop();
        }

        private void ListeningLoop()
        {
            Console.Write("Waiting for a connection... ");
            TcpClient playerClient = Listener1.AcceptTcpClient();
            Console.WriteLine("Player Connected!");
            TcpClient botClient = Listener1.AcceptTcpClient();
            Console.WriteLine("Bot Connected!");
            // Get a stream object for reading and writing
            playerStream = playerClient.GetStream();
            botStream = botClient.GetStream();
            string playerData = null, botData = null;
            while (true)
            {
                //dataRecivedAction(reciveMessage(stream));
                //botData = reciveMessage(botStream);
                if(botStream.DataAvailable)
                    botStream.BeginRead(BufferBot, 0, BufferBot.Length, dataRecivedActionBot, BufferBot);

                if (playerStream.DataAvailable)
                    playerStream.BeginRead(BufferPlayer, 0, BufferPlayer.Length, dataRecivedActionPlayer, BufferPlayer);
                if (botData != null)
                {
                    Console.WriteLine("Bot data");
                    //dataRecivedAction(botData);
                }
                //Console.WriteLine("bb");
                //playerData = reciveMessage(playerStream);
                if(playerData != null)
                {
                    Console.WriteLine("Player data");
                    //dataRecivedAction(playerData);
                } //Console.WriteLine();

            }
            // Shutdown and end connection
            playerClient.Close();
            botClient.Close();
        }

        /// <summary>
        /// returning recived string message
        /// </summary>
        /// <param name="stream"> client stream</param>
        /// <returns></returns>
        private string reciveMessage(NetworkStream stream)
        {
            int i;
            string data;
            // Loop to receive all the data sent by the client.
            //if ((i = stream.Read(Buffer, 0, Buffer.Length)) != 0)
            //{
                // Translate data bytes to a ASCII string.
               // data = System.Text.Encoding.ASCII.GetString(Buffer, 0, i);
               // return data;
            //}
            return null;
            /*
             * Original loop. Hint for future callback implementation
             *while ((i = stream.Read(Buffer, 0, Buffer.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(Buffer, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        dataRecivedAction(data);
                        //data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }
             * */
        }

        private void sendMessage(string message, PlayerEnum target)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            Console.WriteLine("callback: " + message + " length : " + msg.Length);

            if(target.Equals(PlayerEnum.HUMAN_PLAYER))
            {
                playerStream.Write(msg, 0, msg.Length);
            }else
            {
                botStream.Write(msg, 0, msg.Length);
            }
        }

        //move p 2 3 L 4
        public void dataRecivedActionPlayer(IAsyncResult state)
        {
            var buffer = (byte[])state.AsyncState;
            //Console.WriteLine("1");
            Console.WriteLine("Player data recived");
            var length = playerStream.EndRead(state);
            //Console.WriteLine("2 " + length);
            //Console.ReadKey();
            string data = System.Text.Encoding.ASCII.GetString(buffer, 0, length);
            //Console.WriteLine("Async result: " + result.ToString() + " state " + (string)result.AsyncState);
            Console.WriteLine("RECIVED: " + data);
            if(data.StartsWith("move"))
             {
                performMoveFromMessage(data);
             }
            else if(data.StartsWith("gamerPawns"))
            {
                string[] array = data.Split(' ');
                string botType;
                if(array[1].Equals("red"))
                {
                    Console.WriteLine("Player plays white pawns");
                    GameState = new GameState(TablutFieldType.WHITE_PAWN);
                    botType = "black";
                    string messageForBot = "start " + botType;
                    sendMessage(messageForBot, PlayerEnum.BOT_PLAYER);
                }
                else
                {
                    Console.WriteLine("Player plays black pawns");
                    GameState = new GameState(TablutFieldType.BLACK_PAWN);
                    botType = "red";
                    string messageForBot = "start " + botType;
                    sendMessage(messageForBot, PlayerEnum.BOT_PLAYER);
                    sendMessage(getPossibleMovesMessage(), PlayerEnum.BOT_PLAYER);
                }
                //send color to bot
                
            }
        }

        public void dataRecivedActionBot(IAsyncResult state)
        {
            var buffer = (byte[])state.AsyncState;
            var length = botStream.EndRead(state);
            Console.WriteLine("Bot data recived");
            string data = System.Text.Encoding.ASCII.GetString(buffer, 0, length);
            //Console.WriteLine("Async result: " + result.ToString() + " state " + (string)result.AsyncState);
            Console.WriteLine(data);
            if(data.StartsWith("move"))
             {
                performMoveFromMessage(data);
             }
        }
        //enemy 2 3 L 4 {taken 4 5}
        //move p 2 3 L 4
        private void performMoveFromMessage(string message)
        {
            string moveData = message.Substring(5);
            string[] array = moveData.Split(' ');
            string clientCode = array[0];
            Console.WriteLine("clientCode = " + clientCode);
            PlayerEnum player = getPlayerFromCode(clientCode);
            int xCoord = int.Parse(array[1]);
            int yCoord = int.Parse(array[2]);
            string directionCode = array[3];
            DirectionEnum direction = getDirectionFromCode(directionCode);
            int numberOfFields = int.Parse(array[4]);
            Field field = GameState.GetFieldForCoords(xCoord, yCoord);
            if(player.Equals(PlayerEnum.BOT_PLAYER) || GameState.IsChosenMoveValid(numberOfFields, direction, field))
            {
                /* 
                clone somewhere currentBoardState
                make move
                check if number of pawns equals. If not find that pawn, send message to client OK PAWN_TAKEN X Y
                if yes send message OK
                Send player move to bot (or boardstate)
                Send list of possible moves to bot
                wait for bot response
                perform bot's move
                send bot move to player
                REPEAT
                */
                Console.WriteLine("Valid move");
                handleValidMove(numberOfFields, direction, field, player, message);
            } else
            {
                Console.WriteLine("Not valid");
                handleInvalidMove();
            }
        }

        private void handleValidMove(int numberOfFields, DirectionEnum direction, Field field, PlayerEnum player, string message)
        {
            BoardState oldBoardState = (BoardState)GameState.game.currentBoardState.Clone();
            //perform move
            GameState.game.MovePawn(GameState.game.currentBoardState, field, direction, numberOfFields);
            string callbackMessage;
            if(GameState.NumberOfPawnsOnBS(oldBoardState) != GameState.NumberOfPawnsOnBS(GameState.game.currentBoardState))
            {
                Console.WriteLine("Number of Pawns Different");
                Field takenPawn = GameState.getMissingPawnForPlayer
                    (oldBoardState, GameState.game.currentBoardState, getEnemyPlayer(player));
                callbackMessage = "ok taken " + takenPawn.X + " " + takenPawn.Y;
                Console.WriteLine(message);
            }
            else
            {
                callbackMessage = "ok";
            }
            sendMessage(callbackMessage, player);
            sendMessageForOpponent(message, callbackMessage, player);
            //sleep to be sure bot will read 2 messages?
            if(player.Equals(PlayerEnum.HUMAN_PLAYER))
            {
                string possibleMovesMessage = getPossibleMovesMessage();
                Console.WriteLine("TEST POSSIBLE MOVES");
                Console.WriteLine(possibleMovesMessage);
                sendMessage(possibleMovesMessage, PlayerEnum.BOT_PLAYER);
            }
        }
        //enemy 2 3 L 4 {taken 4 5}
        private void sendMessageForOpponent(string originalMessage, string callbackMessage, PlayerEnum player)
        {
            string message = "enemy ";
            //original message like 'move p 2 3 L 4'
            Console.WriteLine("orig: " + originalMessage);
            message += originalMessage.Substring(7);// 7 - index of x-coord
            if(!callbackMessage.Equals("ok"))
            {
                //ok taken 2 3
                message += " ";
                message += callbackMessage.Substring(3);
            }
            Console.WriteLine("message for opponent: " + message);
            sendMessage(message, getEnemyPlayer(player));
        }
        /// <summary>
        /// Only players will make invalid moves. Bot makes no mistakes.
        /// </summary>
        private void handleInvalidMove()
        {
            string message = "notOk";
            sendMessage(message, PlayerEnum.HUMAN_PLAYER);
        }

        private DirectionEnum getDirectionFromCode(string directionCode)
        {
            if(directionCode.Equals("L"))
            {
                return DirectionEnum.LEFT;
            } else if (directionCode.Equals("R"))
            {
                return DirectionEnum.RIGHT;
            } else if (directionCode.Equals("U"))
            {
                return DirectionEnum.UP;
            } else
            {
                return DirectionEnum.DOWN;
            }
        }

        private PlayerEnum getPlayerFromCode(string code)
        {
            if(code.Equals("p"))
            {
                return PlayerEnum.HUMAN_PLAYER;
            }else
            {
                return PlayerEnum.BOT_PLAYER;
            }
        }

        private PlayerEnum getEnemyPlayer(PlayerEnum player)
        {
            if(player.Equals(PlayerEnum.BOT_PLAYER))
            {
                return PlayerEnum.HUMAN_PLAYER;
            } else
            {
                return PlayerEnum.BOT_PLAYER;
            }
        }
        /// <summary>
        /// string message containing possible moves for bot. Format is as follows:
        /// sourceX sourceY direction numOfFields {taken takenX takenY} - taken is optional
        /// </summary>
        private string getPossibleMovesMessage()
        {
            List<BoardState> possibleBoardStates = GameState.game.GetPossibleBoardStates(GameState.game.currentBoardState, PlayerEnum.BOT_PLAYER);
            string message = "possibleMoves";
            string singleMoveMessage = "";
            foreach(BoardState next in possibleBoardStates)
            {
                singleMoveMessage = getMoveStringFromBoardStates(GameState.game.currentBoardState, next);
                message += "|" + singleMoveMessage;
            }

            return message;
        }

        private string getMoveStringFromBoardStates(BoardState previous, BoardState next)
        {
            //This will be called only for bot player
            TablutFieldType botPawn = (TablutFieldType)GameState.game.BotPlayerFieldType;
            Field selectedField = null, targetField = null;
            for (int i=0; i < GameState.game.BoardHeight; i++)
            {
                for(int j=0; j < GameState.game.BoardWidth; j++)
                {
                    if(GameState.pawnsInSameTeam((TablutFieldType)previous.BoardFields[i,j].Type,botPawn) 
                        && next.BoardFields[i,j].Type.Equals(TablutFieldType.EMPTY_FIELD))
                    {
                        selectedField = previous.BoardFields[i, j];
                    }else if(GameState.pawnsInSameTeam((TablutFieldType)next.BoardFields[i, j].Type, botPawn)
                            && previous.BoardFields[i, j].Type.Equals(TablutFieldType.EMPTY_FIELD))
                    {
                        targetField = next.BoardFields[i, j];
                    }
                }
            }
            if(selectedField != null && targetField != null)
            {
                //string message = "" + selectedField.X + " " + selectedField.Y + " " + targetField.X + " " + targetField.Y;
                DirectionEnum direction = GameState.getDirectionFromMove(selectedField, targetField);
                int numOfFields = GameState.getNumOfFieldsFromMove(selectedField, targetField, direction);
                string message = "" + selectedField.X + " " + selectedField.Y + " " + direction.ToString().First() + " " + numOfFields;
                if (GameState.NumberOfPawnsOnBS(previous) != GameState.NumberOfPawnsOnBS(next))
                {
                    Field taken = GameState.getMissingPawnForPlayer(previous, next, PlayerEnum.HUMAN_PLAYER);
                    message += " taken " + taken.X + " " + taken.Y;
                }
                return message;
            }
            return null;
        }

        private string moveStringFromParameters()
        {
            return null;
        }

    }
}
