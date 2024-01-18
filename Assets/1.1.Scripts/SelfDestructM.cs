using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructM : MonoBehaviour
{

    public float DestroyAfter;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyAfter);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
