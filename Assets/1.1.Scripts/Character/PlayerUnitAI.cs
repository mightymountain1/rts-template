using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public enum PlayerUnitState
{
    Idle,
    Move,
    MoveToEnemy,
    MoveToEnemyOrder,
    Attack,
    Dead
}


public class PlayerUnitAI : MonoBehaviour
{


    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<PlayerUnitState> { }
    public StateChangeEvent onStateChange;

    public PlayerUnitState state;

    public float checkRate = 1.0f;

    public LayerMask PlayerLayerMask;

    public float attackRange;
    public float aggroRange;


    // find nearest target
   public EnemyUnit nearestTarget;
 //   private int enemyLayer;
    public EnemyUnit curTarget;

    // movement
    public float pathUpdateRate = 1.0f;
    private float lastpathUpdateTime;

    //atack
    public float attackRate;
    private float lastAttackRate;
    private float lastAttackTime;

    NavMeshAgent navAgent;
    Animator anim;
    
    //bools
    public bool isDead;
    public bool givenOrders;

    Unit unit;
    PlayerUnit playerUnit;

    public GameObject healthBar; // to turn off when die
    public LayerMask enemyLayer;
    [Header("Weapon settings")]
    public Weapon weapon;
    public float weaponRange;
    public float shootAngle;


    public EnemyUnit potentialEnemy;
    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("Check", 0.0f, checkRate);

        unit = GetComponent<Unit>();
        playerUnit = GetComponent<PlayerUnit>();
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

       // enemyLayer = LayerMask.NameToLayer("EnemyUnits");
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerUnitState.Idle:
                {
                    IdleUpdate();
                  //  curPlayerTarget = null;
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", true);
                    break;
                }

            case PlayerUnitState.Move:
                {
                    MoveUpdate();
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    break;
                }
            case PlayerUnitState.MoveToEnemy:
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    MoveToEnemyUpdate();
                    break;
                }
            case PlayerUnitState.MoveToEnemyOrder:
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    MoveToEnemyUpdateOrder();
                    break;
                }
            case PlayerUnitState.Attack:
                {
                    AttackUpdate();
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isIdle", false);
                    break;
                }
            case PlayerUnitState.Dead:
                {

                    anim.SetBool("isDead", true);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    break;
                }
        }


    }


    // Look for Target

    void Check()
    {
        FindTarget.MyInstance.CheckForPlayer(this);
        //if (state == PlayerUnitState.Idle)
        //{
        //    EnemyUnit potentialEnemy = CheckForNearbyEnemies();

        //    if (potentialEnemy != null)
        //        if (!potentialEnemy.isDead)
        //        {
        //            TargetUnit(potentialEnemy);
        //        }
        //}

    }


    public Collider[] hitColliders;



    void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);

    }


    // called every frame the 'Attack' state is active
    void AttackUpdate()
    {
        if (curTarget.isDead)
        {
            curTarget = null;
            navAgent.SetDestination(transform.position);
            navAgent.ResetPath();
            SetState(PlayerUnitState.Idle);

            return;
        }


        LookAt(curTarget.transform.position);
       

        // If we're still moving, stop
        if (!navAgent.isStopped)
            navAgent.isStopped = true;


        // attack every 'attackRate' seconds

        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;

            weapon.Fire(true);

        }

        if (Vector3.Distance(transform.position, curTarget.transform.position) >= attackRange)
        {
            SetState(PlayerUnitState.MoveToEnemy);

        }
        else if (Vector3.Distance(transform.position, curTarget.transform.position) >= aggroRange)
        {
            SetState(PlayerUnitState.Idle);
        }

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

    private void MoveUpdate()
    {
        curTarget = null;

        // check if the units destination is reached. If so then go to idle state
        if (Vector3.Distance(transform.position, navAgent.destination) < 1.00f)
        {
            SetState(PlayerUnitState.Idle);
            //curPlayerTarget = null;
  

        }
    }

    void MoveToEnemyUpdate()
    {
        if (curTarget != null)
        {
            navAgent.SetDestination(curTarget.transform.position);
        }
     
        if (curTarget == null)
        {
            SetState(PlayerUnitState.Idle);
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
            SetState(PlayerUnitState.Attack);

        }
        else if (Vector3.Distance(transform.position, curTarget.transform.position) >= aggroRange)
        {
           SetState(PlayerUnitState.Idle);
        }
    }

    void MoveToEnemyUpdateOrder()
    {
        
        navAgent.SetDestination(curTarget.transform.position);
      
        if (curTarget == null)
        {
            SetState(PlayerUnitState.Idle);
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
            SetState(PlayerUnitState.Attack);

        }
        //else if (Vector3.Distance(transform.position, curTarget.transform.position) >= aggroRange)
        //{
        //    SetState(PlayerUnitState.Idle);
        //}
    }




    public void Die()
    {

        if (!isDead)
        {
            isDead = true;
            playerUnit.isDead = true;

            SetState(PlayerUnitState.Dead);

            anim.SetTrigger("deadTrigger");

            navAgent.enabled = false;
            playerUnit.enabled = false;

            healthBar.SetActive(false);


            this.gameObject.tag = "Dead";
            this.gameObject.layer = LayerMask.NameToLayer("Dead");

        }



        // anim.enabled = false;



    }




    /// <summary>
    /// Movce to position
    /// </summary>
    /// <param name="pos"></param>
    public void MoveToPosition(Vector3 pos)
    {
        SetState(PlayerUnitState.Move);
        curTarget = null;
        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    public void SetState(PlayerUnitState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);


    }
}
