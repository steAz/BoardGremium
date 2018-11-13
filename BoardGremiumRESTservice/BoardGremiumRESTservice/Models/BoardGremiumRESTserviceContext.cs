using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Models
{
    public class BoardGremiumRESTserviceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BoardGremiumRESTserviceContext() : base("name=BoardGremiumRESTserviceContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BoardGremiumRESTserviceContext>()); 
            this.Database.Initialize(true); // <-- NEEDED IF SOMEHOW TABLES ARE/WERE DROPPED !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
                                            // AND IN UPPER LINE change 'IfModelChanges' to 'Always'
        }

        public System.Data.Entity.DbSet<BoardGremiumRESTservice.Models.GameEntity> GameEntities { get; set; }

        public GameEntity GetGameByName(string gameName)
        {
            var allGameEntities = GameEntities.ToArray();
            foreach (var gE in allGameEntities)
            {
                if (gE.GameName.Equals(gameName))
                    return gE;
            }
            return null;
        }
    }
}
