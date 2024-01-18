using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{

    public GameObject[] powerupSelection;

    GameObject currentPowerUp;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        RandomPowerUp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RandomPowerUp()
    {
        index = Random.Range(0, powerupSelection.Length);
        currentPowerUp = powerupSelection[index];

       
    }


    public void DropPowerUp()
    {
  
        Instantiate(currentPowerUp, transform.position + new Vector3(0,2,0), transform.rotation);
    }
 

}
