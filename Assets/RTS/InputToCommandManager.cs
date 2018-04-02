using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;

namespace RTS
{
	public static class InputToCommandManager
	{
		public static void ToChaseState (TargetManager targetManager, StateController stateController, WorldObject chaseTarget)
		{
			// set clicked object as a target
			// stateController.chaseTarget = chaseTarget;
			targetManager.SingleTarget = chaseTarget;
			// transition to Chase Manual State
			string stateName = targetManager.InMultiMode
                ? "Chase Manual Multi"
                : "Chase Manual";
			stateController.TransitionToState (ResourceManager.GetAiState (stateName));
		}

		public static void AbilityHotkeyToState (TargetManager targetManager, StateController stateController, int abilityIndex)
		{

			Ability ability = stateController.unit.FindAbilityByIndex (abilityIndex);

			if (ability != null) {
				if (ability.IsAllyTargettingAbility ()) {
					// Makes HotkeyUnitSelector to treat key press events as ability target selection
					Player player = ability.user.GetPlayer ();
					player.selectedAllyTargettingAbility = ability;
				} else {
					string stateName = targetManager.InMultiMode
                        ? "Ability Chase Manual Multi"
                        : "Ability Chase Manual";
					stateController.abilityToUse = ability;
					stateController.TransitionToState (ResourceManager.GetAiState (stateName));
				}
			}
		}

		public static void AllyAbilityTargetSelectionToState (StateController stateController, Ability ability, WorldObject allyTarget)
		{
//			string stateName = targetManager.InMultiMode
//						? "Ally Ability Chase Manual Multi"
//						: "Ally Ability Chase Manual";
			stateController.abilityToUse = ability;
			stateController.allyAbilityTarget = allyTarget;

			Debug.Log ("Changed state to Ally Ability Chase Manual");
			stateController.TransitionToState (ResourceManager.GetAiState ("Ally Ability Chase Manual"));
		}

		public static void ToBusyState (TargetManager targetManager, StateController stateController, Vector3 destination)
		{
			// remove current target if present
			// stateController.chaseTarget = null;
			// add new destination for nav mesh agent
			stateController.navMeshAgent.SetDestination (destination);
			// transition to Chase Manual State
			string stateName = targetManager.InMultiMode
                ? "Busy Manual Multi"
                : "Busy Manual";
			stateController.TransitionToState (ResourceManager.GetAiState (stateName));
		}

		public static void SwitchAttackMode (TargetManager targetManager, List<StateController> stateControllers)
		{
			if (targetManager.InMultiMode) {
				Debug.Log ("Switched to single mode");
			} else {
				Debug.Log ("Switched to multi mode");
			}
            
			targetManager.InMultiMode = !targetManager.InMultiMode;

			stateControllers.ForEach (SwitchAttackModeForOne);
		}

		private static void SwitchAttackModeForOne (StateController stateController)
		{
			switch (stateController.currentState.name) {
			case "Idle Manual":
				stateController.TransitionToState (ResourceManager.GetAiState ("Idle Manual Multi"));
				break;

			case "Chase Manual":
				stateController.TransitionToState (ResourceManager.GetAiState ("Chase Manual Multi"));
				break;

			case "Busy Manual":
				stateController.TransitionToState (ResourceManager.GetAiState ("Busy Manual Multi"));
				break;

			case "Idle Manual Multi":
				stateController.TransitionToState (ResourceManager.GetAiState ("Idle Manual"));
				break;

			case "Chase Manual Multi":
				stateController.TransitionToState (ResourceManager.GetAiState ("Chase Manual"));
				break;

			case "Busy Manual Multi":
				stateController.TransitionToState (ResourceManager.GetAiState ("Busy Manual"));
				break;

			default:
				break;
			}
		}
	}
}

