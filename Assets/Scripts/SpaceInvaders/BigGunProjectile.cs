using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGunProjectile : WeaponProjectile
{
    public override float Speed => speed * 7.3f;
    //public override int ShotDamage => shotDamage * 2;
    public override int ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    public override Vector3 DirectionVector => directionVector * 1f;

    public BigGunProjectile()
    {
        ShotDamage = ShotDamage * 2;
    }

    public override void Shoot(Vector3 dirVector, int damageMulti)
    {
        base.Shoot(dirVector, damageMulti);
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
