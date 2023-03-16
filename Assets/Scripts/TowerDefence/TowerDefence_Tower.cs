using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//legenda debolezze
public enum ELEMENT { MAELFORCE, ARCADIUM, VIS_VERA, ESCHATON }

public class TowerDefence_Tower : MonoBehaviour
{
    public ELEMENT myElement;

    //if tower is attacked
    float hp;
    float defence = 1;

    //attack vars
    public float fireRate = 1;
    private float fireRateTimer = 0;
    public float damage = 1;
    public float fireRange = 3;
    public float shotSpeed = 5;
    Vector3 direction;

    int towerLevel = 1;

    TowerDefence_Enemy targetEnemy;
    public TowerDefence_TowerAttack attackTemplate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer += Time.deltaTime;
        if(targetEnemy)
        { 
            if(fireRateTimer>=fireRate)
            {
                fireRateTimer = 0;
                Shoot(targetEnemy, this);
            }
        }
    }
    void Shoot(TowerDefence_Enemy enemy, TowerDefence_Tower tower)
    {

        TowerDefence_TowerAttack tAttack = Instantiate(attackTemplate, transform.position+ new Vector3(0,0,0), Quaternion.identity);
        tAttack.Shoot(enemy, tower);
        tAttack.gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        //ogni frame, all'inizio del controllo, resetta i target enemy perchè se escono dal range vengono comunuque conteggiati
        targetEnemy = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, fireRange, 1 << 9);

        if(colliders.Length>0)
        {
            targetEnemy = colliders[0].GetComponent<TowerDefence_Enemy>();

        }
    }
}
