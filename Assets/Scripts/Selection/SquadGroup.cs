using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadGroup : MonoBehaviour
{
    public List<Unit> unitsInSquad = new List<Unit>();

    public int squadSize;
    private Camera cam;

    public LayerMask layerMask;

    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        squadSize = unitsInSquad.Count;
    }

    public void CreateSquad(Unit[] units, Unit leader)
    {
        for (int x = 0; x < units.Length; x++)
        {
            unitsInSquad.Add(units[x]);
            units[x].squadGroup = this;
        }

        if (!unitsInSquad.Contains(leader))
            unitsInSquad.Add(leader);
      
        leader.squadGroup = this;
        leader.isSquadLeader = true;
    }

    public void ClearSquad()
    {  
        unitsInSquad = new List<Unit>();
    }

    public void UnitAddToSquad(Unit unit)
    {
        unitsInSquad.Add(unit);
        unit.isInSquad = true;
        unit.squadGroup = this;
    }

    public void UnitLeaveSquad(Unit unit)
    {
        unitsInSquad.Remove(unit);
        unit.isInSquad = false;
        unit.squadGroup = null;
    }

    public void RemoveLeader(Unit leader)
    {
        UnitCommander.MyInstance.RemoveFromSquadList(leader);
        leader.gameObject.transform.SetParent(SpawnManager.MyInstance.playerUnitParent);
        UnitLeaveSquad(leader);
        UnitCommander.MyInstance.DisbandSquad(this);
    }

    public void LeaderDiesRemoveUnitsFromSquad()
    {
        if (unitsInSquad.Count > 0)
        {
            foreach (Unit unit in unitsInSquad)
            {
                if (!unit.isSquadLeader)
                {
                     UnitCommander.MyInstance.LeaveCurrentSquad(unit.GetComponent<PlayerUnit>());

                }             
            }
        }   
    }
}
