using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 shootDirection;
    bool shooted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shoot(Vector3 direction)
    {
        shooted= true;
        shootDirection= direction;


        //distrugge dopo 10 secondi
        //Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(shooted)
        transform.position += shootDirection * Time.deltaTime;   
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("eh la madòna, go tocào "+other.name);
        Enemy tEnemy = other.GetComponent<Enemy>();
        if (tEnemy)
        {
            //HO COLPITO
            tEnemy.OnHitSuffered();
            //cambia parent al particle system
            ParticleSystem tParticle = GetComponentInChildren<ParticleSystem>();
            SpriteRenderer trenderer = GetComponentInChildren<SpriteRenderer>();
            //così lo sposto al di fuori del parent
            tParticle.gameObject.transform.parent = transform.parent;
            //distrugge 1 secondo dopo, metti tempo della coda particellare
            Destroy(gameObject);
            Destroy(trenderer);
            Destroy(tParticle.gameObject,4);
            //GetComponent<MeshRenderer>().enabled = false;
            //GetComponent<Collider>().enabled = false;
            //si muove grazie a shoot, allora lo metto false
            shooted=false;
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
