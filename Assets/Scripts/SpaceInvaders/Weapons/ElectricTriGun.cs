using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTriGun : WeaponsClass
{
    [SerializeField] private float thisBonusDropLifeTime = 2f;
    public override float DropLifeTime => baseDropLifeTime + thisBonusDropLifeTime;

    public override float DropSpeed => baseDropSpeed + 2;

    [SerializeField] private float thisFireRate = .66f;
    public override float FireRate => baseFireRate * thisFireRate;

    [SerializeField] private int thisDamageMultiplyer = 1;
    public override int DamageMultiplyer => damageMultiplyer * thisDamageMultiplyer;

    public override float Defence { get { return defence; } set { defence = value; } }
    //public override float Defence => defence+1;

    public override Vector3 ProjDirectionVector => projDirectionVector * 1/*speedMultiplyer*/;

    public override float GunRotation => tPlayer.goingRight ? rotazR : rotazL;

    [Header("Weapon Position on Player")]
    [SerializeField] private float rotazR = 15f;
    [SerializeField] private float rotazL = -15f;
    [SerializeField] private Vector3 bigGunOffsetR = new Vector3(-0.166f, -0.2f);
    [SerializeField] private Vector3 bigGunOffsetL = new Vector3(0.166f, -0.2f);
    [SerializeField] private float electricShotXOffsetR = 0.7f;//offset y=0.155
    [SerializeField] private float electricShotXOffsetL = -0.7f;
    /*[SerializeField]*/ private float electricShotRotation/* = -90f*/;

    //private SpriteRenderer gunSpriteRenderer;

    public ElectricTriGun() : base()
    {
        Defence = Defence + 2;
        gunOffsetR = bigGunOffsetR;
        gunOffsetL = bigGunOffsetL;
        projXOffsetR = electricShotXOffsetR;
        projXOffsetL = electricShotXOffsetL;
        projRotation=electricShotRotation;
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
        base.OnTriggerLogic(entering);
    }

    public override void ShootProjectile(/*Vector3 direction*/)
    {
        if (coolDown <= 0)
        {
            for (int i = -1; i < 2; i++)
            {
                ProjectileInstatiation(i);
            }
            coolDown = FireRate;
        }
        else
        {
            return;
        }
    }
    public void ProjectileInstatiation(int _xCoordinate)
    {
        projDirectionVector.x =_xCoordinate;
        electricShotRotation = -90 - ( 45 * projDirectionVector.x);
        GameObject tempProjectile = Instantiate(gunShotTemplate, new Vector3(tPlayer.goingRight ? transform.position.x + electricShotXOffsetR : transform.position.x + electricShotXOffsetL, transform.position.y), Quaternion.Euler(0, 0, electricShotRotation));
        myProjectile = tempProjectile.GetComponent<WeaponProjectile>();
        myProjectile.Shoot(ProjDirectionVector, DamageMultiplyer);
    }
  
}

