using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public Enemy enemyTemplate;
    public float spawnTime = 2;
    public int maxEnemies = 10;

    //private List<Enemy> enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        //enemies.Capacity= maxEnemies;
        //invoca a ripetizione funzione: nome funz, tempo prima di avvio, cadenza
        InvokeRepeating("SpawnEnemy", 1, spawnTime);
    }

    // Update is called once per frame
    void SpawnEnemy()
    {
        if (maxEnemies > 0)
        {
            maxEnemies--;
            /*enemies.Add(*/Instantiate(enemyTemplate, transform.position, transform.rotation)/*)*/;

        }
        else
        {
            CancelInvoke("SpawnEnemy");
        }
    }
    //private void Update()
    //{
    //    for (int i = 0; i < enemies.Count; i++)
    //    {
    //        if (enemies[i].transform.position.y<-4.5)
    //        {
    //            Destroy(enemies[i]);
    //            maxEnemies++;
    //        }
    //    }
        
    //}

}
