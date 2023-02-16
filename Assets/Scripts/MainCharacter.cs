using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public int hp=4;
    public Projectile myProjectile;
    float moveSpeed = 6;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        //SPARO
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            //alternativa
            //Instantiate(myProjectile, transform.position, transform.rotation).Shoot(Vector3.up);
            //Alternativa:
            Shoot();
        }
    }

    public void Shoot ()
    {
        Projectile tempProjectile = Instantiate(myProjectile, transform.position, transform.rotation);
        tempProjectile.Shoot(Vector3.up * 6.6f);
    }
    public void OnHitSuffered(int damage = 1)
    {
        if(--hp<=0) //fa decremento e poi valuta se minore o uguale a 0// hp--<=0 guarda se hp minore o uguale a 0 e poi fa decremento
        {
            //morte
            Destroy(gameObject);
        }
        else
        {
            //come dire transform.localScale*2
            //transform.localScale *= 2;
            //hp--;
            UIManager.instance.OnPlayerHitSuffered();
        }
    }
}
