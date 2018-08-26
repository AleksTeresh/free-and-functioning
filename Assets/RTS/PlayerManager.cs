using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

namespace RTS
{
    public static class PlayerManager
    {
        private struct PlayerDetails
        {
            public PlayerDetails(string name, int avatar) : this()
            {
                Name = name;
                Avatar = avatar;
            }
            public string Name { get; private set; }
            public int Avatar { get; private set; }
        }
        private static List<PlayerDetails> players = new List<PlayerDetails>();
        private static PlayerDetails currentPlayer;
        private static Texture2D[] avatars;

        public static void SetAvatarTextures(Texture2D[] avatarTextures)
        {
            avatars = avatarTextures;
        }

        public static Texture2D GetPlayerAvatar()
        {
            if (avatars == null) return null;

            if (currentPlayer.Avatar >= 0 && currentPlayer.Avatar < avatars.Length) return avatars[currentPlayer.Avatar];
            return null;
        }

        public static void SelectPlayer(string name, int avatar)
        {
            //check player doesnt already exist
            bool playerExists = false;
            foreach (PlayerDetails player in players)
            {
                if (player.Name == name)
                {
                    currentPlayer = player;
                    playerExists = true;
                }
            }
            if (!playerExists)
            {
                PlayerDetails newPlayer = new PlayerDetails(name, avatar);
                players.Add(newPlayer);
                currentPlayer = newPlayer;

                Directory.CreateDirectory("SavedGames" + Path.DirectorySeparatorChar + name);
            }
        }

        public static int GetAvatar(string playerName)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Name == playerName) return players[i].Avatar;
            }
            return 0;
        }

        public static string[] GetSavedGames()
        {
            DirectoryInfo directory = new DirectoryInfo("SavedGames" + Path.DirectorySeparatorChar + currentPlayer.Name);
            FileInfo[] files = directory.GetFiles();
            string[] savedGames = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string filename = files[i].Name;
                savedGames[i] = filename.Substring(0, filename.IndexOf("."));
            }
            return savedGames;
        }

        public static string[] GetPlayerNames()
        {
            string[] playerNames = new string[players.Count];
            for (int i = 0; i < playerNames.Length; i++) playerNames[i] = players[i].Name;
            return playerNames;
        }
        
        public static string GetPlayerName()
        {
            return currentPlayer.Name == "" ? "Unknown" : currentPlayer.Name;
        }

        public static Player GetHumanPlayer (Player[] players)
        {
            foreach (Player player in players)
            {
                if (player.human)
                {
                    return player;
                }
            }

            return null;
        }
    }
}