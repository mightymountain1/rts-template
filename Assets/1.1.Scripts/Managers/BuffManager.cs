using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private static BuffManager instance;

    public static BuffManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BuffManager>();
            }
            return instance;
        }
    }


    [Header("Bools")]
    public bool hasDoubleShot;
    public bool hasMegaBoost;

    [Header("Mega Boost")]
    public GameObject shield;
    public bool hasShieldUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPassive(Perk perk)
    {

       

        switch (perk.MyName)
        {
            case "Double Shot":
                {
                  hasDoubleShot= true;
                    break;
                }
            case "Mega Boost":
                {
                    hasMegaBoost = true;
                    break;
                }

        }
    }

    public void ClearBuffs()
    {
        hasDoubleShot = false;
        hasMegaBoost = false;
    }


}
