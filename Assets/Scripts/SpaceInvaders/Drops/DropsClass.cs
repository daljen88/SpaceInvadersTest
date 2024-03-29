using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DropsClass : MonoBehaviour, IDroppable
{
    protected bool isDropped = false;
    public bool IsDropped { get { return isDropped; } set { isDropped = value; } }

    protected bool isCollected = false;
    public bool IsCollected { get { return isCollected; } set { isCollected = value; } }

    protected float dropSpeed = 2f;
    public abstract float DropSpeed { get; }

    [SerializeField] protected float dropTimer;
    //protected float coolDown;

    protected float dropLifeTime = 10;
    public abstract float DropLifeTime { get; /*set; */}

    protected Vector3 fallingVector;
    private MainCharacter tPlayer;
    public Color hitFxColor;
    private bool vanishRoutine = false;

    Tween shake;

    public DropsClass()
    {
        dropTimer = DropLifeTime;
    }

    public virtual void Drop(Vector3 direction)
    {
        IsDropped = true;
        fallingVector = direction*DropSpeed;

        GameManager.Instance.PlayRadioDropSound();

        //GetComponentInChildren<AudioSource>().Play();

        //dropWeaponSound.clip = dropSounds[0/*Random.Range(0, dropSounds.Count)*/];
        //dropWeaponSound.Play();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoutine();
    }

    protected virtual void UpdateRoutine()
    {
        if (dropTimer > 0)
            dropTimer -= Time.deltaTime;
        //if (coolDown > 0)
        //    coolDown -= Time.deltaTime;

        //debug
        //Debug.LogWarning(dropTimer);
        //Debug.LogWarning(coolDown);


        if (IsDropped && !IsCollected && transform.position.y > -4.1f)
            transform.position += fallingVector * Time.deltaTime;
        else if (transform.position.y < -4.1f)
        {
            transform.position = new Vector2(transform.position.x, -4.1f);
            shake.Kill(false);
        }
        else
        {
            shake.Kill(false);
        }


        //else if (IsCollected == true)
        //{
        //    //cambia questa logica settande posizione arma in un empty object dentro player
        //    //transform.position = transform.position - bigGunOffset;

        //}
        if (transform.position.y <-3.8f&& transform.position.y > -4f)
        {
            shake= transform.DOShakeRotation(.3f, 20, 3, 20, true, ShakeRandomnessMode.Harmonic);
            transform.DORotate(Vector3.zero, .4f, RotateMode.Fast);
        }

        if (dropTimer <= DropLifeTime / 5 && !isCollected && vanishRoutine == false)
        {
            vanishRoutine = true;
            VanishWarn();
        }

        if (dropTimer <= 0 && !isCollected)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //METTO TUTTO IN UNA FUZNIONA DA DARE A WEAPONSCLASS
        tPlayer = other.GetComponent<MainCharacter>();
        //WeaponsClass oldWeapon = tPlayer.gameObject.GetComponentInChildren<WeaponsClass>();
        if (tPlayer != null)
        {
            //if (oldWeapon != null)
            //{ Destroy(oldWeapon.gameObject); }

            CollectionLogic();

            //tPlayer.activeGunPrefab = gameObject;
            //tPlayer.gunPossesed = gameObject.GetComponent<WeaponsClass>();
            //gameObject.transform.parent = tPlayer.gameObject.transform;
            //tPlayer.gunPossesed=this;
        }
    }
    protected virtual void CollectionLogic()
    {
        dropTimer = -1;
        IsCollected = true;
        Destroy(gameObject);
    }
    public void VanishWarn()
    {
        StartCoroutine(VanishWarningCoroutine());
    }
    IEnumerator VanishWarningCoroutine()
    {
        SpriteRenderer tsprite = GetComponentInChildren<SpriteRenderer>();
        tsprite.DOColor(hitFxColor, DropLifeTime / 30);
        yield return new WaitForSeconds(DropLifeTime / 30);
        tsprite.DOColor(Color.white, DropLifeTime / 30);
        yield return new WaitForSeconds(DropLifeTime / 30);
        tsprite.DOColor(hitFxColor, DropLifeTime / 30);
        yield return new WaitForSeconds(DropLifeTime / 30);
        tsprite.DOColor(Color.white, DropLifeTime / 30);
        yield return new WaitForSeconds(DropLifeTime / 30);
        gameObject.transform.DOScale(Vector3.zero, DropLifeTime / 15);
        yield return new WaitForSeconds(DropLifeTime / 15);
        vanishRoutine = false;
    }

}
