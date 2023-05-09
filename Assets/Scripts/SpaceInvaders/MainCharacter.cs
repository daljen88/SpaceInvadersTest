using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.ComponentModel.Design;

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
    public Projectile myProjectile;
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

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
        string animationName = "playerIdleAnimation";

        if (coolDown > 0)
            coolDown -= Time.deltaTime;

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
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            //transform.localScale = vectorScaleLeft;
            //tsprite.sprite = gooseleft[1];
        }
        if (Input.GetKey(KeyCode.D))
        {
            goingRight = true;
            animationName = "playerWalkAnimation";
            tsprite.flipX = !goingRight;
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            //transform.localScale = vectorScaleRight;
            //tsprite.sprite = gooseleft[0];
        }

        //SPARO
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Shoot();
        }
        Animator.Play(animationName);
    }

    //public weaponclass gunPosseses;
    public void Shoot (/*weaponclass gunpossessed*/)
    {
        //cambio in shootProjectile
        //if(gunpossessed==null)
        if (activeGunPrefab == null)
        {
            if (coolDown <= 0)
            {
                //WeaponProjectile tempProjectile = Instantiate(myProjectile, transform.position, transform.rotation);
                Projectile tempProjectile = Instantiate(myProjectile, transform.position, transform.rotation);
                tempProjectile.Shoot(1);
                coolDown = baseFireRate;
            }
            else { return; }

        }
        else /*if (gunPossesed.GetComponent<BigGun>() != null)*/
        {
            gunPossesed.ShootProjectile();
            //gunPossesed.ShootProjectile(/*eventuale shootDirection controllata da player*/);
        }
        //gun possesses.shoot
    }
    public void OnHitSuffered(int damage = 1)
    {
        if (IsInvulnerable)
            return;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        else
        {
            audioSrc.Play();
            if (activeGunPrefab == null)
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
                    StartCoroutine(HitSufferedCoroutine());
                    UIManager.instance.OnPlayerHitUpdateLives(damage);
                }
            }
            else
            {
                if(/*--gunPossesed.Defence*/gunPossesed.Defence-damage<=0)
                {
                    Debug.Log($"arma distrutta! difesa= {gunPossesed.Defence} ");
                    Destroy(gunPossesed.gameObject);
                    gunPossesed=null;
                    activeGunPrefab=null;
                    GameManager.Instance.SetGameManagerGunPossessed(null);
                }
                else
                {
                    gunPossesed.Defence -= damage;
                    Debug.Log($"arma colpita! difesa= {gunPossesed.Defence} ");
                }
            }
        }
    }

    IEnumerator HitSufferedCoroutine()
    {
        Time.timeScale = 0.1f;
        IsInvulnerable = true;
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(Color.red, .05f).SetUpdate(true)/*.timeScale=1*/;
        tsprite.transform.DOPunchScale(Vector3.one * .5f, 0.10f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.05f); //con 0 aspetta 1 frame
        //yield return new WaitForSecondsRealtime(0);
        tsprite.DOColor(Color.white, .05f);
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(.10f);
        IsInvulnerable = false;
    }
}
