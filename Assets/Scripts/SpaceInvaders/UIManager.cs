using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{

    static public UIManager instance;
    public SpriteRenderer hpTemplate;
    public List<SpriteRenderer> hpSpritesList;
    public MainCharacter character;
    public TextMeshPro scoreText;
    public int scorePoints;
    public GameObject playerUI;
    public GameObject pauseGO;

    public float hpOffset=1.2f;

    public void Show(bool show)
    {
        playerUI.SetActive(show);
    }

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
        //scorePoints = 0;
        //instanzia oggetto, in posizione, rotazione e parent oggetto spawnato
        for (int i =0; i< character.hp; i++)
        {
            SpriteRenderer tsprite = Instantiate(hpTemplate, gameObject.transform.GetChild(0).position + Vector3.right * hpOffset * (i+1), gameObject.transform.GetChild(0).rotation, gameObject.transform.GetChild(0));
            tsprite.gameObject.SetActive(true);
            hpSpritesList.Add(tsprite); 
        }
    }
    public void OnPlayerHitSuffered()
    {
        //Destroy(hpSpritesList[hpSpritesList.Count - 1].gameObject);
        //hpSpritesList.RemoveAt(hpSpritesList.Count - 1);
        StartCoroutine(HitsufferedCoroutine());

        //rimuoviamo dalal lista della vite hpSpriteList
        //Doshake sulla sprite
        //cambiamo colore sprite a rosso
        //do scale a 0
        
    }
    public void PointsScoredEnemyKilled()
    {
        StartCoroutine(pointsscoredcoroutine());
    }

    public IEnumerator HitsufferedCoroutine()
    {
        //esegue codice
        //salvo lo sprite da distruggere
        SpriteRenderer tsprite = hpSpritesList[hpSpritesList.Count-1];
        //rimuovo dalla lista degli hp correnti
        hpSpritesList.RemoveAt(hpSpritesList.Count-1);
        tsprite.transform.DOShakePosition(.25f);
        tsprite.DOColor(Color.red, .25f);

        //poi aspetta tot secondi e poi fa le righe successive
        yield return new WaitForSeconds(.25f);

        tsprite.transform.DOScale(Vector3.zero, .25f);
        yield return new WaitForSeconds(.25f);
        Destroy(tsprite.gameObject);


    }
    public IEnumerator pointsscoredcoroutine()
    {
        scorePoints += 666;
        scoreText.text = scorePoints.ToString();
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(LevelManager.instance.TogglePause())
            {
                pauseGO.SetActive(true);

            }
            else
            {
                pauseGO.SetActive(false);
            }
        }

    }
    public void SetScore(int scorePoints)
    {
        scoreText.text = scorePoints.ToString();
        scoreText.transform.DOPunchScale(Vector3.one*.5f,.333f);
    }
}
