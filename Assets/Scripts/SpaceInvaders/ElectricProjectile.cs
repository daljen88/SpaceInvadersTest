using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricProjectile : WeaponProjectile
{
    [SerializeField] private float thisSpeed = 7.5f;
    public override float Speed => baseSpeed * thisSpeed;
    //public override int ShotDamage => baseShotDamage * 2;
    [SerializeField] private int thisShotDamage = 1;
    public override int ShotDamage { get { return baseShotDamage * thisShotDamage; } }

    public override Vector3 DirectionVector => baseDirectionVector * 1f;

    public ElectricProjectile()
    {
        //ShotDamage = ShotDamage * 2;
    }

    public override void Shoot(Vector3 dirVector, int damageMulti)
    {
        base.Shoot(dirVector, damageMulti);
        Destroy(gameObject, 15);
    }
    public override void UpdateMovement()
    {
        base.UpdateMovement();
    }

    public override void OnTriggerLogic(Collider entering)
    {
        base.OnTriggerLogic(entering);
        if (tEnterEnemy != null)
        {
            tEnterEnemy.OnHitSuffered(hittingShotDamage);
            Destroy(gameObject);
            //si muove grazie a shoot, allora lo metto false
            shooted = false;
        }
    }
}
