using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ResourceType
{
    Food,
    Gold,
    Iron
}

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager instance;

    public static CurrencyManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CurrencyManager>();
            }
            return instance;
        }
    }

    ResourceType resourceType;

    [Header("Player Currency")]
    public int playerGold;
    public TextMeshProUGUI playerMyGoldText;
    public int playerFood;
    public TextMeshProUGUI playerMyFoodText;

    [Header("Enemy Currency")]
    public int enemyGold;
    public TextMeshProUGUI enemyMyGoldText;
    public int enemyFood;
    public TextMeshProUGUI enemyMyFoodText;

    [Header("Resources")]
    public ResourceHub[] resourceHubsInGame;
        

    // Start is called before the first frame update
    void Start()
    {
        ResetCurrencies(); // set all currencies at the starting levels
        resourceHubsInGame = FindObjectsOfType<ResourceHub>(); // find all the resource hubs in game and store in an array.
    }


    public void ProcessResource(ResourceType resourceType, int quantity, BuildingOwnership resourceOwnership)
    {
        if (resourceOwnership == BuildingOwnership.Player)
        {
            switch (resourceType)
            {
                case ResourceType.Food:
                    GainFoodPlayer(quantity);
                    break;
                case ResourceType.Gold:
                    GainGoldPlayer(quantity);
                    break;
                case ResourceType.Iron:
                    break;
                default:
                    break;
            }
        } else
        {
            switch (resourceType)
            {
                case ResourceType.Food:
                    GainFoodEnemy(quantity);
                    break;
                case ResourceType.Gold:
                    GainGoldEnemy(quantity);
                    break;
                case ResourceType.Iron:
                    break;
                default:
                    break;
            }
        }
       
    }

    // PLAYER --------------------------------
    public void GainGoldPlayer(int goldCoin)
    {
        playerGold += goldCoin;
        UpdateTextResources();

    }

    public void DeductGoldPlayer(int goldCoin)
    {
        playerGold -= goldCoin;
        UpdateTextResources();
    }

    public void GainFoodPlayer(int foodAmount)
    {
        playerFood += foodAmount;
        UpdateTextResources();

    }

    public void DeductfoodPlayer(int foodAmount)
    {
        playerGold -= foodAmount;
        UpdateTextResources();
    }


    // Enemy --------------------------------

    // enemy gain gold
    public void GainGoldEnemy(int goldCoin)
    {
        enemyGold += goldCoin;

    }
    // enemy deduct gold
    public void DeductGoldEnemy(int goldCoin)
    {
        enemyGold -= goldCoin;
    }

    // Enemy Gain food
    public void GainFoodEnemy(int foodAmount)
    {
        enemyFood += foodAmount;

    }

    // Enemy Deduct food
    public void DeductfoodEnemy(int foodAmount)
    {
        enemyFood -= foodAmount;
    }

    // update UI
    public void UpdateTextResources()
    {
        playerMyGoldText.text = playerGold.ToString();
    }

    // reset currencies
    public void ResetCurrencies()
    {
        playerGold = GameManager.MyInstance.playerStartingGold;
        enemyGold = GameManager.MyInstance.enemyStartingGold;
        playerFood = GameManager.MyInstance.playerStartingFood;
        enemyFood = GameManager.MyInstance.enemyStartingFood;

        UpdateTextResources();
    }

    // Reset ResourceHubs
    public void ResetResourceHubs()
    {
        foreach (ResourceHub resourceHub in resourceHubsInGame)
        {
            resourceHub.SetOwnershipToNeutral();
        }
    }
}
