using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/BossAI/Perses/Healer/HWeaponUseHeal")]
public class HWeaponUseHealAction : BossPartAction
{
    protected override void DoAction(BossPartStateController controller)
    {
        BossPart bossPart = controller.bossPart;

        Ability ability = AbilityUtils.FindAbilityByName(
            "Boss_HealingAbility",
            bossPart.GetAbilityAgent().abilities
        );
        var reachabeAllies = WorkManager.FindReachableObjects(controller.nearbyAllies, bossPart.transform.position, ability.range);
        var damagedMajorAllies = reachabeAllies
            .Where(p => p.hitPoints < p.maxHitPoints &&
                (!(p is Unit) || ((Unit)p).IsMajor())
            )
            .ToList();

        // wait till at least 2 targets are reachable
        if (ability != null && ability.IsReady() && damagedMajorAllies.Count > 0)
        {
            var mostDamagedAlliesHP = damagedMajorAllies.Min(p => p.hitPoints);
            var mostDamagedAllies = damagedMajorAllies.Find(p => p.hitPoints == mostDamagedAlliesHP);

            bossPart.UseAbility(mostDamagedAllies, ability);
        }
    }
}
