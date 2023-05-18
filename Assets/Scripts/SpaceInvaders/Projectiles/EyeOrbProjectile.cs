using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOrbProjectile : WeaponProjectile
{
    [SerializeField] private float thisSpeed = 10f;
    public override float Speed => baseSpeed * thisSpeed;
    //public override int ShotDamage => baseShotDamage * 2;
    [SerializeField] private int thisShotDamage = 2;
    public override int ShotDamage { get { return baseShotDamage * thisShotDamage; } }

    public override Vector3 DirectionVector => baseDirectionVector * 1f;

    public EyeOrbProjectile()
    {
        //ShotDamage = ShotDamage * 2;
    }

    public override void Shoot(Vector3 dirVector, int damageMulti)
    {
        base.Shoot(dirVector, damageMulti);
        //(45°sinistra versoreX=-1 ; 45°destra versoreX=1 ; versoreY=1 sempre verso alto)

        //verso sinistra
        //movementVector = new Vector3(-1,1)*Speed;
        gameObject.GetComponent<Rigidbody>().velocity = movementVector;

        //Velocita X
        //Velocita Y

        //movementVector.x
        //movementVector.x
        //gameObject.GetComponent<Rigidbody>().AddForce(movementVector * Time.deltaTime, ForceMode.VelocityChange);

        Destroy(gameObject, 20);
    }
    public override void UpdateMovement()
    {
        //(1)=n
        if (shooted)
        {
            //gameObject.transform.rotation.z= Quaternion.FromToRotation(movementVector, Vector3.down);
            //gameObject.transform.rotation = Quaternion.FromToRotation(movementVector, Vector3.down);
            //gameObject.transform.rotation= Quaternion.LookRotation(Vector3.forward,movementVector);
            //movementVector.y = Mathf.Sqrt(2) / 2 * Speed * Time.deltaTime;
            //movementVector.x = -Mathf.Sqrt(2) / 2 * Speed * Time.deltaTime;
            //transform.position += new Vector3(movementVector.x, (movementVector.y - 20 * Mathf.Pow(Time.deltaTime, 2)));
        }
            //transform.position += movementVector * Time.deltaTime;
        //base.UpdateMovement();
    }

    public override void OnTriggerLogic(Collider entering)
    {
        base.OnTriggerLogic(entering);
        if (tEnterEnemy != null && tEnterEnemy != tExitEnemy /*&& !hit*/)
        {
            //HO COLPITO
            hit = true;
            tEnterEnemy.OnHitSuffered(hittingShotDamage);
        }
    }
    public override void OnExitTriggerLogic(Collider exiting)
    {
        tExitEnemy = exiting.GetComponent<IHittable>();
        //if (other.GetComponent<Enemy>()!=enemy)
        //if (tExitEnemy!= tEnterEnemy)
        hit = false;
    }
}
