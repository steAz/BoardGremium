using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BoardGremiumRESTservice.Utils;

namespace BoardGremiumRESTservice.Models
{
    public class GameEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerPawnColor { get; }
        public string BoardStateRepresentation { get; }
        public string gameGuid { get; }
        public string currentPlayer;

        public GameEntity(string playerPawnColor, string boardStateRepresentation)
        {
            this.PlayerPawnColor = playerPawnColor;
            this.BoardStateRepresentation = boardStateRepresentation;
            this.gameGuid = Guid.NewGuid().ToString();
            if(MessagesConverterUtils.playerPawnFromMessage(playerPawnColor).Equals(TablutFieldType.RED_PAWN))
            {
                currentPlayer = MessagesConverterUtils.HUMAN_STRING;
            }else
            {
                currentPlayer = MessagesConverterUtils.BOT_STRING;
            }
        }
    }
}