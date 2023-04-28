using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SecondEnemy : MonoBehaviour, IHittable
{
    public float shootCooldown = 0.5f;
    public float enemySpeed = 5f;
    public int hp = 1;
    public int enemyPointsValue = 6666;
    public ParticleSystem ExplosionTemplate;
    public Material myMaterial, myHitTakenMaterial;
    public Color hitFxColor;
    float hitFxDuration = 0.25f;
    Tweener twScale;



    // Start is called before the first frame update
    void Start()
    {
        enemySpeed = 4 + Mathf.Pow(Mathf.Log10(GameManager.Instance.levelCount + 10), 2);
    }

    // Update is called once per frame
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
            //UIManager.instance.PointsScoredEnemyKilled();

            //FX MORTE
            ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
            //controllo particella da codice
            ps.Emit(30);
            UIManager.instance.PointsScoredEnemyKilled(enemyPointsValue);
            Destroy(ps.gameObject, .5f);

            Destroy(gameObject);
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

}
