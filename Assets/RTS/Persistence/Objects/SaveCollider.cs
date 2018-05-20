using UnityEngine;
using Events;
using RTS;
using UnityEngine.SceneManagement;

namespace Persistence
{
    public class SaveCollider : EventObject
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!triggerred)
            {
                triggerred = true;
                SaveManager.SaveGame(SceneManager.GetActiveScene().name + Constants.SAVE_FILENAME_POSTFIX);
            }
        }
    }
}
