using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ResourceSource : MonoBehaviour
{
    public ResourceType type;
    public int quantity;


    // events
    public UnityEvent onQuantityChange;

    public void GatherResource(int amount, Player gatheringPlayer)
    {
        quantity -= amount;

        int amountToGive = amount;

        // make sure we don't give more than we have+
        if (quantity < 0)
            amountToGive = amount + quantity;



     //   gatheringPlayer.GainResource(type, amountToGive);

        // if we're depleted, delete the resource
        if (quantity <= 0)
            Destroy(gameObject);

        if (onQuantityChange != null)
            onQuantityChange.Invoke();

    }

}



