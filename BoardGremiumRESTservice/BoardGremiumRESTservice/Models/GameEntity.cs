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
        public string PlayerPawnColor { get; set; }
        public string BoardStateRepresentation { get; set; }
        public string GameName { get; set; }
        public string CurrentPlayer { get; set; }
        public int Field { get; set; }

        public GameEntity() { }

        public GameEntity(string playerPawnColor, string boardStateRepresentation, string gameName)
        {
            this.PlayerPawnColor = playerPawnColor;
            this.BoardStateRepresentation = boardStateRepresentation;
            this.GameName = gameName;
            if(MessagesConverterUtils.PlayerPawnFromMessage(playerPawnColor).Equals(TablutFieldType.RED_PAWN))
            {
                CurrentPlayer = MessagesConverterUtils.HUMAN_STRING;
            }else
            {
                CurrentPlayer = MessagesConverterUtils.BOT_STRING;
            }
        }

        public PlayerEnum GetEnemyPlayer()
        {
            PlayerEnum currentPlayer = MessagesConverterUtils.PlayerEnumFromString(CurrentPlayer);
            if (currentPlayer.Equals(PlayerEnum.BOT_PLAYER))
            {
                return PlayerEnum.HUMAN_PLAYER;
            }
            else
            {
                return PlayerEnum.BOT_PLAYER;
            }
        }

        public void ChangeCurrentPlayer()
        {
            if (CurrentPlayer.Equals(MessagesConverterUtils.HUMAN_STRING))
            {
                CurrentPlayer = MessagesConverterUtils.BOT_STRING;
            }else
            {
                CurrentPlayer = MessagesConverterUtils.HUMAN_STRING;
            }
        }
    }
}