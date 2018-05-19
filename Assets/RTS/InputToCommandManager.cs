using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Abilities;
using Formation;

namespace RTS
{
    public static class InputToCommandManager
    {
        public static void ToChaseState(TargetManager targetManager, StateController stateController, WorldObject chaseTarget)
        {
            if (!targetManager) return;

            // set clicked object as a target
            // stateController.chaseTarget = chaseTarget;
            targetManager.SingleTarget = chaseTarget;
            // transition to Chase Manual State
            string stateName = targetManager.InMultiMode
                ? "Chase Manual Multi"
                : "Chase Manual";
            stateController.TransitionToState(ResourceManager.GetAiState(stateName));
        }

        public static void AbilityHotkeyToState(TargetManager targetManager, UnitStateController stateController, int abilityIndex)
        {
            if (!targetManager) return;

            Unit unit = stateController.unit;

            if (unit.GetAbilityAgent().CanUseAbilitySlot(abilityIndex))
            {
                Ability ability = null;

                if (targetManager.InMultiMode)
                {
                    ability = stateController.unit
                        .GetAbilityAgent()
                        .FindAbilityMultiByIndex(abilityIndex);
                }
                else
                {
                    ability = stateController.unit
                        .GetAbilityAgent()
                        .FindAbilityByIndex(abilityIndex);
                }

                if (ability != null)
                {
                    if (ability.isSelfOnly)
                    {
                        stateController.abilityToUse = ability;
                        stateController.allyAbilityTarget = unit;

                        stateController.TransitionToState(ResourceManager.GetAiState("Ally Ability Chase Manual"));
                    }
                    else if (ability.isAllyTargetingAbility)
                    {
                        // Makes HotkeyUnitSelector to treat key press events as ability target selection
                        Player player = ability.user.GetPlayer();

                        // Healing ability targetting doesn't depend on global targetting mode, single / multi target selection
                        // happens later, when player hits space
                        player.selectedAllyTargettingAbility = stateController.unit
                            .GetAbilityAgent()
                            .FindAbilityByIndex(abilityIndex);
                        player.selectedAlliesTargettingAbility = stateController.unit
                            .GetAbilityAgent()
                            .FindAbilityMultiByIndex(abilityIndex);

                    }
                    else
                    {
                        string stateName = targetManager.InMultiMode
                        ? "Ability Chase Manual Multi"
                        : "Ability Chase Manual";
                        stateController.abilityToUse = ability;
                        stateController.TransitionToState(ResourceManager.GetAiState(stateName));
                    }
                }
            }
        }

        public static void AllyAbilityTargetSelectionToState(UnitStateController stateController, Ability ability, WorldObject allyTarget)
        {
            //			string stateName = targetManager.InMultiMode
            //						? "Ally Ability Chase Manual Multi"
            //						: "Ally Ability Chase Manual";
            stateController.abilityToUse = ability;
            stateController.allyAbilityTarget = allyTarget;

            stateController.TransitionToState(ResourceManager.GetAiState("Ally Ability Chase Manual"));
        }

        public static void AlliesAbilityTargetSelectionToState(UnitStateController stateController, Ability ability)
        {
            stateController.abilityToUse = ability;

            Debug.Log("Changed state to Allies Ability Use Manual");
            stateController.TransitionToState(ResourceManager.GetAiState("Allies Ability Use Manual"));
        }

        public static void ToBusyState(TargetManager targetManager, UnitStateController stateController, Vector3 destination)
        {
            if (!targetManager || !stateController || !stateController.navMeshAgent || !stateController.navMeshAgent.isActiveAndEnabled)
            {
                return;
            }    

            // add new destination for nav mesh agent
            stateController.navMeshAgent.SetDestination(destination);
            // transition to Chase Manual State
            string stateName = targetManager.InMultiMode
                ? "Busy Manual Multi"
                : "Busy Manual";
            stateController.TransitionToState(ResourceManager.GetAiState(stateName));
        }

        public static void SwitchAttackMode(TargetManager targetManager, List<UnitStateController> stateControllers)
        {
            if (!targetManager) return;

            if (targetManager.InMultiMode)
            {
                Debug.Log("Switched to single mode");
            }
            else
            {
                Debug.Log("Switched to multi mode");
            }

            targetManager.InMultiMode = !targetManager.InMultiMode;

            stateControllers.ForEach(SwitchAttackModeForOne);
        }

        public static void SwitchEnemy(TargetManager targetManager, List<WorldObject> enemies, int currentIdx)
        {
            if (enemies.Count == 0 || !targetManager) return;

            if (currentIdx == enemies.Count - 1 || currentIdx < 0)
            {
                targetManager.SingleTarget = enemies[0];
            }
            else
            {
                targetManager.SingleTarget = enemies[currentIdx + 1];
            }
        }

        public static void StopUnits (Player player, TargetManager targetManager)
        {
            if (!targetManager) return;

            string stateName = targetManager.InMultiMode
                ? "Stop Manual Multi"
                : "Stop Manual";

            player.selectedObjects
                .Where(p => p is Unit)
                .ToList()
                .ForEach(p => p.GetStateController().TransitionToState(ResourceManager.GetAiState(stateName)));
        }

        public static void SwitchHoldPosition (Player player)
        {
            if (!player.SelectedObject || !(player.SelectedObject is Unit))
            {
                return;
            }

            var currentMode = ((Unit)player.SelectedObject).holdingPosition;

            player.selectedObjects
                .Where(p => p is Unit)
                .Select(p => (Unit)p)
                .ToList()
                .ForEach(p =>
                {
                    p.holdingPosition = !currentMode;
                });
        }

        public static void SwitchFormationType (FormationManager formationManager)
        {
            if (!formationManager) return;

            if (formationManager.CurrentFormationType == FormationType.Auto)
            {
                formationManager.CurrentFormationType = FormationType.Manual;
            }
            else if (formationManager.CurrentFormationType == FormationType.Manual)
            {
                formationManager.CurrentFormationType = FormationType.Auto;
            }
        }

        private static void SwitchAttackModeForOne(UnitStateController stateController)
        {
            // if (stateController.unit.CanAttackMulti())
            // {
                switch (stateController.currentState.name)
                {
                    case "Idle Manual":
                        stateController.TransitionToState(ResourceManager.GetAiState("Idle Manual Multi"));
                        break;

                    case "Chase Manual":
                        stateController.TransitionToState(ResourceManager.GetAiState("Chase Manual Multi"));
                        break;

                    case "Busy Manual":
                        stateController.TransitionToState(ResourceManager.GetAiState("Busy Manual Multi"));
                        break;

                    case "Stop Manual":
                        stateController.TransitionToState(ResourceManager.GetAiState("Stop Manual Multi"));
                        break;

                    case "Ability Chase Manual":
                        stateController.TransitionToState(ResourceManager.GetAiState("Ability Chase Manual Multi"));
                        break;

                    case "Idle Manual Multi":
                        stateController.TransitionToState(ResourceManager.GetAiState("Idle Manual"));
                        break;

                    case "Chase Manual Multi":
                        stateController.TransitionToState(ResourceManager.GetAiState("Chase Manual"));
                        break;

                    case "Busy Manual Multi":
                        stateController.TransitionToState(ResourceManager.GetAiState("Busy Manual"));
                        break;

                    case "Stop Manual Multi":
                        stateController.TransitionToState(ResourceManager.GetAiState("Stop Manual"));
                        break;

                    case "Ability Chase Manual Multi":
                        stateController.TransitionToState(ResourceManager.GetAiState("Ability Chase Manual"));
                        break;

                default:
                        break;
                }
            // }

        }
    }
}

