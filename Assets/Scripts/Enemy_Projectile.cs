using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{
    public Vector3 shootDirection;
    bool shooted = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Shoot(Vector3 direction)
    {
        shooted = true;
        shootDirection = direction;

        
        //distrugge dopo 10 secondi
        //Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (shooted)
            transform.position += shootDirection * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("eh la madòna, go tocào " + other.name);
        MainCharacter tPlayer = other.GetComponent<MainCharacter>();
        if (tPlayer)
        {
            //HO COLPITO
            tPlayer.OnHitSuffered();
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}

