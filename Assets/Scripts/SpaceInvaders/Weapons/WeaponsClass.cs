using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class WeaponsClass : MonoBehaviour, IDroppable
{

    protected bool isDropped = false;
    public bool IsDropped { get { return isDropped; } set { isDropped = value; } }

    protected bool isCollected = false;
    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }

    [SerializeField] protected float baseDropSpeed = 2f;
    public abstract float DropSpeed { get; }

    protected float dropTimer;
    protected float coolDown;

    [SerializeField] protected float baseDropLifeTime = 10;
    public abstract float DropLifeTime { get; /*set; */}

    protected float baseFireRate = 1f;
    public abstract float FireRate { get; }

    protected int damageMultiplyer=1;
    public abstract int DamageMultiplyer { get; }

    //serve set perche cambia coi colpi presi
    protected float defence = 1f;
    public abstract float Defence { get; set ; }

    protected Vector3 projDirectionVector=Vector3.up;
    public abstract Vector3 ProjDirectionVector { get; }

    protected float gunRotation;
    public abstract float GunRotation { get;}

    protected MainCharacter tPlayer;
    public AudioSource dropWeaponSound;
    public List<AudioClip> dropSounds;
    public GameObject gunShotTemplate;
    private Vector3 fallingVector;
    protected WeaponProjectile myProjectile;
    protected SpriteRenderer gunSpriteRenderer;
    //[Header("Weapon Position on Player")]
    protected Vector3 gunOffsetR;
    protected Vector3 gunOffsetL;
    protected float projXOffsetR;
    protected float projXOffsetL;
    protected float projRotation;

    //public Material myMaterial, myHitTakenMaterial;
    public Color hitFxColor;
    public float hitFxDuration=.3f;
    private bool routine = false;
    private bool vanishRoutine = false;
    protected Tweener twScale;
    protected Tweener twColor;
    protected Coroutine runningRoutine;
    protected WeaponsClass oldWeapon;

    public virtual bool PlayerIsTriggerCollider => tPlayer != null;
    public virtual bool IsOlderGunWeakerCondition => oldWeapon.gunType <= gunType;/*{ get; }*/

    public static UnityEvent dropEvent;

    public enum GunType 
    {
        StandardGun, 
        BigGun, 
        ElectricGun, 
        EyeOrbCannon 
    }
    public GunType gunType;
    //{ get { return projMovementVector; } set { projMovementVector = value; } }

    public WeaponsClass()
    {
        //gunRotation=GunRotation;
        dropTimer = DropLifeTime;
        coolDown = 0;
        //costruisco qua con v
    }

    public void Start()
    {
        StartRoutine();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerLogic(other);
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
        if(Defence==1&&routine==false)
        {
            routine = true;
            StartCoroutine(HitColorCoroutine());
        }

        if (coolDown > 0)
            coolDown -= Time.deltaTime;

        if (dropTimer > 0)
            dropTimer -= Time.deltaTime;

        //debug
        Debug.LogWarning(dropTimer);
        Debug.LogWarning(coolDown);

        if (IsDropped && IsCollected == false && transform.position.y > -4.1f)
        {
            transform.position += fallingVector * Time.deltaTime;
        }
        else if (IsCollected == true)
        {
            CollectedUpdateLogic();

        }
        if(dropTimer<=DropLifeTime/5&&!isCollected&& vanishRoutine == false)
        {
            vanishRoutine = true;
            VanishWarn();
        }
        if (dropTimer <= 0 && !isCollected)
            Destroy(gameObject);
    }
    public virtual void CollectedUpdateLogic()
    {
        //gunRotation = GunRotation/*tPlayer.goingRight ? 15 : -15*/;
        gunSpriteRenderer.flipX = !tPlayer.goingRight;

        transform.position = tPlayer.goingRight ? tPlayer.gameObject.transform.position - gunOffsetR : tPlayer.gameObject.transform.position - gunOffsetL;

        transform.rotation = Quaternion.Euler(0, 0, GunRotation);
    }

    public virtual void OnTriggerLogic(Collider entering)
    {
        //METTO TUTTO IN UNA FUZNIONA DA DARE A WEAPONSCLASS
        tPlayer = entering.GetComponent<MainCharacter>();
        /*WeaponsClass*/ oldWeapon = tPlayer.gameObject.GetComponentInChildren<WeaponsClass>();
        if (PlayerIsTriggerCollider&& IsOlderGunWeakerCondition)
        {
            if (oldWeapon != null)
            { Destroy(oldWeapon.gameObject); }
            dropTimer = -1;
            IsCollected = true;
            if (runningRoutine != null)
            { StopCoroutine(runningRoutine); }
            if (twScale != null && twScale.IsActive())
            {
                twScale.Kill(false);
                //risistema a dim originale se tween spento a metà
                gameObject.transform.DOScale(Vector3.one, DropLifeTime / 60);
                //transform.localScale = Vector3.one; //new vector3 (1,1,1);
            }

            if(twColor!=null && twColor.IsActive())
            {
                twColor.Kill(false);
                gameObject.GetComponentInChildren<SpriteRenderer>().DOColor(Color.white,0f);

            }

            tPlayer.activeGunPrefab = gameObject;
            tPlayer.gunPossesed = gameObject.GetComponent<WeaponsClass>();
            GameManager.Instance.SetGameManagerGunPossessed(gameObject);
            gameObject.transform.parent = tPlayer.gameObject.transform;
            //tPlayer.gunPossesed=this;
        }
    }

    //public void DropEvent()
    //{
    //    IsDropped = true;

    //}
    public virtual void Drop(Vector3 direction)
    {
        FindObjectOfType<PlayerTextLogic>()?.FoundNewGun();
        //tPlayer.gameObject.GetComponentInChildren<PlayerTextLogic>().FoundNewGun();
        //dropEvent.Invoke();
        IsDropped = true;
        fallingVector = direction*DropSpeed;
        dropWeaponSound.clip = dropSounds[0/*Random.Range(0, dropSounds.Count)*/];
        dropWeaponSound.Play();
        //tPlayer.gameObject.GetComponentInChildren<PlayerTextLogic>().FoundNewGun();

        //distrugge dopo 10 secondi
        
    }
    public virtual void ShootProjectile(/*Vector3 direction*/)
    {
        if (coolDown <= 0)
        {
            //float shootOffset=
            GameObject tempProjectile = Instantiate(gunShotTemplate, new Vector3 (tPlayer.goingRight? transform.position.x+0.7f: transform.position.x - 0.7f, transform.position.y), Quaternion.Euler(0, 0, 90));
            myProjectile = tempProjectile.GetComponent<WeaponProjectile>();
            myProjectile.Shoot(ProjDirectionVector, DamageMultiplyer);
            coolDown = FireRate;
        }
        else
        {
            return;
        }
    }

    public void VanishWarn()
    {
        runningRoutine=StartCoroutine(VanishWarningCoroutine());
    }

    public void WeaponDefenceGFX()
    {
        //    GetComponent<MeshRenderer>().material = myHitTakenMaterial;
        //    //la funzione va chiamata come stringa
        //    Invoke("SetNormalMaterial", 0.25f);
        StartCoroutine(HitColorCoroutine());
    }
    IEnumerator HitColorCoroutine()
    {
        //routine = true;
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(hitFxColor, hitFxDuration* 2/ 3);
        yield return new WaitForSeconds(hitFxDuration*2 / 3);
        tsprite.DOColor(Color.white, hitFxDuration / 3);
        yield return new WaitForSeconds(hitFxDuration/ 3);
        routine = false;
    }
    public IEnumerator VanishWarningCoroutine()
    {
        while (!IsCollected)
        {
            SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
            twColor=tsprite.DOColor(hitFxColor, DropLifeTime / 30);
            yield return new WaitForSeconds(DropLifeTime / 30);
            tsprite.DOColor(Color.white, DropLifeTime / 30);
            yield return new WaitForSeconds(DropLifeTime / 30);
            twColor=tsprite.DOColor(hitFxColor, DropLifeTime / 30);
            yield return new WaitForSeconds(DropLifeTime / 30);
            tsprite.DOColor(Color.white, DropLifeTime / 30);
            yield return new WaitForSeconds(DropLifeTime / 30);
            twScale= gameObject.transform.DOScale(Vector3.zero, DropLifeTime / 15);
            yield return new WaitForSeconds(DropLifeTime / 15);
            vanishRoutine = false;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    //public void SetNormalMaterial()
    //{
    //    GetComponent<MeshRenderer>().material = myMaterial;
    //}

}
