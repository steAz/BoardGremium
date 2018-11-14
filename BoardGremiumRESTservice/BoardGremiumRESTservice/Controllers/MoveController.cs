using BoardGremiumRESTservice.Models;
using BoardGremiumRESTservice.Tablut;
using BoardGremiumRESTservice.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace BoardGremiumRESTservice.Controllers
{
    public class MoveController : ApiController
    {

        private BoardGremiumRESTserviceContext db = new BoardGremiumRESTserviceContext();
        // GET: api/Move
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Move/5
        
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Move

        public void Post([FromBody]string value)
        {

        }

        [ResponseType(typeof(string))]
        [HttpPost]
        [Route("api/Move")]
        public async Task<IHttpActionResult> PostMove([FromBody]string moveMessage)
        {
            string[] moveParams = moveMessage.Split('|');
            string gameName = moveParams[0];
            string moveInfo = moveParams[1];
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return NotFound();
            }
            else
            {
                TablutGameState gameState = MessagesConverterUtils.ConvertStringToTablutGameState(GameEntity.BoardStateRepresentation, GameEntity.PlayerPawnColor);
                if (GameEntity.IsGameWon)
                {
                    var callbackMessage = gameState.game.BotPlayerFieldType;
                    return Ok(GameEntity.CurrentPlayer);
                }
              
                TablutMove move = MessagesConverterUtils.ConvertStringToTablutMove(moveInfo, gameState);
                PlayerEnum currentPlayer = MessagesConverterUtils.PlayerEnumFromString(GameEntity.CurrentPlayer);
                if(gameState.IsChosenMoveValid(move, currentPlayer))
                {
                    BoardState oldBoardState = (BoardState)gameState.game.currentBoardState.Clone();
                    //perform move
                    gameState.game.MovePawn(gameState.game.currentBoardState, move.ChosenField, move.Direction, move.NumOfFields);
                    if(gameState.game.IsGameWon(gameState.game.currentBoardState, currentPlayer))
                    {
                        GameEntity.IsGameWon = true;
                        UpdateBoardStateRepresentation(GameEntity, gameState);
                        db.Entry(GameEntity).State = EntityState.Modified;
                        db.SaveChanges();
                        return Ok("ok");
                    }
                    string callbackMessage;
                    if (gameState.NumberOfPawnsOnBS(oldBoardState) != gameState.NumberOfPawnsOnBS(gameState.game.currentBoardState))
                    {
                        Field takenPawn = gameState.GetMissingPawnForPlayer
                            (oldBoardState, gameState.game.currentBoardState, GameEntity.GetEnemyPlayer());
                        callbackMessage = "ok taken " + takenPawn.X + " " + takenPawn.Y;
                    }
                    else
                    {
                        callbackMessage = "ok";
                    }
                    //change game state
                    UpdateBoardStateRepresentation(GameEntity, gameState);
                    GameEntity.ChangeCurrentPlayer();
                    db.Entry(GameEntity).State = EntityState.Modified;
                    db.SaveChanges();
                    return Ok(callbackMessage);
                }
                else
                {
                    return BadRequest("Error 400 - Chosen move is not valid");
                }
                
            }
        }

        [ResponseType(typeof(string))]
        [HttpPost]
        [Route("api/Move/{gameName}/RedHeuristics")]
        public HttpResponseMessage PostRedHeuristics(string gameName, [FromBody] string redHeuristic)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                GameEntity.AddRedHeuristic(redHeuristic);
                db.Entry(GameEntity).State = EntityState.Modified;
                db.SaveChanges();
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("OK")
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpPost]
        [Route("api/Move/{gameName}/BlackHeuristics")]
        public HttpResponseMessage PostBlackHeuristics(string gameName, [FromBody] string blackHeuristic)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                GameEntity.AddBlackHeuristic(blackHeuristic);
                db.Entry(GameEntity).State = EntityState.Modified;
                db.SaveChanges();
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("OK")
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        private void UpdateBoardStateRepresentation(GameEntity gameEntity, TablutGameState tbs)
        {
            string updatedBoardStateRepresentation = MessagesConverterUtils.ConvertTablutGameStateToString(tbs);
            gameEntity.BoardStateRepresentation = updatedBoardStateRepresentation;
        }

        // PUT: api/Move/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Move/5
        public void Delete(int id)
        {
        }
    }
}
