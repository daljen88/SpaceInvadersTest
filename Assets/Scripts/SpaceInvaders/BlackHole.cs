using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackHole : WeaponProjectile
{
    [SerializeField] private float thisSpeed = 1f;
    public override float Speed => baseSpeed * thisSpeed;
    private float gForceSpeed = 4;
    //public override int ShotDamage => baseShotDamage * 2;
    [SerializeField] private int thisShotDamage = 1;
    public override int ShotDamage { get { return baseShotDamage * thisShotDamage; } }

    public override Vector3 DirectionVector => baseDirectionVector * 1f;

    private int enemiesTouched=0;
    private int swallowedEnemies = 0;
    [SerializeField] private int explosionDamage = 8;

    GameObject targetEnemy;
    GameObject targetEnemy2;
    GameObject targetEnemy3;
    protected List<EnemyClass> swallowedTargets = new List<EnemyClass>();
    //Tweener twScale;
    //Tweener twRotate;
    protected Coroutine spaghettiRoutine1 ;
    protected Coroutine spaghettiRoutine2 ;
    protected Coroutine spaghettiRoutine3 ;
    protected float Condition => Vector3.Distance(transform.position, targetEnemy.transform.position) - .2f;
    public BlackHole()
    {
        //swallowedTargets;
        //ShotDamage = ShotDamage * 2;
    }

    public override void Shoot(Vector3 dirVector, int damageMulti)
    {
        base.Shoot(dirVector, damageMulti);
        Destroy(gameObject, 35);
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
            if (targetEnemy)
            {
                //se il trascinamento lo setto a Speed * 1 con distanza <0.1 li trascina in alto fuori schermo
                targetEnemy.transform.position = Vector3.MoveTowards(targetEnemy.transform.position, transform.position, gForceSpeed * Time.deltaTime);
                if (spaghettiRoutine1 == null)
                {
                    spaghettiRoutine1 = StartCoroutine(SpaghettificationCoroutine1(targetEnemy));
                }
                if (Vector3.Distance(transform.position, targetEnemy.transform.position) < 0.2f)
                {
                    targetEnemy.GetComponent<EnemyClass>().OnHitSuffered(explosionDamage);
                    swallowedEnemies++;
                    //Destroy(gameObject,2f);
                }
            }
            if (targetEnemy2)
            {
                targetEnemy2.transform.position = Vector3.MoveTowards(targetEnemy2.transform.position, transform.position, gForceSpeed * Time.deltaTime);
                if (spaghettiRoutine2 == null)
                {
                    spaghettiRoutine2 = StartCoroutine(SpaghettificationCoroutine2(targetEnemy2));
                }
                if (Vector3.Distance(transform.position, targetEnemy2.transform.position) < 0.2f)
                {
                    targetEnemy2.GetComponent<EnemyClass>().OnHitSuffered(explosionDamage);
                    swallowedEnemies++;
                    //Destroy(gameObject,2f);
                }
            }
            if (targetEnemy3)
            {
                targetEnemy3.transform.position = Vector3.MoveTowards(targetEnemy3.transform.position, transform.position, gForceSpeed * Time.deltaTime);
                if (spaghettiRoutine3 == null)
                {
                    spaghettiRoutine3 = StartCoroutine(SpaghettificationCoroutine3(targetEnemy3));
                }
                if (Vector3.Distance(transform.position, targetEnemy3.transform.position) < 0.2f)
                {
                    targetEnemy3.GetComponent<EnemyClass>().OnHitSuffered(explosionDamage);
                    swallowedEnemies++;
                    //Destroy(gameObject,2f);
                }
            }
        }
        else if(swallowedEnemies>=3)
        {
            Destroy(gameObject, .5f);
        }
        //if (targetEnemy || targetEnemy2 || targetEnemy3)
        //{
            //il delta max per frame è dato dalla speed projetile * time.deltatime, cioè se raggiunge prima l'obiettivo si ferma e non va oltre
           

            

            //se il nemico è a meno di 0.1 è considerato colpito
            
        //}
        //else
        //{
        //    //se nemico esce dallo schermo o viene colpito ed eliminato da altro proiettile spegne proiettile
        //    //gameObject.SetActive(false);
        //}
    }
    public IEnumerator SpaghettificationCoroutine1(GameObject _targetEnemy)
    {
        //while (!IsCollected)
        //{
            //SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
            _targetEnemy.transform.DORotate(new Vector3(0,0,330), .5f, RotateMode.FastBeyond360);
            _targetEnemy.transform.DOScale(Vector3.zero, .5f);
            yield return new WaitForSeconds(.5f);
             spaghettiRoutine1 = null;
            

        //}
    }
    public IEnumerator SpaghettificationCoroutine2(GameObject _targetEnemy)
    {
        //while (!IsCollected)
        //{
        //SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
         _targetEnemy.transform.DORotate(new Vector3(0, 0, 330), .5f, RotateMode.FastBeyond360);
         _targetEnemy.transform.DOScale(Vector3.zero,.5f);
        yield return new WaitForSeconds(.5f/*(Vector3.Distance(transform.position, _targetEnemy.transform.position) - .2f) / (gForceSpeed)*/);
        
         spaghettiRoutine2 = null;
        

        //}
    }
    public IEnumerator SpaghettificationCoroutine3(GameObject _targetEnemy)
    {
        //while (!IsCollected)
        //{
        //SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
         _targetEnemy.transform.DORotate(new Vector3(0, 0, 330), .5f, RotateMode.FastBeyond360);
         _targetEnemy.transform.DOScale(Vector3.zero, .5f);
        yield return new WaitForSeconds(.5f);
        spaghettiRoutine3 = null;

        //}
    }

    private void StartExpandingRay()
    {
        //targetEnemy = null;
        //targetEnemy2 = null;
        //targetEnemy3 = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, 1 << 9);

        if (colliders.Length > 0)
        {
            for (int i = 0; i <= colliders.Length-1; i++)
            {
                if (swallowedTargets.Count==0)
                {
                    targetEnemy = colliders[i].gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
                    swallowedTargets.Add(targetEnemy.GetComponent<EnemyClass>());
                }
                else if (swallowedTargets.Count == 1)
                {
                    targetEnemy2 = colliders[i].gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
                    swallowedTargets.Add(targetEnemy2.GetComponent<EnemyClass>());

                }
                else if (swallowedTargets.Count == 2)
                {
                    targetEnemy3 = colliders[i].gameObject/*.GetComponent<TowerDefence_Enemy>()*/;
                    swallowedTargets.Add(targetEnemy3.GetComponent<EnemyClass>());

                }
                else { return; }
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
