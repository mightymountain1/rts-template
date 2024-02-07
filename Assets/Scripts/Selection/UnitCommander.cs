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


    // Update is called once per frame
    void Update()
    {
        ProcessCommander();
    }


    private void ProcessCommander()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClearSquad();

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "PlayerUnit" && !hit.collider.GetComponent<PlayerUnit>().isDead)
                    {
                        if (selectedUnit)
                        {
                            // clear selected units if have one
                            selectedUnit.ToggleSelectionMarker(false);
                            selectedUnit = null;
                            hasUnitSelected = false;

                        }

                        // assign selected unit
                        selectedUnit = hit.collider.GetComponent<PlayerUnit>();
                        hasUnitSelected = true;
                        selectedUnit.ToggleSelectionMarker(true);
                    }
                }


                if (hit.collider.tag == "Ground")
                {
                    ClearSquad();

                    if (hasUnitSelected)
                    {
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
                                    LeaveCurrentSquad(unit.GetComponent<PlayerUnit>());
                                }
                            }
                        }
                }

                if (hit.collider.CompareTag("EnemyUnit"))
                {
                    // send the selected unit to the enemry target
                    EnemyUnit enemy = hit.collider.gameObject.GetComponent<EnemyUnit>();
                    CreateSelectionMarkerEnemy(enemy);    
                    UnitsAttackEnemy(enemy, selectedUnits);

                }

                // If right click on a squad Leader
                if (hit.collider.GetComponent<SquadLeader>())
                {
                    Unit leader = hit.collider.gameObject.GetComponent<Unit>();

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


    // creates a new selection marker at the given position
    void CreateSelectionMarker(Vector3 pos, bool large, bool enemy)
    {
        GameObject marker = Instantiate(selectionMarkerPrefab, new Vector3(pos.x, pos.y + 0.1f, pos.z), Quaternion.Euler(90, 0, 0)); 
    }

    // creates a new selection marker at the given position
    void CreateSelectionMarkerEnemy(EnemyUnit enemy)
    {
        GameObject marker = Instantiate(enemyMarker, new Vector3(enemy.transform.position.x, 0.05f, enemy.transform.position.z), Quaternion.Euler(90, 0, 0));
        marker.transform.SetParent(enemy.transform);
    }


    void UnitMoveToPosition(Vector3 movePos, Unit[] units)
    {
        Vector3[] destinations = unitMover.GetUnitGroupDestinations(movePos, units.Length, 2.5f);

        for (int x = 0; x < units.Length; x++)
        {
            units[x].GetComponent<PlayerUnitAI>().MoveToPosition(destinations[x]);
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
}