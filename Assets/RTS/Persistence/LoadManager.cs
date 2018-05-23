using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Events;
using RTS;

namespace Persistence
{
    public static class LoadManager
    {
        public static bool SaveExists (string filename)
        {
            char separator = Path.DirectorySeparatorChar;
            string path = "Saves" + separator + filename + ".dat";

            return File.Exists(path);
        }

        public static string GetSavedSceneName(string filename)
        {
            char separator = Path.DirectorySeparatorChar;
            string path = "Saves" + separator + filename + ".dat";
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf = PersistenceManager.AddSurrogatesToBinaryFormatter(bf);

                FileStream file = File.Open(path, FileMode.Open);
                GameData data = (GameData)bf.Deserialize(file);
                file.Close();

                return data.sceneName;
            }

            return null;
        }

        public static GameData LoadGameData(string filename)
        {
            char separator = Path.DirectorySeparatorChar;
            string path = "Saves" + separator + filename + ".dat";
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf = PersistenceManager.AddSurrogatesToBinaryFormatter(bf);

                FileStream file = File.Open(path, FileMode.Open);
                GameData data = (GameData)bf.Deserialize(file);
                file.Close();

                return data;
            }

            return null;
        }

        public static void LoadAssetsToScene (GameData data)
        {
            LoadEventObjectData(data);
            var foundPlayers = LoadPlayers(data);
            var humanPlayer = foundPlayers.Find(player => player.human);
            LoadCamera(data);
            LoadSun(data);
            // data = SetExtraLightData(data);
            LoadFogOfWar(data);
            LoadTargetManager(data, humanPlayer);
        }

        private static void LoadCamera (GameData data)
        {
            GameObject camera = Camera.main.gameObject;
            camera.transform.localPosition = data.mainCamera.position;
            camera.transform.localRotation = data.mainCamera.rotation;
            camera.transform.localScale = data.mainCamera.scale;
        }

        private static void LoadSun (GameData data)
        {
            var sun = GameObject.FindObjectOfType<Sun>();

            sun.SetData(data.sun);
        }

        private static List<Player> LoadPlayers(GameData data)
        {
            var foundPlayers = new List<Player>(GameObject.FindObjectsOfType<Player>());

            foreach (var playerData in data.players)
            {
                var player = foundPlayers.Find(pl => pl.username == playerData.username);
                player.SetData(playerData);

                // createdPlayers.Add(player);
            }

            return foundPlayers;
        }

        private static void LoadFogOfWar (GameData data)
        {
            var fogOfWar = GameObject.FindObjectOfType<FogOfWar>();
            fogOfWar.SetData(data.fogOfWar);
        }

        private static void LoadTargetManager(GameData data, Player humanPlayer)
        {
            var targetManager = humanPlayer.GetComponentInChildren<TargetManager>();
            targetManager.SetData(data.targetManager);
        }

        private static void LoadEventObjectData (GameData data)
        {
            new List<EventObject>(GameObject.FindObjectsOfType<EventObject>())
               .ForEach(obj => GameObject.DestroyImmediate(obj.gameObject));

            new List<EventObjectData>(data.eventObjects).ForEach(objData =>
            {
                GameObject newObject = (GameObject)GameObject.Instantiate(ResourceManager.GetWorldObject(objData.type), objData.position, objData.rotation);

                if (!newObject) return;

                // if there is Unit or Building component on the object, destroy it, because it will be created later as such
                if (newObject.GetComponent<Unit>() || newObject.GetComponent<Building>())
                {
                    GameObject.DestroyImmediate(newObject);
                }
                else
                {
                    EventObject relatedEventObj = newObject.GetComponent<EventObject>();
                    relatedEventObj.SetData(objData);
                }
            });
        }
    }
}
