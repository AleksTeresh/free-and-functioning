using System;
using UnityEngine;

namespace RTS
{
	public static class HotkeyUnitSelector
	{
		public static void HandleInput() {
			if (Input.GetButtonDown ("Select1")) {
				Debug.Log ("Select1 pressed");
			}

			if (Input.GetButtonDown ("Select2")) {
				Debug.Log ("Select2 pressed");
			}

			if (Input.GetButtonDown ("Select3")) {
				Debug.Log ("Select3 pressed");
			}

			if (Input.GetButtonDown ("Select4")) {
				Debug.Log ("Select4 pressed");
			}

			if (Input.GetButtonDown ("Select5")) {
				Debug.Log ("Select5 pressed");
			}
		}
	}
}
