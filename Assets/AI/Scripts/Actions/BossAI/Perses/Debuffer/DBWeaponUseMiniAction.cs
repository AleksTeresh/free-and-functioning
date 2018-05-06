using UnityEngine;
using System.Collections;
using Abilities;
using RTS;

namespace AI.Perses
{
    [CreateAssetMenu(menuName = "AI/Actions/BossAI/Perses/Debuffer/DBWeaponUseMini")]
    public class DBWeaponUseMiniAction : BossPartAction
    {
        protected override void DoAction(BossPartStateController controller)
        {
            BossPart bossPart = controller.bossPart;

            Ability ability = AbilityUtils.FindAbilityByName(
                "Boss_MiniDebuffAoeAbility",
                bossPart.GetAbilityAgent().abilitiesMulti
            );

            if (!ability || !(ability is AoeAbility))
            {
                return;
            }
            AoeAbility aoeAbility = (AoeAbility)ability;

            var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, bossPart.transform.position, ability.range);

            // wait till at least 2 targets are reachable
            if (ability != null && ability.IsReady() && reachabeEnemies.Count > 1)
            {
                var abilityTarget = AoeUtils.GetAoeTargetPosition(
                        ability.range,
                        reachabeEnemies,
                        bossPart.GetPlayer()
                );

                bossPart.UseAbility(abilityTarget, aoeAbility);
            }
        }
    }
}

