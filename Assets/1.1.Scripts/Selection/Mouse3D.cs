using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{
    private static Mouse3D instance;

    public static Mouse3D MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Mouse3D>();
            }
            return instance;
        }
    }

    [SerializeField] private Camera topCamera;
    [SerializeField] private LayerMask layerMask;


    public Vector3 mousePos;

    public Vector3 MyMousePos
    {
        get
        {
            return mousePos;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = topCamera.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layerMask))
        {
            mousePos = raycastHit.point;

        }
    }
}
