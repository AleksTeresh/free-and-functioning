using UnityEngine;
using Abilities;
using RTS;

namespace AI.Perses
{
    [CreateAssetMenu(menuName = "AI/Actions/BossAI/Perses/Debuffer/DBWeaponUseAttackDown")]
    public class DBWeaponUseAttackDownAction : BossPartAction
    {
        protected override void DoAction(BossPartStateController controller)
        {
            BossPart bossPart = controller.bossPart;

            Ability ability = AbilityUtils.FindAbilityByName(
                "Boss_AttackDownAbilityMulti",
                bossPart.GetAbilityAgent().abilitiesMulti
            );
            var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, bossPart.transform.position, ability.range);

            // wait till at least 2 targets are reachable
            if (ability != null && ability.IsReady() && reachabeEnemies.Count > 1)
            {
                bossPart.UseAbility(reachabeEnemies, ability);
            }
        }
    }
}
