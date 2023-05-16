using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefence_TowerAttack : MonoBehaviour
{

    public ELEMENT myElement;
    public float damage;
    public float speed;
    public TowerDefence_Enemy targetEnemy;

    public void Shoot(TowerDefence_Enemy enemy, TowerDefence_Tower tower)
    {
        myElement=tower.myElement;
        damage=tower.damage;
        speed = tower.shotSpeed;

        targetEnemy = enemy;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //controlla se esiste enemy, altrimenti non fa nulla(spegne proiettili)
        if(targetEnemy)
        {
            //il delta max per frame è dato dalla speed projetile * time.deltatime, cioè se raggiunge prima l'obiettivo si ferma e non va oltre
            transform.position= Vector3.MoveTowards(transform.position, targetEnemy.transform.position, speed*Time.deltaTime);

            //se il nemico è a meno di 0.1 è considerato colpito
            if(Vector3.Distance(transform.position, targetEnemy.transform.position)<0.1f)
            {
                targetEnemy.OnHitSuffered(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            //se nemico esce dallo schermo o viene colpito ed eliminato da altro proiettile spegne proiettile
            gameObject.SetActive(false);
        }
    }
    
}
