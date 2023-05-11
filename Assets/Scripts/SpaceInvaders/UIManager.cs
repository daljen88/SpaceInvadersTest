using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{

    static public UIManager instance;
    public TextMeshPro levelText;
    public SpriteRenderer hpTemplate;
    public List<SpriteRenderer> hpSpritesList;
    public MainCharacter character;
    public TextMeshPro scoreText;
    public int scorePoints;
    public int lives;
    public int totalEnemiesKilled;
    public int normalEnemyKilled;
    public int bonusEnemyKilled;
    public int bringerEnemyKilled;
    public GameObject playerUI;
    public GameObject pauseGO;

    public float hpOffset=1.1f;

    public void Show(bool show)
    {
        playerUI.SetActive(show);
    }
    private void OnEnable()
    {
        //InitUI();

    }
    private void Awake()
    {
            instance = this;
    }

    void Start()
    {
        scorePoints = GameManager.Instance? GameManager.Instance.currentScore:0;
        scoreText.text = scorePoints.ToString();
        //InitUI();
    }
    public void InitUI()
    {
        levelText.text = $"LEVEL {GameManager.Instance.levelCount.ToString()}";
        //scorePoints = 0;
        //instanzia oggetto, in posizione, rotazione e parent oggetto spawnato
        for (int i =0; i< character.Hp; i++)
        {
            SpriteRenderer tsprite = Instantiate(hpTemplate, gameObject.transform.GetChild(0).position + Vector3.right * hpOffset * (i+1), gameObject.transform.GetChild(0).rotation, gameObject.transform.GetChild(0));
            tsprite.gameObject.SetActive(true);
            hpSpritesList.Add(tsprite); 
        }

    }
    public void SetUI()
    {
        levelText.text = $"LEVEL {GameManager.Instance.levelCount.ToString()}";
        if (hpSpritesList.Count < character.Hp)
        {
            int hpRemained = hpSpritesList.Count;
            for (int i = hpSpritesList.Count; i < character.Hp; i++)
            {
                SpriteRenderer tsprite = Instantiate(hpTemplate, gameObject.transform.GetChild(0).position + Vector3.right * hpOffset * (i + 1), gameObject.transform.GetChild(0).rotation, gameObject.transform.GetChild(0));
                tsprite.gameObject.SetActive(true);
                hpSpritesList.Add(tsprite);
            }
        }
    }
    public void OnPlayerHitUpdateLives(int damage)
    {
        StartCoroutine(UpdatelivesOnHitCoroutine(damage));

    }
    public void PointsScoredEnemyKilled(int score=666, string enemy="enemy")
    {
        StartCoroutine(ScoredPointsCoroutine(score, enemy));
    }

    public IEnumerator UpdatelivesOnHitCoroutine(int _damage)
    {
        for (int i = 0; i < _damage; i++)
        {
            //TOGLIE CUORE DA NUMERO VITE
            //salvo lo sprite da distruggere
            SpriteRenderer tsprite = hpSpritesList[hpSpritesList.Count - 1];
            //rimuovo dalla lista degli hp correnti
            hpSpritesList.RemoveAt(hpSpritesList.Count - 1);
            tsprite.transform.DOShakePosition(.25f);
            tsprite.DOColor(Color.red, .25f);
            yield return new WaitForSeconds(.25f);

            tsprite.transform.DOScale(Vector3.zero, .25f);
            yield return new WaitForSeconds(.25f);

            Destroy(tsprite.gameObject);
        }

    }
    public IEnumerator ScoredPointsCoroutine(int score, string enemy)
    {
        //scorePoints += 666;
        //scoreText.text = scorePoints.ToString();
        totalEnemiesKilled++;
        switch (enemy)
        {
            case "enemy":
                normalEnemyKilled++;
                break;
            case "bomusEnemy":
                bonusEnemyKilled++;
                break;
            case "bringerEnemy":
                bringerEnemyKilled++;
                break;
        }
        scorePoints += score;
        scoreText.transform.DOPunchScale(Vector3.one * .5f, .333f);
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

    //public void AddScore(int score)
    //{
    //    scorePoints += score;
    //    UpdateScoreText(scorePoints);
    //}
    //public void UpdateScoreText(int scorePoints)
    //{
    //    scoreText.transform.DOPunchScale(Vector3.one*.5f,.333f);
    //    scoreText.text = scorePoints.ToString();
    //}
}
