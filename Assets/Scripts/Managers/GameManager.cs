using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    [Header("Player Settings")]
    public List<Unit> playerUnits = new List<Unit>();
    public PlayerBase playerBase;
    public int playerStartingGold;
    public int playerStartingFood;

    [Header("Enemy Settings")]
    public List<Unit> enemyUnits = new List<Unit>();
    public EnemyBase enemyBase;
    public int enemyStartingGold;
    public int enemyStartingFood;

    [Header("Unit costs")]
    public int playerUnitCost;
    public int enemyUnitCost;

    [Header("Scene Management")]
    public CanvasGroup winScreen;
    public TextMeshProUGUI winLoseText;


    // Scene Manegment 
    public void PauseGameToggle(bool status)
    {
        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }


    // End game called when one of the bases are destroyed
    public void EndGame(bool winStatus)
    {
        PauseGameToggle(true);
        winScreen.alpha = 1;
        winScreen.blocksRaycasts = true;

        if (winStatus)
        {
            winLoseText.text = "You win!";
        } else
        {
            winLoseText.text = "You lose!";
        }

    }

    // Called from button on end game screen
    private void PlayAgain()
    {
        PauseGameToggle(false);
        winScreen.alpha = 0;
        winScreen.blocksRaycasts = false;

        SpawnManager.MyInstance.DeleteAllUnits();

        CurrencyManager.MyInstance.ResetCurrencies();

        playerUnits.Clear(); // deletes stored player unit list
        enemyUnits.Clear();// deletes stored enemy unit list

        SpawnManager.MyInstance.playerUnits.Clear(); // delete any player units from previous game
        SpawnManager.MyInstance.enemyUnits.Clear(); // delete any enemy units from previous game

        CurrencyManager.MyInstance.ResetResourceHubs(); // Set all resource hubs to netruel

        playerBase.InitilizeHealth();
        enemyBase.InitilizeHealth();
        SpawnManager.MyInstance.StartGame();

    }
}
