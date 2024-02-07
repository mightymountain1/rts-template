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

    [SerializeField]
    protected float initHealth;    // The character's initialHealth


    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;
    }

}
