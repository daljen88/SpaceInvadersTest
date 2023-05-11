using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryRoutine : MonoBehaviour
{
    private string storyText = "Aliens from another galaxy have invaded Earth space-time frame and stole some valuable technology from the 80s," +
        " now the continuum is breaking apart! You, as a member of the GUTSS (Galactic Union for Time-Space Stability), have to bring those devices back to the" +
        " earthlings and fix the stability of life as we know it!";
    private char[] storyTextByChar;
    public AudioClip[] keyHitSounds;
    //public TextMeshPro uiStoryText;


    // Start is called before the first frame update
    void Start()
    {
        storyTextByChar = storyText.ToCharArray();

        //int i = 0;
        //foreach (char c in introText)
        //{
        //    introTextByChar[i] = c;
        //    i++;
        //}
    }
    private void OnEnable()
    {
        StartCoroutine(WriteByLetterCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)||Input.GetKeyDown(KeyCode.J))
        {
            StopCoroutine("WriteByLetterCoroutine");
            LevelManager.instance.storyOver = true;
            LevelManager.instance.uiStoryText.text = "";
            LevelManager.instance.uiStoryText.gameObject.SetActive(false);
        }
    }
    IEnumerator WriteByLetterCoroutine()
    {
        yield return new WaitForSeconds(.2f);
        foreach (char c in storyTextByChar)
        {
            LevelManager.instance.uiStoryText.text += c;

            yield return new WaitForSeconds(.03f);
            GetComponent<AudioSource>().clip = keyHitSounds[Random.Range(0, 2)];
            GetComponent<AudioSource>().Play();

        }
        yield return new WaitForSeconds(5f);
        LevelManager.instance.storyOver = true;
        LevelManager.instance.uiStoryText.text = "";
        LevelManager.instance.uiStoryText.gameObject.SetActive(false);

    }
}
