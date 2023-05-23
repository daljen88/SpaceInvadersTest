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

    void VanishWarn();

}
