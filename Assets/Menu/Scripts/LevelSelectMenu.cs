using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LevelSelectMenu : MonoBehaviour
    {
        private void Start()
        {
            var relatedButtons = GetComponentsInChildren<Button>();

            if (relatedButtons.Length > 0)
            {
                relatedButtons.First().Select();
            }
        }

        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}
