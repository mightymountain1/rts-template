using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class SpawnManager : MonoBehaviour
{

    private static SpawnManager instance;

    public static SpawnManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpawnManager>();
            }
            return instance;
        }
    }

    [Header("Player Settings")]
    public GameObject playerUnitPrefab;
    public GameObject playerSquadLeaderPrefab;
    public GameObject playerUnitSpawnPos;
    public List<Unit> playerUnits = new List<Unit>();
    public int playerUnitStartingCount;
    public Transform playerUnitParent;

    [Header("Enemy Settings")]
    public GameObject enemyUnitPrefab;
    public GameObject enemyUnitSpawnPos;
    public List<Unit> enemyUnits = new List<Unit>();
    public int enemyUnitStartingCount;
    public Transform enemyUnitParent;
    public float checkRateToSpawnUnit;

    [Header("Spawn Settings")]
    public float spawnRange = 3.0f;

    // Start is called before the first frame update
    void Start()
    {

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
      //  CountDownUpdate();
    }

    // start Game
    public void StartGame()
    {
        for (int i = 0; i < playerUnitStartingCount; i++)
        {
            CreatePlayerUnit();
        }

        for (int i = 0; i < enemyUnitStartingCount; i++)
        {
            CreateEnemyUnit();
        }

        // enemy spawn
        InvokeRepeating("TrySpawnUnit", 0.0f, checkRateToSpawnUnit);
    }


    // Spawn Player unit

    public void CreatePlayerUnit()
    {
        Vector3 point;

        if (CurrencyManager.MyInstance.playerGold - 1 < GameManager.MyInstance.playerUnitCost)
        {
           // add sound fx for failed click
            return;
        }

        GameObject unitObj = Instantiate(playerUnitPrefab, playerUnitSpawnPos.transform.position, Quaternion.identity, playerUnitParent);

      
        Unit unit = unitObj.GetComponent<Unit>();
        unit.SetUnitType(UnitType.Regular);

        // add to the list of player Units
        playerUnits.Add(unit);
        GameManager.MyInstance.playerUnits.Add(unit);

        CurrencyManager.MyInstance.DeductGoldPlayer(GameManager.MyInstance.playerUnitCost);


        // Find a random spot that doesnt overlap another agent
        if (RandomPoint(playerUnitSpawnPos.transform.position, spawnRange, out point))
        {
            NavMeshAgent nav = unit.GetComponent<NavMeshAgent>();
            nav.Warp(point);
        }


        // To do - add UI unit count
        // UIManager.MyInstance.UpdateUnitCountText();

    }

    // Player Squad Leader
    public void CreatePlayerSquadLeader()
    {
        Vector3 point;

        if (CurrencyManager.MyInstance.playerGold - 1 < GameManager.MyInstance.playerUnitCost)
        {
            // add sound fx for failed click
            return;
        }

        GameObject unitObj = Instantiate(playerSquadLeaderPrefab, playerUnitSpawnPos.transform.position, Quaternion.identity, playerUnitParent);


        Unit unit = unitObj.GetComponent<Unit>();

        unit.SetUnitType(UnitType.SquadLeader);


        // add to the list of player Units
        playerUnits.Add(unit);
        GameManager.MyInstance.playerUnits.Add(unit);

        CurrencyManager.MyInstance.DeductGoldPlayer(GameManager.MyInstance.playerUnitCost);


        // Find a random spot that doesnt overlap another agent
        if (RandomPoint(playerUnitSpawnPos.transform.position, spawnRange, out point))
        {
            NavMeshAgent nav = unit.GetComponent<NavMeshAgent>();
            nav.Warp(point);
        }


        // To do - add UI unit count
        // UIManager.MyInstance.UpdateUnitCountText();

    }



    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }


    // Spawn enemy unit

    // called from an InvokeRepeat in Start()
    public void TrySpawnUnit()
    {
        // If we can create a new unit do so
        if (CurrencyManager.MyInstance.enemyGold >= GameManager.MyInstance.enemyUnitCost)
            CreateEnemyUnit();
    }

    public SquadLeader squadLeader;
    public static List<EnemyUnit> selectedSquad = new List<EnemyUnit>();

    public void CreateEnemyUnit()
    {
        Vector3 point;

        GameObject unitObj = Instantiate(enemyUnitPrefab, enemyUnitSpawnPos.transform.position, Quaternion.identity, enemyUnitParent);

        // add to the list of player Units
        Unit unit = unitObj.GetComponent<Unit>();
        enemyUnits.Add(unit);
        GameManager.MyInstance.enemyUnits.Add(unit);
        CurrencyManager.MyInstance.DeductGoldPlayer(GameManager.MyInstance.enemyUnitCost);
        // To do - add UI unit count
        // UIManager.MyInstance.UpdateUnitCountText();


        //// Squad stuff
        //if (EnemyAI.MyInstance.squadNeedsFilling)
        //{
        //    if (squadLeader == null)
        //    {
        //        unit.GetComponent<SquadLeader>().enabled = true;
        //        EnemyAI.MyInstance.squadLeader = unit.GetComponent<SquadLeader>();
        //    } else
        //    {

        //    }
        //}


        // Find a random spot that doesnt overlap another agent
        if (RandomPoint(enemyUnitSpawnPos.transform.position, spawnRange, out point))
        {
            NavMeshAgent nav = unit.GetComponent<NavMeshAgent>();
            nav.Warp(point);
        }


    }

    //// Create Squad Leader
    //public void CreateEnemySquadLeader()
    //{
    //    Vector3 point;

    //    GameObject unitObj = Instantiate(enemyUnitPrefab, enemyUnitSpawnPos.transform.position, Quaternion.identity, enemyUnitParent);

    //    // add to the list of player Units
    //    Unit unit = unitObj.GetComponent<Unit>();
    //    enemyUnits.Add(unit);
    //    GameManager.MyInstance.enemyUnits.Add(unit);
    //    CurrencyManager.MyInstance.DeductGoldPlayer(GameManager.MyInstance.enemyUnitCost);
    //    // To do - add UI unit count
    //    // UIManager.MyInstance.UpdateUnitCountText();


    //    // Squad stuff
    //    if (EnemyAI.MyInstance.squadNeedsFilling)
    //    {
    //        if (squadLeader == null)
    //        {
    //            unit.GetComponent<SquadLeader>().enabled = true;
    //            EnemyAI.MyInstance.squadLeader = unit.GetComponent<SquadLeader>();
    //        }
    //        else
    //        {

    //        }
    //    }


    //    // Find a random spot that doesnt overlap another agent
    //    if (RandomPoint(enemyUnitSpawnPos.transform.position, spawnRange, out point))
    //    {
    //        NavMeshAgent nav = unit.GetComponent<NavMeshAgent>();
    //        nav.Warp(point);
    //    }


    //}

    // Delete all units in game (restart game)
    public void DeleteAllUnits()
    {
        foreach (Transform child in playerUnitParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in enemyUnitParent)
        {
            Destroy(child.gameObject);
        }
    }
 







    // WAVE SPAWNER

    //public void CountDownUpdate()
    //{
    //    waveCountdown -= Time.deltaTime;

    //    if (waveCountdown <= 0)
    //    {
    //        waveCountdown = timeBetweenWaves;
    //        StartCoroutine(SpawnWave());
    //    }
    //}

    //IEnumerator SpawnWave()
    //{
    //    Vector2 chosenSpot = ChooseRandomSpot();
    //    for (int i = 0; i < numberOfEnmies; i++)
    //    {
    //        Vector2 tmpSpot = ChooseEnemySpawnPoints(chosenSpot);
    //        FXManager.MyInstance.CreateEnemySpawnFX(tmpSpot);
    //        Instantiate(enemy, tmpSpot, Quaternion.identity);

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //   // ChooseRandomSpot();
    //   // Debug.Log("random");
    //    yield return new WaitForSeconds(0.2f);
    //}

    //public Vector2 ChooseRandomSpot()
    //{
    //    Vector2 randomSpot = mainCamera.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
    //    Instantiate(spawnStartFX, randomSpot, Quaternion.identity);
    //    return randomSpot;
    //}

    //public Vector2 ChooseEnemySpawnPoints(Vector2 startingPoint)
    //{
    //    Vector2 enemySpawnPoint;

    //    enemySpawnPoint = startingPoint + Random.insideUnitCircle * spawnRange ;


    //    return enemySpawnPoint;
    //}

}


// testing get a random position
//private NavMeshTriangulation Triangulation;

//Triangulation = NavMesh.CalculateTriangulation();
//Vector3 pos;

//int VertexIndex = Random.Range(0, Triangulation.vertices.Length);

//NavMeshHit Hit;

//if (NavMesh.SamplePosition(Triangulation.vertices[VertexIndex], out Hit, 2f, -1))
//{
//    NavMeshAgent nav = unit.GetComponent<NavMeshAgent>();

//    nav.Warp(Hit.position);

//}