using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UIButton : MonoBehaviour
{
    public UnityEvent onMouseDown;

    private void OnMouseEnter()
    {
        //transform.localScale = Vector3.one *3.7f;
        //transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        //DOTWEEN LIBRERIE: SCALA e tempo di transizione
        transform.DOScale(Vector3.one * 2.7f, .15f).SetEase(Ease.InBounce);

    }
    private void OnMouseExit()
    {
        //transform.localScale = Vector3.one * 3.5f;

        //DOTWEEN: animazione uscita è più velcoe di quella di entrata
        transform.DOScale(Vector3.one * 2.5f, .05f);

    }
    private void OnMouseDown()
    {
        transform.localScale = Vector3.one * 2f;
        onMouseDown.Invoke();
    }
    private void OnMouseUp()
    {
        transform.DOScale(Vector3.one * 2.7f, .05f);

        //transform.localScale = Vector3.one * 3.5f;
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
