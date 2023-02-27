using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using DG.Tweening;

public class MainCharacter : MonoBehaviour, IHittable
{
    public Sprite[] gooseleft;
    public int hp=5;
    public Projectile myProjectile;
    float moveSpeed = 6;
    bool isInvulnerable = false;
    public AudioSource audioSrc;
    //public List<AudioClip> audioClips;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        if (Input.GetKey(KeyCode.A))
        {
            tsprite.sprite = gooseleft[1];
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            tsprite.sprite = gooseleft[0];
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
    }

    public void Shoot ()
    {
        Projectile tempProjectile = Instantiate(myProjectile, transform.position, transform.rotation);
        tempProjectile.Shoot(Vector3.up * 6.6f);
    }
    public void OnHitSuffered(int damage = 1)
    {
        if (isInvulnerable) { return; }

        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSrc.Play();

        if (--hp<=0) //fa decremento e poi valuta se minore o uguale a 0// hp--<=0 guarda se hp minore o uguale a 0 e poi fa decremento
        {
            //se giocatore viene distrutto, sposta audio src fuori così non viene distrutto:
            audioSrc.transform.parent = transform.parent;
            //morte
            Destroy(gameObject);
            UIManager.instance.OnPlayerHitSuffered();

        }
        else
        {
            //fx colpo subito
            StartCoroutine(HitSufferedCoroutine());
            //come dire transform.localScale*2
            //transform.localScale *= 2;
            //hp--;
            UIManager.instance.OnPlayerHitSuffered();
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
