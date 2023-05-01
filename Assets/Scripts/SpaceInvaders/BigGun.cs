using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Numerics;

public class BigGun : WeaponsClass
{
    //quando istanzi BigGun accedi direttam alle funzioni virtual o normali definite in classe padre
    //(anche se la classe è abstract, semplicemnete non potrai istanziare una classe padre)
    //invece se le funzioni sono definite abstract devi ridefinirle qui nella classe figlia.
    //lo stesso vale per le variabili: se sono definite nella classe padre quando isanzi la figli si creano

    //public override bool IsDropped => isDropped;   
    
    public override float DropLifeTime => dropLifeTime+5;
    public override float DropSpeed => dropSpeed+1;
    public override float FireRate => fireRate*1;
    public override Vector3 ProjMovementVector => projMovementVector*5;

    public override int Damage => damage*2;

    public override float Defence { get { return defence; } set{ defence = value; } }
    //public override float Defence => 2;
    //public GameObject bigGunShotTemplate;
    public Vector3 bigGunOffset = new Vector3(0.275f , 0.215f);
    public SpriteRenderer gunSpriteRenderer;
    
    public BigGun(): base()
    {
        Defence=Defence+1;
        //myProjectile = gunShotTemplate.GetComponent<WeaponProjectile>();
    }
    void Start()
    {
        gunSpriteRenderer =  gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //METTO TUTTO IN UNA FUZNIONA DA DARE A WEAPONSCLASS
        /*MainCharacter */tPlayer = other.GetComponent<MainCharacter>();
        if (tPlayer != null)
        {
            //HO COLPITO
            //tPlayer.OnHitSuffered(1);
            dropTimer = -1;
            IsCollected = true;
            tPlayer.activeGunPrefab = gameObject;
            tPlayer.gunPossesed = gameObject.GetComponent<WeaponsClass>();
            //tPlayer.gunPossesed=this;
            //transform.position=tPlayer.gameObject.transform.position;
            //Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //METTO TUTTO IN UNA FUNZIONA DA DARE ALLA APRENT WEAPONSCLASS
        if(dropTimer>=0)
        dropTimer += Time.deltaTime;

        if (IsDropped&&IsCollected==false && transform.position.y > -4.3f)
            transform.position += fallDirection * Time.deltaTime;
        else if(IsCollected==true)
        {
            //setta posizione arma in un empty object dentro player
            //bigGunOffset=tPlayer.goingRight? bigGunOffset: new Vector3(0.275f, 0.215f);
            int bigGunRotation = tPlayer.goingRight ? -15 : 15;
            //transform.localScale =
            gunSpriteRenderer.flipX = !tPlayer.goingRight/*?false:true*/;

            transform.position = tPlayer.gameObject.transform.position-bigGunOffset;
            transform.rotation = Quaternion.Euler(0, 0, bigGunRotation);

        }
        if (dropTimer==DropLifeTime)
            Destroy(gameObject);

    }
}
