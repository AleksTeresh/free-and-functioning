using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class ConfuseStatus : AiStateStatus
    {
        private Player confusedPlayer;
        private Units confusedUnitsWrapper;

        private Transform initialParent;

        protected override void OnStatusStart()
        {
            base.OnStatusStart();

            CreateNewPlayer(target);

            if (target && confusedPlayer && confusedUnitsWrapper)
            {
                initialParent = target.transform.parent;
                target.transform.parent = confusedUnitsWrapper.transform;

                var targetStateController = target.GetStateController();

                if (targetStateController)
                {
                    targetStateController.chaseTarget = null;
                    targetStateController.TransitionToState(ResourceManager.GetAiState(targetStateController.GetDefaultState().name));
                }

                target.SetPlayer();
                target.SetTeamColor();
            }
        }

        protected override void OnStatusEnd()
        {
            base.OnStatusStart();

            if (target && initialParent)
            {
                target.transform.parent = initialParent;

                target.SetPlayer();
                target.SetTeamColor();

                RemovePlayer();
            }
        }

        private void RemovePlayer()
        {
            if (confusedPlayer)
            {
                Destroy(confusedPlayer.gameObject);
            }
        }

        private void CreateNewPlayer(WorldObject target)
        {
            if (!target) return;

            var confusedPlayerObject = (GameObject)Instantiate(ResourceManager.GetEnemyObject(), target.GetPlayer().transform.position, target.GetPlayer().transform.rotation);
            confusedPlayer = confusedPlayerObject.GetComponent<Player>();
            confusedPlayer.teamColor = Color.black;
            confusedPlayer.human = false;
            confusedPlayer.username = "Confused Player " + target.ObjectId;

            if (confusedPlayer)
            {
                confusedUnitsWrapper = confusedPlayer.GetComponentInChildren<Units>();
            }
        }
    }
}

