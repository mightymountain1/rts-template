using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentPriority
{
    ResourceHubs,
    DefendBase,
    AttackPlayerBase,
    AttackClosestUnit,
}


public class EnemyAI : MonoBehaviour
{
    private static EnemyAI instance;

    public static EnemyAI MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyAI>();
            }
            return instance;
        }
    }

    CurrentPriority currentPriority;

    public bool squadNeedsFilling;
    public SquadLeader squadLeader;
    public static List<EnemyUnit> selectedSquad = new List<EnemyUnit>();


   public void CheckResources()
    {
        int numOfPlayerHubs = 0;
        int numOfEnemyHubs = 0;

        foreach (ResourceHub resourceHub in CurrencyManager.MyInstance.resourceHubsInGame)
        {
            if (resourceHub.ownershipState == BuildingOwnership.Player)
            {
                numOfPlayerHubs++;
            } else
            {
                numOfEnemyHubs++;
            }
        }

        if (numOfEnemyHubs < numOfPlayerHubs)
        {
            currentPriority = CurrentPriority.ResourceHubs;
        }
    }

    

    public ResourceHub GetClosestResource(Vector3 pos)
    {
        ResourceHub[] closest = new ResourceHub[3];
        float[] closestDist = new float[3];

        foreach (ResourceHub resource in CurrencyManager.MyInstance.resourceHubsInGame)
        {
            if (resource == null)
                continue;

            float dist = Vector3.Distance(pos, resource.transform.position);

            for (int x = 0; x < closest.Length; x++)
            {

                if (closest[x] == null)
                {
                    closest[x] = resource;
                    closestDist[x] = dist;
                    break;
                }
                else if (dist < closestDist[x])
                {
                    closest[x] = resource;
                    closestDist[x] = dist;
                    break;
                }
            }
        }

        return closest[Random.Range(0, closest.Length)];

    }
}
