using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int levelCount = 1;
    public int currentScore = 0;


    private void Awake()
    {
       // if(Instance)
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            //this distrugge lo script attaccato al game object
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //currentScore= 0;
        //levelCount= 1;
        Debug.Log($"EndlessRunner_Score {LoadData("EndlessRunner_Score")}");
        Debug.Log($"SpaceInvaders_Score {LoadData("SpaceInvaders_Score")}");
        Debug.Log($"TowerDefence_Score {LoadData("TowerDefence_Score")}");


        //SaveData("EndlessRunner_Score", 666);
        //SaveData("SpaceInvaders_Score",UIManager.instance.scorePoints);
        //SaveData("TowerDefence_Score", 66666);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();

    }
    public int LoadData(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
}
