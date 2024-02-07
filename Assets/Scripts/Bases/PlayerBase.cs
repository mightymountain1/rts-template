using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Base
{
    private static PlayerBase instance;

    public static PlayerBase MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerBase>();
            }
            return instance;
        }
    }

    protected override void Start()
    {
        base.Start();

    }

    public override void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {
            GameManager.MyInstance.EndGame(false); // lose game
        }
    }
}
