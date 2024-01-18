using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    private static FindTarget instance;

    public static FindTarget MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FindTarget>();
            }
            return instance;
        }
    }

    // enemy
    private PlayerUnit nearestPlayerTarget;
    public PlayerUnit curPlayerTarget;
    public LayerMask enemyLayer;

    // player
    private EnemyUnit nearestEnemyTarget;
    public EnemyUnit curEnemyTarget;
    public LayerMask playerLayer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // CHECK FOR PLAYER
    public void CheckForPlayer(PlayerUnitAI unitAI)
    {
        if (unitAI.state == PlayerUnitState.Idle)
        {
             unitAI.potentialEnemy = CheckForNearbyEnemies(unitAI);

            if (unitAI.potentialEnemy != null)
                if (!unitAI.potentialEnemy.isDead)
                {
                    TargetUnit(unitAI, unitAI.potentialEnemy);
                }
        }
    }


    // public EnemyUnit nearestTarget = null;
    EnemyUnit CheckForNearbyEnemies(PlayerUnitAI unitAI)
    {
        EnemyUnit nearestTarget = null;
        unitAI.hitColliders = Physics.OverlapSphere(unitAI.transform.position, unitAI.weaponRange, enemyLayer);
        float minimumDistance = Mathf.Infinity;
        foreach (Collider collider in unitAI.hitColliders)
        {
            // if (Vector3.Angle(player.transform.forward, directionToTarget) < angle / 2)
            Vector3 directionToTarget = (unitAI.transform.position - collider.transform.position).normalized;
            float angle3 = Vector3.Angle(unitAI.transform.forward, directionToTarget);
            float distance = Vector3.Distance(unitAI.transform.position, collider.transform.position);

            // if (Mathf.Abs(angle3) > shootAngle && distance < minimumDistance)
            if (distance < minimumDistance)
            {
                minimumDistance = distance;

                if (!collider.GetComponent<EnemyUnit>().isDead)
                {
                   nearestTarget = collider.GetComponent<EnemyUnit>();
                }
            }
        }
        if (nearestTarget != null) //
        {
            return nearestTarget;
        }
        else
        {
            return null;
        }
    }


    public void TargetUnit(PlayerUnitAI unitAI, EnemyUnit target)
    {
        if (!target.isDead)
        {
            unitAI.curTarget = target;

            if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) <= unitAI.attackRange)
            {
                unitAI.SetState(PlayerUnitState.Attack);
            }
            else if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) >= unitAI.attackRange && Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) <= unitAI.aggroRange)
            {
                unitAI.SetState(PlayerUnitState.MoveToEnemy);
            }
            else if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) >= unitAI.aggroRange)
            {
                unitAI.SetState(PlayerUnitState.Idle);
            }
        }
    }

    // CHECK FOR ENEMY

    public void CheckForEnemy(EnemyUnitAI unitAI)
    {
        if (unitAI.state == EnemyUnitState.MoveToBase)
        {
            unitAI.potentialEnemy = CheckForNearbyPlayers(unitAI);

            if (unitAI.potentialEnemy != null)
                if (!unitAI.potentialEnemy.isDead)
                {
                    TargetEnemyUnit(unitAI, unitAI.potentialEnemy);
                }
        }
    }


    // public EnemyUnit nearestTarget = null;
    PlayerUnit CheckForNearbyPlayers(EnemyUnitAI unitAI)
    {
        PlayerUnit nearestTarget = null;
        unitAI.hitColliders = Physics.OverlapSphere(unitAI.transform.position, unitAI.weaponRange, playerLayer);
        float minimumDistance = Mathf.Infinity;
        foreach (Collider collider in unitAI.hitColliders)
        {
            // if (Vector3.Angle(player.transform.forward, directionToTarget) < angle / 2)
            Vector3 directionToTarget = (unitAI.transform.position - collider.transform.position).normalized;
            float angle3 = Vector3.Angle(unitAI.transform.forward, directionToTarget);
            float distance = Vector3.Distance(unitAI.transform.position, collider.transform.position);

            // if (Mathf.Abs(angle3) > shootAngle && distance < minimumDistance)
            if (distance < minimumDistance)
            {
                minimumDistance = distance;

                if (!collider.GetComponent<PlayerUnit>().isDead)
                {
                    nearestTarget = collider.GetComponent<PlayerUnit>();
                }
            }
        }
        if (nearestTarget != null) //
        {
            return nearestTarget;
        }
        else
        {
            return null;
        }
    }


    public void TargetEnemyUnit(EnemyUnitAI unitAI, PlayerUnit target)
    {
        if (!target.isDead)
        {
            unitAI.curTarget = target;

            if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) <= unitAI.attackRange)
            {
                unitAI.SetState(EnemyUnitState.AttackUnit);
            }
            else if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) >= unitAI.attackRange && Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) <= unitAI.aggroRange)
            {
                unitAI.SetState(EnemyUnitState.MoveToEnemy);
            }
            else if (Vector3.Distance(unitAI.transform.position, unitAI.curTarget.transform.position) >= unitAI.aggroRange)
            {
                unitAI.SetState(EnemyUnitState.MoveToBase);
            }
        }
    }

}
