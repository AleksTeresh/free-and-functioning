using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class PauseMenu : MonoBehaviour {

        private List<Button> buttons;

        private void Start()
        {
            buttons = new List<Button>(GetComponentsInChildren<Button>());

            if (buttons.Count > 0)
            {
                buttons[0].onClick.AddListener(DestroySelf);
                buttons[0].Select();
            }
        }

        public void DestroySelf()
        {

            Time.timeScale = 1f;
            Destroy(this.gameObject);
        }
    }
}
