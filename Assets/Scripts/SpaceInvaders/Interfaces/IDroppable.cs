using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDroppable
{

    float DropLifeTime { get;/* set; */}
    float DropSpeed { get; }

    bool IsDropped { get; set; }
    bool IsCollected { get; set; }
    //if collider of trigger is IDroppable set to playerInventory
    void Drop (Vector3 direction);
    //{
    //    //shooted = true;
    //    //shootDirection = direction;
    //    //audioEnemyShot.clip = audioClips[Random.Range(0, audioClips.Count)];
    //    //audioEnemyShot.Play();

    //    //distrugge dopo 10 secondi
    //    //Destroy(gameObject, 10);
    //}


    // S
    // tart is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
