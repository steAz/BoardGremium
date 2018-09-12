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

        //GET api/GameEntitys/{id}/CurrentPlayer
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("api/GameEntitys/{gameName}/CurrentPlayer")]
        public async Task<IHttpActionResult> GetCurrentPlayer(string gameName)
        {
            GameEntity GameEntity = GetGameByName(gameName);
            if (GameEntity == null)
            {
                return NotFound();
            }else
            {
                return Ok(GameEntity.CurrentPlayer);
            }
        }

        private GameEntity GetGameByName(string gameName)
        {
            var allGameEntities = db.GameEntities.ToArray();
            foreach (var gE in allGameEntities)
            {
                if (gE.GameName.Equals(gameName))
                    return gE;
            }
            return null;
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

            var gameParams = gameMessage.Split(','); // 0 - gameName,  1 - playerPawnColor
            var gameName = gameParams[0];
            var playerPawnColor = gameParams[1];


            var allGameEntities = db.GameEntities.ToArray();
            foreach( var gE in allGameEntities)
            {
                if(gE.GameName.Equals(gameName))
                    return BadRequest("Error 400 - Game name already exists in the database");
            }

            TablutFieldType playerType = MessagesConverterUtils.PlayerPawnFromMessage(playerPawnColor);
            TablutGameState tgs = new TablutGameState(playerType);
            string bsRepresentation = MessagesConverterUtils.ConvertTablutGameStateToString(tgs);
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

        /*  string format of TablutGameStateObject:
            every field type has 1 character representation:
            RED_PAWN - R
            BLACK_PAWN - B
            KING - K
            EMPTY_FIELD - E
        */

        
        //przy POST nowej gry musimy dodawać do bazy stringRepresentation poczatkowego stanu
    }
}