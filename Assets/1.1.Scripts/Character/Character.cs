using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    [SerializeField]
    protected Stat health;

    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    protected Stat maxHealth;

    public Stat MyMaxHealth
    {
        get { return maxHealth; }
    }

    [SerializeField]
    private bool isDead;

    public bool MyisDead
    {
        get { return isDead; }
        set { isDead = value; }
    }


    public Image healthBar;
    ///// <summary>
    ///// The character's initialHealth
    ///// </summary>
    [SerializeField]
    protected float initHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {

            health.MyCurrentValue -= damage;

    }

}
