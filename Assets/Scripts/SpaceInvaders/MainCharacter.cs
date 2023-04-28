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
    //public Animator explosionAniamtor;
    //public Animation 
    public int hp;
    public int startingHp=4;
    public Projectile myProjectile;
    float moveSpeed = 6;
    private bool isInvulnerable = false;
    public bool IsInvulnerable { get; set; }
    public AudioSource audioSrc;
    public Animator Animator;
    private bool isDead=false;
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    //public List<AudioClip> audioClips;

    private void Awake()
    {
        //hp = GameManager.Instance? GameManager.Instance.playerHp: startingHp;
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        //this distrugge lo script attaccato al game object
            //instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        string animationName = "playerIdleAnimation";

        if (transform.position.x < -8f)
            transform.position = new Vector3(-8f, transform.position.y);
        if (transform.position.x > 8f)
            transform.position = new Vector3(8f, transform.position.y);

        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        if (Input.GetKey(KeyCode.A))
        {
            animationName = "playerWalkAnimation";
            transform.localScale = new Vector3(-1, 1, 1);
            //tsprite.sprite = gooseleft[1];
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            animationName = "playerWalkAnimation";
            transform.localScale = Vector3.one;

            //tsprite.sprite = gooseleft[0];
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        //SPARO
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            //alternativa
            //Instantiate(myProjectile, transform.position, transform.rotation).Shoot(Vector3.up);
            //Alternativa:
            Shoot();
        }
        Animator.Play(animationName);
    }

    public void Shoot ()
    {
        Projectile tempProjectile = Instantiate(myProjectile, transform.position, transform.rotation);
        tempProjectile.Shoot(Vector3.up * 6.6f);
    }
    public void OnHitSuffered(int damage = 1)
    {
        if (IsInvulnerable)
            return;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        else
        {
            audioSrc.Play();

            if (--hp <= 0) //fa decremento e poi valuta se minore o uguale a 0// hp--<=0 guarda se hp minore o uguale a 0 e poi fa decremento
            {
                //se giocatore viene distrutto, sposta audio src fuori così non viene distrutto:
                audioSrc.transform.parent = transform.parent;
                UIManager.instance.OnPlayerHitUpdateLives();
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
                //come dire transform.localScale*2
                //transform.localScale *= 2;
                //hp--;
                UIManager.instance.OnPlayerHitUpdateLives();
            }
        }
    }
    IEnumerator HitSufferedCoroutine()
    {
        Time.timeScale = 0.1f;
        isInvulnerable = true;
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(Color.red, .05f).SetUpdate(true)/*.timeScale=1*/;
        tsprite.transform.DOPunchScale(Vector3.one * .5f, 0.10f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.05f); //con 0 aspetta 1 frame
        //yield return new WaitForSecondsRealtime(0);
        //yield return new WaitForSecondsRealtime(0);
        //yield return new WaitForSecondsRealtime(0);
        //yield return new WaitForSecondsRealtime(0);
        tsprite.DOColor(Color.white, .05f);
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(.10f);
        isInvulnerable = false;

    }
}
