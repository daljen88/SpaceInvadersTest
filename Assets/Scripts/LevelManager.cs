using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public MainCharacter character;
    public Enemy_Spawner spawner;
    public UIManager uiManager;
    public TextMeshPro introText;
    public AudioSource fightAudioSource;
    public AudioSource victoryAudioSource;
    public enum LogicState { INTRO, RUNNING, OUTRO, END}
    public LogicState state = LogicState.INTRO;
    public int playerScore;
    static public LevelManager instance;
    public bool isPaused = false;

    //public AudioSource readyAudioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntroCoroutine());
    }
    private void Awake()
    {
        instance= this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (state == LogicState.RUNNING) 
        {
            if (spawner.CheckPlayerVictory())
            {
                state= LogicState.OUTRO;
                StartCoroutine(OutroCoroutine());
                character.enabled = false;
                spawner.enabled = false;
                //uiManager.enabled = false;
                uiManager.Show(false);
            }
         }
    }

    IEnumerator OutroCoroutine() 
    {
        victoryAudioSource.Play();
        yield return new WaitForSeconds(1);
        introText.text = "YOU GOT THOSE EYES SHUT!!";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        character.transform.DOMoveX(0, 2.5f);
        yield return new WaitForSeconds(2.5f);
        //scompare Fight
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        character.transform.DOMoveY(8, 2.5f);
    }

    IEnumerator IntroCoroutine()
    {
        
        character.transform.position = new Vector3(0, -6, 0);
        character.transform.DOMoveY(-4,4);
        introText.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1);
        //compare scritta READY
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(1);
        //scompare scritta ready
        introText.transform.DOScale(Vector3.one,.5f).SetEase(Ease.InElastic); 
        yield return new WaitForSeconds(.5f);
        //compare scritta Fight
        introText.text = "KEEP YOUR EYES ON THE ENEMY!";
        introText.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);

        //metti qua eventuale delay: yield return new WaitForSeconds
        fightAudioSource.Play();


        yield return new WaitForSeconds(.5f);


        character.enabled= true;
        spawner.enabled= true;
        uiManager.enabled= true;
        state = LogicState.RUNNING;

        yield return new WaitForSeconds(.5f);
        //scompare Fight
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        //character.gameObject.SetActive(true); così accende tutto object e non solo script

    }
    public void AddScore(int score)
    {
        playerScore += score;
        uiManager.SetScore(playerScore);
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
