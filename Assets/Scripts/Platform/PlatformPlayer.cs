using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformPlayer : MonoBehaviour
{

    #region VARIABLES
    public enum PlatformPlayerState { IDLE, RUN, JUMP, FALL, LAND, DEATH }
    public PlatformPlayerState state = PlatformPlayerState.IDLE;
    PlatformPlayerState previousState;

    public GameObject myGround;
    public GameObject myCrossableGround; //se è valorizzato scendo dal ground
    public Vector2 moveDirection;

    float gravity = 19.8f;
    //fall speed va messa a 0 ogni volta che inizio a cadere
    float fallSpeed;
    public float jumpPower = 1f;
    public float jumpMaxHeight = 5f;
    float startJumpY;

    public float speed = 1f;
    float idleStateTimer = 0;
    float idleStateTime = 1f;
    public bool onGround = false;
    public bool nearGround = false;

    private float groundedDistance;
    private float isFallDistance;

    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite, runSprite, jumpSprite, fallSprite, landSprite, deathSprite;
    public List<Sprite> spriteList;
    private Vector3 respawnSafePosition;

    private Vector3 startingSpriteDirection;
    

    #endregion

    #region INIT

    void Start()
    {
        startingSpriteDirection = spriteRenderer.transform.localScale;
        //Vector3 startingSpriteDirection = spriteRenderer.transform.localScale;
        //spriteList[]
        groundedDistance = 1.1f;
        isFallDistance = 1.6f;
        if (gameObject.layer == 9)
        {
            speed = 2f;
            //groundedDistance = 5.1f;
            //isFallDistance = 5.6f;
        }
        SetState_IDLE();
        InvokeRepeating("SaveSafePosition", 2f, .5f);
    }
    private void Awake()
    {
        respawnSafePosition = transform.position;

    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        moveDirection = Vector2.zero;

        //check input nell update generale così può essere usato nei vari stati
        if (Input.GetKey(KeyCode.D))
        {
            //corre verso destra
            //spriteRenderer.flipX = true;
            spriteRenderer.transform.localScale= new Vector3(-startingSpriteDirection.x, startingSpriteDirection.y, startingSpriteDirection.z);
            moveDirection = Vector2.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //corre verso sinistra
            //spriteRenderer.flipX = false;

            spriteRenderer. transform.localScale = startingSpriteDirection;
            moveDirection = Vector2.left;
        }

        switch (state)
        {
            case PlatformPlayerState.IDLE: Update_IDLE(); break;
            case PlatformPlayerState.RUN: Update_RUN(); break;
            case PlatformPlayerState.JUMP: Update_JUMP(); break;
            case PlatformPlayerState.FALL: Update_FALL(); break;
            case PlatformPlayerState.LAND: Update_LAND(); break;
            case PlatformPlayerState.DEATH: Update_DEATH(); break;

        }
    }

    #region ||>>UPDATES<<||
    //IDLE, RUN, JUMP, FALL, LAND, DEATH
    void Update_IDLE()
    {
        //se cade da piattaf passa a fall
        if (!onGround)
        {
            ChangeState(PlatformPlayerState.FALL);
            //se esiste il collided ground lo metto in myground per salvare ultima piattaf toccata nel caso che cada
            return;
        }

        //se jump passa a jump
        if (Input.GetKey(KeyCode.Space))
        {
            if (groundHit.collider.isTrigger && Input.GetKey(KeyCode.S))
            {
                //scendo dalla piattaforma
                myCrossableGround = groundHit.collider.gameObject;
                onGround = false;
                ChangeState(PlatformPlayerState.FALL);
                return;
            }
            ChangeState(PlatformPlayerState.JUMP);
            return;
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            ChangeState(PlatformPlayerState.RUN);
            return;
        }
        //se morte passa a death
    }

    public float GetRunSpeed()
    {
        return speed * Time.deltaTime;
    }
    void Update_RUN()
    {

        //se cade da piattaf passa a fall
        if (!onGround)
        {
            ChangeState(PlatformPlayerState.FALL);
            //se esiste il collided ground lo metto in myground per salvare ultima piattaf toccata nel caso che cada
            return;
        }
        //prima controlla se è sul ground, poi nel caso calcoli vettore parallelo al terreno per spostarti seguendo profilo terreno
        Vector2 fixedMoveDirection = moveDirection - Vector3.Dot(moveDirection,groundHit.normal)*groundHit.normal;

        //calcolo distanza parete
        float distanceFromWall = 99;

        if(fixedMoveDirection.x>0&&rightWallHit.collider)
        {
            distanceFromWall = Mathf.Abs( transform.position.x + (GetComponent<CapsuleCollider2D>().size.x * transform.localScale.x / 2) //metà capsula in senso orizzonatale
            - rightWallHit.point.x);
            Debug.Log(distanceFromWall);
        }
        else if(fixedMoveDirection.x<0&&leftWallHit.collider)
        {
            distanceFromWall = Mathf.Abs(transform.position.x - (GetComponent<CapsuleCollider2D>().size.x * transform.localScale.x / 2) //metà capsula in senso orizzonatale
            - leftWallHit.point.x);
            Debug.Log(distanceFromWall);

        }

        //corre nella direction
        if (distanceFromWall < Mathf.Abs((fixedMoveDirection * GetRunSpeed()).x))
        {
            //personaggio si ferma
        }
        else
        {
            //mi muovo
            transform.position += (Vector3)fixedMoveDirection * GetRunSpeed();
        }

        //se jump passa a jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlatformPlayerState.JUMP);
            return;
        }
        if(moveDirection==Vector2.zero)
        {
            ChangeState(PlatformPlayerState.IDLE);
            return;
        }
    }
    void Update_JUMP()
    {
        //esco se:
        //sbatto testa contro piattaf

        //mollo bottone salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlatformPlayerState.FALL);
            return;
        }
        //esaurisco spinta salto
        if (transform.position.y - startJumpY >= jumpMaxHeight)
        {
            ChangeState(PlatformPlayerState.FALL);
            return;
        }

        //movimento componente orizzontale
        transform.position += (Vector3)moveDirection * GetRunSpeed();

        if ((transform.position.y + jumpPower * Time.deltaTime) - startJumpY > jumpMaxHeight)
        {
            //sorpasso altezza max
            Vector3 endJumpPos = transform.position;
            endJumpPos.y = startJumpY + jumpMaxHeight;
            transform.position = endJumpPos;
            ChangeState(PlatformPlayerState.FALL);
        }
        else
        {
            //componente verticale
            transform.position += Vector3.up * jumpPower * Time.deltaTime /* *(startJumpY+jumpMaxHeight-transform.position.y)/jumpMaxHeight*/;
        }
    }
    void Update_FALL()
    {
        //muove personaggio verso il basso e verso destra
        fallSpeed += gravity * Time.deltaTime;

        //controllo la distanza dal terreno
        //controllo che groundHit.collider esiste
        //groundHit.point.y coordinate dal terreno sotto ai miei piedi
        //transform.position.y - 1 coordinate piedi personaggio (1 perchè è alto 2 la capsula)
        //distanza tra piedi personaggio e terreno = transform.position.y - 1 - groundHit.point.y

        float distanceFromGround = transform.position.y - 1 - groundHit.point.y;
        if (distanceFromGround>0 && distanceFromGround < fallSpeed * Time.deltaTime) //se maggiore di zero e minore della distanza che andrà a muoversi
        {
            transform.position += Vector3.down * distanceFromGround;
        }
        else
        {
            //componente verticale del fall
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        }

        //componente verticale fall
        //transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        //se la distanza dal terreno è minore del movimento che farà sull'asse Y

        //componente orizzontale fall
        transform.position += (Vector3)moveDirection * GetRunSpeed();

        //esco se tocco terra
        if (onGround)
        {
            ChangeState(PlatformPlayerState.LAND);
        }

        //esco se:
        //uso skill che fermano salto
        //condiz morte
    }
    void Update_LAND()
    {
        //aumento friction per qualche frame

        //gestione animazioni, cambi velocita in atteraggio
        ChangeState(PlatformPlayerState.RUN);
    }
    void Update_DEATH()
    {
        //nulla
    }
    #endregion

    #region CHANGE STATE

    public void ChangeState(PlatformPlayerState newState)
    {
        if (state == newState)
        { return; }
        previousState = state;
        state = newState;
        switch (state)
        {
            case PlatformPlayerState.IDLE: SetState_IDLE(); break;
            case PlatformPlayerState.RUN: SetState_RUN(); break;
            case PlatformPlayerState.JUMP: SetState_JUMP(); break;
            case PlatformPlayerState.FALL: SetState_FALL(); break;
            case PlatformPlayerState.LAND: SetState_LAND(); break;
            case PlatformPlayerState.DEATH: SetState_DEATH(); break;
        }
    }

    //IN QUESTE METTI LE COSE CHE CAMBIANO AL CAMBIO STATE
    void SetState_IDLE()
    {
        spriteRenderer.sprite = idleSprite;
        //parte animazione di intro o idle del personaggio
    }
    void SetState_RUN()
    {
        myGround = groundHit.collider.gameObject;
        spriteRenderer.sprite = runSprite;  
        //animazione run
        //eventuale intro di run che porta a run
    }
    void SetState_JUMP()
    {
        spriteRenderer.sprite = jumpSprite;

        startJumpY = transform.position.y;

        //animaz jump
        //init variabili salto:
        //pre jump(variabile di inizio jump)

        //settare myGround a null
    }
    void SetState_FALL()
    {
        spriteRenderer.sprite = fallSprite;

        //init variabili caduta
        fallSpeed = 0;
        //fall
    }
    void SetState_LAND()
    {
        spriteRenderer.sprite = landSprite;

        //animazione land in base alla velocita di atteraggio
        ChangeState(PlatformPlayerState.IDLE);
    }
    void SetState_DEATH()
    {
        spriteRenderer.sprite = deathSprite;

        //spegnere componenti giocatore
    }

    #endregion

    #region PHYSICS

    RaycastHit2D groundHit, leftWallHit, rightWallHit;
    private void FixedUpdate()
    {
        //1<<6 ---> creazione bitmask, rileva collison con layer da 1 a 6
        //groundHit=Physics2D.CircleCast (transform.position, .1f, Vector3.down, 10, 1 << 6);
        groundHit = Physics2D.Raycast(transform.position, Vector2.down, 10, 1 << 6);
        leftWallHit = Physics2D.Raycast(transform.position, Vector2.left, 3, 1 << 6);
        rightWallHit = Physics2D.Raycast(transform.position, Vector2.right, 3, 1 << 6);

        //if (leftWallHit.collider.is)

        //controllo se mycrossableground va resettato
        //controllo se ho finito di scendere dalla piattaforma crossable
        if (myCrossableGround && groundHit.collider && myCrossableGround != groundHit.collider.gameObject)
            myCrossableGround = null;

        //TO DO metteremo gli altri due raycast per i lati dx, sx del groundHit

        //se la variabile è valorizzata vuol dire che ho preso qualcosa sotto di me
        if (groundHit.collider)
        {
            //distanza tra metà oggetto (perchè raycasta dal centro obj) e terreno
            if (groundHit.distance < groundedDistance /*&& groundHit.distance>0.666f*/) //snappa quando ti avvicini alla superficie tra 0.1 e 0.333 così non compenetra se cala il framerate
            {
                //ho i piedi per terra-on ground
                if (groundHit.collider.gameObject != myCrossableGround)
                {
                    onGround = true;

                    Debug.LogError($"oh no la distanza è: {groundHit.distance - 1}");

                    //snap del personaggio al terreno
                    if (state != PlatformPlayerState.JUMP)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - (groundHit.distance - 1), transform.position.z);
                    }
                }

            }
            else
            {
                if (groundHit.distance < isFallDistance)
                { //sto per atterrare
                    onGround = false;
                    nearGround = true;
                }
                else
                {
                    //sto cadendo
                    onGround = false;
                    nearGround = false;
                }
            }
        }
        else
        {
            //sto cadendo nel vuoto
            onGround = false;
            nearGround = false;
        }
    }
    #endregion

    public void OnHitSuffered()
    {
        //respawno all'ultima posizione sicura
        transform.position = respawnSafePosition;
        //Vector3 respawnSafePosition = transform.position;

        //TO DO: perdo una vita, il personaggio lampeggia per la durata dell'invulnerabilità
        //TO DO 2: se finisco le vite respawno all'ultimo checkpoint
    }

    void SaveSafePosition()
    {
        //controlla se ci sono damagere nelle vicinanze e in caso contrario salva al posizione del player per il respawn
        Collider2D nearObstacle = Physics2D.OverlapCircle(transform.position, 2, 1 << 12);
        if(onGround&&!nearObstacle)
        {
            respawnSafePosition = transform.position;
        }
    }

    IEnumerator RespawnCoroutine()
    {
        yield return null;
    }
}
