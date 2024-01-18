using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }


    public int curHealth = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddHealth()
    {
        curHealth++;
        UIManager.MyInstance.AddHeart();
    }

    public void ReduceHealth()
    {
        curHealth--;
        UIManager.MyInstance.RemoveHeart();
    }
}
