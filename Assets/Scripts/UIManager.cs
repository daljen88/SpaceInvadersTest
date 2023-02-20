using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIManager : MonoBehaviour
{

    static public UIManager instance;
    public SpriteRenderer hpTemplate;
    public List<SpriteRenderer> hpSpritesList;
    public MainCharacter character;

    public float hpOffset=1.4f;
    private void Awake()
    {
        instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }
    public void InitUI()
    {
        //instanzia oggetto, in posizione, rotazione e parent oggetto spawnato
        for (int i =0; i< character.hp; i++)
        {
            SpriteRenderer tsprite = Instantiate(hpTemplate, hpTemplate.transform.position + Vector3.right * hpOffset * i, hpTemplate.transform.rotation, hpTemplate.transform.parent);
            hpSpritesList.Add(tsprite); 
        }
    }
    public void OnPlayerHitSuffered()
    {
        //Destroy(hpSpritesList[hpSpritesList.Count - 1].gameObject);
        //hpSpritesList.RemoveAt(hpSpritesList.Count - 1);
        StartCoroutine(HitsufferesCoroutine());

        //rimuoviamo dalal lista della vite hpSpriteList
        //Doshake sulla sprite
        //cambiamo colore sprite a rosso
        //do scale a 0
        
    }

    public IEnumerator HitsufferesCoroutine()
    {
        //esegue codice
        //salvo lo sprite da distruggere
        SpriteRenderer tsprite = hpSpritesList[hpSpritesList.Count - 1];
        //rimuovo dalla lista degli hp correnti
        hpSpritesList.RemoveAt(hpSpritesList.Count - 1);
        tsprite.transform.DOShakePosition(.25f);
        tsprite.DOColor(Color.red, .25f);

        //poi aspetta tot secondi e poi fa le righe successive
        yield return new WaitForSeconds(.25f);

        tsprite.transform.DOScale(Vector3.zero, .25f);
        yield return new WaitForSeconds(.25f);
        Destroy(tsprite.gameObject);


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
