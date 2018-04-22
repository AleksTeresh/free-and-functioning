using UnityEngine;
using UnityEditor;
using Abilities;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/BossAI/Perses/DBWeaponUseAttackDown")]
public class DBWeaponUseAttackDownAction : BossPartAction
{
    protected override void DoAction(BossPartStateController controller)
    {
        BossPart bossPart = controller.bossPart;

        Ability ability = AbilityUtils.FindAbilityByName(
            "DBEnemyAttackDownAbilityMulti",
            bossPart.GetAbilityAgent().abilitiesMulti
        );
        var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, bossPart.transform.position, ability.range);

        // wait till at least 2 targets are reachable
        if (ability != null && ability.IsReady() && reachabeEnemies.Count > 1)
        {
            // bossPart.abilityToUse = ability;
        }
    }
}