using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_fireProjectile : MonoBehaviour, IShootable
{
    public Vector3 shootDirection;
    bool shooted = false;
    public AudioSource audioEnemyShot;
    public List<AudioClip> audioClips;
    public int projectileDamage = 3;

    void Start()
    {
    }

    public void Shoot(int enemyDamageMulti = 1)
    { }

    public void Shoot(Vector3 direction, int enemyDamageMulti = 1)
    {
        shooted = true;
        shootDirection = direction;
        projectileDamage *= enemyDamageMulti;
        //audioEnemyShot.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioEnemyShot.Play();

        //distrugge dopo 10 secondi
        //Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (shooted)
            transform.position += shootDirection * Time.deltaTime;

        if (transform.position.y < -6)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("eh la madòna, go tocào " + other.name);
        IHittable tPlayer = other.GetComponent<IHittable>();
        if (tPlayer != null)
        {
            //HO COLPITO
            tPlayer.OnHitSuffered(projectileDamage);
            Destroy(gameObject);
        }
    }
    //void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}

}


