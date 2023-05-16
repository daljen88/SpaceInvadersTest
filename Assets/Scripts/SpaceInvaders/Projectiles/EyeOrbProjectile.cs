using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOrbProjectile : WeaponProjectile
{
    [SerializeField] private float thisSpeed = 15f;
    public override float Speed => baseSpeed * thisSpeed;
    //public override int ShotDamage => baseShotDamage * 2;
    [SerializeField] private int thisShotDamage = 2;
    public override int ShotDamage { get { return baseShotDamage * thisShotDamage; } }

    public override Vector3 DirectionVector => baseDirectionVector * 1f;

    public EyeOrbProjectile()
    {
        //ShotDamage = ShotDamage * 2;
    }

    public override void Shoot(Vector3 dirVector, int damageMulti)
    {
        base.Shoot(dirVector, damageMulti);

        GetComponent<Rigidbody>().AddForce(movementVector * Time.deltaTime, ForceMode.Impulse);

        Destroy(gameObject, 20);
    }
    public override void UpdateMovement()
    {
        //base.UpdateMovement();
    }

    public override void OnTriggerLogic(Collider entering)
    {
        base.OnTriggerLogic(entering);
        if (tEnterEnemy != null && tEnterEnemy != tExitEnemy /*&& !hit*/)
        {
            //HO COLPITO
            hit = true;
            tEnterEnemy.OnHitSuffered(hittingShotDamage);
        }
    }
    public override void OnExitTriggerLogic(Collider exiting)
    {
        tExitEnemy = exiting.GetComponent<IHittable>();
        //if (other.GetComponent<Enemy>()!=enemy)
        //if (tExitEnemy!= tEnterEnemy)
        hit = false;
    }
}
