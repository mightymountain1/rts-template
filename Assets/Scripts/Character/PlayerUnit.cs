using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : Unit
{
    // UI
    public GameObject unitSelectionMarker;
    public GameObject unitSelectionMarkerGreen;
    public GameObject unitSelectionMarkerWhite;

    // Refs
    PlayerUnitAI playerUnitAI;

    // HealthBar
    [SerializeField]
    private Stat xpStat;

    [SerializeField]
    protected Stat healthUIPanel;


    public Stat MyXP
    {
        get
        {
            return xpStat;
        }
        set
        {
            xpStat = value;
        }

    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerUnitAI = GetComponent<PlayerUnitAI>();
    }

    // SELECTION MARKERS
    public void ShowSelectionMarker()
    {
        unitSelectionMarker.SetActive(true);
    }


    public void HideSelectionMarker()
    {
        unitSelectionMarker.SetActive(false);
    }

    // toggles the selection ring around the units feet
    public void ToggleSelectionMarker(bool selected)
    {
        if (unitSelectionMarker != null)
            unitSelectionMarker.SetActive(selected);

        UpdateSlectionMarkerStatus();
    }

    public void UpdateSlectionMarkerStatus()
    {
        if (this != null)
        {
            if (isInSquad)
            {
                unitSelectionMarkerGreen.SetActive(true);
                unitSelectionMarkerWhite.SetActive(false);
            }
            else
            {
                unitSelectionMarkerGreen.SetActive(false);
                unitSelectionMarkerWhite.SetActive(true);
            }
        }
       
    }


    // Take Damage
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health.MyCurrentValue <= 0 && !playerUnitAI.isDead)
        {
            isDead = true;

            if (isInSquad && !isSquadLeader)
            {
                UnitCommander.MyInstance.LeaveCurrentSquad(this);
            } else if(isSquadLeader && isInSquad)
            {
                squadGroup.RemoveLeader(this);

            }

            GameManager.MyInstance.playerUnits.Remove(this); // remove from GameManager list
            Destroy(gameObject);
        }
    }
}





