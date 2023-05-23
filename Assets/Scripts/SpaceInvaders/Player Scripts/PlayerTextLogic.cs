using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTextLogic : MonoBehaviour
{
    public static PlayerTextLogic instance;
    public string[] foundNewGunPhrases = { "GEEZ, that's a GUN!" };
    public string[] foundRadio = { "Now THAT will tune this UP! " };
    public string[] thirdLevelOpening = { "WAIT! Don't you also feel something is missing?" };
    public string[] foundAlarmClock = { "Mmm, an Alarm Clock. What if I strike this B.." };


    public TextMeshPro playerText;
    public GameObject TextWindow;
    public AudioClip[] keyHitSounds;
    private char[] textStringToChar;
    private bool firstGun = false;
    private bool firstRadio = false;
    private bool openingDone = false;
    private bool firstAlarm = false;
    private Vector3 startingPos;
    private bool NormalEnemiesKilledCondition => UIManager.instance.normalEnemyKilled == 12;
    public bool routineIsRunning = false;
    public Coroutine runningRoutine;
    public bool routinePaused;

    //public UnityEvent foundGun;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startingPos = TextWindow.transform.localPosition;
        //WeaponsClass.dropEvent.AddListener(FoundNewGun);
        playerText.text = "";
    }

    void Update()
    {
        if (transform.position.x > 1f)
            TextWindow.transform.localPosition = new Vector3(-3.7f, TextWindow.transform.localPosition.y);
        else
            TextWindow.transform.localPosition = new Vector3(3.5f, TextWindow.transform.localPosition.y);

        if (!openingDone && GameManager.Instance.LevelCount == 3 && LevelManager.instance.state == LevelManager.LogicState.RUNNING && NormalEnemiesKilledCondition)
        {
            openingDone = true;
            SecondLevelOpening();
        }
    }

    public void StopCoroutineResetTextTime()
    {
        if (routineIsRunning == true /*&& runningRoutine != null*/ )
        {
            StopCoroutine(runningRoutine);
            routineIsRunning = false;
            Time.timeScale = 1;
            TextWindow.SetActive(false);
            playerText.text = "";
        }
        else { return; }
    }

    private void SecondLevelOpening()
    {
        textStringToChar = thirdLevelOpening[0].ToCharArray();
        runningRoutine=StartCoroutine(FoundFirstCoroutine(textStringToChar));

    }

    public void FoundFirstRadio()
    {
        if (!firstRadio)
        {
            firstRadio = true;
            textStringToChar = foundRadio[0].ToCharArray();
            runningRoutine=StartCoroutine(FoundFirstCoroutine(textStringToChar));
        }
        else { return; }
    }
    public void FoundFirstAlarmClock()
    {
        //COONTROLLO IF QUA DENTRO
        firstAlarm = true;
        textStringToChar = foundAlarmClock[0].ToCharArray();
        runningRoutine=StartCoroutine(FoundFirstCoroutine(textStringToChar));
    }
    public void FoundNewGun()
    {
        if (!firstGun)
        {
            firstGun = true;
            textStringToChar = foundNewGunPhrases[0].ToCharArray();
            runningRoutine=StartCoroutine(FoundFirstCoroutine(textStringToChar));
        }
        else { return; }
    }

    public IEnumerator FoundFirstCoroutine(char[] charsToStamp)
    {
        routineIsRunning = true;

        while (routineIsRunning)
        {
            Time.timeScale = .7f;
            yield return new WaitForSecondsRealtime(.2f);
            yield return new WaitUntil(() => !LevelManager.instance.IsPaused);

            Time.timeScale = .4f;
            yield return new WaitForSecondsRealtime(.2f);
            yield return new WaitUntil(() => !LevelManager.instance.IsPaused);

            Time.timeScale = .2f;
            yield return new WaitForSecondsRealtime(.1f);
            yield return new WaitUntil(() => !LevelManager.instance.IsPaused);

            Time.timeScale = .05f;
            TextWindow.SetActive(true);

            foreach (char c in charsToStamp)
            {
                /*GetComponent<TextMeshPro>()*/
                yield return new WaitForSecondsRealtime(.04f);
                yield return new WaitUntil(()=>!LevelManager.instance.IsPaused);
                Time.timeScale = .05f;
                playerText.text += c;
                TextWindow.GetComponent<AudioSource>().clip = keyHitSounds[Random.Range(0, 2)];
                TextWindow.GetComponent<AudioSource>().Play();

            }
            yield return new WaitForSecondsRealtime(.5f);
            yield return new WaitUntil(() => !LevelManager.instance.IsPaused);

            Time.timeScale = .5f;
            yield return new WaitForSecondsRealtime(.3f);
            yield return new WaitUntil(() => !LevelManager.instance.IsPaused);

            Time.timeScale = 1;
            TextWindow.SetActive(false);
            //LevelManager.instance.storyOver = true;
            playerText.text = "";
            routineIsRunning = false;
        }
    }    
}
