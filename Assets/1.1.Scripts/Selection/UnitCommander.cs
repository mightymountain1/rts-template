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

    // squad stuff
    public static List<Unit> selectedSquad = new List<Unit>();
    public Transform squadParent;

    // References
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
        ProcessPCTouch();
    }


    private void ProcessPCTouch()
    {
        if (Input.GetMouseButtonDown(0)) //   && !EventSystem.current.IsPointerOverGameObject()
        {
            ClearSquad();
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) //, unitLayerMask
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "PlayerUnit" && !hit.collider.GetComponent<PlayerUnit>().isDead)
                    {
                        if (selectedUnit)
                        {
                            // clear selected units if have one
                            selectedUnit.ToggleSelectionMarker(false);
                            //selectedUnit.unitSelectionMarker.SetActive(false);
                            selectedUnit = null;
                            hasUnitSelected = false;

                        }
                        // assign selected unit
                        selectedUnit = hit.collider.GetComponent<PlayerUnit>();
                        hasUnitSelected = true;
                        selectedUnit.ToggleSelectionMarker(true);
                       // selectedUnit.unitSelectionMarker.SetActive(true);
                    }
                }


                if (hit.collider.tag == "Ground")
                {
                    ClearSquad();

                    if (hasUnitSelected)
                    {
                        //selectedUnit.unitSelectionMarker.SetActive(false);
                        selectedUnit.ToggleSelectionMarker(false);
                        selectedUnit = null;
                        hasUnitSelected = false;
                    }

                }
            }
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
                    CreateSelectionMarker(hit.point, false, false);
             
                        foreach (Unit unit in selectedUnits)
                        {
                            if (unit.isInSquad)
                            {
                                // make the units leave squad if selected units doesnt include the squad leader
                                if (!CheckIfASquadLeaderIsInSelection(selectedUnits))
                                {
                                    Debug.Log("unit left its squad");
                                    LeaveCurrentSquad(unit.GetComponent<PlayerUnit>());
                                }
                            }
                        }
                }

                //if (hit.collider.CompareTag("Mine"))
                //{
                //    Debug.Log("Move to Gold Mine and the collider is " + hit.collider.gameObject.transform.position);
                //    UnitsGoToResource(hit.collider.gameObject, selectedUnits);
                //    //  CreateSelectionMarker(hit.collider.transform.position, true);

                //    //      UnitMoveToPosition(hit.point, selectedUnits);

                //    //selectedUnit.GetComponent<PlayerUnitAI>().MoveToPosition(hit.point);
                //    CreateSelectionMarker(hit.point, false, false);


                //}
                if (hit.collider.CompareTag("EnemyUnit"))
                {
                    // send the selected unit to the enemry target
                    EnemyUnit enemy = hit.collider.gameObject.GetComponent<EnemyUnit>();
                    CreateSelectionMarkerEnemy(enemy);    
                    UnitsAttackEnemy(enemy, selectedUnits);

                }
                //if (hit.collider.CompareTag("PowerUp"))
                //{
                //    // create selection marker on targeted enemy
                //    GameObject powerUp = hit.collider.gameObject;
                //    //  PowerUpScript powerUp = hit.collider.gameObject.GetComponent<EnemyUnit>();

                //    CreateSelectionMarker(powerUp.transform.position, false, false);

                //    selectedUnit.GetComponent<PlayerUnitAI>().MoveToPosition(powerUp.transform.position);


                //}
                // If right click on a squad Leader
                if (hit.collider.GetComponent<SquadLeader>())
                {
                    Unit leader = hit.collider.gameObject.GetComponent<Unit>();
                     //SquadGroup squadGroup = hit.collider.gameObject.GetComponentInChildren<SquadGroup>();

                    foreach (Unit unit in selectedUnits)
                    {
                        if (unit.isInSquad && !unit.isSquadLeader)
                        {
                            LeaveCurrentSquad(unit.GetComponent<PlayerUnit>());
                        }
                    }
                    if (leader.isSquadLeader && leader.isInSquad)
                    {
                        // add units to the existing squad
                        unitSelection.RemoveNullUnitsFromSelection();
                        AddToExistingSquad(selectedUnits, leader.squadGroup);
                        leader.GetComponent<PlayerUnit>().UpdateSlectionMarkerStatus();
                    } else
                    {
                        unitSelection.RemoveNullUnitsFromSelection();
                        CreateSquadron(selectedUnits, leader);
                    }

                }
            }
        }

        // did we press down our right mouse button and do we have a squad selected selected?
        if (Input.GetMouseButtonDown(1) && HasSquadSelected())
        {
            // shoot a raycast from our mouse to see what we hit
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 900, layerMask))
            {
                // are we click on the ground?
                if (hit.collider.CompareTag("Ground"))
                {
                    UnitMoveToPosition(hit.point, GetSelectedSquadUnits());
                  //  CreateSelectionMarker(hit.point, false);  // to do
                }
            }
        }
    }


    // add to existing squad
    public void AddToExistingSquad(Unit[] units, SquadGroup squadGroup)
    {
        for (int x = 0; x < units.Length; x++)
        {
            units[x].gameObject.transform.parent = squadGroup.transform;
            selectedSquad.Add(units[x]);
            squadGroup.UnitAddToSquad(units[x]);
            units[x].GetComponent<PlayerUnit>().UpdateSlectionMarkerStatus();
        }
    }

    public void LeaveCurrentSquad(PlayerUnit playerUnit)
    {
            playerUnit.squadGroup.UnitLeaveSquad(playerUnit);
            playerUnit.gameObject.transform.SetParent(SpawnManager.MyInstance.playerUnitParent);
            selectedSquad.Remove(playerUnit);
            playerUnit.UpdateSlectionMarkerStatus(); // update selection marker after leaving squad
    }

    public void DisbandSquad(SquadGroup squadGroup)
    {
        foreach (Unit unit in squadGroup.unitsInSquad)
        {  
            selectedSquad.Remove(unit);
            unit.gameObject.transform.SetParent(SpawnManager.MyInstance.playerUnitParent);
            unit.isInSquad = false;
            unit.GetComponent<PlayerUnit>().UpdateSlectionMarkerStatus();
        }
        Destroy(squadGroup.gameObject);
    }

    public void RemoveFromSquadList(Unit unit)
    {
        selectedSquad.Remove(unit);
    }


    // is there a squad Leader in this selection of unit
    public bool CheckIfASquadLeaderIsInSelection(Unit[] selectedUnits)
    {
        foreach (Unit unit in selectedUnits)
        {
            if (unit.isSquadLeader)
            {
                return true;
            }

        }
        return false;
    }

    public void ClearSelectedUnit()
    {
        unitSelection.ClearSelectedUnit();
        selectedUnit.unitSelectionMarker.SetActive(false);
        selectedUnit = null;
        hasUnitSelected = false;
    }


    // create Squadron
    void CreateSquadron(Unit[] units, Unit leader)
    {
        SquadGroup oldSquadToDelete = leader.squadGroup;
        GameObject newSquadGO = new GameObject();
        newSquadGO.name = "Squad";
        SquadGroup newSquadGroup = newSquadGO.AddComponent<SquadGroup>();
        Transform newSquadTransform = newSquadGO.transform;
        newSquadTransform.SetParent(squadParent);

        for (int x = 0; x < units.Length; x++)
        {
            units[x].gameObject.transform.parent = newSquadTransform;
            selectedSquad.Add(units[x]);
            units[x].isInSquad = true;
            units[x].GetComponent<PlayerUnit>().UpdateSlectionMarkerStatus();
        }
      
        leader.gameObject.transform.parent = newSquadTransform;   // add the leader to the new squad Transform
        selectedSquad.Add(leader);
        leader.isInSquad = true;
        leader.GetComponent<PlayerUnit>().UpdateSlectionMarkerStatus();
        newSquadGroup.CreateSquad(units, leader);

      //  Destroy(oldSquadToDelete.gameObject);



    }


    public Unit[] GetSelectedSquadUnits()
    {
        return selectedSquad.ToArray();
    }

    public bool HasSquadSelected()
    {
        return selectedSquad.Count > 0 ? true : false;
    }

    public void ClearSquad()
    {

        selectedSquad = new List<Unit>();
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