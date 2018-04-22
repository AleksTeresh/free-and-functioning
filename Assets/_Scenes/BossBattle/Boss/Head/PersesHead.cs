using RTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PersesHead : SpawnHouse {

    public override void TakeDamage(int attackPoints, AttackType attackType)
    {
        var hitPoints = this.hitPoints;

        base.TakeDamage(attackPoints, attackType);

        if (player || hitPoints <= 0)
        {
            player.AddUnit(rangeSwarmling.name, transform.position, transform.position, transform.rotation, this);
        }
    }
}
