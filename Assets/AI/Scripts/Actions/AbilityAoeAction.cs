﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Abilities;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/AbilityAoe")]
    public class AbilityAoeAction : UnitAction
    {
        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;
            Ability abilityToUse = controller.abilityToUse;

            if (!abilityToUse || !(abilityToUse is AoeAbility))
            {
                return;
            }

            AoeAbility aoeAbility = (AoeAbility)abilityToUse;

            if (controller.aoeAbilityTarget == new Vector3())
            {
                controller.unit.UseAbility(unit.transform.position, aoeAbility);
                controller.abilityToUse = null;
            }

            Vector3 currentPosition = unit.transform.position;
            Vector3 direction = controller.aoeAbilityTarget - currentPosition;

            if (direction.sqrMagnitude < aoeAbility.range * aoeAbility.range)
            {
                controller.unit.UseAbility(controller.aoeAbilityTarget, aoeAbility);

                if (!unit.aiming)
                {
                    controller.abilityToUse = null;
                }
            }
        }
    }
}

