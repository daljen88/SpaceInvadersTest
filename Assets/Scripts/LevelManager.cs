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
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntroCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
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

        yield return new WaitForSeconds(.5f);


        character.enabled= true;
        spawner.enabled= true;
        uiManager.enabled= true;

        yield return new WaitForSeconds(.5f);
        //scompare Fight
        introText.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InElastic);
        //character.gameObject.SetActive(true); così accende tutto object e non solo script

    }
}
