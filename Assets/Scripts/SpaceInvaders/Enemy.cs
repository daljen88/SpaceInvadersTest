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

public class Enemy : MonoBehaviour, IHittable
{
    public GameObject myProjectile;
    public List<GameObject> guns;
    public List<GameObject> drops;
    public ParticleSystem ExplosionTemplate;
    public List<AudioClip> audioClips;
    public Material myMaterial, myHitTakenMaterial;
    public AudioSource audioSrc;
    public Color hitFxColor;
    private Vector3 startingScale;
    Tweener twScale;

    private StateMachine SM;
    public List<EnemyState> enemyStates = new List<EnemyState>();

    float hitFxDuration = 0.25f;
    float shootTimer = 0;

    [Header("ENEMY VALUES")]
    [Tooltip("speed = 6, hp = 3, shootCD = 0.5")]
    public float shootCooldown = 0.5f;
    public float enemySpeed = 6f;
    public int hp = 3;
    public int enemyPointsValue=666;
    public int enemyDamageMultiplyer=1;

    #region INIT
    private void Awake()
    {
        startingScale = transform.localScale;
    }

    void Start()
    {
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
    void Update()
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

    RaycastHit hit;

    private void FixedUpdate()
    {
        //usiamo overload 15/16, con out dei dati in variabile Raycast "hit"
        //if(Physics.Raycast(transform.position, Vector3.down, out hit, 100,1<<7))
        //{
        //    hit.collider.GetComponent<MainCharacter>().OnHitSuffered();
        //}

        //ogni 0.5 secondi spara reycast
        shootTimer += Time.fixedDeltaTime;
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, 1 << 7))
        {
            //Debug.Log("Raycast Hit");
            if (shootTimer >= shootCooldown)
            {
                //Debug.Log("Shoot");
                GameObject tempProjectile = Instantiate(myProjectile, transform.position + Vector3.down, transform.rotation);
                Enemy_Projectile tempProj = tempProjectile.GetComponent<Enemy_Projectile>();
                tempProj.Shoot(Vector3.down * 4f, enemyDamageMultiplyer);

                shootTimer = 0;
                //Debug.Break();
            }
        }
        //Mathf.Log10(transform.position+5)
        transform.localScale = startingScale*.6f * (1+(5-transform.position.y)/10);

    }

    private void OnCollisionEnter(Collision collision)
    {
        IHittable tPlayer = collision.gameObject.GetComponent<IHittable>();
        if (tPlayer != null)
        {
            //HO COLPITO
            tPlayer.OnHitSuffered(1);
            //Destroy(gameObject);
            DestroyThisEnemy();
        }
    }

    public void OnHitSuffered(int damage = 1)
    {
        hp -= damage;
        audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSrc.Play();
        if (hp <= 0)
        {
            //SCORE
            UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, "enemy");
            //FX MORTE
            ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
            //controllo particella da codice
            ps.Emit(60);
            Destroy(ps.gameObject, .5f);
            if (UIManager.instance.totalEnemiesKilled%3==0&&UIManager.instance.totalEnemiesKilled!=0&&Random.Range(0,11)<7 /*&&GameManager.Instance.typeGunPossessed.name!="BigGun"*/)
            {
                GameObject bigGunz = Instantiate(guns[0], transform.position, Quaternion.identity);
                WeaponsClass bigGunDropping = bigGunz.GetComponent<BigGun>();
                bigGunDropping.Drop(Vector3.down);
            } 
            //if(UIManager.instance.totalEnemiesKilled % 5 == 0 && UIManager.instance.totalEnemiesKilled != 0&&GameManager.Instance.musicRadioCollected==false)
            //{
            //    GameObject musicRadio = Instantiate(drops[0], transform.position, Quaternion.identity);
            //    RadioDrop radioScript = musicRadio.GetComponent<RadioDrop>();
            //    radioScript.Drop(Vector3.down);
            //}

            //Destroy(gameObject);
            DestroyThisEnemy();
        }
        else
        {
            //FX COLPO SUBITO
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
            StartCoroutine(HitColorCoroutine());
            //anche la coroutine si sovrappone se viene chiamata più volte, quindi va checkato se è già attiva e nel caso spegnerla

        }
    }

    IEnumerator HitColorCoroutine()
    {
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(hitFxColor, hitFxDuration / 2);
        yield return new WaitForSeconds(hitFxDuration / 2);
        tsprite.DOColor(Color.white, hitFxDuration / 2);
    }
    public void SetNormalMaterial()
    {
        GetComponent<MeshRenderer>().material = myMaterial;
    }

    public void DestroyThisEnemy()
    {
        Enemy_Spawner.Instance.enemyList.Remove(gameObject.GetComponent<Enemy>());
        Destroy(gameObject);
    }

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