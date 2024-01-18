using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Enemy Settings")]
    public List<Unit> enemyUnits = new List<Unit>();
    public EnemyBase enemyBase;
    public int enemyStartingGold;


    [Header("Unit costs")]
    public int playerUnitCost;
    public int enemyUnitCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
