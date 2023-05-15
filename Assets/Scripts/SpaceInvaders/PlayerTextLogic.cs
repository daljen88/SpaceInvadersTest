using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTextLogic : MonoBehaviour
{
    public string[] foundNewGunPhrases = { "GEEZ, that's a GUN!" };
    public string[] foundRadio = { "Now THAT will tune this UP! " };
    public string[] secondLevelOpening = { "WAIT! Don't you also feel something is missing?" };
    public string[] foundAlarmClock = { "Mmm, an Alarm Clock. What if I strike this B.." };


    public TextMeshPro playerText;
    public GameObject TextWindow;
    public AudioClip[] keyHitSounds;
    private char[] textStringToChar;
    private bool firstGun = false;
    private bool firstRadio = false;
    private bool openingDone=false;
    private bool firstAlarm=false;
    private Vector3 startingPos;
    //public UnityEvent foundGun;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = TextWindow.transform.localPosition;
        //WeaponsClass.dropEvent.AddListener(FoundNewGun);
        playerText.text = "";    
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x>1f)
            TextWindow.transform.localPosition=new Vector3(-3.7f,TextWindow.transform.localPosition.y);
        else
            TextWindow.transform.localPosition = new Vector3(3.5f, TextWindow.transform.localPosition.y);

        if (!openingDone&&GameManager.Instance.levelCount==2&&LevelManager.instance.state==LevelManager.LogicState.RUNNING&&UIManager.instance.normalEnemyKilled==5)
        {
            openingDone = true;
            SecondLevelOpening();
        }
    }
    private void SecondLevelOpening()
    {
        textStringToChar = secondLevelOpening[0].ToCharArray();
        StartCoroutine(FoundFirstCoroutine(textStringToChar));

    }

    public void FoundFirstRadio()
    {
        if (!firstRadio)
        {
            firstRadio = true;
            textStringToChar = foundRadio[0].ToCharArray();
            StartCoroutine(FoundFirstCoroutine(textStringToChar));
        }
        else { return; }
    }
    public void FoundFirstAlarmClock()
    {
        firstAlarm = true;
        textStringToChar = foundAlarmClock[0].ToCharArray();
        StartCoroutine(FoundFirstCoroutine(textStringToChar));
    }
    public void FoundNewGun()
    {
        if (!firstGun)
        {
            firstGun = true;
            textStringToChar = foundNewGunPhrases[0].ToCharArray();
            StartCoroutine(FoundFirstCoroutine(textStringToChar));
        }
        else { return; }
    }

    IEnumerator FoundFirstCoroutine(char[] charsToStamp)
    {
        Time.timeScale = .7f;
        yield return new WaitForSecondsRealtime(.2f);
        Time.timeScale = .4f;
        yield return new WaitForSecondsRealtime(.2f);
        Time.timeScale = .2f;
        yield return new WaitForSecondsRealtime(.1f);
        TextWindow.SetActive(true);
        Time.timeScale = .05f;

        foreach (char c in charsToStamp)
        {
            /*GetComponent<TextMeshPro>()*/playerText.text += c;

            yield return new WaitForSecondsRealtime(.04f);
            TextWindow.GetComponent<AudioSource>().clip = keyHitSounds[Random.Range(0, 2)];
            TextWindow.GetComponent<AudioSource>().Play();

        }
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = .5f;
        yield return new WaitForSecondsRealtime(.3f);
        Time.timeScale = 1;
        TextWindow.SetActive(false);
        //LevelManager.instance.storyOver = true;
        playerText.text = "";
    }
}
