using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SpaceInvaders_GameScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
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
            }
        }


        //eliminare dalla lista le risoluzioni non compatibili con il gioco
        //
        //creare un componente nel menu che permetta la scelta della risoluzione
        //applicare la risoluzione

    }
}
