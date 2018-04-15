using System;
using UnityEngine;

namespace RTS
{
    public class Gamepad : MonoBehaviour
    {
        private static GameObject instance;

        private static bool wasWinSelect13AxisSwitchedToActive = false;
        private static bool wasWinSelect24AxisSwitchedToActive = false;
        private static bool wasWinAttackModeAxisSwitchedToActive = false;

        private static bool isWinSelect13AxixInUse = false;
        private static bool isWinSelect24AxisInUse = false;
        private static bool isWinAttackModeAxisInUse = false;

        private Gamepad() { }

        private void Awake()
        {
            Debug.Log("Instantiated Gamepad singleton");
        }

        private void Update()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    HandleWinAxisInteraction();
                    break;
            }
        }

        private void HandleWinAxisInteraction()
        {
            if (Input.GetAxisRaw("Win Select 1 3") != 0f)
            {

                if (isWinSelect13AxixInUse == false)
                {
                    isWinSelect13AxixInUse = true;
                    wasWinSelect13AxisSwitchedToActive = true;
                }
            }
            if (Input.GetAxisRaw("Win Select 1 3") == 0f)
            {
                isWinSelect13AxixInUse = false;
                wasWinSelect13AxisSwitchedToActive = false;
            }

            if (Input.GetAxisRaw("Win Select 2 4") != 0f)
            {

                if (isWinSelect24AxisInUse == false)
                {
                    isWinSelect24AxisInUse = true;
                    wasWinSelect24AxisSwitchedToActive = true;
                }
            }
            if (Input.GetAxisRaw("Win Select 2 4") == 0f)
            {
                isWinSelect24AxisInUse = false;
                wasWinSelect24AxisSwitchedToActive = false;
            }

            if (Input.GetAxisRaw("Win Attack Mode") != 0f)
            {

                if (isWinAttackModeAxisInUse == false)
                {
                    isWinAttackModeAxisInUse = true;
                    wasWinAttackModeAxisSwitchedToActive = true;
                }
            }
            if (Input.GetAxisRaw("Win Attack Mode") == 0f)
            {
                isWinAttackModeAxisInUse = false;
                wasWinAttackModeAxisSwitchedToActive = false;
            }
        }

        public static bool GetButtonDown(string button)
        {
            CheckInstance();

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return GetOSXButtonDown(button);
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return GetWindowsButtonDown(button);
                default:
                    return false;
            }
        }


        public static float GetAxis(string axis)
        {
            CheckInstance();

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return GetOSXAxis(axis);
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return GetWindowsAxis(axis);
                default:
                    return 0.0f;
            }
        }

        private static void CheckInstance()
        {
            if (instance == null)
            {
                instance = new GameObject();
                instance.AddComponent<Gamepad>();
            }
        }

        private static float GetWindowsAxis(string axis)
        {
            return Input.GetAxis("Win " + axis);

        }

        private static float GetOSXAxis(string axis)
        {
            return Input.GetAxis("OSX " + axis);
        }

        private static bool GetWindowsButtonDown(string button)
        {
            switch (button)
            {
                // Handle cases where axis should be treated as a button (DPad, RT, LT)
                case "Select1":
                case "Select2":
                case "Select3":
                case "Select4":
                case "Attack Mode":
                    return GetWindowsSelectAxisButtonDown(button);
                // Temporary fix till mapping if any necessary will be decided
                case "Select5":
                case "Ability5":
                    return false;
                default:
                    return Input.GetButtonDown("Win " + button);
            }
        }

        private static bool GetWindowsSelectAxisButtonDown(string button)
        {
            if (wasWinSelect13AxisSwitchedToActive)
            {
                if (button == "Select1")
                {
                    if (Input.GetAxis("Win Select 1 3") < 0)
                    {
                        wasWinSelect13AxisSwitchedToActive = false;
                        return true;
                    }
                }

                if (button == "Select3")
                {
                    if (Input.GetAxis("Win Select 1 3") > 0)
                    {
                        wasWinSelect13AxisSwitchedToActive = false;
                        return true;
                    }
                }
            }

            if (wasWinSelect24AxisSwitchedToActive)
            {
                if (button == "Select2")
                {
                    if (Input.GetAxis("Win Select 2 4") > 0)
                    {
                        wasWinSelect24AxisSwitchedToActive = false;
                        return true;
                    }
                }

                if (button == "Select4")
                {
                    if (Input.GetAxis("Win Select 2 4") < 0)
                    {
                        wasWinSelect24AxisSwitchedToActive = false;
                        return true;
                    }
                }
            }

            if (button == "Attack Mode" && wasWinAttackModeAxisSwitchedToActive)
            {
                wasWinAttackModeAxisSwitchedToActive = false;
                return true;
            }

            return false;
        }

        private static bool GetOSXButtonDown(string button)
        {
            return Input.GetButtonDown("OSX " + button);
        }
    }
}
