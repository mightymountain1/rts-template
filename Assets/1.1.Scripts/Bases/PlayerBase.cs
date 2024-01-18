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
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float damage)
    {

        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {

        }
    }
}