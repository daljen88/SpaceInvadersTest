using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : WeaponProjectile
{
    [SerializeField] private float thisSpeed = 1f;
    public override float Speed => baseSpeed * thisSpeed;
    //public override int ShotDamage => baseShotDamage * 2;
    [SerializeField] private int thisShotDamage = 1;
    public override int ShotDamage { get { return baseShotDamage * thisShotDamage; } }

    public override Vector3 DirectionVector => baseDirectionVector * 1f;

    private int enemiesTouched=0;
    private int swallowedEnemies = 0;
    [SerializeField] private int explosionDamage = 5;

    GameObject targetEnemy;
    GameObject targetEnemy2;
    GameObject targetEnemy3;


    public BlackHole()
    {
        //ShotDamage = ShotDamage * 2;
    }

    public override void Shoot(Vector3 dirVector, int damageMulti)
    {
        base.Shoot(dirVector, damageMulti);
        Destroy(gameObject, 25);
    }
    public override void UpdateMovement()
    {
        //base.UpdateMovement();


        if (shooted&&enemiesTouched<2)
            transform.position += movementVector * Time.deltaTime;
        else if(enemiesTouched>=2&&swallowedEnemies<3)
        {
            transform.position = transform.position;
            StartExpandingRay();
        }
        else if(swallowedEnemies>=3)
        {
            Destroy(gameObject, .5f);
        }
        if (targetEnemy || targetEnemy2 || targetEnemy3)
        {
            //il delta max per frame è dato dalla speed projetile * time.deltatime, cioè se raggiunge prima l'obiettivo si ferma e non va oltre
            if (targetEnemy)
            {
                targetEnemy.transform.position = Vector3.MoveTowards(targetEnemy.transform.position, transform.position, Speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetEnemy.transform.position) < 0.2f)
                {
                    swallowedEnemies++;
                    targetEnemy.GetComponent<Enemy>().OnHitSuffered(explosionDamage);
                    //Destroy(gameObject,2f);
                }
            }
            if (targetEnemy2)
            {
                targetEnemy2.transform.position = Vector3.MoveTowards(targetEnemy2.transform.position, transform.position, Speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetEnemy2.transform.position) < 0.2f)
                {
                    swallowedEnemies++;
                    targetEnemy2.GetComponent<Enemy>().OnHitSuffered(explosionDamage);
                    //Destroy(gameObject,2f);
                }
            }
            if (targetEnemy3)
            { 
                targetEnemy3.transform.position = Vector3.MoveTowards(targetEnemy3.transform.position, transform.position, Speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetEnemy3.transform.position) < 0.2f)
                {
                    swallowedEnemies++;
                    targetEnemy3.GetComponent<Enemy>().OnHitSuffered(explosionDamage);
                    //Destroy(gameObject,2f);
                }
            }

            //se il nemico è a meno di 0.1 è considerato colpito
            
        }
        else
        {
            //se nemico esce dallo schermo o viene colpito ed eliminato da altro proiettile spegne proiettile
            //gameObject.SetActive(false);
        }
    }

    private void StartExpandingRay()
    {
        //targetEnemy = null;
        //targetEnemy2 = null;
        //targetEnemy3 = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, 1 << 9);

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length-1; i++)
            {
                if (targetEnemy == null)
                    targetEnemy = colliders[i].gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
                else if (targetEnemy2 == null)
                    targetEnemy2 = colliders[i].gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
                else if (targetEnemy3 == null)
                    targetEnemy3 = colliders[i].gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
            }
        }
    }

    public override void OnTriggerLogic(Collider entering)
    {
        base.OnTriggerLogic(entering);
        if (tEnterEnemy != null && tEnterEnemy != tExitEnemy &&enemiesTouched<2/*&& !hit*/)
        {
            //HO COLPITO
            hit = true;
            tEnterEnemy.OnHitSuffered(hittingShotDamage);
            enemiesTouched++;
        }
        else
        {
            return;
        }
    }
    public override void OnExitTriggerLogic(Collider exiting)
    {
        tExitEnemy = exiting.GetComponent<IHittable>();
        //if (other.GetComponent<Enemy>()!=enemy)
        //if (tExitEnemy!= tEnterEnemy)
        hit = false;
    }
    public void FixedUpdate()
    {
        ////ogni frame, all'inizio del controllo, resetta i target enemy perchè se escono dal range vengono comunuque conteggiati
        //targetEnemy = null;
        //Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, 1 << 9);

        //if (colliders.Length > 0)
        //{
        //    targetEnemy = colliders[0].gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
        //    targetEnemy2 = colliders[1]?.gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
        //    targetEnemy3 = colliders[2]?.gameObject/*.GetComponent<TowerDefence_Enemy>()*/;


        //}
    }
}
