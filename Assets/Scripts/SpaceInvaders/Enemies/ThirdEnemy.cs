using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdEnemy : EnemyClass
{
    //public List<GameObject> guns;
    //public List<GameObject> drops;


    //public GameObject myProjectile;

    //public ParticleSystem ExplosionTemplate;

    //public Material myMaterial, myHitTakenMaterial;
    //public Color hitFxColor;

    //public List<AudioClip> audioClips;
    //public AudioSource audioSrc;

    public GameObject dropSlot;
    public DropsClass bringingDrop;

    [Header("ENEMY VALUES")]
    private int thisEnemyPointsValue = 4999;
    private float thisShootCooldown = 1f;
    private float thisEnemySpeed = 3.5f;
    private int thisHp = 5;
    private int thisEnemyDamageMultiplyer = 1;
    public override float ShootCooldown => baseShootCooldown * thisShootCooldown;

    public override float EnemySpeed { get { return baseEnemySpeed * thisEnemySpeed; } set { thisEnemySpeed = value; } }

    public override int Hp { get { return hp; } set { hp = value; } }

    public override int EnemyDamageMultiplyer => baseEnemyDamageMultiplyer * thisEnemyDamageMultiplyer;

    //float shootTimer = 0;
    //Tweener twScale;
    //float hitFxDuration = 0.25f;
    public ThirdEnemy() : base()
    {
        enemyType = EnemyType.bringerEnemy;
        Hp = thisHp;
        enemyPointsValue = thisEnemyPointsValue;

    }
    void Start()
    {
        StartRoutine();       
    }
    public override void StartRoutine()
    {
        base.StartRoutine();
        EnemySpeed = 2.5f + Mathf.Log10(GameManager.Instance.LevelCount + 10);
        Invoke("DestroyThisEnemy", 15f);
    }
    public override void DestroyThisEnemy()
    {
        this.bringingDrop = null;
        SecondEnemySpawner.Instance.bringerEnemyList.Remove(this);
        base.DestroyThisEnemy();

    }
    void Update()
    {
        UpdateRoutine();
    }
    public override void UpdateRoutine()
    {
        base.UpdateRoutine();
    }

    RaycastHit hit;

    private void FixedUpdate()
    {
        FixedUpdateRoutine();

    }
    public override void FixedUpdateRoutine()
    {
        base.FixedUpdateRoutine();

    }
    public override void InstatiateProjectile()
    {
        GameObject tempProjectile = Instantiate(myProjectile, transform.position + Vector3.down, transform.rotation);
        Enemy_fireProjectile tempProj = tempProjectile.GetComponent<Enemy_fireProjectile>();
        tempProj.Shoot(Vector3.down * 4f, EnemyDamageMultiplyer);
    }

    public override void OnHitSuffered(int damage = 1)
    {
        #region old hitSuffer
        //hp -= damage;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioSrc.Play();
        //if (hp <= 0)
        //{
        //    UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, "bringerEnemy");
        //    //FX MORTE
        //    ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
        //    //controllo particella da codice
        //    ps.Emit(60);
        //    Destroy(ps.gameObject, .5f);
        //    if (bringingDrop == null)
        //    {
        //        if (UIManager.instance.totalEnemiesKilled % 1 == 0 && UIManager.instance.totalEnemiesKilled != 0 && Random.Range(0, 11) < 11 /*&&GameManager.Instance.typeGunPossessed.name!="BigGun"*/)
        //        {
        //            GameObject bigGunz = Instantiate(guns[0], transform.position, Quaternion.identity);
        //            WeaponsClass bigGunDropping = bigGunz.GetComponent<BigGun>();
        //            bigGunDropping?.Drop(Vector3.down);
        //        }
        //    }
        //    if(bringingDrop!=null)
        //    bringingDrop.gameObject.transform.parent = transform.parent;
        //    bringingDrop?.Drop(Vector3.down);

        //    //if (UIManager.instance.totalEnemiesKilled % 5 == 0 && UIManager.instance.totalEnemiesKilled != 0 && GameManager.Instance.musicRadioCollected == false)
        //    //{
        //    //    GameObject musicRadio = Instantiate(drops[0], transform.position, Quaternion.identity);
        //    //    RadioDrop radioScript = musicRadio.GetComponent<RadioDrop>();
        //    //    radioScript.Drop(Vector3.down);
        //    //}

        //    DestroyThisEnemy();
        //    //Destroy(gameObject);
        //}
        //else
        //{
        //    //FX COLPO SUBITO
        //    GetComponent<MeshRenderer>().material = myHitTakenMaterial;
        //    //la funzione va chiamata come stringa
        //    Invoke("SetNormalMaterial", 0.25f);
        //    //se il tween esiste ed � attivo, killa il tween precedente senn� si sovrappongono
        //    if (twScale != null && twScale.IsActive())
        //    {
        //        twScale.Kill();
        //        //risistema a dim originale se tween spento a met�
        //        transform.localScale = Vector3.one; //new vector3 (1,1,1);
        //    }
        //    transform.DOPunchPosition(Vector3.up, .25f, 2);
        //    twScale = transform.DOPunchScale(Vector3.one * 0.2f, hitFxDuration, 2);
        //    StartCoroutine(HitColorCoroutine());
        //    //anche la coroutine si sovrappone se viene chiamata pi� volte, quindi va checkato se � gi� attiva e nel caso spegnerla
        //}
        #endregion
        base.OnHitSuffered();

    }
    public override void GunDropLogic()
    {
        if (!BringingDropCondition)
        {
            if (FirstGunDropCondition())
                InstantiateGunDrop();
        }
        else
        {
            DropBringedTech();
        }
    }
    public bool BringingDropCondition => bringingDrop != null;

    public override bool FirstGunDropCondition()
    {
        if (UIManager.instance.totalEnemiesKilled % 1 == 0 && UIManager.instance.totalEnemiesKilled != 0 && Random.Range(0, 11) < 11 /*&&GameManager.Instance.typeGunPossessed.name!="BigGun"*/)
        {
            gunToSpawn = "BigGun";
            return true;
        }
        else
        return false;
    }
    public override void InstantiateGunDrop()
    {
        base.InstantiateGunDrop();
    }

    public void DropBringedTech()
    {
        bringingDrop.gameObject.transform.parent = transform.parent;
        bringingDrop?.Drop(Vector3.down);
    }
    public void SetNormalMaterial()
    {
        GetComponent<MeshRenderer>().material = myMaterial;
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionRoutine(collision);
    }
    public override void OnCollisionRoutine(Collision _collision)
    {
        base.OnCollisionRoutine(_collision);
    }

    //IEnumerator HitColorCoroutine()
    //{
    //    SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
    //    tsprite.DOColor(hitFxColor, hitFxDuration / 2);
    //    yield return new WaitForSeconds(hitFxDuration / 2);
    //    tsprite.DOColor(Color.white, hitFxDuration / 2);
    //}
}
