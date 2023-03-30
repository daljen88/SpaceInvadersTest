using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDamager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlatformPlayer tPlayer = collision.GetComponent<PlatformPlayer>();
        if(tPlayer)
        {
            tPlayer.OnHitSuffered();
        }
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
