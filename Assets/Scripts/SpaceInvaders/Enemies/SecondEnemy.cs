using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SecondEnemy : MonoBehaviour, IHittable
{
    public List<GameObject> guns;
    public float shootCooldown = 0.5f;
    public float enemySpeed = 5;
    public int hp = 1;
    public int enemyPointsValue = 6666;
    public ParticleSystem ExplosionTemplate;
    public Material myMaterial, myHitTakenMaterial;
    public Color hitFxColor;
    float hitFxDuration = 0.25f;
    Tweener twScale;

    void Start()
    {
        enemySpeed = 4 + Mathf.Pow(Mathf.Log10(GameManager.Instance.LevelCount + 10), 2);
        Invoke("DestroyEnemy",10f);
    }

    public void DestroyEnemy()
    {
        SecondEnemySpawner.Instance.bonusEnemyList.Remove(this);
        Destroy(gameObject);
    }
    void Update()
    {
        transform.position += Vector3.right * enemySpeed * Time.deltaTime;
    }

    public void OnHitSuffered(int damage = 1)
    {
        hp -= damage;
        //audioSrc.clip = audioClips[Random.Range(0, audioClips.Count)];
        //audioSrc.Play();
        if (hp <= 0)
        {
            //SCORE OK
            UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue, "bonusEnemy");
            //FX MORTE
            ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
            //controllo particella da codice
            ps.Emit(30);
            Destroy(ps.gameObject, .5f);
            if (UIManager.instance.totalEnemiesKilled > 5 && Random.Range(0, 11) < 8)
            {
                GameObject electricGun = Instantiate(guns[0], transform.position, Quaternion.identity);
                WeaponsClass electricGunDropping = electricGun.GetComponent<ElectricTriGun>();
                electricGunDropping.Drop(Vector3.down);
            }
            else
            {
                GameObject bigGun = Instantiate(guns[1], transform.position, Quaternion.identity);
                WeaponsClass bigGunDropping = bigGun.GetComponent<BigGun>();
                bigGunDropping.Drop(Vector3.down);
            }
            //Distrugge enemy
            DestroyEnemy();
        }
        else
        {
            //FX COLPO SUBITO
            GetComponent<MeshRenderer>().material = myHitTakenMaterial;
            //la funzione va chiamata come stringa
            Invoke("SetNormalMaterial", 0.25f);
            //se il tween esiste ed � attivo, killa il tween precedente senn� si sovrappongono
            if (twScale != null && twScale.IsActive())
            {
                twScale.Kill();
                //risistema a dim originale se tween spento a met�
                transform.localScale = Vector3.one; //new vector3 (1,1,1);
            }
            transform.DOPunchPosition(Vector3.up, .25f, 2);
            twScale = transform.DOPunchScale(Vector3.one * 0.2f, hitFxDuration, 2);
            StartCoroutine(HitColorCoroutine());
            //anche la coroutine si sovrappone se viene chiamata pi� volte, quindi va checkato se � gi� attiva e nel caso spegnerla

        }
    }
    IEnumerator HitColorCoroutine()
    {
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(hitFxColor, hitFxDuration / 2);
        yield return new WaitForSeconds(hitFxDuration / 2);
        tsprite.DOColor(Color.white, hitFxDuration / 2);
    }

}
