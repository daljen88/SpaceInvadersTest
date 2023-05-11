using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BigGun : WeaponsClass
{
    //quando istanzi BigGun accedi direttam alle funzioni virtual o normali definite in classe padre
    //(anche se la classe è abstract, semplicemnete non potrai istanziare una classe padre)
    //invece se le funzioni sono definite abstract devi ridefinirle qui nella classe figlia.
    //lo stesso vale per le variabili: se sono definite nella classe padre quando istanzi la figli si creano

    public override float DropLifeTime => dropLifeTime+1;
    public override float DropSpeed => dropSpeed+2;
    public override float FireRate => fireRate*.5f;
    public override int DamageMultiplyer => damageMultiplyer*1;
    public override Vector3 ProjDirectionVector => projDirectionVector *1/*speedMultiplyer*/;
    public override float GunRotation => tPlayer.goingRight ? rotazR : rotazS;

    public override float Defence { get { return defence; } set{ defence = value; } }
    //public override float Defence => defence+1;



    //public GameObject bigGunShotTemplate;
    [SerializeField] private float rotazR = 15f;
    [SerializeField] private float rotazS = -15f;
    [SerializeField] private Vector3 bigGunOffsetR = new Vector3(-0.166f , 0.11f);//offset y=0.155
    [SerializeField] private Vector3 bigGunOffsetS = new Vector3(0.166f, 0.11f);
    //[SerializeField] private int bigGunRotation= tPlayer.goingRight ? 15 : -15;

    //private SpriteRenderer gunSpriteRenderer;

    public BigGun(): base()
    {
        Defence=Defence+1;
        //GunRotation = bigGunRotation;
        gunOffsetR = bigGunOffsetR;
        gunOffsetS = bigGunOffsetS;
        //costruisco qua tipo
        //
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
