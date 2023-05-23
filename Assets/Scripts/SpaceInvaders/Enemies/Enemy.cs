using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Playables;
using static TowerDefence_Enemy;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class Enemy : EnemyClass
{
    //public List<GameObject> guns;
    //public List<GameObject> drops;

    //public GameObject myProjectile;

    //public ParticleSystem ExplosionTemplate;
    //public List<AudioClip> audioClips;
    //public AudioSource audioSrc;

    //public Material myMaterial, myHitTakenMaterial;
    //public Color hitFxColor;
    private Vector3 startingScale;

    private StateMachine SM;
    public List<EnemyState> enemyStates = new List<EnemyState>();

    [Header("ENEMY VALUES")]
    [Tooltip("speed = 6, hp = 3, shootCD = 0.5")]
    private int thisEnemyPointsValue = 666;
    private float thisShootCooldown = 0.5f;
    private float thisEnemySpeed = 3;
    private int thisHp = 3;
    private int thisEnemyDamageMultiplyer = 1;
    public override float ShootCooldown => baseShootCooldown*thisShootCooldown;

    public override float EnemySpeed { get { return  baseEnemySpeed* thisEnemySpeed; }set{ thisEnemySpeed = value; } }

    public override int Hp { get { return hp; } set { hp = value; } }

    public override int EnemyDamageMultiplyer => baseEnemyDamageMultiplyer * thisEnemyDamageMultiplyer;

    //float shootTimer = 0;
    //float hitFxDuration = 0.25f;
    //Tweener twScale;
    public Enemy():base()
    {
        enemyType = EnemyType.normalEnemy;
        Hp = thisHp;
        enemyPointsValue = thisEnemyPointsValue;
    }
    #region INIT
    private void Awake()
    {
        startingScale = transform.localScale;
    }

    void Start()
    {
        StartRoutine();
        //InizializeStateMachine();
        //ChangeState(EnemyState.State.IDLE);
    }
    public override void StartRoutine()
    {
        base.StartRoutine();
        EnemySpeed = 2 + Mathf.Log10(GameManager.Instance.LevelCount + 10);
        InizializeStateMachine();
        ChangeState(EnemyState.State.IDLE);
    }
    #endregion

    #region STATE MACHINE
    private void InizializeStateMachine()
    {
        SM = new StateMachine();
        enemyStates = new List<EnemyState>();
        enemyStates.Add(new EnemyState(this, EnemyState.State.IDLE));
        enemyStates.Add(new EnemyState(this, EnemyState.State.MOVE_DOWN));
        enemyStates.Add(new EnemyState(this, EnemyState.State.MOVE_LEFT));
        enemyStates.Add(new EnemyState(this, EnemyState.State.MOVE_RIGHT));
        enemyStates.Add(new EnemyState(this, EnemyState.State.DESTROYED));

    }

    public EnemyState.State ChangeState(EnemyState.State state)
    {
        EnemyState.State newState;
        foreach (EnemyState es in enemyStates)
        {
            if (es.EnState == state)
            {
                SM.ChangeState(es);
                return newState=es.EnState;
            }
        }
        Debug.LogError("State " + state + " not found!");
        return newState=EnemyState.State.IDLE;

    }
    #endregion

    #region UPDATE SM
    public void UpdateStateMachine()
    {
        SM.Execute();

        #region old state machine
        //    if (transform.position.y < screenLowerLimit)
        //    {
        //        currentState=EnemyState.DESTROYED;
        //    }
        //    switch (currentState)
        //    {
        //        case EnemyState.IDLE:
        //            Update_IDLE();
        //            break;
        //        case EnemyState.MOVE_DOWN:
        //            Update_MOVE_DOWN();
        //            break;
        //        case EnemyState.MOVE_LEFT:
        //            Update_MOVE_LEFT();
        //            break;
        //        case EnemyState.MOVE_RIGHT:
        //            Update_MOVE_RIGHT();
        //            break;
        //        case EnemyState.DESTROYED:
        //            Update_DESTROYED();
        //            break;
        //    }
        #endregion

    }
    #endregion
    void Update()
    {
        UpdateRoutine();
    }
    public override void UpdateRoutine()
    {
        //base.UpdateRoutine();
        UpdateStateMachine();

    }
    RaycastHit hit;

    private void FixedUpdate()
    {
        FixedUpdateRoutine();

    }
    public override void FixedUpdateRoutine()
    {
        base.FixedUpdateRoutine();
        transform.localScale = startingScale*.6f * (1+(5-transform.position.y)/10);

    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionRoutine(collision);     
    }
    public override void OnCollisionRoutine(Collision _collision)
    {
        IHittable tPlayer = _collision.gameObject.GetComponent<IHittable>();
        if (tPlayer != null)
        {
            //HO COLPITO
            tPlayer.OnHitSuffered(1);
            //Destroy(gameObject);
            DestroyThisEnemy();
        }
    }

    public override void OnHitSuffered(int damage = 1)
    {
        base.OnHitSuffered(damage);

        #region old hitSuffer
        //hp -= damage;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioSrc.Play();
        //if (hp <= 0)
        //{
        //    //SCORE
        //    UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, "enemy");
        //    //FX MORTE
        //    ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
        //    //controllo particella da codice
        //    ps.Emit(60);
        //    Destroy(ps.gameObject, .5f);
        //    if (UIManager.instance.totalEnemiesKilled%3==0&&UIManager.instance.totalEnemiesKilled!=0&&Random.Range(0,11)<11 /*&&GameManager.Instance.typeGunPossessed.name!="BigGun"*/)
        //    {
        //        GameObject bigGunz = Instantiate(guns[0], transform.position, Quaternion.identity);
        //        WeaponsClass bigGunDropping = bigGunz.GetComponent<BigGun>();
        //        bigGunDropping?.Drop(Vector3.down);
        //    }
        //    else if(UIManager.instance.totalEnemiesKilled % 20 == 0 && UIManager.instance.totalEnemiesKilled !=0 && Random.Range(0, 11) < 8)
        //    {
        //        GameObject electricGun = Instantiate(guns[1], transform.position, Quaternion.identity);
        //        WeaponsClass electricGunDropping = electricGun.GetComponent<ElectricTriGun>();
        //        electricGunDropping?.Drop(Vector3.down);
        //    }
        //    else if (UIManager.instance.totalEnemiesKilled % 9999 == 0 && UIManager.instance.totalEnemiesKilled != 0 && Random.Range(0, 11) < 11)
        //    {
        //        GameObject eyeCannon = Instantiate(guns[2], transform.position, Quaternion.identity);
        //        WeaponsClass eyeCannonDrop = eyeCannon.GetComponent<EyeOrbsCannon>();
        //        eyeCannonDrop?.Drop(Vector3.down);
        //    }
        //    //if(UIManager.instance.totalEnemiesKilled % 5 == 0 && UIManager.instance.totalEnemiesKilled != 0&&GameManager.Instance.musicRadioCollected==false)
        //    //{
        //    //    GameObject musicRadio = Instantiate(drops[0], transform.position, Quaternion.identity);
        //    //    RadioDrop radioScript = musicRadio.GetComponent<RadioDrop>();
        //    //    radioScript.Drop(Vector3.down);
        //    //}

        //    //Destroy(gameObject);
        //    DestroyThisEnemy();
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
    }
    public override void InstantiateGunDrop()
    {
        base.InstantiateGunDrop();

    }

    public void SetNormalMaterial()
    {
        GetComponent<MeshRenderer>().material = myMaterial;
    }

    public override void DestroyThisEnemy()
    {
        Enemy_Spawner.Instance.enemyList.Remove(gameObject.GetComponent<Enemy>());
        base.DestroyThisEnemy();
    }

    //IEnumerator HitColorCoroutine()
    //{
    //    SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
    //    tsprite.DOColor(hitFxColor, hitFxDuration / 2);
    //    yield return new WaitForSeconds(hitFxDuration / 2);
    //    tsprite.DOColor(Color.white, hitFxDuration / 2);
    //}

    #region OLD FUNCTIONS
    ////IEnumerator RotationCoroutine()
    ////{
    ////    //SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
    ////    //tsprite.
    ////    //transform.position += Vector3.left * enemySpeed * Time.deltaTime;
    ////    gameObject.transform.DORotate(new Vector3(5, 5, 0), 0.25f, RotateMode.Fast);
    ////    yield return new WaitForSeconds(.25f);
    ////    gameObject.transform.DORotate(new Vector3(5, 5, 0), 0.25f, RotateMode.Fast);
    ////    yield return new WaitForSeconds(.25f);
    ////}
    #endregion

}