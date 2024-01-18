using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingOwnership
{
    Neutral,
    Player,
    Enemy
}

public class ResourceHub : MonoBehaviour
{
    public BuildingOwnership ownershipState;

    public ResourceType type;

    public GameObject HorseCart;
    public Transform HorseCartSpnPos;

    bool isRunning = false;
    public bool playerInSightRange;
    public LayerMask whatIsPlayer, whatIsEnemy;
    public bool farmSendCartToggle = true;

    private float sightRange = 30f;

    public float captureRange = 100;

    public int timeBetweenSends;
    private int sendAmount = 10;

    public float captureTick = 0;

    // flags
    public GameObject redFlag;
    public GameObject blueFlag;
    public ParticleSystem changeFlagFX;

    float captureRate = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
  
        ownershipState = BuildingOwnership.Enemy;

    }

    void Update()
    {

        // playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        // if (playerInSightRange && state == BuildingOwnership.Neutral) CapturedByPlayer();

        //if (state == BuildingOwnership.Player && farmSendCartToggle == true)
        //{
        //    if (isRunning == false)
        //    {
        //        StartCoroutine(SendCart());
        //    }

        //}

        if (captureTick >= 100)
        {
            if (ownershipState == BuildingOwnership.Enemy)
            {
                CapturedByPlayer();
            }
            else if (ownershipState == BuildingOwnership.Player)
            {
                CapturedByEnemy();
            }
        

        }


        switch (ownershipState)
        {
            case BuildingOwnership.Player:
                {
                    PlayerInControlUpdate();
                    redFlag.SetActive(false);
                    blueFlag.SetActive(true);
                    break;
                }

            case BuildingOwnership.Enemy:
                {
                    redFlag.SetActive(true);
                    blueFlag.SetActive(false);
                    break;
                }

        }
    }
    void PlayerInControlUpdate()
    {


            if (isRunning == false)
            {
                StartCoroutine(SendCart());
            }

        

    }

    public void CapturedByPlayer()
    {
        ownershipState = BuildingOwnership.Player;
        captureTick = 0;
        Debug.Log("Captured by player");
        changeFlagFX.Play();
        blueFlag.gameObject.SetActive(true);
        redFlag.gameObject.SetActive(false);

        StartCoroutine(SendCart());
    }

    public void CapturedByEnemy()
    {
        ownershipState = BuildingOwnership.Enemy;
        captureTick = 0;
        //  blueFlag.gameObject.SetActive(false);
        //  redFlag.gameObject.SetActive(true);

        Debug.Log("Captured by Enemy");
    }

    public void BeingCapturedTick()
    {
        Debug.Log("Being captured and rate increasing");
        // captureTick += captureRate * Time.deltaTime;
        captureTick += 10;
    }

    IEnumerator SendCart()
    {
        isRunning = true;

        yield return new WaitForSeconds(timeBetweenSends);
        CreateMineHorseCart();
        isRunning = false;

    }


    void CreateMineHorseCart()
    {
        GameObject go = Instantiate(HorseCart, HorseCartSpnPos.position, Quaternion.Euler(45, 0, 0));
        go.GetComponent<HorseCart>().FillResource(type, sendAmount);
        go.GetComponent<HorseCart>().resourceHub = this; 
    }



    //public void CaptureStart()
    //{

    //    StartCoroutine(BeingCaptured());


    //}

    //IEnumerator BeingCaptured()
    //{
    //    bool isBeingCaptured = true;


    //    yield return new WaitForSeconds(10f);

    //    isBeingCaptured = false;

    //}




}
