using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectile : Projectile {
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tank projectile Enter");
    }
}
