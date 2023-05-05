using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : WeaponProjectile
{
    public AudioSource audioPlayShot;
    public List<AudioClip> audioClips;
    public float speed = 6.6f;

    public override void Shoot(int damage=1)
    {
        base.Shoot(damage);
        movementVector = Vector3.up*speed;

        //audioClips.Clear();
        audioPlayShot.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioPlayShot.Play();
        //distrugge dopo 10 secondi
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
            //HO COLPITO
            tEnemy.OnHitSuffered(shotDamage);
            //cambia parent al particle system
            ParticleSystem tParticle = GetComponentInChildren<ParticleSystem>();
            SpriteRenderer trenderer = GetComponentInChildren<SpriteRenderer>();
            //così lo sposto al di fuori del parent
            tParticle.gameObject.transform.parent = transform.parent;
            //distrugge 1 secondo dopo, metti tempo della coda particellare
            Destroy(gameObject);
            Destroy(trenderer);
            Destroy(tParticle.gameObject, 4);
            //GetComponent<MeshRenderer>().enabled = false;
            //GetComponent<Collider>().enabled = false;
            //si muove grazie a shoot, allora lo metto false
            shooted = false;
        }

    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
