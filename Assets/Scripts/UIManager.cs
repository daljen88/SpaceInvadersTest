using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Destroy(hpSpritesList[hpSpritesList.Count - 1].gameObject);
        hpSpritesList.RemoveAt(hpSpritesList.Count - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
