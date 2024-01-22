using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Base
{
    protected override void Start()
    {
        base.Start();

    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void TakeDamage(float damage)
    {

        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {
            GameManager.MyInstance.EndGame(true); // win game
        }
    }
}
