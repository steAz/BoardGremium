using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Models
{
    public class GameEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerPawnColor;

        public GameEntity(string playerPawnColor)
        {
            this.PlayerPawnColor = playerPawnColor;
        }
    }
}