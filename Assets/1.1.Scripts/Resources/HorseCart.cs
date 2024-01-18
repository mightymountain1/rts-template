using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HorseCart : MonoBehaviour
{

    private NavMeshAgent navAgent;
    public Transform cartHorseHome;

    public ResourceType resourceCarryType;

    public ResourceHub resourceHub;

    [SerializeField] private int quantity;

    PlayerBase playerBase;
    EnemyBase enemyBase;
    void Awake()
    {

        navAgent = GetComponent<NavMeshAgent>();

    }


    void Start()
    {

        playerBase = GameManager.MyInstance.playerBase;
        enemyBase = GameManager.MyInstance.enemyBase;

        if (resourceHub.ownershipState == BuildingOwnership.Player)
        {
            navAgent.SetDestination(playerBase.transform.position);

        } else
        {
            navAgent.SetDestination(enemyBase.transform.position);
        }

   
      //  Player player = GetComponent<Player>();
    }
    public void FillResource(ResourceType type, int carryLoad)
    {
        resourceCarryType = type;
        quantity = carryLoad;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBase" && resourceHub.ownershipState == BuildingOwnership.Player)
        {
            GatherResourcePlayer();
        }

        if (other.gameObject.tag == "EnemyBase" && resourceHub.ownershipState == BuildingOwnership.Enemy)
        {
            GatherResourceEnemy();
        }
    }


    public void GatherResourcePlayer()
    {
        CurrencyManager.MyInstance.GainGoldPlayer(quantity);
        Destroy(gameObject);


    }
    public void GatherResourceEnemy()
    {

        Destroy(gameObject);
    }
}
