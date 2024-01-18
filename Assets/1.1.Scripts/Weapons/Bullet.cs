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



    //void OnCollisionEnter(Collision col)
    //{
    //    if (isPlayersBullet)
    //    {
    //        if (col.gameObject.tag == "EnemyUnit")
    //        {
    //            Debug.Log("HIT ENEMY");
    //            col.gameObject.GetComponent<EnemyUnit>().TakeDamage(damage);

    //            Destroy(gameObject);
    //        }
    //    }
    //    else
    //    {
    //        if (col.gameObject.tag == "PlayerUnit")
    //        {
    //            Debug.Log("HIT Player");
    //            col.gameObject.GetComponent<PlayerUnit>().TakeDamage(damage);

    //            Destroy(gameObject);
    //        }
    //    }

    //}


    void OnTriggerEnter(Collider col)
    {

        if (isPlayersBullet)
        {
            if (col.gameObject.tag == "EnemyUnit")
            {
                Debug.Log("HIT ENEMY");
                col.gameObject.GetComponent<EnemyUnit>().TakeDamage(damage);

                Destroy(gameObject);
            }
        }
        else
        {
            if (col.gameObject.tag == "PlayerUnit")
            {
                Debug.Log("HIT Player");
                col.gameObject.GetComponent<PlayerUnit>().TakeDamage(damage);

                Destroy(gameObject);
            }
        }



    }


}
