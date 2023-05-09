using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SecondEnemySpawner : MonoBehaviour
{
    public static SecondEnemySpawner Instance;
    public ThirdEnemy bringerEnemyTemplate;
    public SecondEnemy bonusEnemyTemplate;
    public float spawnTime = 2;
    public int maxBonusEnemies = 4;
    public int maxBringerEnemies = 4;

    public List<SecondEnemy> bonusEnemyList;
    public List<ThirdEnemy> bringerEnemyList;

    //public bool win = false;
    public enum State { IDLE, MOVE_DOWN, MOVE_UP }
    public State state=State.IDLE;
    public float yGoalPosition;
    //numero di controllo per impedire il respawn raggiunto un determinato score
    public int controlNum = 1;
    public int controlNum2 = 1;

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
        if (maxBonusEnemies > 0)
        {
            maxBonusEnemies--;
            /*enemies.Add(*/
            bonusEnemyList.Add(Instantiate(bonusEnemyTemplate, transform.position, transform.rotation))/*)*/;

        }
        //else
        //{
        //    CancelInvoke("SpawnEnemy");

        //}
    }
    public void SpawnBringerEnemy()
    {
        if (maxBringerEnemies > 0)
        {
            maxBringerEnemies--;
            /*enemies.Add(*/
            ThirdEnemy activeBringerEnemy = Instantiate(bringerEnemyTemplate, transform.position, transform.rotation);
            bringerEnemyList.Add(activeBringerEnemy)/*)*/;
            //GameObject dropToEnemy=  Instantiate(activeBringerEnemy.drops[0], activeBringerEnemy.dropSlot.transform.position, activeBringerEnemy.dropSlot.transform.rotation);
            //ISTANZIA RADIO SE NON GIA PRESA
            if (GameManager.Instance.musicRadioCollected == false)
            {
                GameObject dropToEnemy = Instantiate(activeBringerEnemy.drops[0], activeBringerEnemy.dropSlot.transform.position, activeBringerEnemy.dropSlot.transform.rotation);
                dropToEnemy.transform.parent = activeBringerEnemy.gameObject.transform;
                activeBringerEnemy.bringingDrop = dropToEnemy.GetComponent<DropsClass>();
            }
            //activeBringerEnemy.bringingDrop =
            //activeBringerEnemy.dropSlot
        }
        //else
        //{
        //    CancelInvoke("SpawnEnemy");

        //}
    }
    private bool IsEnemyDead => bonusEnemyList.Count == 0;/*&&controlNum != UIManager.instance.scorePoints*/
    private bool IsBringerEnemyDead => bringerEnemyList.Count == 0;


    public void SpawnOneEnemy(string enemy)
    {
        //se ha già spawnato un nemico ad un determinato score, salta i successivi 
        if (enemy == "bonusEnemy")
        {
            
            if (IsEnemyDead)
            {
                SpawnEnemy();
                //controlNum = UIManager.instance.scorePoints;
            }
        }
        else
        {
            if (IsBringerEnemyDead)
            {
                SpawnBringerEnemy();
                //controlNum2 = UIManager.instance.scorePoints;
            }
        }
    }

    private void Update()
    {
        if(/*UIManager.instance.scorePoints!=0&&*/UIManager.instance.totalEnemiesKilled>4/*&& UIManager.instance.scorePoints % 42==0*/)
        {
            if (UIManager.instance.scorePoints % 24 == 0 || UIManager.instance.scorePoints % 30 == 0 || UIManager.instance.scorePoints % 36 == 0)
                if (Random.Range(0, 11) < 8)
                {
                    if (!Enemy_Spawner.Instance.CheckPlayerVictory())
                        SpawnOneEnemy("bonusEnemy");
                }
        }
        if (UIManager.instance.totalEnemiesKilled != 0 && UIManager.instance.totalEnemiesKilled % 5 == 0)
        {
            if (true/*maxBringerEnemies < 4 && Random.Range(0, GameManager.Instance.levelCount) < 2*/)
            {
                if (Enemy_Spawner.Instance.CheckPlayerVictory()==false)
                    SpawnOneEnemy("bringerEnemy");
            }
            else
            {
                //if (!Enemy_Spawner.Instance.CheckPlayerVictory())
                //    SpawnOneEnemy("bringerEnemy");
            }
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
