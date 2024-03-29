using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioDrop : DropsClass
{
    public override float DropSpeed => dropSpeed+1;

    public override float DropLifeTime => dropLifeTime+10;

    public RadioDrop():base()
    {
    }

    protected override void UpdateRoutine()
    {
        base.UpdateRoutine();
    }
    protected override void CollectionLogic()
    {
        base.CollectionLogic(); 
        GameManager.Instance.MusicRadioCollected = true;
        FindObjectOfType<PlayerTextLogic>().FoundFirstRadio();
    }
}
