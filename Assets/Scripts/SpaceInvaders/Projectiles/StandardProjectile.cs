using System.Collections;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : WeaponProjectile
{
    public AudioSource audioPlayShot;
    public List<AudioClip> audioClips;
    [SerializeField] private float thisSpeed = 6.6f;
    public override float Speed => baseSpeed * thisSpeed;
    [SerializeField] private int thisShotDamage = 1;
    public override int ShotDamage { get { return baseShotDamage * thisShotDamage; } }

    public override Vector3 DirectionVector => baseDirectionVector * 1f;

    public StandardProjectile()
    {
        //senza variabile finale hittingShotDamage:
        //ShotDamage = ShotDamage * 1;
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
        base.OnTriggerLogic(entering);
        tEnterEnemy = entering.GetComponent<IHittable>();
        if (tEnterEnemy != null)
        {
            //cambia parent al particle system
            ParticleSystem tParticle = GetComponentInChildren<ParticleSystem>();
            SpriteRenderer trenderer = GetComponentInChildren<SpriteRenderer>();
            //così lo sposto al di fuori del parent
            if (tParticle.gameObject != null)
                tParticle.gameObject.transform.parent = transform.parent;
            //HO COLPITO
            tEnterEnemy.OnHitSuffered(hittingShotDamage);
            //distrugge 1 secondo dopo, metti tempo della coda particellare
            Destroy(trenderer);
            Destroy(tParticle.gameObject, 4);
            Destroy(gameObject);
            //si muove grazie a shoot, allora lo metto false
            shooted = false;
        }
    }
}
