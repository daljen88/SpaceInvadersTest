using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyClass;

public abstract class EnemyClass : MonoBehaviour, IHittable
{
    //GAME OBJECTS
    public GameObject myProjectile;
    public ParticleSystem ExplosionTemplate;
    public List<AudioClip> audioClips;
    public AudioSource audioSrc;

    [Tooltip("baseSpeed = 1, shootCD = 1, damageMulty = 1")]
    [Header("BASE CLASS VALUES")]
    [SerializeField] protected float baseEnemySpeed = 1f;
    [SerializeField] protected float baseShootCooldown = 1f;
    [SerializeField] protected int baseEnemyDamageMultiplyer = 1;
    [SerializeField] protected float hitFxDuration = 0.25f;
    protected int enemyPointsValue/* = 666*/;
    protected int hp;
    public enum EnemyType { normalEnemy, bonusEnemy, bringerEnemy }
    public Material myMaterial, myHitTakenMaterial;
    public Color hitFxColor;

    [Header("ENEMY TYPE")]
    [SerializeField] protected EnemyType enemyType;
    [Header("THIS ENEMY TYPE DROPS")]
    public List<GameObject> drops;
    public List<GameObject> guns;
    public enum GunDrop { BigGun, ElectricGun, EyeOrbCannon}
    protected GunDrop gunType;
    protected IDictionary<GunDrop, GameObject> gunDrops;

    //CONTROL VARIABLES
    protected float shootTimer = 0;
    protected Tweener twScale;

    //ENEMY VALUES PROPERTIES
    protected abstract float ShootCooldown {get;}
    public abstract float EnemySpeed { get; protected set; }
    public abstract int Hp { get; set; }
    protected abstract int EnemyDamageMultiplyer { get; }
    //DROP CONDITION PROPERTIES
    protected bool EnemiesKilledMoreThanZero => UIManager.instance.totalEnemiesKilled != 0;
    protected abstract bool EnemiesKilledFirstDrop { get; }
    protected abstract bool EnemiesKilledSecondDrop { get; }
    protected abstract bool EnemiesKilledThirdDrop { get; }
    //RANDOM RANGE DROP CONDITION PROPERTIES
    protected abstract bool IsFirstDropRandomTrue { get; }
    protected abstract bool IsSecondDropRandomTrue { get; }
    protected abstract bool IsThirdDropRandomTrue { get; }

    //protected StateMachine SM;
    //public List<EnemyState> enemyStates = new List<EnemyState>();

    public EnemyClass()
    {

    }

    void Start()
    {
        StartRoutine();
    }

    public virtual void StartRoutine()
    {
        gunDrops = new Dictionary<GunDrop, GameObject>();
        int i = 0;
        foreach (GunDrop gunDropType in Enum.GetValues(typeof(GunDrop)))
        {
            if (i < guns.Count)//if i minore di guns count
            {
                gunDrops.Add(gunDropType, guns[i]); //adding a key/value using the Add() method
                i++;
            }
            else
            {
                i++;
                return;
            }
        }
    }

    void Update()
    {
        UpdateRoutine();
    }
    public virtual void UpdateRoutine()
    {
        transform.position += Vector3.right * EnemySpeed * Time.deltaTime;
    }

