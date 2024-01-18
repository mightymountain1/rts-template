using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{

    private static FXManager instance;

    public static FXManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FXManager>();
            }
            return instance;
        }
    }

    [Header("Impact FX")]
    [SerializeField] private GameObject enemyDieFX;
    [SerializeField] private GameObject wallBumpFX;
    [SerializeField] private GameObject enemySpawnFX;

    public Transform fxParent;

    public Camera mainCamera;

    [Header("Powerups")]
    public GameObject healthPowerup;
    public GameObject perkPowerup;

    [Header("Thrust")]
    public ParticleSystem thrustPS;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InstantiateHealthPowerUp", 2, 15);
        InvokeRepeating("InstantiatePerkPowerUp", 2, 15);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void CreatWallBumpFX(Vector3 pos)
    {
        GameObject bloodPoolGO = Instantiate(wallBumpFX, pos, Quaternion.identity);
        bloodPoolGO.transform.SetParent(fxParent);
    }

    public void CreateEnemyDieFX(Vector3 pos)
    {
        GameObject bloodPoolGO = Instantiate(enemyDieFX, pos, Quaternion.identity);
        bloodPoolGO.transform.SetParent(fxParent);
    }

    public void CreateEnemySpawnFX(Vector3 pos)
    {
        GameObject bloodPoolGO = Instantiate(enemySpawnFX, pos, Quaternion.identity);
        bloodPoolGO.transform.SetParent(fxParent);
    }



    public Vector2 InstantiateHealthPowerUp()
    {
        Vector2 randomSpot = mainCamera.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        Instantiate(healthPowerup, randomSpot, Quaternion.identity);
        return randomSpot;
    }

    public Vector2 InstantiatePerkPowerUp()
    {
        Vector2 randomSpot = mainCamera.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        Instantiate(perkPowerup, randomSpot, Quaternion.identity);
        return randomSpot;
    }

}
