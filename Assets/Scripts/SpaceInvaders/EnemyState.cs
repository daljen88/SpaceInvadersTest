using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TowerDefence_Enemy;

public class EnemyState
{
    public enum State { IDLE, MOVE_DOWN, MOVE_RIGHT, MOVE_LEFT, DESTROYED }
    //public State currentState;/* = State.IDLE;*/

    public Enemy enemyOwner;

    protected State enState;
    public State EnState => enState;

    private Vector3 goalPosition;
   // public Vector3 GoalPosition { get { return goalPosition; } set { goalPosition = SetGoal(this.EnState); } }

    public EnemyState(Enemy owner, State state/*, Vector3 goalPos=new Vector3()*/)
    {
        //goalPos = new Vector3(0, 0);
        this.enemyOwner = owner;
        this.enState = state;
        //this.goalPosition = GoalPosition;
        //this.goalPosition = new Vector3();
    }

    private float screenLowerLimit = -5f;

    public Vector3 SetGoal(EnemyState currentState)
    {
        Vector3 goal = this.enemyOwner.transform.position;
        switch (currentState.enState)
        {
            case State.IDLE:
                break;
            case State.MOVE_DOWN:
                goal = this.enemyOwner.transform.position + Vector3.down;
                break;
            case State.MOVE_LEFT:
                goal.x = -8f;
                break;
            case State.MOVE_RIGHT:
                goal.x = 8f;
                break;
            case State.DESTROYED:
                break;
        }
        return goal;
    }

    public void Inizialize(EnemyState currentState)
    {
       goalPosition = SetGoal(currentState);

    }

        public void Execute(EnemyState enemyState)
    {
        switch (enemyState.enState)
        {
            case State.IDLE:
                Update_IDLE();
                break;
            case State.MOVE_DOWN:
                Update_MOVE_DOWN();
                break;
            case State.MOVE_LEFT:
                Update_MOVE_LEFT();
                break;
            case State.MOVE_RIGHT:
                Update_MOVE_RIGHT();
                break;
            case State.DESTROYED:
                Update_DESTROYED();
                break;
        }
    }
    public void Exit()
    {
        return;
    }

    void Update()
    {
        //switch (currentState)
        //{
        //    case State.IDLE:
        //        Update_IDLE();
        //        break;
        //    case State.MOVE_DOWN:
        //        Update_MOVE_DOWN();
        //        break;
        //    case State.MOVE_LEFT:
        //        Update_MOVE_LEFT();
        //        break;
        //    case State.MOVE_RIGHT:
        //        Update_MOVE_RIGHT();
        //        break;
        //    case State.DESTROYED:
        //        Update_DESTROYED();
        //        break;
        //}
    }

    void Update_IDLE()
    {
        CheckLowerLimit();

        //currentState = State.MOVE_DOWN;
        this.goalPosition = this.enemyOwner.transform.position + Vector3.down;
        this.enState=enemyOwner.ChangeState(State.MOVE_DOWN);
    }
    void Update_MOVE_DOWN()
    {
        CheckLowerLimit();

        if (this.enemyOwner.transform.position.y > this.goalPosition.y)
        {
            this.enemyOwner.transform.position += Vector3.down * this.enemyOwner.enemySpeed * Time.deltaTime;
        }
        else
        {
            //cambio stato
            if (this.enemyOwner.transform.position.x < 0)
            {
                this.goalPosition.x = 8f;
                this.enState = enemyOwner.ChangeState(State.MOVE_RIGHT);
            }
            else
            {
                this.goalPosition.x = -8f;
                this.enState=enemyOwner.ChangeState(State.MOVE_LEFT);
            }
        }
    }
    void Update_MOVE_LEFT()
    {
        CheckLowerLimit();

        if (this.enemyOwner.transform.position.x > this.goalPosition.x)
        {
            this.enemyOwner.transform.position += Vector3.left * this.enemyOwner.enemySpeed * Time.deltaTime;
            //RotationCoroutine();
        }
        else
        {
            this.goalPosition = this.enemyOwner.transform.position + Vector3.down;
            this.enState = enemyOwner.ChangeState(State.MOVE_DOWN);
        }
    }

    void Update_MOVE_RIGHT()
    {
        CheckLowerLimit();

        if (this.enemyOwner.transform.position.x < this.goalPosition.x)
        {
            this.enemyOwner.transform.position += Vector3.right * this.enemyOwner.enemySpeed * Time.deltaTime;
        }
        else
        {
            this.goalPosition = this.enemyOwner.transform.position + Vector3.down;
            this.enState = enemyOwner.ChangeState(State.MOVE_DOWN);
        }
    }
    void Update_DESTROYED()
    {
        enemyOwner.DestroyThisEnemy();
    }

    void CheckLowerLimit()
    {
        if (this.enemyOwner.transform.position.y < screenLowerLimit)
            this.enState = enemyOwner.ChangeState(State.DESTROYED);
    }

}
