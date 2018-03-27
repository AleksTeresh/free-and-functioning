using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Scan")]
public class ScanDecision : Decision {

    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = Scan(controller);

        return noEnemyInSight;
    }

    private bool Scan(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.transform.Rotate(0, controller.navMeshAgent.angularSpeed * Time.deltaTime, 0);

       return controller.CheckIfCountDownElapsed(5000); // TODO: replace with search duration of the correlated unit
    }
}
