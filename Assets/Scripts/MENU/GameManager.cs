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
    public bool musicRadioCollected = false;
    private bool musicPlaying = false;

    public WeaponsClass typeGunPossessed;
    public GameObject objectGunPossessed;

    public void SetGameManagerGunPossessed(GameObject gun)
    {
        objectGunPossessed = gun;
        typeGunPossessed = gun.GetComponent<WeaponsClass>();
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
        musicRadioCollected = false;
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
