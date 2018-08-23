using UnityEngine;
using Events;
using RTS.Constants;
using UnityEngine.SceneManagement;

namespace Persistence
{
    public class SaveCollider : EventObject
    {
        private void OnTriggerEnter(Collider other)
        {
            WorldObject playerObj = other.transform.parent.GetComponent<WorldObject>();

            // make sure save is only triggered by players units/buildings
            if (!triggerred && playerObj && playerObj.GetPlayer() && playerObj.GetPlayer().human)
            {
                triggerred = true;
                SaveManager.SaveGame(SceneManager.GetActiveScene().name + PersistanceConstants.SAVE_FILENAME_POSTFIX);
            }
        }
    }
}
