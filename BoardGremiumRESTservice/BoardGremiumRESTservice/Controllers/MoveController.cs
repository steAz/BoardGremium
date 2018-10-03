﻿using BoardGremiumRESTservice.Models;
using BoardGremiumRESTservice.Tablut;
using BoardGremiumRESTservice.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        //GET api/GameEntitys/{id}/CurrentPlayer
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
                TablutMove move = MessagesConverterUtils.ConvertStringToTablutMove(moveInfo, gameState);
                PlayerEnum currentPlayer = MessagesConverterUtils.PlayerEnumFromString(GameEntity.CurrentPlayer);
                if(gameState.IsChosenMoveValid(move, currentPlayer))
                {
                    BoardState oldBoardState = (BoardState)gameState.game.currentBoardState.Clone();
                    //perform move
                    gameState.game.MovePawn(gameState.game.currentBoardState, move.ChosenField, move.Direction, move.NumOfFields);
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
                    string updatedBoardStateRepresentation= MessagesConverterUtils.ConvertTablutGameStateToString(gameState);
                    GameEntity.BoardStateRepresentation = updatedBoardStateRepresentation;
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