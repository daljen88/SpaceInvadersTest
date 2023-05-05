using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGunProjectile : WeaponProjectile
{
    public override void Shoot(Vector3 moveVector, int damage)
    {
        base.Shoot(moveVector, damage);
        Destroy(gameObject,15);
    }
    public override void UpdateMovement()
    {
        base.UpdateMovement();
    }

    public override void OnTriggerLogic(Collider entering)
    {
        base.OnTriggerLogic(entering);
    }
}
