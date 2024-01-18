using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
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
        get { return level; }
        set { level = value; }
    }

    ///// <summary>
    ///// The character's initialHealth
    ///// </summary>

    public float initHealth;

    public Animator anim;

    public bool isDead;




    //[Header("Stats")]
    //public float attackPower;
    //public float critChance = .10f;
    //public int agility;
    //public int minDamage;
    //public int maxDamage;
    //public float damage;

    //[Header("Stat Display")]
    //public Text attackPowerText;
    //public Text agilityText;
    //public Text damageText;
    //public Text critText;


    public bool isPlayer;
    public bool isDoingDamage;

    NavMeshAgent navAgent;

   

    // int level;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);

     

        anim = GetComponentInChildren<Animator>();
       // anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
   

      
    }

    // Update is called once per frame
    void Update()
    {


    }


    //public void CalculateStats()
    //{
    //    //  level = Player.MyInstance.MyLevel;

    //    attackPower = (level * 2) + ((agility * 2) - 20);
    //    damage = ((minDamage + maxDamage) / 2) + (attackPower / 14f);


    // //  agilityText.text = agility.ToString();
    //    attackPowerText.text = "Attack Power: " + attackPower.ToString();
    //    damageText.text = "Damage: " + damage.ToString();
    //    critText.text = "Crit Chance: " + critChance.ToString();

    //}



    //public float CalculateDamage(float critModifier, out bool isCrit)
    //{


    //    baseDamage = Random.Range(minDamage, maxDamage);

    //    damageToDeal = baseDamage + (attackPower / 14f);

    //    damageToDeal += CalculateCrit(damageToDeal, critModifier, out isCrit);

    //    //  Debug.Log("using new system CalculateDamage4: baseDamage: " + baseDamage + ". Damage to deal: " + damageToDeal + ". Is a crit: " + isCrit);

    //    return damageToDeal;



    //}


    ///*
    // * Weapon Damage Per Second (DPS) Formula
    // * ((Min Weapon Damage + Max Weapon Damage) / 2) / Weapon Speed
    // *  
    // * 
    // * 
    // * 
    // * 
    // * */

    //private float CalculateCrit(float damageToDeal, float critModifier, out bool isCrit)
    //{
    //    if (Random.value <= critChance + critModifier)
    //    {
    //        float critDamage = damageToDeal * Random.Range(.75F, .45F);
    //        isCrit = true;
    //        return critDamage;
    //    }
    //    else
    //    {
    //        isCrit = false;
    //        return 0;
    //    }



    //}


    public virtual void TakeDamage(float damage, Unit attacker, bool isHitACrit)
    {
        health.MyCurrentValue -= damage;
        //   Debug.Log("taking damage " + damage + ". health remaining: " + health.MyCurrentValue);


        if (isPlayer)
        {
            //     STManager.MyInstance.CreateText(this, damage.ToString("F0"), STTYPE.PLAYERDAMAGE, isHitACrit);

        }
        else
        {
            //   STManager.MyInstance.CreateText(this, damage.ToString("F0"), STTYPE.ENEMYDAMAGE, isHitACrit);

        }
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;
        //   Debug.Log("taking damage " + damage + ". health remaining: " + health.MyCurrentValue);


        if (isPlayer)
        {//
         //  STManager.MyInstance.CreateText(this, damage.ToString("F0"), STTYPE.PLAYERDAMAGE, false);

        }
        else
        {
            //   STManager.MyInstance.CreateText(this, damage.ToString("F0"), STTYPE.ENEMYDAMAGE, false);

        }
    }


    //public virtual void IncreaseHealth(float healthIncrease)
    //{

    //    health.MyCurrentValue += healthIncrease;

    //   // STManager.MyInstance.CreateText(this, healthIncrease.ToString("F0"), STTYPE.HEAL, false);
    //}


    //public void DoDamage(Collider collision)
    //{
    //    tmpDamage = CalculateDamage(0, out isCrit);
    //    collision.GetComponent<Unit>().TakeDamage(tmpDamage, this, isCrit);
    //}

    //public void AttackBase(Collider collision)
    //{
    //    tmpDamage = CalculateDamage(0, out isCrit);
    //    collision.GetComponent<PlayerBase>().TakeDamage(tmpDamage, this, isCrit);
    //}







}
