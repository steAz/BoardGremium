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

        // POST: api/GameEntitys
        [ResponseType(typeof(GameEntity))]
        public async Task<IHttpActionResult> PostGameEntity([FromBody]string playerPawnColor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GameEntity xd = new GameEntity(playerPawnColor);
            db.GameEntities.Add(xd);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = xd.Id }, xd);
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

        private string ConvertTablutGameStateToString(TablutGameState tgs)
        {
            
            string result = "";
            if ((TablutFieldType)tgs.game.HumanPlayerFieldType == TablutFieldType.BLACK_PAWN)
            {
                result += "B,";
            }
            else
            {
                result += "R,";
            }
            foreach(Field f in tgs.game.currentBoardState.BoardFields)
            {
                if((TablutFieldType)f.Type == TablutFieldType.BLACK_PAWN)
                {
                    result += "B";
                } else if ((TablutFieldType)f.Type == TablutFieldType.RED_PAWN)
                {
                    result += "R";
                }else if ((TablutFieldType)f.Type == TablutFieldType.KING)
                {
                    result += "K";
                }else
                {
                    result += "E";
                }
            }
            return result;
        }

        private TablutGameState ConvertStringToTablutBoardState(string stringRepresentation, string playerPawnColor)
        {
            BoardState result = new BoardState(TablutGame.BOARD_WIDTH, TablutGame.BOARD_HEIGHT);
            string[] arguments = stringRepresentation.Split(',');
            TablutFieldType playerType;
            if(playerPawnColor.Equals("B"))
            {
                playerType = TablutFieldType.BLACK_PAWN;
            }else
            {
                playerType = TablutFieldType.RED_PAWN;
            }
            var enumerator = stringRepresentation.GetEnumerator();
            int horizontalIndex = 0, verticalIndex = 0;
            do
            {
                char character = enumerator.Current;
                if(character.Equals('B'))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.BLACK_PAWN;
                }else if (character.Equals('R'))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.RED_PAWN;
                }else if (character.Equals('K'))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.KING;
                }else
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.EMPTY_FIELD;
                }

                horizontalIndex++;
                if(horizontalIndex >= TablutGame.BOARD_WIDTH )
                {
                    horizontalIndex = 0;
                    verticalIndex++;
                }

                if(verticalIndex >= TablutGame.BOARD_HEIGHT)
                {
                    throw new ArgumentOutOfRangeException("Exception thrown while parsing BoardState string representation - string is too long");
                }
            } while (enumerator.MoveNext());
            return new TablutGameState(playerType, result);
        }
        //przy POST nowej gry musimy dodawać do bazy stringRepresentation poczatkowego stanu
    }
}