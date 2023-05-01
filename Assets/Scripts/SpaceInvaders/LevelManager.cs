using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelManager : MonoBehaviour
{
    public MainCharacter character;
    public Enemy_Spawner spawner;
    public UIManager uiManager;
    public TextMeshPro introText;
    public AudioSource fightAudioSource;
    public AudioSource victoryDefeatAudioSource;
    public enum LogicState { START, INTRO, RUNNING, OUTRO, DEAD, END }
    public LogicState state = LogicState.START;
    public int playerScore;
    static public LevelManager instance;
    public bool isPaused = false;
    public List<AudioClip> menuAudioClips;
    private int playerHp;
    private string[] victoryText = new string[] { "HONK HONK HONK!", "YOU GOT THOSE EYES SHUT!", "NICE JOB, OLD GEEZER", "GG GOOSE", "RADUCKAL", "IMPRESSIVE" };
    private string[] victoryText2;
    private string[] fightText = new string[] { "DUCK!!!", "EYES UP, OLD DUCK", "KEEP YOUR EYES ON THE ENEMY!", "WATCH YOUR NECK!", "ACTION IS GO!", "TIME TO GO HONKERS!" };
    //public int levelCount = 1;
    private string quitText = "GODSPEED YOU, GEEZ";

    //public AudioSource readyAudioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
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
    }

    // Update is called once per frame
    private void Update()
    {
        //if (state == LogicState.END&&character!=null)
        //    state = LogicState.START;

        if (state == LogicState.START)
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
        spawner.maxEnemies = 3 + GameManager.Instance.levelCount;
        character.gameObject.SetActive(true);
        if (GameManager.Instance.activeGunPossessed != null)
        {

            GameObject actWeap = Instantiate(GameManager.Instance.activeGunPossessed, character.transform.position, Quaternion.Euler(0, 0, 45));
            WeaponsClass actWeapp = actWeap.GetComponent<WeaponsClass>();

            character.activeGunPrefab = actWeap;
            character.gunPossesed = actWeapp;

            //character.activeGunPrefab = GameManager.Instance.activeGunPossessed;
            ////character.gunPossesed = GameManager.Instance.gunPossessed;
            ///*GameObject bigGunz =*/ Instantiate(character.activeGunPrefab, character.gameObject.transform.position, Quaternion.Euler(0,0,45));
            ////BigGun bigGunDropping = bigGunz.GetComponent<BigGun>();
            ////bigGunDropping.Drop(Vector3.down * 3f);


            //character.gunPossesed = Instantiate(GameManager.Instance.gunPossessed, character.gameObject.transform.position, Quaternion.Euler(0, 0, 45));


            //character.activeGunPrefab = GameManager.Instance.activeGunPossessed;
            //WeaponsClass activeWeap = Instantiate(character.activeGunPrefab.GetComponent<WeaponsClass>(), character.gameObject.transform.position, Quaternion.Euler(0, 0, 45));

            //character.activeGunPrefab = Instantiate(character.activeGunPrefab, character.gameObject.transform.position, Quaternion.Euler(0, 0, 45));

        }
        playerHp = GameManager.Instance.PlayerHp + 1 + GameManager.Instance.levelCount / 3;
        character.hp = playerHp > 9 ? 9 : playerHp;
        //playerHp = character.hp;
        uiManager.scorePoints = GameManager.Instance.currentScore;
        uiManager.enemiesKilled = GameManager.Instance.enemiesKilledInRun;
        uiManager.Show(true);
        if (GameManager.Instance.levelCount == 1)
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
        if (GameManager.Instance.levelCount < 10)
            introText.text = fightText[Random.Range(0, 5)];
        else
            introText.text = fightText[Random.Range(0, 6)];

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
        //character.gameObject.SetActive(true); cos� accende tutto object e non solo script

    }

    IEnumerator OutroCoroutine()
    {
        //spawner.enabled = false;
        //playerHp = character.hp;
        if(character.activeGunPrefab!=null)
        GameManager.Instance.SetGameManagerGunPossessed(character.activeGunPrefab);
        //GameObject collectedGunPref= character.activeGunPrefab;
        //GameManager.Instance.activeGunPossessed = collectedGunPref;
        //WeaponsClass collectedGunScript = character.gunPossesed;
        //GameManager.Instance.gunPossessed = collectedGunScript;
        character.enabled = false;
        victoryDefeatAudioSource.clip = menuAudioClips[0];
        victoryDefeatAudioSource.Play();
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.levelCount < 10)
            introText.text = victoryText[Random.Range(0, 5)];
        else
            introText.text = victoryText[Random.Range(0, 6)];

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

        GameManager.Instance.levelCount++;
        GameManager.Instance.currentScore = uiManager.scorePoints;
        GameManager.Instance.PlayerHp = character.hp;
        GameManager.Instance.enemiesKilledInRun = uiManager.enemiesKilled;
        //levelCount++;
        character.IsInvulnerable = false;
        state = LogicState.END;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SpaceInvaders_GameScene");
    }

    IEnumerator DeathCoroutine()
    {
        spawner.enabled = false;
        introText.text = "ROASTED.";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        victoryDefeatAudioSource.clip = menuAudioClips[1];
        yield return new WaitForSeconds(2f);
        victoryDefeatAudioSource.Play();
        yield return new WaitForSeconds(.5f);
        //uiManager.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        uiManager.Show(false);
        yield return new WaitForSeconds(.3f);
        Enemy[] enemyInScene = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemyInScene)
        {
            //    //GameObject enemyObj= enemy.gameObject;
            //    if (enemy.gameObject != null)
            //    {
            enemy.gameObject.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InElastic);
            Destroy(enemy.gameObject, .3f);
            //        //spawner.enemyList.Remove(enemy);
            //        Destroy(enemy.gameObject, .5f);
            //        //enemy.DestroyThisEnemy();
            //    }
            //    //Destroy(enemy?.gameObject,.5f);  
        }
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        if (uiManager.scorePoints > GameManager.Instance?.LoadData("SpaceInvaders_Score"))
        {
            GameManager.Instance?.SaveData("SpaceInvaders_Score", uiManager.scorePoints);
            MainMenu.instance.hiScoreText.text = uiManager.scorePoints.ToString();
        }
        yield return new WaitForSeconds(.5f);
        introText.text = "HONK";
        introText.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutElastic);
        introText.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(.7f);
        GameManager.Instance.ResetRun();
        state = LogicState.END;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN_MENU");
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
