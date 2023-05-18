using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponProjectile : MonoBehaviour, IShootable
{

    protected Vector3 movementVector;

    [SerializeField] protected Vector3 baseDirectionVector=Vector3.up;
    public abstract Vector3 DirectionVector { get; }

    protected float baseSpeed = 1;
    public abstract float Speed { get; }

    protected int baseShotDamage=1;
    public abstract int ShotDamage { get;}

    protected int hittingShotDamage;

    protected bool shooted = false;
    protected bool hit = false;
    //public List<AudioClip> audioClips;
    protected IHittable tEnterEnemy;
    protected IHittable tExitEnemy;

    void Start()
    {
        
    }

    void Update()
    {
        UpdateMovement();
    }

    private void OnTriggerEnter(Collider entering)
    {
        OnTriggerLogic(entering);
    }
   
    private void OnTriggerExit(Collider exiting)
    {
        OnExitTriggerLogic(exiting);
    }

    public virtual void UpdateMovement()
    {
        if (shooted)
            transform.position += movementVector * Time.deltaTime;
    }

    public virtual void Shoot(int weaponMulti=1)
    {
        hittingShotDamage = weaponMulti * ShotDamage;
        shooted = true;
        //Vettore velocità totale(verso alto => X=0;Y=1)
        movementVector=DirectionVector*Speed;
    }

    public virtual void Shoot(Vector3 weaponDirVector, int weaponMulti = 1)
    {
        hittingShotDamage = weaponMulti * ShotDamage;
        shooted = true;
        movementVector = weaponDirVector * Speed;
        //audioClips.Clear();
        //audioPlayShot.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioPlayShot.Play();
        //distrugge dopo 10 secondi
        //Destroy(gameObject, 10);
    }

    public virtual void OnTriggerLogic(Collider entering)
    {
        Debug.Log("eh la madòna, go tocào " + entering.name);
        tEnterEnemy = entering.GetComponent<IHittable>();
        //enemy = other.GetComponent<Enemy>();
        //if (tEnterEnemy != null && tEnterEnemy != tExitEnemy /*&& !hit*/)
        //{
        //    //HO COLPITO
        //    hit = true;
        //    tEnterEnemy.OnHitSuffered(hittingShotDamage);
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
        //}
    }
    public virtual void OnExitTriggerLogic(Collider exiting)
    {
        return;
    }
}
