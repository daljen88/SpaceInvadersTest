using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public Sprite AccessSprite;
    public Sprite InformationSprite;
    public Sprite NormalSprite;

    public bool isAccessNode = false;
    public bool isInformationNode = false;

    public GridElement(bool _isInfo, bool _isAccess)
    {
            isInformationNode = _isInfo;
            isAccessNode = _isAccess;
    }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = NormalSprite;

        int infoRoll = Random.Range(0, 19);
        int accessRoll = Random.Range(0, 9);

        if (infoRoll == 0)
        {
            isInformationNode = true;
            GetComponent<SpriteRenderer>().sprite = AccessSprite;
        }
        if (accessRoll == 0)
        {
            isAccessNode = true;
            GetComponent<SpriteRenderer>().sprite = InformationSprite;
        }
    }
}
