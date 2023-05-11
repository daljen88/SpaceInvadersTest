using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.TextCore.Text;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    static public LevelManager instance;
    public MainCharacter character;
    public Enemy_Spawner spawner;
    public UIManager uiManager;
    public TextMeshPro introText;
    public AudioSource readyAudioSource;
    public AudioSource fightAudioSource;
    public AudioSource victoryDefeatAudioSource;
    public enum LogicState { STORY, START, INTRO, RUNNING, OUTRO, DEAD, END }
    public LogicState state = LogicState.START;
    public int playerScore;
    public bool isPaused = false;
    public List<AudioClip> menuAudioClips;
    private int GetStartingPlayerHp 
    {
        get=> GameManager.Instance.PlayerHp + 1 + GameManager.Instance.levelCount / 3;
    }
    private string[] fightText = new string[] { "GOOSPEED YOU!", "DUCK!!!", "EYES UP, OLD DUCK", "KEEP YOUR EYES ON THE ENEMY!", "WATCH YOUR NECK!", "ACTION IS GO!", "TIME TO GO HONKERS!" };
    private string[] victoryText = new string[] { "HONK HONK HONK!", "YOU GOT THOSE EYES SHUT!", "NICE JOB, OLD GEEZER", "GG GOOSE", "RADUCKAL", "IMPRESSIVE" };
    private string[] victoryText2;
    private string quitText = "GOOSPEED YOU";

    private string storyText = "Aliens from another galaxy have invaded Earth space-time frame and stole some valuable technology from the 80s,"+
        " now the continuum is breaking apart! You, as a member of the GUTSS (Galactic Union for Time-Space Stability), have to bring those devices back to the" +
        " earthlings and fix the stability of life as we know it!";
    private char[] storyTextByChar;
    public TextMeshPro uiStoryText;
    public bool storyOver = false;
    public bool story2Over = false;

    //public AudioSource readyAudioSource;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        introText.transform.localScale = Vector3.zero;
        uiStoryText.enabled = true;

        //Time.timeScale = 0;

        #region old variables setting
        //character=new MainCharacter();  
        //character.hp=playerHp;
        //playerHp = character.hp;
        //introLevelText=(LevelNumber);
        //enemyNumber=6+LevelNumber
        //livesBefore=0;
        //livesBefore=mainChar?.lives
        //if lives<9
        //lives=livesBefore+1
        //StartCoroutine(IntroCoroutine());
        #endregion
    }

    private void Update()
    {
        //if(state==LogicState.STORY)
        //{
        //    uiStoryText.enabled = true;
        //    StartCoroutine(WriteByLetterCoroutine());
        //}

        if (state == LogicState.END&&character!=null)
            state = LogicState.START;

        if (state == LogicState.START && storyOver)
        {
            state = LogicState.INTRO;
            StartCoroutine(IntroCoroutine());
        }

        if (state == LogicState.RUNNING)
        {
            if (character.IsDead == true)
            {
                state = LogicState.DEAD;
                StartCoroutine(DeathCoroutine());
            }
            if (spawner.win)
            {
                character.IsInvulnerable = true;
                state = LogicState.OUTRO;
                StartCoroutine(OutroCoroutine());
            }
        }
    }

    IEnumerator WriteByLetterCoroutine()
    {
        yield return new WaitForSeconds(.2f);
        foreach (char c in storyTextByChar)
        {
            uiStoryText.text += c;
            yield return new WaitForSecondsRealtime(.02f);
        }

    }

    IEnumerator IntroCoroutine()
    {
        //ATTIVA PLAYER E SETTA VALORI HP
        character.gameObject.SetActive(true);
        //playerHp = GameManager.Instance.PlayerHp + 1 + GameManager.Instance.levelCount / 3;
        character.Hp = GetStartingPlayerHp > 9 ? 9 : GetStartingPlayerHp;

        #region assegna gun e script al player
        //ASSEGNA GAME OBJECT E SCRIPT ARMA PRESI DAL GAMAE MANAGER
        //if (GameManager.Instance.activeGunPossessed != null)
        //{
        //    character.activeGunPrefab = GameManager.Instance.activeGunPossessed;
        //    character.gunPossesed = GameManager.Instance.typeGunPossessed;
        //}
        #endregion

        //SETTA VALORI UI
        uiManager.scorePoints = GameManager.Instance.currentScore;
        uiManager.totalEnemiesKilled = GameManager.Instance.enemiesKilledInRun;
        uiManager.Show(true);
        if (GameManager.Instance.levelCount == 1)
            uiManager.InitUI();
        else
            uiManager.SetUI();
        //SETTA VALORI SPAWNER
        spawner.win = false;
        spawner.maxEnemies = 3 + GameManager.Instance.levelCount;
        SecondEnemySpawner.Instance.maxBonusEnemies = 4;
        SecondEnemySpawner.Instance.maxBringerEnemies = 4;
        //SPOSTA PLAYER IN GIOCO
        if (character.transform.position.y > -3)
        {
            character.transform.position = new Vector3(0, -8, 0);
            character.transform.DOMoveY(-3.5f, 3);
        }
        introText.transform.localScale = Vector3.zero;

        readyAudioSource.Play();
        yield return new WaitForSeconds(1f);

        //compare scritta READY
        introText.text = "GEEZ. READY?";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(1);

        //scompare scritta ready
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(.5f);

        //compare scritta Fight
        if (GameManager.Instance.levelCount < 10)
            introText.text = fightText[Random.Range(0, fightText.Count()-2)];
        else
            introText.text = fightText[Random.Range(0, fightText.Count()-1)];
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        fightAudioSource.Play();
        yield return new WaitForSeconds(.5f);

        //STATE RUNNING: PLAYER AND SPAWNER ENABLE
        character.enabled = true;
        spawner.enabled = true;
        SecondEnemySpawner.Instance.enabled = true;

        state = LogicState.RUNNING;
        yield return new WaitForSeconds(.5f);

        //scompare Fight
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
    }

    IEnumerator OutroCoroutine()
    {
        //SETTA GAME OBJECT ARMA AL GAME MANAGER
        //if(character.activeGunPrefab!=null)
        //GameManager.Instance.SetGameManagerGunPossessed(character.activeGunPrefab);

        //DISATTIVA PLAYER E SPAWNER
        character.enabled = false;
        spawner.enabled = false;
        SecondEnemySpawner.Instance.enabled = false;

        //AUDIO VITTORIA
        victoryDefeatAudioSource.clip = menuAudioClips[0];
        victoryDefeatAudioSource.Play();
        yield return new WaitForSeconds(1);

        //TEXT VITTORIA
        if (GameManager.Instance.levelCount < 10)
            introText.text = victoryText[Random.Range(0, victoryText.Count()-2)];
        else
            introText.text = victoryText[Random.Range(0, victoryText.Count()-1)];
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        //sposta player parte1
        character.transform.DOMoveX(0, 2.5f);
        yield return new WaitForSeconds(2.5f);

        //sposta player parte2
        character.transform.DOMoveY(8, 2.5f);
        yield return new WaitForSeconds(3f);

        //scompare victory message
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        //disattiva UI
        uiManager.Show(false);
        //salva punteggio se maggiore dell'HIGH SCORE
        if (uiManager.scorePoints > GameManager.Instance?.LoadData("SpaceInvaders_Score"))
        {
            GameManager.Instance?.SaveData("SpaceInvaders_Score", uiManager.scorePoints);
            MainMenu.instance.hiScoreText.text = uiManager.scorePoints.ToString();
        }
        yield return new WaitForSeconds(2.2f);

        //AUMENTA VARIABILI NUOVO LIVELLO DEL GAME MANAGER
        GameManager.Instance.levelCount++;
        GameManager.Instance.currentScore = uiManager.scorePoints;
        GameManager.Instance.PlayerHp = character.Hp;
        GameManager.Instance.enemiesKilledInRun = uiManager.totalEnemiesKilled;
        //toglie invulerabilità al player
        character.IsInvulnerable = false;
        //END STATE
        state = LogicState.END;
        //UnityEngine.SceneManagement.SceneManager.LoadScene("SpaceInvaders_GameScene");
    }

    IEnumerator DeathCoroutine()
    {
        //disattiva spawner
        spawner.enabled = false;
        //TEXT SCONFITTA
        introText.text = "ROASTED.";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        //AUDIO SCONFITTA
        victoryDefeatAudioSource.clip = menuAudioClips[1];
        yield return new WaitForSeconds(2f);

        victoryDefeatAudioSource.Play();
        yield return new WaitForSeconds(.5f);

        //SCOMAPRE UI
        //uiManager.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        uiManager.Show(false);
        yield return new WaitForSeconds(.3f);

        //RIMUOVE ENEMYS RIMASTI DALLA SCENA
        Enemy[] enemyInScene = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyInScene)
        {
            enemy.gameObject.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InElastic);
            Destroy(enemy.gameObject, .3f);
        }
        //scompare text
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        //salva punteggio se maggiore dell'HIGH SCORE
        if (uiManager.scorePoints > GameManager.Instance?.LoadData("SpaceInvaders_Score"))
        {
            GameManager.Instance?.SaveData("SpaceInvaders_Score", uiManager.scorePoints);
            MainMenu.instance.hiScoreText.text = uiManager.scorePoints.ToString();
        }
        yield return new WaitForSeconds(.5f);

        //text chiusura
        introText.text = "HONK";
        introText.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutElastic);
        introText.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(.7f);

        //resetta valori game manager
        GameManager.Instance.ResetRun();
        state = LogicState.END;
        //ricarica Menu Principale
        UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN_MENU");
    }

    public bool TogglePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
        return isPaused;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        isPaused = false;
        UIManager.instance.pauseGO.SetActive(false);
    }

    public void BackToMain()
    {
        if (uiManager.scorePoints > GameManager.Instance?.LoadData("SpaceInvaders_Score"))
        {
            GameManager.Instance?.SaveData("SpaceInvaders_Score", uiManager.scorePoints);
            MainMenu.instance.hiScoreText.text = uiManager.scorePoints.ToString();
        }
        GameManager.Instance.ResetRun();       
        UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN_MENU");
    }
}
