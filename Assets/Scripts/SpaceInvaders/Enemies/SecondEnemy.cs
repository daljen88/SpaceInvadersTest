using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SecondEnemy : EnemyClass
{
    //public List<GameObject> guns;

    //public ParticleSystem ExplosionTemplate;

    //public Material myMaterial, myHitTakenMaterial;
    //public Color hitFxColor;

    [Header("ENEMY VALUES")]
    private int thisEnemyPointsValue = 6666;
    private float thisShootCooldown = 0.5f;
    private float thisEnemySpeed = 5;
    private int thisHp = 1;
    private int thisEnemyDamageMultiplyer = 1;
    public override float ShootCooldown => baseShootCooldown * thisShootCooldown;

    public override float EnemySpeed { get { return baseEnemySpeed* thisEnemySpeed; } set { thisEnemySpeed = value; } }

    public override int Hp { get { return hp; } set { hp = value; } }

    public override int EnemyDamageMultiplyer => baseEnemyDamageMultiplyer * thisEnemyDamageMultiplyer;

    //Tweener twScale;
    //float hitFxDuration = 0.25f;
    //float shootTimer=0f;
    public SecondEnemy() : base()
    {
        enemyType = EnemyType.bonusEnemy;
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
        EnemySpeed = 4 + Mathf.Pow(Mathf.Log10(GameManager.Instance.LevelCount + 10), 2);
        Invoke("DestroyThisEnemy", 10f);
    }

    public override void DestroyThisEnemy()
    {
        SecondEnemySpawner.Instance.bonusEnemyList.Remove(this);
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

    public override void OnHitSuffered(int damage = 1)
    {
        #region old hitSuffer
        //hp -= damage;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioSrc.Play();
        //if (hp <= 0)
        //{
        //    //SCORE OK
        //    UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, "bonusEnemy");
        //    //FX MORTE
        //    ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
        //    //controllo particella da codice
        //    ps.Emit(30);
        //    Destroy(ps.gameObject, .5f);
        //    if (UIManager.instance.totalEnemiesKilled > 5 && Random.Range(0, 11) < 9)
        //    {
        //        GameObject electricGun = Instantiate(guns[0], transform.position, Quaternion.identity);
        //        WeaponsClass electricGunDropping = electricGun.GetComponent<ElectricTriGun>();
        //        electricGunDropping?.Drop(Vector3.down);
        //    }
        //    else
        //    {
        //        GameObject bigGun = Instantiate(guns[1], transform.position, Quaternion.identity);
        //        WeaponsClass bigGunDropping = bigGun.GetComponent<BigGun>();
        //        bigGunDropping?.Drop(Vector3.down);
        //    }
        //    //Distrugge enemy
        //    DestroyThisEnemy();
        //}
        //else
        //{
        //    //FX COLPO SUBITO
        //    GetComponent<MeshRenderer>().material = myHitTakenMaterial;
        //    //la funzione va chiamata come stringa
        //    Invoke("SetNormalMaterial", 0.25f);
        //    //se il tween esiste ed è attivo, killa il tween precedente sennò si sovrappongono
        //    if (twScale != null && twScale.IsActive())
        //    {
        //        twScale.Kill();
        //        //risistema a dim originale se tween spento a metà
        //        transform.localScale = Vector3.one; //new vector3 (1,1,1);
        //    }
        //    transform.DOPunchPosition(Vector3.up, .25f, 2);
        //    twScale = transform.DOPunchScale(Vector3.one * 0.2f, hitFxDuration, 2);
        //    StartCoroutine(HitColorCoroutine());
        //    //anche la coroutine si sovrappone se viene chiamata più volte, quindi va checkato se è già attiva e nel caso spegnerla

        //}
        #endregion

        base.OnHitSuffered();

    }
    public override void GunDropLogic()
    {
        if (FirstGunDropCondition())
        {
            InstantiateGunDrop();
        }
        else
        {
            InstantiateGunDrop();
        }
    }
    public override bool FirstGunDropCondition()
    {
        if (UIManager.instance.totalEnemiesKilled > 5 && Random.Range(0, 11) < 9)
        {
            gunToSpawn = "ElectricGun";
            return true;
        }
        else
            gunToSpawn = "BigGun";
            return false;
    }
    public override bool SecondGunDropCondition()
    {
        return false;
    }
    public override bool ThirdGunDropCondition()
    {
        return false;
    }

    public override void InstantiateGunDrop()
    {
        base.InstantiateGunDrop();

    }

    private void FixedUpdate()
    {
        FixedUpdateRoutine();
    }
    public override void FixedUpdateRoutine()
    {
        //AGGIUNGO CHE CAMBIA DIREZIONE SE INCONTRA PLAYER
        return;
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
