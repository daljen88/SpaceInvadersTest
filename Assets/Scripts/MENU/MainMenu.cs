using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MainMenu : MonoBehaviour
{
    static public MainMenu instance;
    public TextMeshProUGUI hiScoreText;
    public TextMeshProUGUI resolutionTextMessage;

    public string hi_score_key;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        int score = GameManager.Instance.LoadData(hi_score_key);
        hiScoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        Time.timeScale= 1.0f;
        SceneManager.LoadScene("SpaceInvaders_GameScene");
        GameManager.Instance.LevelCount = 1;
        //if (FindObjectOfType<MainCharacter>() == null) 
        //{
            
        //}
    }
    public void QuitGame()
    {

        Application.Quit();
    }
    //IEnumerator QuitCoroutine()
    //{
    //    //Time.timeScale = 0f;

    //}
    public void SetResolution()
    {
        //trovare risoluzioni compatibili schermo giocatore e aggiungerle alla lista
        Resolution[] resolutions = Screen.resolutions;

        //crea lista da array
        //List<Resolution> listaRisoluzioni = new List<Resolution>(resolutions);

        List<Resolution> listaRisoluzioni = new List<Resolution>();
        foreach (Resolution res in resolutions)
        { 
            Debug.Log($"Resolution: {res.width} x {res.height}");
            if (res.width/res.height==16/9)
            {
                listaRisoluzioni.Add(res);
                Debug.Log("Resolution compatible with the game");
                MoveResolutionMessage();
            }
        }


        //eliminare dalla lista le risoluzioni non compatibili con il gioco
        //
        //creare un componente nel menu che permetta la scelta della risoluzione
        //applicare la risoluzione

    }
    public IEnumerator resolutionMessageRoutineRunning;
    private Coroutine running;

    public void MoveResolutionMessage()
    {
        resolutionMessageRoutineRunning = resolutionMessageMove();
        if (running == null)
        {
            running = StartCoroutine(resolutionMessageRoutineRunning);
        }

        IEnumerator resolutionMessageMove()
        {
            resolutionTextMessage.enabled = true;
            float duration = 3.5f /*(resolutionTextMessage.transform.position.x - 880f) / 500f*/;
            resolutionTextMessage.rectTransform.DOMoveX(2600f, duration, false);
            yield return new WaitForSeconds(duration);
            resolutionTextMessage.enabled = false;
            resolutionTextMessage.rectTransform.DOMoveX(-900f, 0.01f, false);
            yield return new WaitForSeconds(0.01f);


            running = null;
        }
    }
}
