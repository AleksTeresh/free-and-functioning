using RTS;

public class PersesHead : SpawnHouse {
    public override void TakeDamage(int attackPoints, AttackType attackType)
    {
        var hitPoints = this.hitPoints;

        base.TakeDamage(attackPoints, attackType);

        if (player && hitPoints <= 0)
        {
            var spawnPoint = WorkManager.GetClosestPointOnNavMesh(transform.position, "Walkable", 30);

            if (spawnPoint.HasValue)
            {
                player.AddUnit(
                    rangeSwarmling.name,
                    spawnPoint.Value,
                    spawnPoint.Value,
                    transform.rotation,
                    this
                );
            }
        }
    }
}
