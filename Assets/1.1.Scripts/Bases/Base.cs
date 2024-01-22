using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    [SerializeField]
    protected Stat health;

    public GameObject healthBarPaenl;
    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    private int level;

    public int MyLevel
    {
        get { return level;}
        set { level = value;}
    }

    public float initHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitilizeHealth();

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;
 
    }

    public void InitilizeHealth()
    {
        health.Initialize(initHealth, initHealth);
    }
}
