using RTS.Constants;
using UnityEngine;

namespace RTS
{
    public class Gamepad : MonoBehaviour
    {
        private static GameObject instance;

        private static bool wasWinSelect13AxisSwitchedToActive = false;
        private static bool wasWinSelect24AxisSwitchedToActive = false;
        private static bool wasWinAttackModeAxisSwitchedToActive = false;
        private static bool wasWinSelectionModifierAxisSwitchedToActive = false;

        private static bool wasOSXAttackModeAxisSwitchedToActive = false;
        private static bool wasOSXSelectionModifierAxisSwitchedToActive = false;

        private static bool isWinSelect13AxixInUse = false;
        private static bool isWinSelect24AxisInUse = false;
        private static bool isWinAttackModeAxisInUse = false;
        private static bool isWinSelectionModifierAxisInUse = false;

        private static bool isOSXAttackModeAxisInUse = false;
        private static bool wasOSXAttackModeAxisEverUsed = false;
        private static bool isOSXSelectionModifierAxisInUse = false;
        private static bool wasOSXSelectionModifierAxisEverUsed = false;


        private Gamepad()
        {
        }

        private void Awake()
        {
            Debug.Log("Instantiated Gamepad singleton");
        }

        private void Update()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    HandleOSXAxisInteraction();
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    HandleWinAxisInteraction();
                    break;
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

        public static bool GetButton(string button)
        {
            CheckInstance();

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return GetOSXButton(button);
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return GetWindowsButton(button);
                default:
                    return false;
            }
        }

        private static bool GetOSXButtonDown(string button)
        {
            switch (button)
            {
                // Handle cases where axis should be treated as a button (DPad, RT, LT)
                case InputNames.ATTACK_MODE:
                case InputNames.SELECTION_MODIFIER:
                    return GetOSXAxisButtonDown(button);
                // Temporary fix till mapping if any necessary will be decided
                // case "Select5":
                case InputNames.ABILITY5:
                    return false;
                default:
                    return Input.GetButtonDown(InputNames.OSX_PREFIX + button);
            }
        }


        private static bool GetWindowsButtonDown(string button)
        {
            switch (button)
            {
                // Handle cases where axis should be treated as a button (DPad, RT, LT)
                case InputNames.SELECT1:
                case InputNames.SELECT2:
                case InputNames.SELECT3:
                case InputNames.SELECT4:
                case InputNames.ATTACK_MODE:
                case InputNames.SELECTION_MODIFIER:
                    return GetWindowsAxisButtonDown(button);
                // Temporary fix till mapping if any necessary will be decided
                // case "Select5":
                case InputNames.ABILITY5:
                    return false;
                default:
                    return Input.GetButtonDown(InputNames.WIN_PREFIX + button);
            }
        }

        private static bool GetOSXButton(string button)
        {
            switch (button)
            {
                // Handle cases where axis should be treated as a button (DPad, RT, LT)
                case InputNames.ATTACK_MODE:
                case InputNames.SELECTION_MODIFIER:
                    return GetOSXAxisButton(button);
                // Temporary fix till mapping if any necessary will be decided
                // case "Select5":
                case InputNames.ABILITY5:
                    return false;
                default:
                    return Input.GetButtonDown(InputNames.OSX_PREFIX + button);
            }
        }

        private static bool GetWindowsButton(string button)
        {
            switch (button)
            {
                // Handle cases where axis should be treated as a button (DPad, RT, LT)
                case InputNames.SELECT1:
                case InputNames.SELECT2:
                case InputNames.SELECT3:
                case InputNames.SELECT4:
                case InputNames.ATTACK_MODE:
                case InputNames.SELECTION_MODIFIER:
                    return GetWindowsAxisButton(button);
                // Temporary fix till mapping if any necessary will be decided
                // case "Select5":
                case InputNames.ABILITY5:
                    return false;
                default:
                    return Input.GetButton(InputNames.WIN_PREFIX + button);
            }
        }

