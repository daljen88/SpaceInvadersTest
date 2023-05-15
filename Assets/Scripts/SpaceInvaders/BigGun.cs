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

    [SerializeField] private float thisBonusDropLifeTime = 2f;
    public override float DropLifeTime => baseDropLifeTime+ thisBonusDropLifeTime;

    public override float DropSpeed => baseDropSpeed+2;

    [SerializeField] private float thisFireRate = .33f;
    public override float FireRate => baseFireRate*thisFireRate;

    [SerializeField] private int thisDamageMultiplyer = 1;
    public override int DamageMultiplyer => damageMultiplyer* thisDamageMultiplyer;

    public override float Defence { get { return defence; } set{ defence = value; } }
    //public override float Defence => defence+1;

    public override Vector3 ProjDirectionVector => projDirectionVector *1/*speedMultiplyer*/;

    public override float GunRotation => tPlayer.goingRight ? rotazR : rotazL;

    [Header("Weapon Position on Player")]
    [SerializeField] private float rotazR = 15f;
    [SerializeField] private float rotazL = -15f;
    [SerializeField] private Vector3 bigGunOffsetR = new Vector3(-0.166f , 0.11f);//offset y=0.155
    [SerializeField] private Vector3 bigGunOffsetL = new Vector3(0.166f, 0.11f);

    //[SerializeField] private int bigGunRotation= tPlayer.goingRight ? 15 : -15;
    //private SpriteRenderer gunSpriteRenderer;

    public BigGun(): base()
    {
        Defence=Defence+1;
        gunOffsetR = bigGunOffsetR;
        gunOffsetL = bigGunOffsetL;
        //GunRotation = bigGunRotation;
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
    public override void OnTriggerLogic(Collider entering)
    {
        tPlayer = entering.GetComponent<MainCharacter>();
        WeaponsClass oldWeapon = tPlayer.gameObject.GetComponentInChildren<WeaponsClass>();
        if (tPlayer != null && oldWeapon.GetComponent<ElectricTriGun>() == null)
        {
            if (oldWeapon != null)
            { Destroy(oldWeapon.gameObject); }
            dropTimer = -1;
            IsCollected = true;
            tPlayer.activeGunPrefab = gameObject;
            tPlayer.gunPossesed = gameObject.GetComponent<WeaponsClass>();
            GameManager.Instance.SetGameManagerGunPossessed(gameObject);
            gameObject.transform.parent = tPlayer.gameObject.transform;
            //tPlayer.gunPossesed=this;
        }
    }
    //void Update()
    //{
    //    base.Update();
    //}
}
