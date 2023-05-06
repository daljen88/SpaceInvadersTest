using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTextLogic : MonoBehaviour
{
    public string[] foundNewGunPhrases = { "Geez, that's a gun!" };
    public char[] textStringToChar;
    public TextMeshPro playerText;
    public AudioClip[] keyHitSounds;

    // Start is called before the first frame update
    void Start()
    {
        playerText.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FoundNewGun()
    {
        textStringToChar= foundNewGunPhrases[0].ToCharArray();
        StartCoroutine(FoundNewGunCoroutine());
    }

    IEnumerator FoundNewGunCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        foreach (char c in textStringToChar)
        {
            /*GetComponent<TextMeshPro>()*/playerText.text += c;

            yield return new WaitForSeconds(.02f);
            GetComponent<AudioSource>().clip = keyHitSounds[Random.Range(0, 2)];
            GetComponent<AudioSource>().Play();

        }
        yield return new WaitForSeconds(1.5f);
        //LevelManager.instance.storyOver = true;
        playerText.text = "";
        //playerText.text = "";
        //playerText.text.gameObject.SetActive(false);
    }
}
