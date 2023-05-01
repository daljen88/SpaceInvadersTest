using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{

    public Vector3 movementVector;
    public bool shooted = false;
    public int shotDamage;
    public bool hit = false;


    public void Shoot(Vector3 moveVector, int damage)
    {
        shotDamage = damage;
        shooted = true;
        movementVector = moveVector;
        ////audioClips.Clear();
        //audioPlayShot.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioPlayShot.Play();


        ////distrugge dopo 20 secondi
        Destroy(gameObject, 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shooted)
            transform.position += movementVector * Time.deltaTime;
    }
    IHittable tEnterEnemy;
    IHittable tExitEnemy;
    //Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("eh la madòna, go tocào " + other.name);
        /*IHittable */tEnterEnemy = other.GetComponent<IHittable>();
        //enemy = other.GetComponent<Enemy>();
        if (tEnterEnemy != null&& tEnterEnemy != tExitEnemy && hit==false)
        {
            //HO COLPITO
            hit = true;
            tEnterEnemy.OnHitSuffered(shotDamage);
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
   
    private void OnTriggerExit(Collider other)
    {
        tExitEnemy= other.GetComponent<IHittable>();
        //if (other.GetComponent<Enemy>()!=enemy)
        //if (tExitEnemy!= tEnterEnemy)
        hit = false;
    }

}
