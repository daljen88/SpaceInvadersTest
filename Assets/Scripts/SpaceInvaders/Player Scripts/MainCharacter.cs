using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using System;
using UnityEngine.TextCore.Text;

public class MainCharacter : MonoBehaviour, IHittable
{
    //public static MainCharacter instance;
    public Sprite[] gooseleft;
    public GameObject playerExplosion;
    //public Animator explosionAnimator;
    //public Animation 
    private int hp;
    public int Hp { get { return hp; } set { hp = value; } }
    public int startingHp=4;
    public GameObject startingGunPrefab;
    //public StandardProjectile myProjectile;
    float moveSpeed = 6;
    private bool isInvulnerable = false;
    public bool IsInvulnerable 
    {
        get=>isInvulnerable; 
        set=>isInvulnerable=value;
    }
    public AudioSource audioSrc;
    public Animator Animator;
    private bool isDead=false;
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    //public List<AudioClip> audioClips;
    public GameObject activeGunPrefab;
    public WeaponsClass gunPossesed;
    private Vector3 vectorScaleLeft = new Vector3(-1, 1, 1);
    private Vector3 vectorScaleRight=Vector3.one;
    public bool goingRight=true;
    public float baseFireRate = 0.2f;
    private float coolDown=0;
    private float powerCoolDown = 10;
    private float powerCoolDownCounter = 0;
    public float deltaTimeScale;
    public bool IsSlowingTime = false;
    public IEnumerator hitCoroutineRunning;
    private bool hitSufferedRoutineIsRunning = false;

    //OUTATTIME
    private float maxPowerDurationCounter;
    [SerializeField] private float maxSlowingPowerDuration = 1.8f;
    public float MaxSlowingPowerDuration=>maxSlowingPowerDuration+.2f* GameManager.Instance.NumberOfAlarmsCollected>5?5: maxSlowingPowerDuration + .2f * GameManager.Instance.NumberOfAlarmsCollected;
    [SerializeField] private float everyThisSecondsPowerReloadsOneSecond = 4.2f;
    public float EveryThisSecondsPowerReloadsOneSecond => everyThisSecondsPowerReloadsOneSecond - .2f * GameManager.Instance.NumberOfAlarmsCollected < 4 ? 4 : everyThisSecondsPowerReloadsOneSecond - .2f * GameManager.Instance.NumberOfAlarmsCollected;
    [SerializeField] private float slowingPowerSpeedBoost = 50f;

    private void Awake()
    {
        deltaTimeScale = Time.deltaTime;
        //ISTANZIA STANDARD GUN
        activeGunPrefab = Instantiate(startingGunPrefab, transform.position, Quaternion.identity);
        gunPossesed = activeGunPrefab.GetComponent<StandardGun>();
        gunPossesed.IsCollected=true;
        enabled = false;
    }

    void Start()
    {
    }

    void Update()
    {
        string animationName = "playerIdleAnimation";

        if (coolDown > 0)
            coolDown -= Time.deltaTime;

        //if (powerCoolDownCounter > 0)
        //    powerCoolDownCounter -= Time.deltaTime;

        if (transform.position.x < -8f)
            transform.position = new Vector3(-8f, transform.position.y);
        if (transform.position.x > 8f)
            transform.position = new Vector3(8f, transform.position.y);

        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        if (Input.GetKey(KeyCode.A))
        {
            goingRight = false;
            animationName = "playerWalkAnimation";
            tsprite.flipX = !goingRight;
            transform.position += Vector3.left * moveSpeed * deltaTimeScale;

            //transform.localScale = vectorScaleLeft;
            //tsprite.sprite = gooseleft[1];
        }
        if (Input.GetKey(KeyCode.D))
        {
            goingRight = true;
            animationName = "playerWalkAnimation";
            tsprite.flipX = !goingRight;
            transform.position += Vector3.right * moveSpeed * deltaTimeScale;

            //transform.localScale = vectorScaleRight;
            //tsprite.sprite = gooseleft[0];
        }

        //SPARO
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Shoot();
        }

        Animator.Play(animationName);

        if(Input.GetKeyDown(KeyCode.AltGr))
        {
            ShootSecond();
        }

