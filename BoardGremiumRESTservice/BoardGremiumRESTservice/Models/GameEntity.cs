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
        public bool IsGameWon { get; set; }
        public bool IsFirstPlayerJoined { get; set; }
        public bool IsSecPlayerJoined { get; set; }
        public string BotAlgorithmParamsJSON { get; set; }
        public string RedHeuristics { get; set; } //comma-separated list of ints e.g 10,12,15,20 ...
        public string BlackHeuristics { get; set; }
        
        public GameEntity() { }

        public GameEntity(string playerPawnColor, string boardStateRepresentation, string gameName)
        {
            this.PlayerPawnColor = playerPawnColor;
            this.BoardStateRepresentation = boardStateRepresentation;
            this.GameName = gameName;
            this.IsFirstPlayerJoined = false;
            this.IsSecPlayerJoined = false;
            this.IsGameWon = false;
            RedHeuristics = string.Empty;
            BlackHeuristics = string.Empty;
            //CreationDate = new DateTime();
            if (MessagesConverterUtils.PlayerPawnFromMessage(playerPawnColor).Equals(FieldType.RED_PAWN))
            {
                CurrentPlayer = MessagesConverterUtils.HUMAN_STRING;
            }else
            {
                CurrentPlayer = MessagesConverterUtils.BOT_STRING;
            }
        }

        public void AddRedHeuristic(string heuristic)
        {
            RedHeuristics += (heuristic + ",");
        }

        public void AddBlackHeuristic(string heuristic)
        {
            BlackHeuristics += (heuristic + ",");
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