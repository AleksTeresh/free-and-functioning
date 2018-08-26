using UnityEngine;

public class ProjectilePart : MonoBehaviour {

    private Projectile parent;

    void Start()
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

            if (worldObject == parent.target)
            {
                parent.HandleCollision(worldObject);
            }
        }
    }
}
