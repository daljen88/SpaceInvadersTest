using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIButton : MonoBehaviour
{
    public UnityEvent onMouseDown;

    private void OnMouseEnter()
    {
        transform.localScale = Vector3.one *3.5f;
        //transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

    }
    private void OnMouseExit()
    {
        transform.localScale = Vector3.one * 3;
    }
    private void OnMouseDown()
    {
        transform.localScale = Vector3.one * 3f;
        onMouseDown.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