        private void HandleOSXAxisInteraction()
        {
            if (wasOSXAttackModeAxisEverUsed)
            {
                if (Input.GetAxisRaw(InputNames.OSX_PREFIX + InputNames.ATTACK_MODE) != -1.0f)
                {
                    if (isOSXAttackModeAxisInUse == false)
                    {
                        isOSXAttackModeAxisInUse = true;
                        wasOSXAttackModeAxisSwitchedToActive = true;
                    }
                }

                if (Input.GetAxisRaw(InputNames.OSX_PREFIX + InputNames.ATTACK_MODE) == -1.0f)
                {
                    isOSXAttackModeAxisInUse = false;
                    wasOSXAttackModeAxisSwitchedToActive = false;
                }
            }
            else
            {
                if (Input.GetAxisRaw(InputNames.OSX_PREFIX + InputNames.ATTACK_MODE) != 0.0f)
                {
                    if (isOSXAttackModeAxisInUse == false)
                    {
                        isOSXAttackModeAxisInUse = true;
                        wasOSXAttackModeAxisSwitchedToActive = true;
                        wasOSXAttackModeAxisEverUsed = true;
                    }
                }
            }

            if (wasOSXSelectionModifierAxisEverUsed)
            {
                if (Input.GetAxisRaw(InputNames.OSX_PREFIX + InputNames.SELECTION_MODIFIER) != -1.0f)
                {
                    if (isOSXSelectionModifierAxisInUse == false)
                    {
                        isOSXSelectionModifierAxisInUse = true;
                        wasOSXSelectionModifierAxisSwitchedToActive = true;
                    }
                }

                if (Input.GetAxisRaw(InputNames.OSX_PREFIX + InputNames.SELECTION_MODIFIER) == -1.0f)
                {
                    isOSXSelectionModifierAxisInUse = false;
                    wasOSXSelectionModifierAxisSwitchedToActive = false;
                }
            }
            else
            {
                if (Input.GetAxisRaw(InputNames.OSX_PREFIX + InputNames.SELECTION_MODIFIER) != 0.0f)
                {
                    if (isOSXSelectionModifierAxisInUse == false)
                    {
                        isOSXSelectionModifierAxisInUse = true;
                        wasOSXSelectionModifierAxisSwitchedToActive = true;
                        wasOSXSelectionModifierAxisEverUsed = true;
                    }
                }
            }
        }

