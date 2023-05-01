using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponsClass : MonoBehaviour, IDroppable
{
    public MainCharacter tPlayer;

    public bool isDropped = false;
    public /*abstract*/ bool IsDropped { get { return isDropped; } set { isDropped = value; } }
    public bool isCollected = false;
    //public bool IsCollected { get; set; }
    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }


    public float dropSpeed = 2f;
    public abstract float DropSpeed { get; }

    public float dropTimer = 0;
    public float dropLifeTime = 10;
    public abstract float DropLifeTime { get; /*set; */}

    //public float dropDuration = 10f;
    //public abstract float DropDuration { get; }

    public float fireRate = .5f;
    public abstract float FireRate { get; }

    public int damage=1;
    public abstract int Damage { get; }

    //public float defence;
    //serve set perche cambia coi colpi presi
    protected float defence = 1f;
    public abstract float Defence { get; set ; }

    public AudioSource dropWeaponSound;
    public List<AudioClip> dropSounds;
    public Vector3 fallDirection;
    public WeaponProjectile myProjectile;
    public GameObject gunShotTemplate;

    public Vector3 projMovementVector=Vector3.up;
    public abstract Vector3 ProjMovementVector { get; }
        /*{ get { return projMovementVector; } set { projMovementVector = value; } }*/

    public WeaponsClass()
    {

    }

    public void Drop(Vector3 direction)
    {
        IsDropped= true;
        fallDirection = direction;
        dropWeaponSound.clip = dropSounds[0/*Random.Range(0, dropSounds.Count)*/];
        dropWeaponSound.Play();

        //distrugge dopo 10 secondi
        //Destroy(gameObject, DropLifeTime);
    }
    public void ShootProjectile(/*Vector3 direction*/)
    {
        GameObject tempProjectile = Instantiate(gunShotTemplate, transform.position, Quaternion.Euler(0,0,90));
        myProjectile = tempProjectile.GetComponent<WeaponProjectile>();
        myProjectile.Shoot(ProjMovementVector, Damage);
    }


}
