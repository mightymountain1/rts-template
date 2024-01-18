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

    [Header("Enemy Currency")]
    public int enemyGold;
    public TextMeshProUGUI enemyMyGoldText;

    // To do
    // - points


    // Start is called before the first frame update
    void Start()
    {
        playerGold = GameManager.MyInstance.playerStartingGold;
        enemyGold = GameManager.MyInstance.enemyStartingGold;
        UpdateTextResources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessResource(ResourceType resourceType, int quantity, BuildingOwnership resourceOwnership)
    {
        if (resourceOwnership == BuildingOwnership.Player)
        {
            switch (resourceType)
            {
                case ResourceType.Food:

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


    // Enemy --------------------------------
    public void GainGoldEnemy(int goldCoin)
    {
        enemyGold += goldCoin;
        //UpdateTextResources();

    }

    public void DeductGoldEnemy(int goldCoin)
    {
        enemyGold -= goldCoin;
       // UpdateTextResources();
    }
    // update UI
    public void UpdateTextResources()
    {
        playerMyGoldText.text = playerGold.ToString();
        // to do points
    }
}
