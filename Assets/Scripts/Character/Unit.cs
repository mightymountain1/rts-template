using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitType
{
   Regular,
   SquadLeader
}

public abstract class Unit : MonoBehaviour
{
    public UnitType unitType;

    [SerializeField]
    protected Stat health;

    public GameObject healthBarPanel;
    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    private int level;

    public int MyLevel
    {
        get { return level; }
        set { level = value; }
    }

    ///// <summary>
    ///// The character's initialHealth
    ///// </summary>
    public float initHealth;

    public Animator anim;
    public bool isDead;
    public bool isPlayer;
    public bool isDoingDamage;
    NavMeshAgent navAgent;

    [Header("Squad Settings")]
    public bool isInSquad;
    public bool isSquadLeader;
    public SquadGroup squadGroup;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);
        anim = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();  
    }

    public void SetUnitType(UnitType setUnitType)
    {
        unitType = setUnitType;
    }

    // Take Damage Full 
    public virtual void TakeDamage(float damage, Unit attacker, bool isHitACrit)
    {
        health.MyCurrentValue -= damage;
    }
    // Take damage simple
    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;
    }
}
