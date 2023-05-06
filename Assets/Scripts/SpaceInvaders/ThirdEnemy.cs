using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ThirdEnemy : MonoBehaviour, IHittable
{
    public GameObject dropSlot;
    public DropsClass bringingDrop;
    public GameObject myProjectile;
    public List<GameObject> guns;
    public List<GameObject> drops;
    public ParticleSystem ExplosionTemplate;
    public List<AudioClip> audioClips;
    public Material myMaterial, myHitTakenMaterial;
    //public AudioSource audioSrc;
    public Color hitFxColor;
    private Vector3 startingScale;
    Tweener twScale;

    public float shootCooldown = 1.5f;
    float shootTimer = 0;
    public float enemySpeed = 3f;
    public int hp = 4;
    public int enemyPointsValue = 4999;
    float hitFxDuration = 0.25f;


    void Start()
    {
        Invoke("DestroyEnemy", 15f);

    }
    public void DestroyEnemy()
    {
        SecondEnemySpawner.Instance.bringerEnemyList.Remove(this);
        Destroy(gameObject);
    }
    void Update()
    {
        transform.position += Vector3.right * enemySpeed * Time.deltaTime;

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    IHittable tPlayer = collision.gameObject.GetComponent<IHittable>();
    //    if (tPlayer != null)
    //    {
    //        //HO COLPITO
    //        tPlayer.OnHitSuffered(1);
    //        Destroy(this.gameObject);
    //    }
    //}

    public void OnHitSuffered(int damage = 1)
    {
        hp -= damage;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioSrc.Play();
        if (hp <= 0)
        {
            //FX MORTE
            ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
            //controllo particella da codice
            ps.Emit(60);
            UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, "bringerEnemy");
            Destroy(ps.gameObject, .5f);
            if (UIManager.instance.totalEnemiesKilled % 3 == 0 && UIManager.instance.totalEnemiesKilled != 0 && Random.Range(0, 11) < 7 /*&&GameManager.Instance.typeGunPossessed.name!="BigGun"*/)
            {
                GameObject bigGunz = Instantiate(guns[0], transform.position, Quaternion.identity);
                WeaponsClass bigGunDropping = bigGunz.GetComponent<BigGun>();
                bigGunDropping.Drop(Vector3.down);
            }
            //if (UIManager.instance.totalEnemiesKilled % 5 == 0 && UIManager.instance.totalEnemiesKilled != 0 && GameManager.Instance.musicRadioCollected == false)
            //{
            //    GameObject musicRadio = Instantiate(drops[0], transform.position, Quaternion.identity);
            //    RadioDrop radioScript = musicRadio.GetComponent<RadioDrop>();
            //    radioScript.Drop(Vector3.down);
            //}
            bringingDrop.gameObject.transform.parent = transform.parent;
            bringingDrop.Drop(Vector3.down);

            //DropsClass dropping = GetComponentInChildren<DropsClass>();
            //dropping.gameObject.transform.parent = transform.parent;
            //dropping.Drop(Vector3.down);
            //GetComponentInChildren<IDroppable>().Drop(Vector3.down);
            //shootTimer = 0;
            DestroyEnemy();
            //Destroy(gameObject);
        }
        else
        {
            //FX COLPO SUBITO
            GetComponent<MeshRenderer>().material = myHitTakenMaterial;
            //chiama funzione per tot tempo dichiarato come numero, in pratica cambia materiale per .25 sec
            //la funzione va chiamata come stringa
            Invoke("SetNormalMaterial", 0.25f);
            //se il tween esiste ed è attivo, killa il tween precedente sennò si sovrappongono
            if (twScale == null && twScale.IsActive())
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
                //hit.collider.GetComponent<CharacterController>().OnHitSuffered();
                GameObject tempProjectile = Instantiate(myProjectile, transform.position + Vector3.down, transform.rotation);
                Enemy_Projectile tempProj = tempProjectile.GetComponent<Enemy_Projectile>();
                tempProj.Shoot(Vector3.down * 4f);

                shootTimer = 0;
                //Debug.Break();
            }
        }
        //Mathf.Log10(transform.position+5)
        //transform.localScale = startingScale * .6f * (1 + (5 - transform.position.y) / 10);

    }

}
