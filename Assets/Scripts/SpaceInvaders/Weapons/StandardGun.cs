using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : WeaponsClass
{
    public override float DropLifeTime => baseDropLifeTime + 1;
    public override float DropSpeed => baseDropSpeed + 2;
    [SerializeField] private float thisFireRate = .2f;
    public override float FireRate => baseFireRate * thisFireRate;
    public override int DamageMultiplyer => damageMultiplyer * 1;
    public override Vector3 ProjDirectionVector => projDirectionVector * 1/*speedMultiplyer*/;
    public override float GunRotation => tPlayer.goingRight ? rotazR : rotazL;

    public override float Defence { get { return defence; } set { defence = value; } }
    //public override float Defence => defence+1;

    //public GameObject bigGunShotTemplate;
    [Header("Weapon Position on Player")]
    [SerializeField] private float rotazR = 15f;
    [SerializeField] private float rotazL = -15f;
    [SerializeField] private Vector3 standardGunOffsetR = new Vector3(-0.166f, 0.11f);//offset y=0.155
    [SerializeField] private Vector3 standardGunOffsetL = new Vector3(0.166f, 0.11f);

    //private SpriteRenderer gunSpriteRenderer;

    public StandardGun() : base()
    {
        IsCollected = true;
        Defence = 0;
        gunOffsetR = standardGunOffsetR;
        gunOffsetL = standardGunOffsetL;

        //myProjectile = gunShotTemplate.GetComponent<WeaponProjectile>();
    }
    protected override void StartRoutine()
    {
        base.StartRoutine();
        //gunSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        GameManager.Instance.SetGameManagerGunPossessed(gameObject);
        tPlayer = FindObjectOfType<MainCharacter>();
        gameObject.transform.parent = tPlayer.gameObject.transform;
        // gunSpriteRenderer =  gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    protected override void UpdateRoutine()
    {
        base.UpdateRoutine();

        //if (coolDown > 0)
        //    coolDown -= Time.deltaTime;
        //gunSpriteRenderer.flipX = !tPlayer.goingRight;
        //transform.position = tPlayer.goingRight ? tPlayer.gameObject.transform.position - gunOffsetR : tPlayer.gameObject.transform.position - gunOffsetL;
        //transform.rotation = Quaternion.Euler(0, 0, GunRotation);
    }
    public override void ShootProjectile(/*Vector3 direction*/)
    {
        if (coolDown <= 0)
        {
            GameObject tempProjectile = Instantiate(gunShotTemplate, new Vector3(tPlayer.goingRight ? transform.position.x + 0.4f : transform.position.x - 0.4f, transform.position.y), Quaternion.Euler(0, 0, 0));
            myProjectile = tempProjectile.GetComponent<WeaponProjectile>();
            myProjectile.Shoot(ProjDirectionVector, DamageMultiplyer);
            coolDown = FireRate;
        }
        else
        {
            return;
        }
    }
}
