using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCommander : MonoBehaviour
{
    private static UnitCommander instance;

    public static UnitCommander MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UnitCommander>();
            }
            return instance;
        }
    }


    public LayerMask unitLayerMask;

    public bool hasUnitSelected = false;
    public LayerMask layerMask;
    public PlayerUnit selectedUnit;

    public GameObject selectionMarkerPrefab;
    public GameObject enemyMarker;
    public Camera cam;
    private UnitSelection unitSelection;


    UnitMover unitMover;
    private void Awake()
    {
        unitSelection = GetComponent<UnitSelection>();
        unitMover = GetComponent<UnitMover>();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessMobileTouch();
        ProcessPCTouch();



    }

    private void ProcessMobileTouch()
    {
        //foreach (Touch touch in Input.touches)
        //{
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        // Construct a ray from the current touch coordinates
        //        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //        if (Physics.Raycast(ray))
        //        {
        //            // Create a particle if hit
        //          //  Instantiate(particle, transform.position, transform.rotation);
        //        }
        //    }
        //}


        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    // Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
                    // hit.collider.GetComponent<MeshRenderer>().material.color = newColor;
                }
            }
        }


    }

    private void ProcessPCTouch()
    {


        if (Input.GetMouseButtonDown(0)) //   && !EventSystem.current.IsPointerOverGameObject()
        {

            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit)) //, unitLayerMask
            //{


            //    if (hit.collider != null)
            //    {


            //        if (hit.collider.tag == "PlayerUnit" && !hit.collider.GetComponent<PlayerUnit>().isDead)
            //        {

            //            if (selectedUnit)
            //            {
            //                // clear selected units if have one

            //          //      selectedUnit.unitUIPanel.gameObject.SetActive(false);
            //                selectedUnit.unitSelectionMarker.SetActive(false);
            //                selectedUnit = null;
            //                //      UIManager.MyInstance.selectedunitName.text = null;
            //                hasUnitSelected = false;

            //            }
            //            // assign selected unit
            //            selectedUnit = hit.collider.GetComponent<PlayerUnit>();
            //            hasUnitSelected = true;
            //        //    selectedUnit.unitUIPanel.gameObject.SetActive(true);
            //            selectedUnit.unitSelectionMarker.SetActive(true);

            //            //  selectedUnit.unitUIPanel.alpha = 1;

            //            //  UIManager.MyInstance.UpdateUnitProfile(selectedUnit);
            //            // UIManager.MyInstance.selectedunitName.text = selectedUnit.gameObject.name.ToString();




            //        }
            //    }


            //    if (hit.collider.tag == "Ground")
            //    {
            //        if (hasUnitSelected)
            //        {
            //            // selectedUnit.unitUIPanel.alpha = 0;
            //      //      selectedUnit.unitUIPanel.gameObject.SetActive(false);
            //            selectedUnit.unitSelectionMarker.SetActive(false);
            //            selectedUnit = null;
            //           // UIManager.MyInstance.selectedunitName.text = null;
            //            hasUnitSelected = false;
            //        }
            //    }






            //}
        }

        if (Input.GetMouseButtonDown(1) && unitSelection.HasUnitSelected())
        {
            unitSelection.RemoveNullUnitsFromSelection();

            // shoot a raycast from our mouse to see what we hit
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // cache the selected units into an array
            Unit[] selectedUnits = unitSelection.GetSelectedUnits();



            if (Physics.Raycast(ray, out hit))

            {
                if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("PowerUp"))
                {
                    UnitMoveToPosition(hit.point, selectedUnits);
                    //    Debug.Log("Mouse pos is " + hit.point);
                    //selectedUnit.GetComponent<PlayerUnitAI>().MoveToPosition(hit.point);
                    CreateSelectionMarker(hit.point, false, false);


                }

                if (hit.collider.CompareTag("Mine"))
                {
                    Debug.Log("Move to Gold Mine and the collider is " + hit.collider.gameObject.transform.position);
                    UnitsGoToResource(hit.collider.gameObject, selectedUnits);
                    //  CreateSelectionMarker(hit.collider.transform.position, true);

                    //      UnitMoveToPosition(hit.point, selectedUnits);

                    //selectedUnit.GetComponent<PlayerUnitAI>().MoveToPosition(hit.point);
                    CreateSelectionMarker(hit.point, false, false);


                }
                if (hit.collider.CompareTag("Enemy"))
                {
                    // create selection marker on targeted enemy
                    EnemyUnit enemy = hit.collider.gameObject.GetComponent<EnemyUnit>();
                    CreateSelectionMarkerEnemy(enemy);
                    UnitsAttackEnemy(enemy, selectedUnits);

                    //// send the selected unit to the enemry target
                    //selectedUnit.GetComponent<PlayerUnitAI>().SetState(PlayerUnitState.MoveToEnemyOrder);
                    //selectedUnit.GetComponent<PlayerUnitAI>().curTarget = enemy;

                }
                if (hit.collider.CompareTag("PowerUp"))
                {
                    // create selection marker on targeted enemy
                    GameObject powerUp = hit.collider.gameObject;
                    //  PowerUpScript powerUp = hit.collider.gameObject.GetComponent<EnemyUnit>();

                    CreateSelectionMarker(powerUp.transform.position, false, false);

                    selectedUnit.GetComponent<PlayerUnitAI>().MoveToPosition(powerUp.transform.position);


                }



            }

        }

    }

    public void ClearSelectedUnit()
    {
        unitSelection.ClearSelectedUnit();
        //   selectedUnit.unitUICircle.gameObject.SetActive(false);
        //   selectedUnit.CloseTalentTree();
        selectedUnit.unitSelectionMarker.SetActive(false);
        selectedUnit = null;
        hasUnitSelected = false;
        //   UIManager.MyInstance.ClearCirclePanel();
    }

    public void ClosePlayerBasePanel()
    {
        //playerBaseCanvas.alpha = 0;
        //playerBaseCanvas.blocksRaycasts = false;
    }
    public void CloseGateUI()
    {
        //gateCanvasGroup.alpha = 0;
        //gateCanvasGroup.blocksRaycasts = false;
    }

    // creates a new selection marker at the given position
    void CreateSelectionMarker(Vector3 pos, bool large, bool enemy)
    {

        if (large)      // if it's large marker then multply the scale by three. Used for resource trees
        {

        }
        //else if (enemy)
        //{
        //    GameObject marker = Instantiate(enemyMarker, new Vector3(pos.x, 0.1f, pos.z), Quaternion.Euler(90, 0, 0));

        //}
        else
        {
            GameObject marker = Instantiate(selectionMarkerPrefab, new Vector3(pos.x, pos.y + 0.1f, pos.z), Quaternion.Euler(90, 0, 0));
        }
    }

    // creates a new selection marker at the given position
    void CreateSelectionMarkerEnemy(EnemyUnit enemy)
    {

        GameObject marker = Instantiate(enemyMarker, new Vector3(enemy.transform.position.x, 0.05f, enemy.transform.position.z), Quaternion.Euler(90, 0, 0));
        // GameObject marker = Instantiate(enemyMarker, enemy.transform.position, Quaternion.Euler(90, 0, 0));
        marker.transform.SetParent(enemy.transform);
    }


    void UnitMoveToPosition(Vector3 movePos, Unit[] units)
    {
        Vector3[] destinations = unitMover.GetUnitGroupDestinations(movePos, units.Length, 2);


        for (int x = 0; x < units.Length; x++)
        {
            units[x].GetComponent<PlayerUnitAI>().MoveToPosition(destinations[x]);
            Debug.Log("Move toPOSITION");
        }

    }

    // attack enemy player
    void UnitsAttackEnemy(Unit target, Unit[] units)
    {
        for (int x = 0; x < units.Length; x++)
        {

            units[x].GetComponent<PlayerUnitAI>().SetState(PlayerUnitState.MoveToEnemyOrder);
            units[x].GetComponent<PlayerUnitAI>().curTarget = (EnemyUnit)target;

        }
    }
    void UnitsGoToResource(GameObject resource, Unit[] units)
    {
        Debug.Log("Move to Gold Mine2 and the resource pos it " + resource.transform.position);
        if (units.Length == 1)
        {
            // units[0].GetComponent<PlayerUnitAI>().OrderUnitsToResource(resource, unitMover.GetUnitDestinationAroundResource(resource.transform.position));
        }
        else
        {
            Vector3[] destinations = unitMover.GetunitGroupDestinationsAroundResource(resource.transform.position, units.Length);

            for (int x = 0; x < units.Length; x++)
            {
                //   units[x].GetComponent<PlayerUnitAI>().OrderUnitsToResource(resource, destinations[x]);
            }

        }

    }
}