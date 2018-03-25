using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    public float velocity = 1;
    public int damage = 1;

    public Player Player { get; set; }

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

    private void InflictDamage(WorldObject collidedObject)
    {
        if (collidedObject) collidedObject.TakeDamage(damage);
    }
    
    /*
    private bool HitSomething()
    {
        if (target && target.GetSelectionBounds().Contains(transform.position)) return true;
        return false;
    }   */
}