using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BoardGremiumRESTservice.Controllers
{
    public class MoveController : ApiController
    {
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
        /* 
         * tu w value string z calym ruchem (liczba, kierunek itd.)
         * 
         */

        public void Post([FromBody]string value)
        {
            //walidacja ruchu 

            /* jak jest poprawny, to wtedy wysyla 200 ok do tego ktory wyslal mu ruch i zmienia aktualnego gracza NA SERWERZE
             , ktory wykonuje ruch i musi zapamietac ostatni ruch gracza, zeby ten drugi mogl sobie to wziac get'em i zaktualizowac swoja plansze
             
             */
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
