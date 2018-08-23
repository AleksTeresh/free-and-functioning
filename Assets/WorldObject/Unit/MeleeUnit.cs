using RTS;

public class MeleeUnit : Unit
{
    protected override void UseWeapon(WorldObject target)
    {
        base.UseWeapon(target);
        target.TakeDamage(damage, AttackType.Melee);
    }
}
