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
    public GameObject myProjectile;

    public ParticleSystem ExplosionTemplate;
    public List<AudioClip> audioClips;
    public AudioSource audioSrc;

    public Material myMaterial, myHitTakenMaterial;
    public Color hitFxColor;
    //private Vector3 startingScale;

    //protected StateMachine SM;
    //public List<EnemyState> enemyStates = new List<EnemyState>();

    [Header("ENEMY VALUES")]
    [Tooltip("speed = 6, hp = 3, shootCD = 0.5")]
    protected int enemyPointsValue/* = 666*/;
    public float baseShootCooldown = 1f;
    public float baseEnemySpeed = 1f;
    public int hp;
    public int baseEnemyDamageMultiplyer = 1;

    public abstract float ShootCooldown {get;}
    public abstract float EnemySpeed { get; set; }
    public abstract int Hp { get; set; }
    public abstract int EnemyDamageMultiplyer { get; }


    protected float shootTimer = 0;
    public float hitFxDuration = 0.25f;
    Tweener twScale;

    public List<GameObject> drops;

    public List<GameObject> guns;
    public enum GunDrop { BigGun, ElectricGun, EyeOrbCannon}
    //protected GunDrop gunTypez;

    public enum EnemyType { normalEnemy, bonusEnemy, bringerEnemy }
    protected EnemyType enemyType;

    protected IDictionary<string, GameObject> gunDrops/* = new Dictionary<int, GameObject>()*/;
    protected string gunToSpawn;
    
    public EnemyClass()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        StartRoutine();
    }
    public virtual void StartRoutine()
    {
        gunDrops = new Dictionary<string, GameObject>();
        int i = 0;
        foreach (string gunType in Enum.GetNames(typeof(GunDrop)))
        {
            //foreach (var gunObj in guns)
            //{

            if (i<guns.Count)//if i minore di guns count
            {
                gunDrops.Add(gunType, guns[i]); //adding a key/value using the Add() method
                i++;
            }
            else
            {
                i++;
                return;
            }
                //gunDrops.Add(GunDrop.Electric_Gun, guns[1]);
                //gunDrops.Add(GunDrop.EyeOrb_Cannon, guns[2]);
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoutine();
    }
    public virtual void UpdateRoutine()
    {
        transform.position += Vector3.right * EnemySpeed * Time.deltaTime;
    }

    public virtual void ExplosionFX()
    {
        ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
        //controllo particella da codice
        ps.Emit(60);
        Destroy(ps.gameObject, .5f);
    }

    public virtual bool FirstGunDropCondition()
    {
        if (UIManager.instance.totalEnemiesKilled % 6 == 0 && UIManager.instance.totalEnemiesKilled != 0 && UnityEngine.Random.Range(0, 11) < 8)
        {
            gunToSpawn = "BigGun";
            return true;
        }
        else
            return false;
    }
    public virtual bool SecondGunDropCondition()
    {
        if (UIManager.instance.totalEnemiesKilled % 50 == 0 && UIManager.instance.totalEnemiesKilled != 0 && UnityEngine.Random.Range(0, 11) < 8)
        {
            gunToSpawn = "ElectricGun";
            return true;
        }
        else
            return false;
    }
    public virtual bool ThirdGunDropCondition()
    {
        if (UIManager.instance.totalEnemiesKilled % 9999 == 0 && UIManager.instance.totalEnemiesKilled != 0 && UnityEngine.Random.Range(0, 11) < 11)
        {
            gunToSpawn = "EyeOrbCannon";
            return true;
        }
        else
            return false;
    }
    public virtual void InstantiateGunDrop()
    {
        GameObject gun = Instantiate(gunDrops[gunToSpawn], transform.position, Quaternion.identity);
        WeaponsClass gunDropping = gun.GetComponent<WeaponsClass>();
        gunDropping?.Drop(Vector3.down);

        //Type gunkkk = Assembly.GetAssembly(typeof(WeaponsClass)).GetType("BigGun", false, false);
        //IEnumerable<Type> gunkkarray= Assembly.GetAssembly(typeof(WeaponsClass)).GetTypes().Where(thisType => !thisType.IsAbstract && thisType.IsSubclassOf(typeof(WeaponsClass)));

        //bigGunDropping.GetType().IsSubclassOf(typeof(WeaponsClass))
        //Type gunxxxk = bigGunDropping.GetType().IsSubclassOf(typeof(WeaponsClass))
        //bigGunDropping.GetType();/* .GetType(gunToSpawn,false,false);*/
        //bigGunz.GetType().GetNestedType()

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
        //if(UIManager.instance.totalEnemiesKilled % 5 == 0 && UIManager.instance.totalEnemiesKilled != 0&&GameManager.Instance.musicRadioCollected==false)
        //{
        //    GameObject musicRadio = Instantiate(drops[0], transform.position, Quaternion.identity);
        //    RadioDrop radioScript = musicRadio.GetComponent<RadioDrop>();
        //    radioScript.Drop(Vector3.down);
        //}
    }
    public virtual void OnHitSuffered(int damage = 1)
    {
        Hp -= damage;

        HitAudioFX();

        if (Hp <= 0)
        {
            //SCORE
            UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, enemyType.ToString() /*GetType().ToString()*/);
            //FX MORTE
            ExplosionFX();
            //GUN DROP
            GunDropLogic();
            //Destroy(gameObject);
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

    public virtual void DestroyThisEnemy()
    {
        Destroy(gameObject);
    }


    private void FixedUpdate()
    {
        FixedUpdateRoutine();

    }
    RaycastHit hit;
    public virtual void FixedUpdateRoutine()
    {
        //usiamo overload 15/16, con out dei dati in variabile Raycast "hit"
        //if(Physics.Raycast(transform.position, Vector3.down, out hit, 100,1<<7))
        //{
        //    hit.collider.GetComponent<MainCharacter>().OnHitSuffered();
        //}

        //ogni 0.5 secondi spara reycast
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red);

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
    public virtual bool HasRaycastHit => Physics.Raycast(transform.position, Vector3.down, out hit, 100, 1 << 7);
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