    public virtual void OnHitSuffered(int damage = 1)
    {
        Hp -= damage;

        HitAudioFX();

        if (Hp <= 0)
        {
            //MORTE ENEMY
            UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, enemyType.ToString() /*GetType().ToString()*/);
            //FX MORTE
            ExplosionFX();
            //GUN DROP
            GunDropLogic();
            DestroyThisEnemy();
        }
        else
        {
            //FX COLPO SUBITO
            HitTweenFX();
            StartCoroutine(HitColorCoroutine());
            //anche la coroutine si sovrappone se viene chiamata più volte, quindi va checkato se è già attiva e nel caso spegnerla

        }
    }

    public virtual void HitAudioFX()
    {
        audioSrc.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
        audioSrc.Play();
    }

    public virtual void ExplosionFX()
    {
        ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
        ps.Emit(60);
        Destroy(ps.gameObject, .55f);
    }

    public virtual void GunDropLogic()
    {
        if (FirstGunDropCondition() /*&&GameManager.Instance.typeGunPossessed.name!="BigGun"*/)
        {
            InstantiateGunDrop();
        }
        else if (SecondGunDropCondition())
        {
            InstantiateGunDrop();
        }
        else if (ThirdGunDropCondition())
        {
            InstantiateGunDrop();
        }
    }

    #region <GUN DROP CONDITIONS>
    public virtual bool FirstGunDropCondition()
    {
        if (EnemiesKilledFirstDrop && EnemiesKilledMoreThanZero && IsFirstDropRandomTrue)
        {
            gunType = GunDrop.BigGun;
            return true;
        }
        else
            return false;
    }
    public virtual bool SecondGunDropCondition()
    {
        if (EnemiesKilledSecondDrop && EnemiesKilledMoreThanZero && IsSecondDropRandomTrue)
        {
            gunType=GunDrop.ElectricGun;
            return true;
        }
        else
            return false;
    }
    public virtual bool ThirdGunDropCondition()
    {
        if (EnemiesKilledThirdDrop && EnemiesKilledMoreThanZero && IsThirdDropRandomTrue)
        {
            gunType=GunDrop.EyeOrbCannon;
            return true;
        }
        else
            return false;
    }
    #endregion

    public virtual void InstantiateGunDrop()
    {
        GameObject gun = Instantiate(gunDrops[gunType], transform.position, Quaternion.identity);
        WeaponsClass gunDropping = gun.GetComponent<WeaponsClass>();
        gunDropping?.Drop(Vector3.down);

        //IEnumerable<Type> typeArray= Assembly.GetAssembly(typeof(WeaponsClass)).GetTypes().Where(thisType => !thisType.IsAbstract && thisType.IsSubclassOf(typeof(WeaponsClass)));
        //Type gunType = Assembly.GetAssembly(typeof(WeaponsClass)).GetType("bigGun", false,false);
    }
    
    public virtual void DestroyThisEnemy()
    {
        Destroy(gameObject);
    }

    public virtual void HitTweenFX()
    {
        GetComponent<MeshRenderer>().material = myHitTakenMaterial;
        //la funzione va chiamata come stringa
        Invoke("SetNormalMaterial", 0.25f);
        //se il tween esiste ed è attivo, killa il tween precedente sennò si sovrappongono
        if (twScale != null && twScale.IsActive())
        {
            twScale.Kill();
            //risistema a dim originale se tween spento a metà
            transform.localScale = Vector3.one; //new vector3 (1,1,1);
        }
        transform.DOPunchPosition(Vector3.up, .25f, 2);
        twScale = transform.DOPunchScale(Vector3.one * 0.2f, hitFxDuration, 2);
    }

    IEnumerator HitColorCoroutine()
    {
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(hitFxColor, hitFxDuration / 2);
        yield return new WaitForSeconds(hitFxDuration / 2);
        tsprite.DOColor(Color.white, hitFxDuration / 2);
    }


    private void FixedUpdate()
    {
        FixedUpdateRoutine();
    }

    public virtual bool HasRaycastHit => Physics.Raycast(transform.position, Vector3.down, out hit, 100, 1 << 7);
    RaycastHit hit;

    public virtual void FixedUpdateRoutine()
    {
        #region summary
        //usiamo overload 15/16, con out dei dati in variabile Raycast "hit"
        //if(Physics.Raycast(transform.position, Vector3.down, out hit, 100,1<<7))
        //{
        //    hit.collider.GetComponent<MainCharacter>().OnHitSuffered();
        //}
        //ogni 0.5 secondi spara reycast
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red);
        #endregion

        shootTimer += Time.fixedDeltaTime;
        if (HasRaycastHit)
        {
            //Debug.Log("Raycast Hit");
            if (shootTimer >= ShootCooldown)
            {
                //Debug.Log("Shoot");
                InstatiateProjectile();

                shootTimer = 0;
                //Debug.Break();
            }
        }
    }

    public virtual void InstatiateProjectile()
    {
        GameObject tempProjectile = Instantiate(myProjectile, transform.position + Vector3.down, transform.rotation);
        Enemy_Projectile tempProj = tempProjectile.GetComponent<Enemy_Projectile>();
        tempProj.Shoot(Vector3.down * 4f, EnemyDamageMultiplyer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionRoutine(collision);
    }

    public virtual void OnCollisionRoutine(Collision _collision)
    {
        return;
    }
}
