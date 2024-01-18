using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [Header ("Hearts")]
    public GameObject heartPrefab;
    public Transform heartContainer;
    public int numberOfHearts = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHeart()
    {
        numberOfHearts++;
        Instantiate(heartPrefab, heartContainer);
    }

    public void RemoveHeart()
    {
        if (numberOfHearts > 0)
        {
            Destroy(heartContainer.GetChild(0).gameObject);
            numberOfHearts--;
        }
    }
}
