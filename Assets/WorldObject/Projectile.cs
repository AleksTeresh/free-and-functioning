using UnityEngine;
using System.Collections;
using Statuses;
using RTS;

public class Projectile : MonoBehaviour
{

    public float velocity = 1;
    public Status[] statuses;

    public Player Player { get; set; }

    private int damage = 1;
    private float range = 1;
    private WorldObject target;

    void Update()
    {
        /*
        if (HitSomething())
        {
            
        } */
        if (range > 0)
        {
            float positionChange = Time.deltaTime * velocity;
            range -= positionChange;
            transform.position += (positionChange * transform.forward);
        }
        else
        {
            // destroy the projectile
            Destroy(gameObject);
        }
    }

    public void HandleCollision (WorldObject collidedObject)
    {
        InflictDamage(collidedObject);
        InflictStatuses(collidedObject);

        // destroy the projectile
        Destroy(gameObject);
    }

    public void HandleCollision(Projectile collidedProjectile)
    {
        // destroy both projectiles
        Destroy(collidedProjectile);
        Destroy(gameObject);
    }

    public void SetRange(float range)
    {
        this.range = range;
    }

    public void SetTarget(WorldObject target)
    {
        this.target = target;
    }

    public void SetDamage (int damage)
    {
        this.damage = damage;
    }

    private void InflictDamage(WorldObject collidedObject)
    {
        if (collidedObject) collidedObject.TakeDamage(damage, AttackType.Range);
    }

    private void InflictStatuses(WorldObject target)
    {
        if (target != null)
        {
            for (int i = 0; i<statuses.Length; i++)
            {
                StatusManager.InflictStatus(null, statuses[i], target);
            }
        }
    }
    
    /*
    private bool HitSomething()
    {
        if (target && target.GetSelectionBounds().Contains(transform.position)) return true;
        return false;
    }   */
}