using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefence_Enemy : MonoBehaviour
{
    public enum ENEMY_STATE { IDLE, WALK, ATTACK, DEATH }
    public ENEMY_STATE myState= ENEMY_STATE.IDLE;
    public float hp;
    public ELEMENT myElement;
    public float speed = 1;
    public float defence;
    public float attack;
    public NarutoGridNode myGround;

    // Start is called before the first frame update
    void Start()
    {
        myState = ENEMY_STATE.WALK;   
    }

    // Update is called once per frame
    void Update()
    {
        if(myState==ENEMY_STATE.WALK)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;

            //se muoio passo a death, poi devo passarlo allo stato e mettere animaz morte
            if(hp<=0)
            { Destroy(gameObject); }

            //se arriva alla torre (punto di arrivo) perde punti

            if(myGround&& myGround.isGoal)
            {
                Destroy(gameObject, .5f);
                //aggiungere perdita punti o sconfitta per il giocatore
            }

        }
    }

    RaycastHit groundHit;
    private void FixedUpdate()
    {
        Physics.Raycast(transform.position, Vector3.down, out groundHit, 5, 1 << 6);
        if (groundHit.collider)
        {
            myGround = groundHit.collider.GetComponent<NarutoGridNode>();
            if (!myGround) {
                Debug.LogError($"script not present in { groundHit.collider.name}");
            }
        }
    }

    public void OnHitSuffered(float damageRecived)
    {
        hp -= damageRecived;
        //aggiunta hit fx
    }
}
