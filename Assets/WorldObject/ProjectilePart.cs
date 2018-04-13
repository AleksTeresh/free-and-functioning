using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePart : MonoBehaviour {

    private Projectile parent;

    void Awake()
    {
        parent = GetComponentInParent<Projectile>();
    }

    void OnTriggerEnter(Collider other)
    {
        var worldObject = other.gameObject.GetComponentInParent<WorldObject>();

        if (worldObject)
        {
            bool collidedObjectHasPlayer = worldObject.GetPlayer() != null;
            bool projectileHasPlayer = this.parent.Player != null;

            // if the player of the projectile is not the same as the player of the collided object
//            if (!collidedObjectHasPlayer || !projectileHasPlayer || worldObject.GetPlayer().username != this.parent.Player.username)
            if (worldObject == parent.target)
            {
                parent.HandleCollision(worldObject);
            }
        }
    }
}
