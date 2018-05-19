using UnityEngine;
using Events;
using RTS;

namespace Persistence
{
    public class SaveCollider : EventObject
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!triggerred)
            {
                triggerred = true;
                SaveManager.SaveGame(Constants.LAST_SAVE_FILENAME);
            }
        }
    }
}
