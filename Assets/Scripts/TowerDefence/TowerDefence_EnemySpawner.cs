using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefence_EnemySpawner : MonoBehaviour
{
    public List<TowerDefence_Enemy> EnemyList;
    public List<NarutoGridNode> SpawnPoint;
    public int maxEnemeyCount = 50;
    // Start is called before the first frame update
    void Start()
    {
        //parte spawner e invoca un nemico
        Invoke("SpawnRandomEnemy", Random.Range(1f, 3f));
    }
    public void SpawnRandomEnemy()
    {
        TowerDefence_Enemy tEnemy = Instantiate(EnemyList[Random.Range(0, EnemyList.Count)],
            SpawnPoint[Random.Range(0, EnemyList.Count)].transform.position+Vector3.up, Quaternion.Euler(Vector3.zero));
        //se max nemici raggiunti non spawna
        if(--maxEnemeyCount > 0)
        {
            Invoke("SpawnRandomEnemy", Random.Range(1f,3f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
