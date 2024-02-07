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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponentInChildren<Animator>();
        enemayUnitAi = GetComponent<EnemyUnitAI>();
    }


    public override void TakeDamage(float damage, Unit attacker, bool isHitACrit)
    {
        base.TakeDamage(damage, attacker, isHitACrit);

        if (health.MyCurrentValue <= 0 && !enemayUnitAi.isDead)
        {  
            enemayUnitAi.Die();
            healthBarPanel.gameObject.SetActive(false);
        }
    }

    // Take Damage simple
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health.MyCurrentValue <= 0 && !enemayUnitAi.isDead)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
