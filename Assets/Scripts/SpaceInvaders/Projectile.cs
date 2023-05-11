using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : WeaponProjectile
{
    public AudioSource audioPlayShot;
    public List<AudioClip> audioClips;
    public override float Speed => speed * 6.6f;
    //public override int ShotDamage => shotDamage * 2;
    public override int ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    public override Vector3 DirectionVector => directionVector * 1f;

    public Projectile()
    {
        ShotDamage = ShotDamage * 1;
    }

    public override void Shoot(Vector3 dirVector, int damageMulti)
    {
        base.Shoot(dirVector, damageMulti);
        audioPlayShot.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioPlayShot.Play();
        Destroy(gameObject, 10);
    }
    public override void UpdateMovement()
    {
        base.UpdateMovement();
    }

    public override void OnTriggerLogic(Collider entering)
    {
        //base.OnTriggerLogic(entering);
        Debug.Log("eh la madòna, go tocào " + entering.name);
        IHittable tEnemy = entering.GetComponent<IHittable>();
        if (tEnemy != null)
        {
            //cambia parent al particle system
            ParticleSystem tParticle = GetComponentInChildren<ParticleSystem>();
            SpriteRenderer trenderer = GetComponentInChildren<SpriteRenderer>();
            //così lo sposto al di fuori del parent
            tParticle.gameObject.transform.parent = transform.parent;
            //HO COLPITO
            tEnemy.OnHitSuffered(shotDamage);
            //distrugge 1 secondo dopo, metti tempo della coda particellare
            Destroy(trenderer);
            Destroy(tParticle.gameObject, 4);
            Destroy(gameObject);
            //si muove grazie a shoot, allora lo metto false
            shooted = false;
        }
    }



    //public override float Speed => speed* 6.6f;
    ////public override int ShotDamage => shotDamage * 1;
    //public override int ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    //public override Vector3 DirectionVector => directionVector * 1f;

    ////public Vector3 directionVector = Vector3.up;

    //public Projectile()
    //{
    //    ShotDamage = ShotDamage * 1;
    //}

    //public override void Shoot(int damageMulti)
    //{
    //    base.Shoot(damageMulti);

    //    //audioClips.Clear();
    //    audioPlayShot.clip = audioClips[Random.Range(0, audioClips.Count)];
    //    audioPlayShot.Play();
    //    //distrugge dopo 10 secondi
    //    Destroy(gameObject, 10);
    //}

    //public override void UpdateMovement()
    //{
    //    base.UpdateMovement();
    //}

    //public override void OnTriggerLogic(Collider entering)
    //{
    //    //base.OnTriggerLogic(entering);
    //    Debug.Log("eh la madòna, go tocào " + entering.name);
    //    IHittable tEnemy = entering.GetComponent<IHittable>();
    //    if (tEnemy != null)
    //    {
    //        //cambia parent al particle system
    //        ParticleSystem tParticle = GetComponentInChildren<ParticleSystem>();
    //        SpriteRenderer trenderer = GetComponentInChildren<SpriteRenderer>();
    //        //così lo sposto al di fuori del parent
    //        tParticle.gameObject.transform.parent = transform.parent;
    //        //HO COLPITO
    //        tEnemy.OnHitSuffered(shotDamage);
    //        //distrugge 1 secondo dopo, metti tempo della coda particellare
    //        Destroy(trenderer);
    //        Destroy(tParticle.gameObject, 4);
    //        Destroy(gameObject);
    //        //si muove grazie a shoot, allora lo metto false
    //        shooted = false;
    //    }

    //}
    //void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}

}
