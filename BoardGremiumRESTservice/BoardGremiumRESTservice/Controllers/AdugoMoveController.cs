﻿using BoardGremiumRESTservice.Models;
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
            string moveInfo = moveParams[1];
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return NotFound();
            }

            AdugoGameState gameState = MessagesConverterUtils.ConvertStringToAdugoGameState(GameEntity.BoardStateRepresentation, GameEntity.PlayerPawnColor);
            if (GameEntity.IsGameWon)
            {
                return Ok(GameEntity.CurrentPlayer);
            }

            var move = MessagesConverterUtils.ConvertStringToAdugoMove(moveInfo, gameState);
            PlayerEnum currentPlayer = MessagesConverterUtils.PlayerEnumFromString(GameEntity.CurrentPlayer);
            if (gameState.IsChosenMoveValid(move, currentPlayer))
            {
                //BoardState oldBoardState = (BoardState)gameState.game.currentBoardState.Clone();
                ////perform move
                //gameState.game.MovePawn(gameState.game.currentBoardState, move.ChosenField, move.Direction, move.NumOfFields);
                //if (gameState.game.IsGameWon(gameState.game.currentBoardState, currentPlayer))
                //{
                //    GameEntity.IsGameWon = true;
                //    UpdateBoardStateRepresentation(GameEntity, gameState);
                //    db.Entry(GameEntity).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return Ok("ok");
                //}
                //string callbackMessage;
                //if (gameState.NumberOfPawnsOnBS(oldBoardState) != gameState.NumberOfPawnsOnBS(gameState.game.currentBoardState))
                //{
                //    Field takenPawn = gameState.GetMissingPawnForPlayer
                //        (oldBoardState, gameState.game.currentBoardState, GameEntity.GetEnemyPlayer());
                //    callbackMessage = "ok taken " + takenPawn.X + " " + takenPawn.Y;
                //}
                //else
                //{
                //    callbackMessage = "ok";
                //}
                ////change game state
                //UpdateBoardStateRepresentation(GameEntity, gameState);
                //GameEntity.ChangeCurrentPlayer();
                //db.Entry(GameEntity).State = EntityState.Modified;
                //db.SaveChanges();
                //return Ok(callbackMessage);
            }

            return BadRequest("Error 400 - Chosen move is not valid");
        }
    }
}
