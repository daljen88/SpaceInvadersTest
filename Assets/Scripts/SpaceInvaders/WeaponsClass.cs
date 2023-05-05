using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponsClass : MonoBehaviour, IDroppable
{

    protected bool isDropped = false;
    public bool IsDropped { get { return isDropped; } set { isDropped = value; } }

    protected bool isCollected = false;
    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }

    protected float dropSpeed = 2f;
    public abstract float DropSpeed { get; }

    protected float dropTimer;
    protected float coolDown;

    protected float dropLifeTime = 10;
    public abstract float DropLifeTime { get; /*set; */}

    protected float fireRate = 1f;
    public abstract float FireRate { get; }

    protected int damage=1;
    public abstract int Damage { get; }

    //serve set perche cambia coi colpi presi
    protected float defence = 1f;
    public abstract float Defence { get; set ; }

    protected Vector3 projMovementVector=Vector3.up;
    public abstract Vector3 ProjMovementVector { get; }

    public MainCharacter tPlayer;
    public AudioSource dropWeaponSound;
    public List<AudioClip> dropSounds;
    public GameObject gunShotTemplate;
    private Vector3 fallingVector;
    private WeaponProjectile myProjectile;
    private SpriteRenderer gunSpriteRenderer;
    protected Vector3 gunOffset;

    //{ get { return projMovementVector; } set { projMovementVector = value; } }

    public WeaponsClass()
    {
        dropTimer = DropLifeTime;
        coolDown = 0;
    }

    public void Start()
    {
        StartRoutine();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //METTO TUTTO IN UNA FUZNIONA DA DARE A WEAPONSCLASS
        tPlayer = other.GetComponent<MainCharacter>();
        WeaponsClass oldWeapon = tPlayer.gameObject.GetComponentInChildren<WeaponsClass>();
        if (tPlayer != null)
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
    protected virtual void StartRoutine()
    {
        gunSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

    }
    void Update()
    {
        UpdateRoutine();
    }

    protected virtual void UpdateRoutine()
    {
        if (dropTimer > 0)
            dropTimer -= Time.deltaTime;
        if (coolDown > 0)
            coolDown -= Time.deltaTime;

        //debug
        Debug.LogWarning(dropTimer);
        Debug.LogWarning(coolDown);


        if (IsDropped && IsCollected == false && transform.position.y > -4.1f)
            transform.position += fallingVector * Time.deltaTime;
        else if (IsCollected == true)
        {
            //cambia questa logica settande posizione arma in un empty object dentro player
            int bigGunRotation = tPlayer.goingRight ? -15 : 15;
            gunSpriteRenderer.flipX = !tPlayer.goingRight;

            //transform.position = transform.position - bigGunOffset;

            transform.position = tPlayer.gameObject.transform.position - gunOffset;
            transform.rotation = Quaternion.Euler(0, 0, bigGunRotation);

        }
        if (dropTimer <= 0 && !isCollected)
            Destroy(gameObject);
    }

    public virtual void Drop(Vector3 direction)
    {
        IsDropped= true;
        fallingVector = direction*DropSpeed;
        dropWeaponSound.clip = dropSounds[0/*Random.Range(0, dropSounds.Count)*/];
        dropWeaponSound.Play();

        //distrugge dopo 10 secondi
        
    }
    public virtual void ShootProjectile(/*Vector3 direction*/)
    {
        if (coolDown <= 0)
        {
            GameObject tempProjectile = Instantiate(gunShotTemplate, transform.position, Quaternion.Euler(0, 0, 90));
            myProjectile = tempProjectile.GetComponent<WeaponProjectile>();
            myProjectile.Shoot(ProjMovementVector, Damage);
            coolDown = FireRate;
        }
        else
        {
            return;
        }
    }


}