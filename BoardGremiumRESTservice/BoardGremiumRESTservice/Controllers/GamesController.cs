using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGremiumRESTservice.Models;
using BoardGremiumRESTservice.Utils;
using System.Net.Http.Headers;
using BoardGremiumRESTservice.Adugo;
using Newtonsoft.Json;
using BoardGremiumRESTservice.Tablut;

namespace BoardGremiumRESTservice.Controllers
{
    public class GameEntitysController : ApiController
    {
        private BoardGremiumRESTserviceContext db = new BoardGremiumRESTserviceContext();

        // GET: api/GameEntitys
        public IQueryable<GameEntity> GetGameEntitys()
        {
            return db.GameEntities;
        }

        // GET: api/GameEntitys/5
        [ResponseType(typeof(GameEntity))]
        public async Task<IHttpActionResult> GetGameEntity(int id)
        {
            GameEntity GameEntity = await db.GameEntities.FindAsync(id);
            if (GameEntity == null)
            {
                return NotFound();
            }
            return Ok(GameEntity);
        }

        //GET api/GameEntitys/{gameName}/CurrentPlayer
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/CurrentPlayer")]
        public HttpResponseMessage GetCurrentPlayer(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(GameEntity.CurrentPlayer)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/RedHeuristics")]
        public HttpResponseMessage GetRedHeuristics(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(GameEntity.RedHeuristics)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/BlackHeuristics")]
        public HttpResponseMessage GetBlackHeuristics(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(GameEntity.BlackHeuristics)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpPost]
        [Route("api/GameEntitys/{gameName}/HumanPlayerJoined")]
        public HttpResponseMessage PostHumanPlayerJoined(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                GameEntity.IsFirstPlayerJoined = true;
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

        //GET api/GameEntitys/{gameName}/CurrentPlayer
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/CurrentBoardState")]
        public HttpResponseMessage GetCurrentBoardState(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(GameEntity.BoardStateRepresentation)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        //GET api/GameEntitys/{gameName}/BotColor
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/BotPawnColor")]
        public HttpResponseMessage GetBotPawnColor(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                string botPawnColorString;
                if (!GameEntity.IsFirstPlayerJoined)
                {
                    GameEntity.IsFirstPlayerJoined = true; // first player joined
                    botPawnColorString = GameEntity.PlayerPawnColor;
                }
                else if (!GameEntity.IsSecPlayerJoined)
                {
                    GameEntity.IsSecPlayerJoined = true;
                    var botPawnColor = MessagesConverterUtils.EnemyPawnFromMessage(GameEntity.PlayerPawnColor);
                    botPawnColorString = MessagesConverterUtils.MessageFromPlayerPawn(botPawnColor);
                }
                else // 
                {
                    // var botPawnColor = MessagesConverterUtils.EnemyPawnFromMessage(GameEntity.PlayerPawnColor);
                    // botPawnColorString = MessagesConverterUtils.MessageFromPlayerPawn(botPawnColor);
                    //after error
                    botPawnColorString = GameEntity.PlayerPawnColor;
                    GameEntity.IsSecPlayerJoined = false;
                }

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(botPawnColorString)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                db.Entry(GameEntity).State = EntityState.Modified;
                db.SaveChanges();
                return response;
            }

            
        }

        // POST: api/GameEntitys/{gameName}/BotAlgorithms - string Body format: firstBotAlgorithm,secBotAlgorithm(NONE if there is Human vs Bot)
        [ResponseType(typeof(GameEntity))]
        [HttpPost]
        [Route("api/GameEntitys/{gameName}/SetBotAlgorithms")]
        public HttpResponseMessage PostSetBotAlgorithms(string gameName, [FromBody]string botAlgorithmsParamsJSON)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
               // var botAlgParamsJSONbeforeReplace = botAlgorithmsParamsJSON.Substring(1, botAlgorithmsParamsJSON.Length - 1);
                botAlgorithmsParamsJSON = botAlgorithmsParamsJSON.Replace('\'', '"') ;
                var allGameEntities = db.GameEntities.ToArray();
                foreach (var gE in allGameEntities)
                {
                    if (gE.GameName.Equals(gameName)) gE.BotAlgorithmParamsJSON = botAlgorithmsParamsJSON;
                }

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
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/BotAlgorithmParamsJSON")]
        public HttpResponseMessage GetBotAlgorithmParamsJSON(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(GameEntity.BotAlgorithmParamsJSON)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/IsWon")]
        public HttpResponseMessage GetIsGameWon(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                bool isWon = GameEntity.IsGameWon;

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(isWon.ToString())
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/IsFirstPlayerJoined")]
        public HttpResponseMessage GetIsFirstPlayerJoined(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                bool isFirstPlayerJoined = GameEntity.IsFirstPlayerJoined;
                bool isSecondPlayerJoined = GameEntity.IsSecPlayerJoined;
                string isFirstPlayerJoinedMessage;

                //This GET is sent only once on start of game in bot application.
                //So if both bools are true(firstJoined and secondJoined) it means that game has stopped and bots are reconnecting.
                //Thats why we set both bools to false.
                if(isFirstPlayerJoined && isSecondPlayerJoined)
                {
                    isFirstPlayerJoined = false;
                    isSecondPlayerJoined = false;
                }
                if(isFirstPlayerJoined)
                {
                    isFirstPlayerJoinedMessage = MessagesConverterUtils.FIRST_JOINED_STRING;
                }else
                {
                    isFirstPlayerJoinedMessage = MessagesConverterUtils.FIRST_NOT_JOINED_STRING;
                }

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(isFirstPlayerJoinedMessage)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/FirstPlayerColor")]
        public HttpResponseMessage GetFirstPlayerColor(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                string firstPlayerColor = GameEntity.PlayerPawnColor;

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(firstPlayerColor)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/SecondPlayerColor")]
        public HttpResponseMessage GetSecondPlayerColor(string gameName)
        {
            GameEntity GameEntity = db.GetGameByName(gameName);
            if (GameEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                HttpResponseMessage response = null;
                string secondPlayerColor = "";
                try
                {
                    secondPlayerColor = MessagesConverterUtils.GetEnemyColor(GameEntity.PlayerPawnColor);
                }catch(ArgumentException e)
                {
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(e.Message)
                    };
                }
                

                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(secondPlayerColor)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }

        // PUT: api/GameEntitys/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGameEntity(int id, GameEntity GameEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != GameEntity.Id)
            {
                return BadRequest();
            }

            db.Entry(GameEntity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/GameEntitys - string Body format: gameName,playerPawnColor(UpperCase)
        [ResponseType(typeof(GameEntity))]
        public async Task<IHttpActionResult> PostGameEntity([FromBody]string gameMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gameParams = gameMessage.Split(','); // 0 - gameName,  1 - playerPawnColor,
                                                     // 2 - gameType (Adugo or Tablut)
            var gameName = gameParams[0];
            var playerPawnColor = gameParams[1];
            var gameType = gameParams[2];


            var allGameEntities = db.GameEntities.ToArray();
            foreach( var gE in allGameEntities)
            {
                if(gE.GameName.Equals(gameName))
                    return BadRequest("Error 400 - Game name already exists in the database");
            }

            FieldType playerType = MessagesConverterUtils.PlayerPawnFromMessage(playerPawnColor);
            string bsRepresentation;
            if (gameType == MessagesConverterUtils.TABLUT_STRING)
            {
                TablutGameState tgs = new TablutGameState(playerType);
                bsRepresentation = MessagesConverterUtils.ConvertTablutGameStateToString(tgs);
            }else if (gameType == MessagesConverterUtils.ADUGO_STRING)
            {
                var ags = new AdugoGameState(playerType);
                bsRepresentation = MessagesConverterUtils.ConvertAdugoGameStateToString(ags);
            }
            else
            {
                throw new NullReferenceException("bsRepresentation variable has not been initialized in PostGameEntity method");
            }

            GameEntity ge = new GameEntity(playerPawnColor, bsRepresentation, gameName);
            db.GameEntities.Add(ge);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ge.Id }, ge);
        }

        // DELETE: api/GameEntitys/5
        [ResponseType(typeof(GameEntity))]
        public async Task<IHttpActionResult> DeleteGameEntity(int id)
        {
            GameEntity GameEntity = await db.GameEntities.FindAsync(id);
            if (GameEntity == null)
            {
                return NotFound();
            }

            db.GameEntities.Remove(GameEntity);
            await db.SaveChangesAsync();

            return Ok(GameEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameEntityExists(int id)
        {
            return db.GameEntities.Count(e => e.Id == id) > 0;
        }

    }
}