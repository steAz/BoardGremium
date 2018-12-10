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
        public string RedJaguarHeuristics { get; set; } //comma-separated list of ints e.g 10,12,15,20 ...
        public string BlackDogHeuristics { get; set; } //comma-separated list of ints e.g 10,12,15,20 ...
        public string GameType { get; set; }
        public string TakenPawnsByRedJaguar { get; set; } //comma-separated list of ints e.g 0,1,2,3,4 ...
        public string TakenPawnsByBlack { get; set; } //comma-separated list of ints e.g 0,1,2,3,4 ...

        public GameEntity() { }

        public GameEntity(string playerPawnColor, string boardStateRepresentation, string gameName, string gameType)
        {
            this.PlayerPawnColor = playerPawnColor;
            this.BoardStateRepresentation = boardStateRepresentation;
            this.GameName = gameName;
            this.IsFirstPlayerJoined = false;
            this.IsSecPlayerJoined = false;
            this.IsGameWon = false;
            this.GameType = gameType;
            RedJaguarHeuristics = string.Empty;
            BlackDogHeuristics = string.Empty;
            TakenPawnsByRedJaguar = string.Empty;
            TakenPawnsByBlack = string.Empty;
            //CreationDate = new DateTime();
            if (MessagesConverterUtils.PlayerPawnFromMessage(playerPawnColor).Equals(FieldType.RED_PAWN) ||
                MessagesConverterUtils.PlayerPawnFromMessage(playerPawnColor).Equals(FieldType.JAGUAR_PAWN))
                // in Tablut RED moves first, in Adugo JAGUAR moves first
            {
                CurrentPlayer = MessagesConverterUtils.HUMAN_STRING;
            }
            else
            {
                CurrentPlayer = MessagesConverterUtils.BOT_STRING;
            }
        }

        public void AddRedHeuristic(string heuristic)
        {
            RedJaguarHeuristics += (heuristic + ",");
        }

        public void AddBlackHeuristic(string heuristic)
        {
            BlackDogHeuristics += (heuristic + ",");
        }

        public void AddTakenPawnsByRedJaguar(bool add)
        {
            string[] elementsOfTakenPawns = TakenPawnsByRedJaguar.Split(',');
            int numberOfTakenPawns;
            if (elementsOfTakenPawns.Length == 1 && elementsOfTakenPawns[0] == "") // if there is no elements in TakenPawns (one empty element "" is from split when TakenPawns was empty)
            {
                numberOfTakenPawns = 0;
                if (add) numberOfTakenPawns++;
                TakenPawnsByRedJaguar = numberOfTakenPawns.ToString();
            }
            else
            {
                string xd = elementsOfTakenPawns.Last();
                numberOfTakenPawns = Int32.Parse(elementsOfTakenPawns.Last());
                if (add) numberOfTakenPawns++;
                TakenPawnsByRedJaguar += ("," + numberOfTakenPawns);
            }
            
           
        }

        public void AddTakenPawnsByBlack(bool add)
        {
            string[] elementsOfTakenPawns = TakenPawnsByBlack.Split(',');
            int numberOfTakenPawns;
            if (elementsOfTakenPawns.Length == 1 && elementsOfTakenPawns[0] == "") // if there is no elements in TakenPawns (one empty element "" is from split when TakenPawns was empty)
            {
                numberOfTakenPawns = 0;
                if (add) numberOfTakenPawns++;
                TakenPawnsByBlack = numberOfTakenPawns.ToString();
            }
            else
            {
                string xd = elementsOfTakenPawns.Last();
                numberOfTakenPawns = Int32.Parse(elementsOfTakenPawns.Last());
                if (add) numberOfTakenPawns++;
                TakenPawnsByBlack += ("," + numberOfTakenPawns);
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