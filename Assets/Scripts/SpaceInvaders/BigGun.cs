using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigGun : WeaponsClass
{
    //quando istanzi BigGun accedi direttam alle funzioni virtual o normali definite in classe padre
    //(anche se la classe è abstract, semplicemnete non potrai istanziare una classe padre)
    //invece se le funzioni sono definite abstract devi ridefinirle qui nella classe figlia.
    //lo stesso vale per le variabili: se sono definite nella classe padre quando istanzi la figli si creano

    public override float DropLifeTime => dropLifeTime+1;
    public override float DropSpeed => dropSpeed+1;
    public override float FireRate => fireRate*.5f;
    public override int Damage => damage*2;
    public override Vector3 ProjMovementVector => projMovementVector*5;

    public override float Defence { get { return defence; } set{ defence = value; } }
    //public override float Defence => defence+1;

    //public GameObject bigGunShotTemplate;
    [SerializeField] private Vector3 bigGunOffset = new Vector3(0.275f , 0.215f);
    //private SpriteRenderer gunSpriteRenderer;
    
    public BigGun(): base()
    {
        Defence=Defence+1;
        gunOffset = bigGunOffset;

        //myProjectile = gunShotTemplate.GetComponent<WeaponProjectile>();
    }
    protected override void StartRoutine()
    {
        base.StartRoutine();
       // gunSpriteRenderer =  gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    
    protected override void UpdateRoutine()
    {
        base.UpdateRoutine();
    }
    //void Update()
    //{
    //    //METTO TUTTO IN UNA FUNZIONA DA DARE ALLA PARENT WEAPONSCLASS
    //    base.Update();

    //}
}
