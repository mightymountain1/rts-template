using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{

    public GameObject frostShotDebuff;

    public int XPWhenDIe;

    [Header("Xp/Gold when die")]
    public int goldReward;
    public int XPReward;

    //References
    EnemyUnitAI enemayUnitAi;
    PowerUpDrop powerUpDrop;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
         anim = GetComponentInChildren<Animator>();
       // anim = GetComponent<Animator>();
        enemayUnitAi = GetComponent<EnemyUnitAI>();
        powerUpDrop = GetComponent<PowerUpDrop>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float damage, Unit attacker, bool isHitACrit)
    {
        base.TakeDamage(damage, attacker, isHitACrit);

    //    attacker.GetComponent<PlayerUnit>().GainXP(1);

        if (health.MyCurrentValue <= 0 && !enemayUnitAi.isDead)
        {
            //GameManager.MyInstance.GainXP(10);
            //GameManager.MyInstance.GainGold(1);
            //attacker.GetComponent<PlayerUnit>().GainXP(XPWhenDIe);
          
            
        //    attacker.GetComponent<PlayerUnit>().GainXP(100);

       

            StartCoroutine(DelayedScrollingText(XPReward, goldReward));

            enemayUnitAi.Die();

            healthBarPaenl.gameObject.SetActive(false);

            powerUpDrop.DropPowerUp();

        }

    }

    // Take Damage simple
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        //attacker.GetComponent<PlayerUnit>().GainXP(1);

        if (health.MyCurrentValue <= 0 && !enemayUnitAi.isDead)
        {


            //  GameManager.MyInstance.GainXP(50);
            //    attacker.GetComponent<PlayerUnit>().GainXP(100);
            isDead = true;
          //  enemayUnitAi.Die();
            Destroy(gameObject);


        }

    }


    public void GiveRewards(int gold, int XP)
    {


       

        // Player recieves Gold and show Gold popup over dead enemy
    
        

    }

    IEnumerator DelayedScrollingText(int XP, int gold)
    {      
       // Player recieves XP 
   //     GameManager.MyInstance.GainXP(XP);
     //   GameManager.MyInstance.GainGold(gold);

        yield return new WaitForSeconds(0.25f);

     //   STManager.MyInstance.CreateText(this, XP.ToString("F0"), STTYPE.XP, false);
        
        yield return new WaitForSeconds(0.25f);

        AudioManager.MyInstance.PlayOneShot("ReceiveGold", 0.7f, 0f);
       // STManager.MyInstance.CreateText(this, gold.ToString("F0"), STTYPE.GOLD, false);
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FrostShot" && !isDead)
        {
        
        //    BuffsAndDebuffs.MyInstance.FrostShotDebuff(this);

        }

        if (other.gameObject.tag == "IceDrop" && !isDead)
        {
         //   BuffsAndDebuffs.MyInstance.FrostShotDebuff(this);
            TakeDamage(50);
        }

        if (other.gameObject.tag == "Lava")
        {
            Debug.Log("enemy  touches the lava = InTriggerEnter");
            TakeDamage(5);
        }
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Lava")
        {
            Debug.Log("enemy  touches the lava - OnParticleEnter");
            TakeDamage(5);
        }
    }

}
