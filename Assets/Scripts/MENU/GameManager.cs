using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int levelCount = 1;
    public int LevelCount { get => levelCount; set => levelCount = value; }
    public int currentScore = 0;
    public int enemiesKilledInRun = 0;

    [Header("PLAYER HP FORMULA = (int)[ basePlayerHp + baseHpIncrement + (levelCount / levelsPerHpIncrementRatio) ]")]
    [SerializeField] private int playerHp;      //this is variable of player Hp left, updated at the end of each level
    public int PlayerHp 
    { 
        get 
        {
            return LevelCount>1? playerHp:BasePlayerHp; 
        } 
        set 
        { 
            playerHp = value>9?9:value;
        } 
    }
    [SerializeField] private int basePlayerHp = 4;  //this is variable of first level base Hp
    public int BasePlayerHp => basePlayerHp;
    [SerializeField] private int baseHpIncrement=1;
    public int BaseHpIncrement=>baseHpIncrement;
    [SerializeField] private int levelsPerHpIncrementRatio = 10;
    public int LevelsPerHpIncrementRatio => levelsPerHpIncrementRatio;

    public int GetNewLevelPlayerHP              //here you get new level starting Hp
    {
        get => (PlayerHp + BaseHpIncrement + (LevelCount / LevelsPerHpIncrementRatio));
    }

    [SerializeField] private bool musicRadioCollected = false;
    public bool MusicRadioCollected { get => musicRadioCollected; set => musicRadioCollected = value; }
    private bool musicPlaying = false;

    [SerializeField] private bool alarmClockCollected = false;
    public bool AlarmClockCollected { get => alarmClockCollected; set => alarmClockCollected = value; }

    [SerializeField] private int numberOfAlarmsCollected=0;
    public int NumberOfAlarmsCollected { get => numberOfAlarmsCollected; set => numberOfAlarmsCollected=value; }

    [Header("MAX ENEMIES FORMULA = (int)[ StartingMaxEnemies + LevelCount * (LevelCount / 10 + 1) - (10 * ( (LevelCount / 10) * (LevelCount / 10 + 1) ) / 2) ]")]
    [SerializeField] private int startingMaxEnemies = 4;
    public int StartingMaxEnemies => startingMaxEnemies;

    //[SerializeField] private int maxEnemies;
    public int MaxEnemies
    {
        //per ottenere il numero corretto di nemici, all'aumentare dei nemici per livello,
        //sottrae la somma dei numeri naturali a cadenze di 10 => formula Gauss: [Sn] = n(n+1)/2  --> con n=LevelCount
        //for (int i = 1; i<= 50; i++)
        //{
        //   Debug.Log($"Level {i} max enemies = {4 + i * (i / 10 + 1) - (10 * ( (i / 10) * (i / 10 + 1) ) / 2)}");
        //}

        get => StartingMaxEnemies + LevelCount * (LevelCount / 10 + 1) - (10 * ( (LevelCount / 10) * (LevelCount / 10 + 1) ) / 2);

        //set => ;
    }

    public WeaponsClass typeGunPossessed;
    public GameObject objectGunPossessed;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void CollectAlarmClock()
    {
        AlarmClockCollected = true;
        numberOfAlarmsCollected++;
    }

    public void SetGameManagerGunPossessed(GameObject gun)
    {
        objectGunPossessed = gun;
        typeGunPossessed = gun?.GetComponent<WeaponsClass>();
    }


    void Start()
    {
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
        MusicRadioCollected = false;
        AlarmClockCollected = false;
        NumberOfAlarmsCollected = 0;
        musicPlaying =false;
        //GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Stop();
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

    void Update()
    {
        if (musicRadioCollected && !musicPlaying)
        {
            GetComponent<AudioSource>().Play();
            musicPlaying = true;
        }
    }
}
