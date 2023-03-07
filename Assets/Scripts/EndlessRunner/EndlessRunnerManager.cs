using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerManager : MonoBehaviour
{
    static public EndlessRunnerManager instance;
    public List<GameObject> activePlatformList;
    public List<GameObject> platformsPool;


    private void Awake()
    {
        instance = this;
    }

    public void PutBackInPool(GameObject poolObject)
    {
        if(activePlatformList.Contains(poolObject))
        {
            platformsPool.Add(poolObject);
            activePlatformList.Remove(poolObject);
            poolObject.SetActive(false);
            PlacePlatform();
        }
    }
    public GameObject GetFromPool()
    {
        //se ho oggetti nella pool
        if(platformsPool.Count > 0)
        {
            //mi salvo l'oggetto da prendere
            GameObject poolGameObject = platformsPool[0];

            //lo rimuovo dalla pool
            platformsPool.RemoveAt(0);

            //lo aggiungo alla lista degli oggetti attivi
            activePlatformList.Add(poolGameObject);

            //ne restituisco l'istanza al giocatore
            return poolGameObject;
        }
        else
        {
            //la pool è vuota
            //TO DO creare un nuovo oggetto, da restituire al giocartore
            return null;
        }
    }


    public void PlacePlatform()
    {
        Vector3 lastPlatformPos = activePlatformList[activePlatformList.Count - 1].transform.position;
        lastPlatformPos += Vector3.right * activePlatformList[activePlatformList.Count - 1].transform.localScale.x / 2;
        //prende piattaf dalla pool
        GameObject platform = GetFromPool();

        //posiziona la piattaf a destra dell'ultima in active platform
        platform.transform.localScale=new Vector3(UnityEngine.Random.Range(10,30) ,1,1);
        platform.transform.position = lastPlatformPos
            + Vector3.right * UnityEngine.Random.Range(5, 10)
            + Vector3.up * UnityEngine.Random.Range(-3, 3)
            + Vector3.right * platform.transform.localScale.x / 2;
        platform.SetActive(true);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
