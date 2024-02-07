using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HorseCart : MonoBehaviour
{
    [SerializeField] private int quantity;
    private NavMeshAgent navAgent;
    public Transform cartHorseHome;
    public ResourceType resourceCarryType;
    public ResourceHub resourceHub;

    PlayerBase playerBase;
    EnemyBase enemyBase;

    BuildingOwnership resourceOwner;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        playerBase = GameManager.MyInstance.playerBase;
        enemyBase = GameManager.MyInstance.enemyBase;
        resourceOwner = resourceHub.ownershipState;

        if (resourceOwner == BuildingOwnership.Player)
        {
            navAgent.SetDestination(playerBase.transform.position);

        } else
        {
            navAgent.SetDestination(enemyBase.transform.position);
        }
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
            ProcessResource();
        }

        if (other.gameObject.tag == "EnemyBase" && resourceHub.ownershipState == BuildingOwnership.Enemy)
        {
            ProcessResource();
        }
    }

    
    public void ProcessResource()
    {
        CurrencyManager.MyInstance.ProcessResource(resourceCarryType, quantity, resourceOwner);
        Destroy(gameObject);
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