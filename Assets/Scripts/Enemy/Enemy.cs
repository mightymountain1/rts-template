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

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health.MyCurrentValue <= 0)
        {
            isDead = true;
            this.gameObject.layer = LayerMask.NameToLayer("Dead");
            Destroy(gameObject);
        }
    }
}
