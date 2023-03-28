using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformPlayer : MonoBehaviour
{

    #region VARIABLES
    public enum PlatformPlayerState { IDLE, RUN, JUMP, FALL, LAND, DEATH }
    public PlatformPlayerState state = PlatformPlayerState.IDLE;
    PlatformPlayerState previousState;

    public GameObject myGround;
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

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        groundedDistance = 1.1f;
        isFallDistance = 1.6f;
        if (gameObject.layer == 9)
        {
            speed = 2f;
            //groundedDistance = 5.1f;
            //isFallDistance = 5.6f;
        }
        SetState_IDLE();
    }

    #region INIT
    // Update is called once per frame
    void Update()
    {
        moveDirection = Vector2.zero;

        //check input nell update generale così può essere usato nei vari stati
        if (Input.GetKey(KeyCode.D))
            //corre verso destra
            moveDirection = Vector2.right;

        if (Input.GetKey(KeyCode.A))
            //corre verso sinistra
            moveDirection = Vector2.left;

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
    #endregion

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
            ChangeState(PlatformPlayerState.JUMP);
            return;
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
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

        transform.position += (Vector3)fixedMoveDirection * GetRunSpeed();

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
            transform.position += Vector3.up * jumpPower * Time.deltaTime;
        }
    }
    void Update_FALL()
    {
        //muove personaggio verso il basso e verso destra
        fallSpeed += gravity * Time.deltaTime;
        //componente verticale fall
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
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
        //parte animazione di intro o idle del personaggio
    }
    void SetState_RUN()
    {
        myGround = groundHit.collider.gameObject;
        //animazione run
        //eventuale intro di run che porta a run
    }
    void SetState_JUMP()
    {
        startJumpY = transform.position.y;

        //animaz jump
        //init variabili salto:
        //pre jump(variabile di inizio jump)

        //settare myGround a null
    }
    void SetState_FALL()
    {
        //init variabili caduta
        fallSpeed = 0;
        //fall
    }
    void SetState_LAND()
    {
        //animazione land in base alla velocita di atteraggio
        ChangeState(PlatformPlayerState.IDLE);
    }
    void SetState_DEATH()
    {
        //spegnere componenti giocatore
    }


    #endregion

    #region PHYSICS

    RaycastHit2D groundHit;
    private void FixedUpdate()
    {
        //1<<6 ---> creazione bitmask, rileva collison con layer da 1 a 6
        //groundHit=Physics2D.CircleCast (transform.position, .1f, Vector3.down, 10, 1 << 6);
        groundHit = Physics2D.Raycast(transform.position, Vector2.down, 10, 1 << 6);
        //TO DO metteremo gli altri due raycast per i lati dx, sx del groundHit

        //se la variabile è valorizzata vuol dire che ho preso qualcosa sotto di me
        if (groundHit.collider)
        {
            //distanza tra metà oggetto (perchè raycasta dal centro obj) e terreno
            if (groundHit.distance < groundedDistance)
            {
                //ho i piedi per terra-on ground
                onGround = true;
                Debug.LogError($"oh no la distanza è: {groundHit.distance-1}");

                //snap del personaggio al terreno
                if (state != PlatformPlayerState.JUMP)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - (groundHit.distance - 1), transform.position.z);
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

}
