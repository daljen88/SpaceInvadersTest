using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyeOrbsCannon : WeaponsClass
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

    public override Vector3 ProjDirectionVector => projDirectionVector/*speedMultiplyer*/;

    public override float GunRotation => tPlayer.goingRight ? rotazR : rotazL;

    [Header("Weapon Position on Player")]
    [SerializeField] private float rotazR = 15f;
    [SerializeField] private float rotazL = -15f;
    [SerializeField] private Vector3 eyeOrbCannonOffsetR = new Vector3(-0.166f, -0.2f);
    [SerializeField] private Vector3 eyeOrbCannonOffsetL = new Vector3(0.166f, -0.2f);
    [Header("Shot Firing Offset")]
    [SerializeField] private float eyeOrbShotXOffsetR = 0.7f;//offset y=0.155
    [SerializeField] private float eyeOrbShotXOffsetL = -0.7f;
    /*[SerializeField]*/
    private float eyeOrbShotRotation/* = -90f*/;

    public GameObject blackHoleSwoshTemplate;
    protected BlackHole myBlackHole;


    //private SpriteRenderer gunSpriteRenderer;

    public EyeOrbsCannon() : base()
    {
        Defence = Defence + 2;
        gunOffsetR = eyeOrbCannonOffsetR;
        gunOffsetL = eyeOrbCannonOffsetL;
        projXOffsetR = eyeOrbShotXOffsetR;
        projXOffsetL = eyeOrbShotXOffsetL;
        projRotation = eyeOrbShotRotation;
        projDirectionVector = ProjDirectionVector * 1;
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
            for (int i = 1; i < 4; i++)
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
        projDirectionVector.x = tPlayer.goingRight ? Mathf.Cos(Mathf.Deg2Rad * (75 - 15 * _xCoordinate)) : -Mathf.Cos(Mathf.Deg2Rad * (75 - 15 * _xCoordinate));
        projDirectionVector.y = Mathf.Sin(Mathf.Deg2Rad * (75 - 15 * _xCoordinate));
        eyeOrbShotRotation = tPlayer.goingRight ? 75 - (15 * _xCoordinate):(105+ (15 * _xCoordinate));
        GameObject tempProjectile = Instantiate(gunShotTemplate, new Vector3(tPlayer.goingRight ? transform.position.x + eyeOrbShotXOffsetR : transform.position.x + eyeOrbShotXOffsetL, transform.position.y), Quaternion.Euler(0, 0, eyeOrbShotRotation));
        myProjectile = tempProjectile.GetComponent<WeaponProjectile>();
        myProjectile.Shoot(ProjDirectionVector, DamageMultiplyer);
    }

    public void SecondShootProjectile()
    {
        if (coolDown <= 0)
        {
            GameObject tempBlackHole = Instantiate(blackHoleSwoshTemplate, new Vector3(tPlayer.goingRight ? transform.position.x + eyeOrbShotXOffsetR : transform.position.x + eyeOrbShotXOffsetL, transform.position.y), Quaternion.Euler(0, 0, 0));
            myBlackHole = tempBlackHole.GetComponent<BlackHole>();
            myBlackHole.Shoot(Vector3.up, DamageMultiplyer);
            coolDown = FireRate;
        }
        else
        {
            return;
        }

    }

}