        //SLOWING POWER
        if(IsSlowingTime)
        {
            deltaTimeScale = Time.unscaledDeltaTime;
        }
        else
        {
            deltaTimeScale = Time.deltaTime;
        }
        if(GameManager.Instance.AlarmClockCollected && !LevelManager.instance.IsPaused&&!PlayerTextLogic.instance.routineIsRunning /*&& powerCoolDownCounter <= 0*/)
        {
            if (Input.GetKey(KeyCode.B) && maxPowerDurationCounter >= 0/*&& !LevelManager.instance.IsPaused*//*&&LevelManager.instance.state==LevelManager.LogicState.RUNNING*/)
            {
                IsSlowingTime = true;
                Time.timeScale = 0.1f;
                //deltaTimeScale = Time.unscaledDeltaTime;
                maxPowerDurationCounter -= Time.unscaledDeltaTime;
                //moveSpeed = slowingPowerSpeedBoost;
                if(maxPowerDurationCounter<0)
                {
                        IsSlowingTime = false;
                        //deltaTimeScale = Time.deltaTime;
                        Time.timeScale = 1f;
                        return;
                }              
            }
            else/* if(!LevelManager.instance.IsPaused)*/
            {
                IsSlowingTime = false;
                //deltaTimeScale = Time.deltaTime;
                Time.timeScale = 1f;
                if (maxPowerDurationCounter <= MaxSlowingPowerDuration)
                {
                    //unscaledDeltaTie perchè quando c'è intermezzo playerText si carica comunque a velocità normale
                    //in pausa non si ricarica perchè c'è già condizione IsPaused==false
                    maxPowerDurationCounter += (Time.unscaledDeltaTime / EveryThisSecondsPowerReloadsOneSecond);
                }
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                if (!LevelManager.instance.IsPaused && IsSlowingTime)
                {
                    IsSlowingTime = false;
                    //deltaTimeScale = Time.deltaTime;
                    Time.timeScale = 1f;
                }
            }
        }
        else
        {
            IsSlowingTime = false;
        }
    }

    private void ShootSecond()
    {
       if(activeGunPrefab.GetComponent<EyeOrbsCannon>()!=null)
        {
            activeGunPrefab.GetComponent<EyeOrbsCannon>().SecondShootProjectile();
        }
    }

    //public weaponclass gunPosseses;
    public void Shoot (/*weaponclass gunpossessed*/)
    {
        //cambio in shootProjectile
        //if(gunpossessed==null)
        if (activeGunPrefab != null)
        {
            if (activeGunPrefab.GetComponent<StandardGun>() !=null)
            {
                gunPossesed.ShootProjectile();

                //if (coolDown <= 0)
                //{
                //    Projectile tempProjectile = Instantiate(myProjectile, transform.position, transform.rotation);
                //    tempProjectile.Shoot(1);
                //    coolDown = baseFireRate;
                //}
                //else { return; }

            }
            else if (gunPossesed.GetComponent<WeaponsClass>() != null)
            {
                gunPossesed.ShootProjectile();
                //gunPossesed.ShootProjectile(/*eventuale shootDirection controllata da player*/);
            }
            //gun possesses.shoot
        }
    }
    public void PausePlayer()
    {
        IsSlowingTime = false;
        enabled = false;
        if(hitSufferedRoutineIsRunning)
            StopCoroutine(hitCoroutineRunning);
    }
    public void UnpausePlayer()
    {
        enabled = true;
        if (hitCoroutineRunning != null)
            StartCoroutine(hitCoroutineRunning);
    }
    public void OnHitSuffered(int damage = 1)
    {
        if (IsInvulnerable)
            return;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        else
        {
            audioSrc.Play();
            if (/*activeGunPrefab == null|| */gunPossesed.GetComponent<StandardGun>()!=null)
            {
                hp -= damage;
                if (hp<= 0) //fa decremento e poi valuta se minore o uguale a 0// hp--<=0 guarda se hp minore o uguale a 0 e poi fa decremento
                {
                    //se giocatore viene distrutto, sposta audio src fuori così non viene distrutto:
                    audioSrc.transform.parent = transform.parent;
                    UIManager.instance.OnPlayerHitUpdateLives(damage);
                    //morte
                    IsDead = true;
                    //Enemy_Spawner.Instance.StopSpawner();
                    gameObject.SetActive(false);
                    //Destroy(gameObject);
                    GameObject playExplosion = Instantiate(playerExplosion, transform.position, transform.rotation);
                    string animationName2 = "Explosion";
                    //playerExplosion.GetComponent<AudioSource>().enabled = true;
                    playExplosion.GetComponent<AudioSource>().Play();
                    playExplosion.GetComponent<Animator>().Play(animationName2);

                }
                else
                {
                    //fx colpo subito
                    hitCoroutineRunning = HitSufferedCoroutine();
                    StartCoroutine(hitCoroutineRunning);
                    UIManager.instance.OnPlayerHitUpdateLives(damage);
                }
            }
            else/* if(gunPossesed.GetComponent<StandardGun>() == null)*/
            {
                if(/*--gunPossesed.Defence*/gunPossesed.Defence-damage<=0)
                {
                    Debug.Log($"arma distrutta! difesa= {gunPossesed.Defence} ");
                    Destroy(gunPossesed.gameObject);
                    activeGunPrefab = Instantiate(startingGunPrefab, transform.position, Quaternion.identity);
                    gunPossesed = activeGunPrefab.GetComponent<StandardGun>();
                    gunPossesed.IsCollected = true;
                    GameManager.Instance.SetGameManagerGunPossessed(startingGunPrefab);
                }
                else
                {
                    StartCoroutine(WeaponHitSufferedCoroutine());
                    gunPossesed.Defence -= damage;
                    Debug.Log($"arma colpita! difesa= {gunPossesed.Defence} ");
                }
            }
        }
    }

    IEnumerator HitSufferedCoroutine()
    {
        hitSufferedRoutineIsRunning = true;
        Time.timeScale = 0.1f;
        IsInvulnerable = true;
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(Color.red, .05f).SetUpdate(true)/*.timeScale=1*/;
        tsprite.transform.DOPunchScale(Vector3.one * .5f, 0.10f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.05f); //con 0 aspetta 1 frame
        //yield return new WaitForSecondsRealtime(0);
        tsprite.DOColor(Color.white, .05f);
        Time.timeScale = 1;
        //moveSpeed = 6f;
        yield return new WaitForSecondsRealtime(.10f);
        IsInvulnerable = false;
        hitSufferedRoutineIsRunning = false;

    }
    IEnumerator WeaponHitSufferedCoroutine()
    {
        Time.timeScale = 0.1f;
        IsInvulnerable = true;
        SpriteRenderer tGunSprite = gunPossesed.GetComponentInChildren<SpriteRenderer>();
        tGunSprite.DOColor(Color.red, .05f).SetUpdate(true)/*.timeScale=1*/;
        gunPossesed.transform.DOPunchScale(Vector3.one * .5f, 0.10f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.05f); //con 0 aspetta 1 frame
        //yield return new WaitForSecondsRealtime(0);
        tGunSprite.DOColor(Color.white, .05f);
        Time.timeScale = 1;
        //moveSpeed = 6f;
        yield return new WaitForSecondsRealtime(.10f);
        IsInvulnerable = false;
    }
}
