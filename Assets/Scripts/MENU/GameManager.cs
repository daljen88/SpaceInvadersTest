using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int levelCount = 1;
    public int currentScore = 0;
    public int enemiesKilledInRun = 0;
    [SerializeField] private int playerHp = 4;
    public int PlayerHp 
    { 
        get { return playerHp; } 
        set 
        { 
            playerHp = value>9?9:value;
        } 
    }
    public WeaponsClass gunPossessed;
    public GameObject activeGunPossessed;

    public void SetGameManagerGunPossessed(GameObject gun)
    {
        GameObject actGun = gun;
        WeaponsClass actGunScript = gun.GetComponent<WeaponsClass>();
        gunPossessed = actGunScript;
        activeGunPossessed = actGun;
    }

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

    public void ResetRun()
    {
        levelCount = 1;
        currentScore = 0;
        playerHp = 4;
        enemiesKilledInRun = 0;
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
