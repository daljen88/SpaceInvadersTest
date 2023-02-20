using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { IDLE, MOVE_DOWN, MOVE_RIGHT, MOVE_LEFT, DESTROYED}
    public EnemyState currentState = EnemyState.IDLE;
    public Enemy_Projectile myProjectile;
    public MainCharacter character;
    public ParticleSystem ExplosionTemplate;


    public int hp = 3;
    public float enemySpeed = 6;
    public Material myMaterial, myHitTakenMaterial;
    Vector3 goalPosition;
    float hitFxDuration = 0.25f;
    public Color hitFxColor;

    float shootCooldown = 0.5f;
    float shootTimer = 0;
    Tweener twScale;

    // Start is called before the first frame update
    void Start()
    {

    }

    #region UPDATES FUNCTIONS

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                Update_IDLE();
                break;
            case EnemyState.MOVE_DOWN:
                Update_MOVE_DOWN();
                break;
            case EnemyState.MOVE_LEFT:
                Update_MOVE_LEFT();
                break;
            case EnemyState.MOVE_RIGHT:
                Update_MOVE_RIGHT();
                break;
            case EnemyState.DESTROYED:
                Update_DESTROYED();
                break;
        }
    }

    void Update_IDLE()
    {
        currentState = EnemyState.MOVE_DOWN;
        goalPosition = transform.position + Vector3.down;
    }
    void Update_MOVE_DOWN()
    {
        if(transform.position.y>goalPosition.y)
        {
            transform.position += Vector3.down*enemySpeed*Time.deltaTime;
        }
        else
        {
            //cambio stato
            if(transform.position.x<0)
            {
                currentState= EnemyState.MOVE_RIGHT;
                goalPosition.x = 9;
            }
            else 
            {
                currentState= EnemyState.MOVE_LEFT;
                goalPosition.x = -9;
            }
        }
    }
    void Update_MOVE_LEFT() 
    {
        if (transform.position.x > goalPosition.x)
        {
            transform.position += Vector3.left * enemySpeed * Time.deltaTime;
        }
        else
        {
            currentState = EnemyState.MOVE_DOWN;
            goalPosition = transform.position + Vector3.down;
        }
    }
    void Update_MOVE_RIGHT()
    {
        if(transform.position.x < goalPosition.x)
        {
            transform.position += Vector3.right * enemySpeed * Time.deltaTime;
        }
        else
        {
            currentState = EnemyState.MOVE_DOWN;
            goalPosition =transform.position + Vector3.down;
        }
    }
    void Update_DESTROYED()
    {

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
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, 1 << 7))
        {
            if (shootTimer >= shootCooldown)
            {
                //hit.collider.GetComponent<CharacterController>().OnHitSuffered();
                Enemy_Projectile tempProjectile = Instantiate(myProjectile, transform.position + Vector3.down, transform.rotation);
                tempProjectile.Shoot(Vector3.down * 4f);

                shootTimer = 0;
            }
        }
    }
    


    public void OnHitSuffered(int damage = 1)
    {
        hp -= damage;
        if (hp<=0)
        {
            UIManager.instance.PointsScoredEnemyKilled();
           
            //FX MORTE
            ParticleSystem ps = Instantiate(ExplosionTemplate, transform.position, Quaternion.identity);
            //controllo particella da codice
            ps.Emit(60);
            Destroy(ps.gameObject,.5f);

            Destroy(gameObject);
        }
        else
        {
            //FX COLPO SUBITO
            GetComponent<MeshRenderer>().material = myHitTakenMaterial;
            //chiama funzione per tot tempo dichiarato come numero, in pratica cambia materiale per .25 sec
            //la funzione va chiamata come stringa
            Invoke("SetNormalMaterial", 0.25f);
            //se il tween esiste ed � attivo, killa il tween precedente senn� si sovrappongono
            if (twScale == null && twScale.IsActive())
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
    public void SetNormalMaterial()
    {
        GetComponent<MeshRenderer>().material = myMaterial;
    }
    
}