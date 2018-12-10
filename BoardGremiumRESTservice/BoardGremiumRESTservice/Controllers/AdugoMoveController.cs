using BoardGremiumRESTservice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGremiumRESTservice.Adugo;
using BoardGremiumRESTservice.Tablut;
using BoardGremiumRESTservice.Utils;

namespace BoardGremiumRESTservice.Controllers
{
    public class AdugoMoveController : ApiController
    {
        private BoardGremiumRESTserviceContext db = new BoardGremiumRESTserviceContext();

        [ResponseType(typeof(string))]
        [HttpPost]
        [Route("api/AdugoMove")]
        public async Task<IHttpActionResult> PostMove([FromBody]string moveMessage)
        {
            string[] moveParams = moveMessage.Split('|');
            string gameName = moveParams[0];
            var moveInfo = moveParams[1];
            var GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return NotFound();
            }

            var gameState = MessagesConverterUtils.ConvertStringToAdugoGameState(GameEntity.BoardStateRepresentation, GameEntity.PlayerPawnColor);
            if (GameEntity.IsGameWon)
            {
                return Ok(GameEntity.CurrentPlayer);
            }

            var move = MessagesConverterUtils.ConvertStringToAdugoMove(moveInfo, gameState);
            PlayerEnum currentPlayer = MessagesConverterUtils.PlayerEnumFromString(GameEntity.CurrentPlayer);

            if (!gameState.IsChosenMoveValid(move, currentPlayer, out var fieldToBeat, out var fieldToMove))
                return BadRequest("Error 400 - Chosen move is not valid");

            gameState.Game.MovePawnAndBeatIfNecessary(gameState.Game.CurrentBoardState, move, fieldToBeat, fieldToMove);


            if (currentPlayer == PlayerEnum.HUMAN_PLAYER)
            {
                var playerPawn = MessagesConverterUtils.PlayerPawnFromMessage(GameEntity.PlayerPawnColor);
                if (playerPawn.Equals(FieldType.JAGUAR_PAWN)) // only write taken pawns by jaguar (dogs cannot beat jaguar)
                {
                    if (fieldToBeat != null) // if taken
                    {
                        GameEntity.AddTakenPawnsByRedJaguar(true);
                    }
                    else
                    {
                        GameEntity.AddTakenPawnsByRedJaguar(false);
                    }
                }
            }
            else
            {
                var enemyPawn = MessagesConverterUtils.EnemyPawnFromMessage(GameEntity.PlayerPawnColor);
                if (enemyPawn.Equals(FieldType.JAGUAR_PAWN)) // only write taken pawns by jaguar (dogs cannot beat jaguar)
                {
                    if (fieldToBeat != null) // if taken
                    {
                        GameEntity.AddTakenPawnsByRedJaguar(true);
                    }
                    else
                    {
                        GameEntity.AddTakenPawnsByRedJaguar(false);
                    }
                }
            }

            if (gameState.Game.IsGameWon(gameState.Game.CurrentBoardState, currentPlayer))
            {
                GameEntity.IsGameWon = true;
                UpdateBoardStateRepresentation(GameEntity, gameState);
                db.Entry(GameEntity).State = EntityState.Modified;
                db.SaveChanges();
                return Ok("ok");
            }
            string callbackMessage;
            if (fieldToBeat != null)
            {
                callbackMessage = "ok taken " + fieldToBeat.X + " " + fieldToBeat.Y;
            }
            else
            {
                callbackMessage = "ok";
            }
            ////change game state
            UpdateBoardStateRepresentation(GameEntity, gameState);
            GameEntity.ChangeCurrentPlayer();
            db.Entry(GameEntity).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(callbackMessage);

        }

        private static void UpdateBoardStateRepresentation(GameEntity gameEntity, AdugoGameState abs)
        {
            var updatedBoardStateRepresentation = MessagesConverterUtils.ConvertAdugoGameStateToString(abs);
            gameEntity.BoardStateRepresentation = updatedBoardStateRepresentation;
        }
    }
}
