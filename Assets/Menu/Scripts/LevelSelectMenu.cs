using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LevelSelectMenu : MonoBehaviour
    {
        // private List<Button> buttons;

        private void Start()
        {
            var buttons = new List<Button>(GetComponentsInChildren<Button>());

            if (buttons.Count > 0)
            {
                buttons.First().Select();
            }
        }

        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}
