using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statuses;

namespace RTS
{
    public static class ResourceManager
    {
        // mouse actions
        public static float ScrollSpeed { get { return 50; } }
        public static float RotateSpeed { get { return 100; } }
        public static float RotateAmount { get { return 10; } }
        public static int ScrollWidth { get { return 15; } }

        // building units
        public static int BuildSpeed { get { return 2; } }

        // camera
        public static float MinCameraHeight { get { return 10; } }
        public static float MaxCameraHeight { get { return 40; } }

        public static bool MenuOpen { get; set; }

        // health bar
        public static Texture2D HealthyTexture { get; private set; }
        public static Texture2D DamagedTexture { get; private set; }
        public static Texture2D CriticalTexture { get; private set; }

        private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
        public static Vector3 InvalidPosition { get { return invalidPosition; } }

        private static Bounds invalidBounds = new Bounds(new Vector3(-99999, -99999, -99999), new Vector3(0, 0, 0));
        public static Bounds InvalidBounds { get { return invalidBounds; } }

        // list that holds all game objects
        private static GameObjectList gameObjectList;

        public static GUISkin SelectBoxSkin { get; private set; }

        // pause menu
        private static float buttonHeight = 40;
        private static float headerHeight = 32, headerWidth = 256;
        private static float textHeight = 25, padding = 10;
        public static float PauseMenuHeight { get { return headerHeight + 2 * buttonHeight + 4 * padding; } }
        public static float MenuWidth { get { return headerWidth + 2 * padding; } }
        public static float ButtonHeight { get { return buttonHeight; } }
        public static float ButtonWidth { get { return (MenuWidth - 3 * padding) / 2; } }
        public static float HeaderHeight { get { return headerHeight; } }
        public static float HeaderWidth { get { return headerWidth; } }
        public static float TextHeight { get { return textHeight; } }
        public static float Padding { get { return padding; } }

        // save menu related
        public static string LevelName { get; set; }

        public static void StoreSelectBoxItems(GUISkin skin, Texture2D healthy, Texture2D damaged, Texture2D critical)
        {
            SelectBoxSkin = skin;
            HealthyTexture = healthy;
            DamagedTexture = damaged;
            CriticalTexture = critical;
        }

        public static void SetGameObjectList(GameObjectList objectList)
        {
            gameObjectList = objectList;
        }

        public static GameObject GetBuilding(string name)
        {
            return gameObjectList.GetBuilding(name);
        }

        public static GameObject GetUnit(string name)
        {
            return gameObjectList.GetUnit(name);
        }

        public static GameObject GetWorldObject(string name)
        {
            return gameObjectList.GetWorldObject(name);
        }

        public static GameObject GetPlayerObject()
        {
            return gameObjectList.GetPlayerObject();
        }

        public static Texture2D GetBuildImage(string name)
        {
            return gameObjectList.GetBuildImage(name);
        }

        public static State GetAiState(string name)
        {
            return gameObjectList.GetAiState(name);
        }

        public static GameObject GetUIElement(string name)
        {
            return gameObjectList.GetUIElement(name);
        }

        public static GameObject GetStatus(string name)
        {
            return gameObjectList.GetStatus(name);
        }

        public static int GetNewObjectId()
        {
            LevelLoader loader = (LevelLoader)GameObject.FindObjectOfType(typeof(LevelLoader));
            if (loader) return loader.GetNewObjectId();
            return -1;
        }

        public static Texture2D[] GetAvatars()
        {
            return gameObjectList.GetAvatars();
        }
    }
}

