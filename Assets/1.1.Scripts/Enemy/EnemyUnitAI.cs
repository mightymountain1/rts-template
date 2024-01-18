using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EnemyUnitState
{
    MoveToBase,
    MoveToEnemy,
    AttackBase,
    AttackUnit,
    Dead,
    Idle,

}

public class EnemyUnitAI : MonoBehaviour
{
    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<EnemyUnitState> { }
    public StateChangeEvent onStateChange;

    public EnemyUnitState state;

    public float checkRate = 1.0f;

    public LayerMask PlayerLayerMask;

    public PlayerUnit curEnemyTarget;

    public GameObject healthBar;
    public float attackRate;
    public float pathUpdateRate = 1.0f;
    public float attackRange;
    public float aggroRange;
    public Text levelTextHB;

    private float lastpathUpdateTime;
    private float lastAttackRate;
    private float lastAttackTime;
    private Transform nearestEnemy;
    private PlayerUnit nearestTarget;
 
    NavMeshAgent navAgent;


    // meh


    public bool isDead;
    
    // References
    Animator anim;
    EnemyUnit enemyUnit;
    PlayerBase playerBase;
    Unit unit;



   
    public Collider[] hitColliders;

    public PlayerUnit curTarget;
    [Header("Weapon settings")]
    public Weapon weapon;
    public float weaponRange;
    public float shootAngle;

    // hide in inspector
    public PlayerUnit potentialEnemy;
    private void Awake()
    {

      

        navAgent = GetComponent<NavMeshAgent>();
         anim = GetComponentInChildren<Animator>();
       // anim = GetComponent<Animator>();

        enemyUnit = GetComponent<EnemyUnit>();
    }

    void Start()
    {
        playerBase = GameManager.MyInstance.playerBase;

        InvokeRepeating("Check", 0.0f, checkRate);

        unit = GetComponent<Unit>();

           navAgent.SetDestination(playerBase.transform.position);
        SetState(EnemyUnitState.MoveToBase);
        anim.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case EnemyUnitState.Idle:
                {
                    IdleUpdate();
                    //  curPlayerTarget = null;
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", true);
                    break;
                }

            case EnemyUnitState.MoveToBase:
                {
                    MoveToBaseUpdate();
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isRagdoll", false);
                    break;
                }
            case EnemyUnitState.MoveToEnemy:
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isRagdoll", false);
                    // navAgent.SetDestination(PlayerBase.MyInstance.transform.position);
                    MoveToEnemyUpdate();
                    break;
                }
            case EnemyUnitState.AttackUnit:
                {
                    AttackUnitUpdate();
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isRagdoll", false);
                    break;
                }
            case EnemyUnitState.AttackBase:
                {
                    AttackBaseUpdate();
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isRagdoll", false);
                    break;
                }
            case EnemyUnitState.Dead:
                {
                //    BodyFall();
                    anim.SetBool("isDead", true);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isRagdoll", false);
                    break;
                }

          
        }




    }
    void Check()
    {
        if (state == EnemyUnitState.MoveToBase)
            FindTarget.MyInstance.CheckForEnemy(this);

    }



    private void IdleUpdate()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isIdle", true);
        curTarget = null;
        navAgent.SetDestination(this.transform.position);
        navAgent.ResetPath();

    }

    // called every frame the 'Attack' state is active
    void AttackUnitUpdate()
    {

        if (curTarget.isDead)
        {
            curTarget = null;
            navAgent.SetDestination(transform.position);
            navAgent.ResetPath();
            SetState(EnemyUnitState.MoveToBase);

            return;
        }

        LookAt(curTarget.transform.position);

        if (curTarget == null)
        {
            SetState(EnemyUnitState.Idle);
            return;
        }

        if (Vector3.Distance(transform.position, curTarget.transform.position) >= attackRange)
        {
            SetState(EnemyUnitState.MoveToEnemy);

        }
        else if (Vector3.Distance(transform.position, curTarget.transform.position) >= aggroRange)
        {
            SetState(EnemyUnitState.MoveToBase);
        }

        if (!navAgent.isStopped)
            navAgent.isStopped = true;

        FireWeapon();
    }


    public void FireWeapon()
    {
        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            weapon.Fire(false);
        }
    }


    /// <summary>
    /// Move Update
    /// </summary>
    void MoveToBaseUpdate()
    {
        if (navAgent.destination != playerBase.transform.position)
        {
            navAgent.SetDestination(playerBase.transform.position);
        }

        // check if the units destination is reached. If so then go to idle state
        if (Vector3.Distance(transform.position, playerBase.transform.position) <= attackRange)
        {
            SetState(EnemyUnitState.AttackBase);  
        }

        //if (curTarget != null)
        //{
        //    if (Vector3.Distance(transform.position, curTarget.transform.position) <= unitAI.attackRange)
        //    {
        //        unitAI.SetState(EnemyUnitState.AttackUnit);
        //    }
        //    else if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) >= unitAI.attackRange && Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) <= unitAI.aggroRange)
        //    {
        //        unitAI.SetState(EnemyUnitState.MoveToEnemy);
        //    }
        //    else if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) >= unitAI.aggroRange)
        //    {
        //        unitAI.SetState(EnemyUnitState.MoveToBase);
        //    }
        //}

    }



    void AttackBaseUpdate()
    {
        navAgent.SetDestination(transform.position);
        LookAt(playerBase.transform.position);
        FireWeapon();
    }

    void MoveToEnemyUpdate()
    {
        navAgent.SetDestination(curTarget.transform.position);
       
        if (curTarget == null)
        {
            SetState(EnemyUnitState.MoveToBase);
            return;
        }

        if (Time.time - lastpathUpdateTime > pathUpdateRate)
        {
            lastpathUpdateTime = Time.time;
            navAgent.isStopped = false;
            navAgent.SetDestination(curTarget.transform.position);
        }

        if (Vector3.Distance(transform.position, curTarget.transform.position) <= attackRange)
        {
            SetState(EnemyUnitState.AttackUnit);

        }
       
        else if (Vector3.Distance(transform.position, curTarget.transform.position) >= aggroRange)
        {
            SetState(EnemyUnitState.MoveToBase);
        }
    }



    // DIE
    public void Die()
    {
        isDead = true;
        enemyUnit.isDead = true;
        anim.SetTrigger("deadTrigger");
        SetState(EnemyUnitState.Dead);

        navAgent.enabled = false;
        //   anim.enabled = false;
        enemyUnit.enabled = false;
        healthBar.SetActive(false);
        this.gameObject.tag = "Dead";
        this.gameObject.layer = LayerMask.NameToLayer("Dead");
        
    }


    void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
  
    public void SetState(EnemyUnitState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }
}
