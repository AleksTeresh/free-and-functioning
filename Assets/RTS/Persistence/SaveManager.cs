using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using Events;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

namespace Persistence
{
    public static class SaveManager
    {
        public static void SaveGame(string filename)
        {
            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");

            // FireSaveEvent();

            BinaryFormatter bf = new BinaryFormatter();

            bf = PersistenceManager.AddSurrogatesToBinaryFormatter(bf);

            char separator = Path.DirectorySeparatorChar;
            string path = "Saves" + separator + filename + ".dat";
            FileStream file = File.Create(path);
            SaveGameDetails(bf, file);
            file.Close();

            Debug.Log("The game was succesfully saved");
        }

        private static void SaveGameDetails(BinaryFormatter bf, FileStream file)
        {
            GameData data = new GameData();

            data.sceneName = SceneManager.GetActiveScene().name;

            data = SetPlayersData(data);
            data = SetCameraData(data);
            data = SetSunData(data);
            // data = SetExtraLightData(data);
            data = SetEventObjectData(data);
            data = SetFogOfWarData(data);
            data = SetTargetManagerData(data);

            bf.Serialize(file, data);
        }

        private static GameData SetSunData(GameData data)
        {
            Sun sun = GameObject.FindObjectOfType<Sun>();
            if (data == null || sun == null) return data;

            data.sun = sun.GetData();

            return data;
        }

        private static GameData SetFogOfWarData(GameData data)
        {
            //needs to be adapted for terrain once if that gets implemented
            FogOfWar fogOfWar = GameObject.FindObjectOfType<FogOfWar>();
            if (data == null || fogOfWar == null) return data;

            data.fogOfWar = fogOfWar.GetData();

            return data;
        }

        private static GameData SetCameraData(GameData data)
        {
            if (data == null) return data;

            Camera mainCamera = Camera.main;
            data.mainCamera = new CameraData(mainCamera);

            return data;
        }

        private static GameData SetPlayersData(GameData data)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();
            if (data == null || players == null) return data;


            data.players = new List<Player>(players).Select(player => player.GetData()).ToArray();

            return data;
        }

        private static GameData SetTargetManagerData (GameData data)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();
            if (data == null || players == null) return data;

            TargetManagerData targetManagerData = null;
            foreach (var player in players)
            {
                if (player.human)
                {
                    targetManagerData = player.GetComponentInChildren<TargetManager>().GetData();
                    break;
                }
            }

            data.targetManager = targetManagerData;

            return data;
        }

        private static GameData SetEventObjectData (GameData data)
        {
            List<EventObject> eventObjects = new List<EventObject>(GameObject.FindObjectsOfType<EventObject>());

            data.eventObjects = eventObjects.Select(obj => ((EventObject)obj).GetData()).ToArray();

            return data;
        }
    }
}