        private void HandleWinAxisInteraction()
        {
            if (Input.GetAxisRaw(InputNames.WIN_SELECT_1_3) != 0f)
            {
                if (isWinSelect13AxixInUse == false)
                {
                    isWinSelect13AxixInUse = true;
                    wasWinSelect13AxisSwitchedToActive = true;
                }
            }

            if (Input.GetAxisRaw(InputNames.WIN_SELECT_1_3) == 0f)
            {
                isWinSelect13AxixInUse = false;
                wasWinSelect13AxisSwitchedToActive = false;
            }

            if (Input.GetAxisRaw(InputNames.WIN_SELECT_2_4) != 0f)
            {
                if (isWinSelect24AxisInUse == false)
                {
                    isWinSelect24AxisInUse = true;
                    wasWinSelect24AxisSwitchedToActive = true;
                }
            }

            if (Input.GetAxisRaw(InputNames.WIN_SELECT_2_4) == 0f)
            {
                isWinSelect24AxisInUse = false;
                wasWinSelect24AxisSwitchedToActive = false;
            }

            if (Input.GetAxisRaw(InputNames.WIN_PREFIX + InputNames.ATTACK_MODE) != 0f)
            {
                if (isWinAttackModeAxisInUse == false)
                {
                    isWinAttackModeAxisInUse = true;
                    wasWinAttackModeAxisSwitchedToActive = true;
                }
            }

            if (Input.GetAxisRaw(InputNames.WIN_PREFIX + InputNames.ATTACK_MODE) == 0f)
            {
                isWinAttackModeAxisInUse = false;
                wasWinAttackModeAxisSwitchedToActive = false;
            }

            if (Input.GetAxisRaw(InputNames.WIN_PREFIX + InputNames.SELECTION_MODIFIER) != 0f)
            {
                if (isWinSelectionModifierAxisInUse == false)
                {
                    isWinSelectionModifierAxisInUse = true;
                    wasWinSelectionModifierAxisSwitchedToActive = true;
                }
            }

            if (Input.GetAxisRaw(InputNames.WIN_PREFIX + InputNames.SELECTION_MODIFIER) == 0f)
            {
                isWinSelectionModifierAxisInUse = false;
                wasWinSelectionModifierAxisSwitchedToActive = false;
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
            return Input.GetAxis(InputNames.WIN_PREFIX + axis);
        }

        private static float GetOSXAxis(string axis)
        {
            return Input.GetAxis(InputNames.OSX_PREFIX + axis);
        }

        private static bool GetOSXAxisButtonDown(string button)
        {
            if (button == InputNames.ATTACK_MODE && wasOSXAttackModeAxisSwitchedToActive)
            {
                wasOSXAttackModeAxisSwitchedToActive = false;
                return true;
            }

            if (button == InputNames.SELECTION_MODIFIER && wasOSXSelectionModifierAxisSwitchedToActive)
            {
                wasOSXSelectionModifierAxisSwitchedToActive = false;
                return true;
            }

            return false;
        }

        private static bool GetWindowsAxisButtonDown(string button)
        {
            if (wasWinSelect13AxisSwitchedToActive)
            {
                if (button == InputNames.SELECT1)
                {
                    if (Input.GetAxis(InputNames.WIN_SELECT_1_3) < 0)
                    {
                        wasWinSelect13AxisSwitchedToActive = false;
                        return true;
                    }
                }

                if (button == InputNames.SELECT3)
                {
                    if (Input.GetAxis(InputNames.WIN_SELECT_1_3) > 0)
                    {
                        wasWinSelect13AxisSwitchedToActive = false;
                        return true;
                    }
                }
            }

            if (wasWinSelect24AxisSwitchedToActive)
            {
                if (button == InputNames.SELECT2)
                {
                    if (Input.GetAxis(InputNames.WIN_SELECT_2_4) > 0)
                    {
                        wasWinSelect24AxisSwitchedToActive = false;
                        return true;
                    }
                }

                if (button == InputNames.SELECT4)
                {
                    if (Input.GetAxis(InputNames.WIN_SELECT_2_4) < 0)
                    {
                        wasWinSelect24AxisSwitchedToActive = false;
                        return true;
                    }
                }
            }

            if (button == InputNames.ATTACK_MODE && wasWinAttackModeAxisSwitchedToActive)
            {
                wasWinAttackModeAxisSwitchedToActive = false;
                return true;
            }

            if (button == InputNames.SELECTION_MODIFIER && wasWinSelectionModifierAxisSwitchedToActive)
            {
                wasWinSelectionModifierAxisSwitchedToActive = false;
                return true;
            }

            return false;
        }

        
        private static bool GetOSXAxisButton(string button)
        {
            if (button == InputNames.ATTACK_MODE && isOSXAttackModeAxisInUse)
            {
                return true;
            }

            if (button == InputNames.SELECTION_MODIFIER && isOSXSelectionModifierAxisInUse)
            {
                return true;
            }

            return false;
        }
        
        private static bool GetWindowsAxisButton(string button)
        {
            if (button == InputNames.SELECT1)
            {
                if (Input.GetAxis(InputNames.WIN_SELECT_1_3) < 0)
                {
                    return true;
                }
            }

            if (button == InputNames.SELECT3)
            {
                if (Input.GetAxis(InputNames.WIN_SELECT_1_3) > 0)
                {
                    return true;
                }
            }

            if (button == InputNames.SELECT2)
            {
                if (Input.GetAxis(InputNames.WIN_SELECT_2_4) > 0)
                {
                    return true;
                }
            }

            if (button == InputNames.SELECT4)
            {
                if (Input.GetAxis(InputNames.WIN_SELECT_2_4) < 0)
                {
                    return true;
                }
            }

            if (button == InputNames.ATTACK_MODE && isWinAttackModeAxisInUse)
            {
                return true;
            }

            if (button == InputNames.SELECTION_MODIFIER && isWinSelectionModifierAxisInUse)
            {
                return true;
            }

            return false;
        }
    }
}