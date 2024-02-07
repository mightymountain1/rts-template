using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactEffect;

    private float damage;

    public bool isPlayersBullet;

    public float MyDamage
    {
        get { return damage; }
        set { damage = value; }
    }


    void OnTriggerEnter(Collider col)
    {
        if (isPlayersBullet)
        {
            if (col.gameObject.tag == "EnemyUnit")
            {
                col.gameObject.GetComponent<EnemyUnit>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (col.gameObject.tag == "EnemyBase")
            {
                col.gameObject.GetComponent<Base>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (!isPlayersBullet)
        {
            if (col.gameObject.tag == "PlayerUnit")
            {
                col.gameObject.GetComponent<PlayerUnit>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (col.gameObject.tag == "PlayerBase")
            {
                col.gameObject.GetComponent<Base>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
