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
    private Vector3 startingScale;
    private StateMachine SM;
    public List<EnemyState> enemyStates = new List<EnemyState>();

    [Tooltip("speed = 6, hp = 3, shootCD = 0.5")]
    [Header("NORMAL ENEMY VALUES")]
    [SerializeField] private int thisEnemyPointsValue = 666;
    [SerializeField] private float thisShootCooldown = 0.5f;
    [SerializeField] private float thisEnemySpeed = 3;
    [SerializeField] private int thisHp = 3;
    [SerializeField] private int thisEnemyDamageMultiplyer = 1;

    protected override float ShootCooldown => baseShootCooldown*thisShootCooldown;
    public override float EnemySpeed { get { return  baseEnemySpeed* thisEnemySpeed; }protected set{ thisEnemySpeed = value; } }
    public override int Hp { get { return hp; } set { hp = value; } }
    protected override int EnemyDamageMultiplyer => baseEnemyDamageMultiplyer * thisEnemyDamageMultiplyer;

    [SerializeField] private int enemiesKilledFirstDrop=6;
    protected override bool EnemiesKilledFirstDrop => UIManager.instance.totalEnemiesKilled % enemiesKilledFirstDrop == 0;
    [SerializeField] private int enemiesKilledSecondDrop=45;
    protected override bool EnemiesKilledSecondDrop => UIManager.instance.totalEnemiesKilled % enemiesKilledSecondDrop == 0;
    [SerializeField] private int enemiesKilledThirdDrop=6666;
    protected override bool EnemiesKilledThirdDrop => UIManager.instance.totalEnemiesKilled % enemiesKilledThirdDrop == 0;

    private IDictionary<string, int> FirstDropRandomRange  = new Dictionary<string, int>() { { "min", 0 }, { "max(excluded)", 11 }, { "IsLessThan", 7 } };
    private IDictionary<string, int> SecondDropRandomRange = new Dictionary<string, int>() { { "min", 0 }, { "max(excluded)", 11 }, { "IsLessThan", 8 } };
    private IDictionary<string, int> ThirdDropRandomRange  = new Dictionary<string, int>() { { "min", 0 }, { "max(excluded)", 11 }, { "IsLessThan", 11 } };
    protected override bool IsFirstDropRandomTrue => Random.Range(FirstDropRandomRange["min"], FirstDropRandomRange["max(excluded)"]) < FirstDropRandomRange["IsLessThan"];
    protected override bool IsSecondDropRandomTrue => Random.Range(SecondDropRandomRange["min"], SecondDropRandomRange["max(excluded)"]) < SecondDropRandomRange["IsLessThan"];
    protected override bool IsThirdDropRandomTrue => Random.Range(ThirdDropRandomRange["min"], ThirdDropRandomRange["max(excluded)"]) < ThirdDropRandomRange["IsLessThan"];

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