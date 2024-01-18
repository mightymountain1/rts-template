using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public bool isDead;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health.Initialize(initHealth, initHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void TakeDamage(float damage)
    {

        base.TakeDamage(damage);


    //    FXManager.MyInstance.CameraShakeFX(cameraShakeDuration, cameraShakeMagnitude);

 

        if (health.MyCurrentValue <= 0)
        {

            FXManager.MyInstance.CreateEnemyDieFX(transform.position);
            isDead = true;
            this.gameObject.layer = LayerMask.NameToLayer("Dead");
            Destroy(gameObject);
            //  StartCoroutine(Flash());
         //   Die();

          //  isDead = true;
        }
        else
        {
          //  enemyAI.PlayHitSound();


          //  StartCoroutine(Flash());

          //  StartCoroutine(KnockBack(knockbackForce));

        }

    }

  

}
