using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScore : MonoBehaviour
{
    public string key;

    // Start is called before the first frame update
    void Start()
    {
        int score = GameManager.Instance.LoadData(key);
        GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
    }

}
