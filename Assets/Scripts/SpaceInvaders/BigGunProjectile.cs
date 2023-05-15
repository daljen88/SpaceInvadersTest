using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGunProjectile : WeaponProjectile
{
    [SerializeField] private float thisSpeed = 7.3f;
    public override float Speed => baseSpeed * thisSpeed;
    //public override int ShotDamage => baseShotDamage * 2;
    [SerializeField] private int thisShotDamage = 2;
    public override int ShotDamage { get { return baseShotDamage*thisShotDamage; } }

    public override Vector3 DirectionVector => baseDirectionVector * 1f;

    public BigGunProjectile()
    {
        //ShotDamage = ShotDamage * 2;
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
        if (tEnterEnemy != null && tEnterEnemy != tExitEnemy /*&& !hit*/)
        {
            //HO COLPITO
            hit = true;
            tEnterEnemy.OnHitSuffered(hittingShotDamage);
            //cambia parent al particle system
            //ParticleSystem tParticle = GetComponentInChildren<ParticleSystem>();
            //SpriteRenderer trenderer = GetComponentInChildren<SpriteRenderer>();
            //così lo sposto al di fuori del parent
            //tParticle.gameObject.transform.parent = transform.parent;
            //distrugge 1 secondo dopo, metti tempo della coda particellare
            //Destroy(trenderer);
            //Destroy(tParticle.gameObject, 4);
            //GetComponent<MeshRenderer>().enabled = false;
            //GetComponent<Collider>().enabled = false;

            //si muove grazie a shoot, allora lo metto false
            //FACCIO CHE NON SI DITRUGGE E OLTREPASSA NEMICI FACENDO 2 DANNO
            //Destroy(gameObject);
            //shooted = false;
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
