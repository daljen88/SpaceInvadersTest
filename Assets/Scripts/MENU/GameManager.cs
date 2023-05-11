using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private bool musicRadioCollected = false;
    public bool MusicRadioCollected { get => musicRadioCollected; set => musicRadioCollected = value; }
    private bool musicPlaying = false;

    [SerializeField] private bool alarmClockCollected = false;
    public bool AlarmClockCollected { get => alarmClockCollected; set => alarmClockCollected = value; }
    [SerializeField] private int numberOfAlarmsCollected=0;
    public int NumberOfAlarmsCollected { get => numberOfAlarmsCollected; set => numberOfAlarmsCollected=value; }

    public WeaponsClass typeGunPossessed;
    public GameObject objectGunPossessed;
    
    public void CollectAlarmClock()
    {
        AlarmClockCollected = true;
        numberOfAlarmsCollected++;
    }

    public void SetGameManagerGunPossessed(GameObject gun)
    {
        NumberOfAlarmsCollected++;
        objectGunPossessed = gun;
        typeGunPossessed = gun?.GetComponent<WeaponsClass>();
    }

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
        musicPlaying=false;
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
