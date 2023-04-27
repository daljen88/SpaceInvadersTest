using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Timeline.Actions;

public class LevelManager : MonoBehaviour
{
    public MainCharacter character;
    public Enemy_Spawner spawner;
    public UIManager uiManager;
    public TextMeshPro introText;
    public AudioSource fightAudioSource;
    public AudioSource victoryAudioSource;
    public enum LogicState { START, INTRO, RUNNING, OUTRO, DEAD, END}
    public LogicState state = LogicState.START;
    public int playerScore;
    static public LevelManager instance;
    public bool isPaused = false;
    public List<AudioClip> menuAudioClips;
    private int playerHp;
    public int levelCount = 1;


    //public AudioSource readyAudioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            //this distrugge lo script attaccato al game object
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //character=new MainCharacter();  
        //character.hp=playerHp;

        playerHp = character.hp;
        //introLevelText=(LevelNumber);
        //enemyNumber=6+LevelNumber
        //livesBefore=0;
        //livesBefore=mainChar?.lives
        //if lives<9
        //lives=livesBefore+1


        //StartCoroutine(IntroCoroutine());
    }

    // Update is called once per frame
    private void Update()
    {
        if (state == LogicState.END&&character!=null)
            state = LogicState.START;

        if (state == LogicState.START)
        {
            state = LogicState.INTRO;
            StartCoroutine(IntroCoroutine());
        }

        if (state == LogicState.RUNNING) 
        {
            if(character.IsDead == true)
            {
                state = LogicState.DEAD;
                StartCoroutine(DeathCoroutine());
            }
            if (spawner.win)
            {
                character.IsInvulnerable = true;
                state = LogicState.OUTRO;
                StartCoroutine(OutroCoroutine());
                //spawner.enabled = false;
                //uiManager.enabled = false;
                //spegne UI player
                //uiManager.Show(false);
            }
        }
    }

    IEnumerator IntroCoroutine()
    {
        spawner.win = false;
        spawner.maxEnemies = 1 + levelCount;
        character.hp = playerHp + 1 + levelCount / 3;
        playerHp = character.hp;
        uiManager.Show(true);
        if (levelCount == 1) 
            uiManager.InitUI();
        else
            uiManager.SetUI();
        //spawner.enabled = true;
        character.transform.position = new Vector3(0, -8, 0);
        character.transform.DOMoveY(-3.5f, 4);
        introText.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1);
        //compare scritta READY
        introText.text = "GEEZ. READY?";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(1);
        //scompare scritta ready
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(.5f);
        //compare scritta Fight
        introText.text = "KEEP YOUR EYES ON THE ENEMY!";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);

        //metti qua eventuale delay: yield return new WaitForSeconds
        fightAudioSource.Play();

        yield return new WaitForSeconds(.5f);

        character.enabled = true;
        spawner.enabled = true;
        state = LogicState.RUNNING;

        yield return new WaitForSeconds(.5f);
        //scompare Fight
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        //character.gameObject.SetActive(true); così accende tutto object e non solo script

    }

    IEnumerator OutroCoroutine() 
    {
        //spawner.enabled = false;
        playerHp = character.hp;
        character.enabled = false;
        victoryAudioSource.clip = menuAudioClips[0];
        victoryAudioSource.Play();
        yield return new WaitForSeconds(1);
        introText.text = "YOU GOT THOSE EYES SHUT!!";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        character.transform.DOMoveX(0, 2.5f);
        yield return new WaitForSeconds(2.5f);
        //scompare victory message
        character.transform.DOMoveY(8, 2.5f);

        yield return new WaitForSeconds(3f);
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        uiManager.Show(false);
        if (uiManager.scorePoints > GameManager.Instance?.LoadData("SpaceInvaders_Score"))
        {
            GameManager.Instance?.SaveData("SpaceInvaders_Score", uiManager.scorePoints);
            MainMenu.instance.hiScoreText.text = uiManager.scorePoints.ToString();
        }
        yield return new WaitForSeconds(2.2f);
        spawner.enabled = false;
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN_MENU");
        levelCount++;
        character.IsInvulnerable = false;
        state = LogicState.END;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SpaceInvaders_GameScene");
    }

    IEnumerator DeathCoroutine()
    {
        spawner.enabled = false;
        introText.text = "ROASTED.";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        victoryAudioSource.clip = menuAudioClips[1];
        yield return new WaitForSeconds(2f);
        victoryAudioSource.Play();
        yield return new WaitForSeconds(.5f);
        uiManager.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(.5f);
        Enemy[] enemyInScene= FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyInScene)
        {
        //    //GameObject enemyObj= enemy.gameObject;
        //    if (enemy.gameObject != null)
        //    {
               enemy.gameObject.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
               Destroy(enemy.gameObject,.5f);
        //        //spawner.enemyList.Remove(enemy);
        //        Destroy(enemy.gameObject, .5f);
        //        //enemy.DestroyThisEnemy();
        //    }
        //    //Destroy(enemy?.gameObject,.5f);  
        }
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        state = LogicState.END;
        if (uiManager.scorePoints > GameManager.Instance?.LoadData("SpaceInvaders_Score"))
        {
            GameManager.Instance?.SaveData("SpaceInvaders_Score", uiManager.scorePoints);
            MainMenu.instance.hiScoreText.text = uiManager.scorePoints.ToString();
        }
        //GameManager.Instance?.SaveData("SpaceInvaders_Score", uiManager.scorePoints);
        yield return new WaitForSeconds(.6f);
        introText.text = "HONK";
        introText.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutElastic);
        introText.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(.6f);
        //Application.Quit();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN_MENU");
        //UnityEngine.SceneManagement.SceneManager.UnloadScene("SpaceInvaders_GameScene");
        //Destroy(gameObject);
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
}
