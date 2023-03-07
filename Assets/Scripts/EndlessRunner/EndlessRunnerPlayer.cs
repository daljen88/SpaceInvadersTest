using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EndlessRunnerPlayer : MonoBehaviour
{
    #region VARIABLES
    public enum EndlessRuneerPlayerState { IDLE, RUN, JUMP, FALL, LAND, DEATH }
    public EndlessRuneerPlayerState state = EndlessRuneerPlayerState.IDLE;
    EndlessRuneerPlayerState previousState;

    float gravity = 19.8f;
    //fall speed va messa a 0 ogni volta che inizio a cadere
    float fallSpeed;
    public float jumpPower = 1f;
    public float jumpMaxHeight=5f;
    float startJumpY;

    public float speed = 1f;
    float idleStateTimer = 0;
    float idleStateTime = 1f;
    public bool onGround=false;
    public bool nearGround = false;
    public GameObject myGround;

    private float groundedDistance;
    private float isFallDistance;
    #endregion

   
    // Start is called before the first frame update
    void Start()
    {
        groundedDistance = 1.1f;
        isFallDistance = 1.6f;
        if(gameObject.layer==9)
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
         switch (state)
         {
            case EndlessRuneerPlayerState.IDLE: Update_IDLE(); break;
            case EndlessRuneerPlayerState.RUN: Update_RUN(); break;
            case EndlessRuneerPlayerState.JUMP: Update_JUMP(); break;
            case EndlessRuneerPlayerState.FALL: Update_FALL(); break;
            case EndlessRuneerPlayerState.LAND: Update_LAND(); break;
            case EndlessRuneerPlayerState.DEATH: Update_DEATH(); break;

         }
        
    }
    #endregion

    #region ||>>UPDATES<<||
    //IDLE, RUN, JUMP, FALL, LAND, DEATH
    void Update_IDLE()
    {
        idleStateTimer += Time.deltaTime;
        if(idleStateTimer>=idleStateTime)
        {
            ChangeState(EndlessRuneerPlayerState.RUN);
        }
        //aspetta finisca intro e parte run
    }

    public float GetRunSpeed()
    {
        return speed*Time.deltaTime;
    }
    void Update_RUN()
    {
        //corre verso destra
        transform.position+=Vector3.right* GetRunSpeed();

        //se cade da piattaf passa a fall
        if(!onGround)
        {
            ChangeState(EndlessRuneerPlayerState.FALL);
            //se esiste il collided ground lo metto in myground per salvare ultima piattaf toccata nel caso che cada
            if(myGround)
            EndlessRunnerManager.instance.PutBackInPool(myGround);
            myGround = null;
        }

        //se jump passa a jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(EndlessRuneerPlayerState.JUMP);
            EndlessRunnerManager.instance.PutBackInPool(myGround);
            myGround = null;
        }
        //se condiz morte passa a death
    }
    void Update_JUMP()
    {
        //esco se:
        //sbatto testa contro piattaf

        //mollo bottone salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(EndlessRuneerPlayerState.FALL);
            return;
        }
        //esaurisco spinta salto
        if (transform.position.y - startJumpY >= jumpMaxHeight)
        {
            ChangeState(EndlessRuneerPlayerState.FALL);
            return;
        }

        transform.position += Vector3.right * GetRunSpeed();

        if ((transform.position.y + jumpPower * Time.deltaTime) -startJumpY > jumpMaxHeight)
        {
            //sorpasso altezza max
            Vector3 endJumpPos = transform.position;
            endJumpPos.y = startJumpY + jumpMaxHeight;
            transform.position=endJumpPos;
            ChangeState(EndlessRuneerPlayerState.FALL);
        }
        else
        {
            transform.position += Vector3.up * jumpPower * Time.deltaTime;
        }
    }
    void Update_FALL()
    {
        //muove personaggio verso il basso e verso destra
        fallSpeed += gravity * Time.deltaTime;
        transform.position += Vector3.down* fallSpeed * Time.deltaTime;
        transform.position += Vector3.right * GetRunSpeed();
        //esco se:
        //tocco terra
        //uso skill che fermano salto
        //condiz morte
        if(onGround)
        {
            ChangeState(EndlessRuneerPlayerState.LAND);
        }
    }
    void Update_LAND()
    {
        //gestione animazioni, cambi velocita in atteraggio
        ChangeState(EndlessRuneerPlayerState.RUN);
    }
    void Update_DEATH()
    {
        //nulla
    }
    #endregion

    #region CHANGE STATE

    public void ChangeState(EndlessRuneerPlayerState newState)
    {
        if (state == newState)
        { return; }
        previousState = state;
        state = newState;
            switch(state)
            {
                    case EndlessRuneerPlayerState.IDLE:SetState_IDLE(); break;
                    case EndlessRuneerPlayerState.RUN:SetState_RUN(); break;
                    case EndlessRuneerPlayerState.JUMP:SetState_JUMP(); break;
                    case EndlessRuneerPlayerState.FALL:SetState_FALL(); break;
                    case EndlessRuneerPlayerState.LAND:SetState_LAND(); break;
                    case EndlessRuneerPlayerState.DEATH:SetState_DEATH(); break;
            }
    }

    void SetState_IDLE()
    {
        //animazione di intro o idle del personaggio
    }
    void SetState_RUN()
    {
        myGround= groundHit.collider.gameObject;
        //animazione run
        //eventuale intro di run che porta a run
    }
    void SetState_JUMP()
    {
        startJumpY= transform.position.y;

        //animaz jump
        //init variabili salto:
        //pre jump(variabile di inizio jump)
    }
    void SetState_FALL()
    {
        //init variabili caduta
        fallSpeed= 0;
        //fall
    }
    void SetState_LAND()
    {
        //animazione land in base alla velocita di atteraggio
    }
    void SetState_DEATH()
    {
        //spegnere componenti giocatore
    }


    #endregion

    #region PHYSICS
    RaycastHit groundHit;
    private void FixedUpdate()
    {
        //1<<6 ---> creazione bitmask, rileva collison con layer da 1 a 6
        Physics.SphereCast(transform.position, .1f, Vector3.down, out groundHit, 10, 1 << 6);
        //se la variabile è valorizzata vuol dire che ho preso qualcosa sotto di me
        if(groundHit.collider)
        {
            if(groundHit.distance< groundedDistance)
            {
                //ho i piedi per terra-on ground
                onGround=true;
            }
            else
            {
                if (groundHit.distance < isFallDistance)
                { //sto per atterrare
                    onGround=false;
                    nearGround=true;
                }
                else 
                { 
                    //sto cadendo
                    onGround=false;
                    nearGround=false;
                }
            }
        }
        else
        {
            //sto cadendo nel vuoto
            onGround=false;
            nearGround=false;
        }
    }
    #endregion

}
