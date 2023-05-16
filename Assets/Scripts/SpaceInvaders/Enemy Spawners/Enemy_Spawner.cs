using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public static Enemy_Spawner Instance;
    public Enemy enemyTemplate;
    public float spawnTime = 2;
    public int maxEnemies = 3;
    public List<Enemy> enemyList;
    public bool win = false;

    //private List<Enemy> enemies;

    private void Awake()
    {
        Instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //enemies.Capacity= maxEnemies;
        //invoca a ripetizione funzione: nome funz, tempo prima di avvio, cadenza
        //InvokeRepeating("SpawnEnemy", 1, spawnTime);
    }
    private void OnEnable()
    {
        InvokeRepeating("SpawnEnemy", 1, spawnTime);

    }
    private void OnDisable()
    {
        CancelInvoke("SpawnEnemy");

    }
    //private void ActivateSpawner()
    //{
    //    InvokeRepeating("SpawnEnemy", 1, spawnTime);

    //}

    // Update is called once per frame
    public void SpawnEnemy()
    {
        if (maxEnemies > 0)
        {
            maxEnemies--;
            enemyList.Add( Instantiate(enemyTemplate, transform.position, transform.rotation));

        }
        else
        {
            CancelInvoke("SpawnEnemy");

        }
    }

    public bool CheckPlayerVictory()
    {
        if (maxEnemies > 0)
        {
            win = false;
            return win;
        }
        else
        {
            foreach (Enemy enemy in enemyList)
            {//se trovo un nemico vivo, return false
                if (enemy/* && enemy.hp > 0 && enemy.isActiveAndEnabled*/)
                {
                    win = false;
                    return win;
                }
            }
        }
        //arrivati qua nel controllo tutti i nemici sono morti

        win = true;
        return win;
    }
    public void StopSpawner()
    {
        CancelInvoke("SpawnEnemy");

    }

    private void Update()
    {
        CheckPlayerVictory();

        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        if (enemies[i].transform.position.y<-4.5)
        //        {
        //            Destroy(enemies[i]);
        //            maxEnemies++;
        //        }
        //    }

    }
    //IEnumerator SpawnEnemyCoroutine()
    //{ 
    
    //}

}
