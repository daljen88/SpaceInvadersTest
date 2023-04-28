using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SecondEnemySpawner : MonoBehaviour
{
    public static SecondEnemySpawner Instance;
    public SecondEnemy enemyTemplate;
    public float spawnTime = 2;
    public int maxEnemies = 4;
    public List<SecondEnemy> enemyList;
    //public bool win = false;
    public enum State { IDLE, MOVE_DOWN, MOVE_UP }
    public State state=State.IDLE;
    public float yGoalPosition;
    public int controlNum = 1;

    //private List<Enemy> enemies;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //InvokeRepeating("SpawnEnemy", 1, spawnTime);
    }

    // Update is called once per frame
    public void SpawnEnemy()
    {
        if (maxEnemies > 0)
        {
            maxEnemies--;
            /*enemies.Add(*/
            enemyList.Add(Instantiate(enemyTemplate, transform.position, transform.rotation))/*)*/;

        }
        //else
        //{
        //    CancelInvoke("SpawnEnemy");

        //}
    }

    
    public void SpawnOneEnemy()
    {
        if (controlNum != UIManager.instance.scorePoints)
        {
            SpawnEnemy();
            controlNum = UIManager.instance.scorePoints;
        }
    }

    private void Update()
    {
        if(UIManager.instance.scorePoints!=0&& UIManager.instance.scorePoints % 60==0)
        {
            if(Enemy_Spawner.Instance.CheckPlayerVictory()==false)
            SpawnOneEnemy();
        }

        //CheckPlayerVictory();

        switch (state)
        {
            case State.IDLE:
                Update_IDLE();
                break;
            case State.MOVE_DOWN:
                Update_MOVE_DOWN();
                break;
            case State.MOVE_UP:
                Update_MOVE_UP();
                break;
        }

    }
    void Update_IDLE()
    {

        //currentState = State.MOVE_DOWN;
        yGoalPosition = 0;
        state = State.MOVE_DOWN;
    }
    void Update_MOVE_DOWN()
    {

        if (transform.position.y > yGoalPosition)
        {
            transform.position += Vector3.down * 2 * Time.deltaTime;
        }
        else
        {
            yGoalPosition = 4;
            state = State.MOVE_UP;
        }
    }
    void Update_MOVE_UP()
    {

        if (transform.position.y < yGoalPosition)
        {
            transform.position += Vector3.up * 2 * Time.deltaTime;
        }
        else
        {
            yGoalPosition = 0;
            state = State.MOVE_DOWN;
        }
    }
}
