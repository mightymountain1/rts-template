using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PerkManager : MonoBehaviour
{
    private static PerkManager instance;

    public static PerkManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PerkManager>();
            }
            return instance;
        }
    }

    // Initialize some lists we'll be using later
    public List<Perk> consumedPerk = new();
    public List<Perk> availablePerks = new();

    public List<Perk> tmpPerkList = new();
    public List<Perk> displayedPerks = new();

    public CanvasGroup perkWindow;
    //public CanvasGroup placementWindow;
    //public CanvasGroup mainActionBars;


    Perk tmpBuff;
    public Perk selectedPerk;

    public PerkWindow perkWindow1;
    public PerkWindow perkWindow2;
  //  public PerkWindow perkWindow3;


    [Header("Unlocks/collectables")]
    [SerializeField]
    private GameObject[] collectables;


    // Buffs
    [SerializeField]
    private Perk[] perks;

    public Perk[] MyPerks
    {
        get
        {
            return perks;
        }
    }



    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {


    }

    private Vector3 velocity;

    public void PauseGame()
    {

     //   velocity = Player.MyInstance.velocity;

        Time.timeScale = 0;

    }

    // Open the Perk Window - called from the perk pickup
    public void DisplayPerkWindow()
    {


        tmpPerkList = AvailablePerks();

        perkWindow.alpha = 1.0f;
        perkWindow.blocksRaycasts = true;

        while (tmpPerkList.Count > 0 && displayedPerks.Count < 2)
        {
            int index = UnityEngine.Random.Range(0, tmpPerkList.Count);

            Perk chosenBuff = tmpPerkList[index];

            displayedPerks.Add(tmpPerkList[index]);
            tmpPerkList.RemoveAt(index);
        }

        perkWindow1.DisplayIndividualPerk(displayedPerks[0]);
        perkWindow2.DisplayIndividualPerk(displayedPerks[1]);
    }


    public List<Perk> AvailablePerks()
    {
        // Clear the list
        availablePerks = new();

        // Repopulate it
        foreach (Perk perk in MyPerks)
        {
            if (perk.MyIsUnlocked)
            {
                if (!IsPerkAllreadyConsumed(perk.perkCode))
                {
                    if (perk.requiresAPrerequisite)
                    {
                        if (IsRequiredPerkAlreadyChosen(perk))
                        {
                            availablePerks.Add(perk);
                        }
                    }
                    else
                    {
                        availablePerks.Add(perk);
                    }
                }
            }
        }
        return availablePerks;
    }

    public void AddPerk(Perk perk)
    {
        ConsumePerk(perk);
        Time.timeScale = 1;

    }

    // This will be called when a player chooses a perk  in the perk choice windoow
    public void ConsumePerk(Perk consumedPerk)
    {
        this.consumedPerk.Add(consumedPerk);

        // BuffManager.MyInstance.AddBuffIconDisplay(consumedPerk);

        ClearPerkWindow();
    }




    public void ClearPerkWindow()
    {
        perkWindow.alpha = 0.0f;
        perkWindow.blocksRaycasts = false;
        displayedPerks.Clear();
        tmpPerkList.Clear();
        tmpBuff = null;
    }



    // This function determines if a given perk should be shown to the player
    private bool IsRequiredPerkAlreadyChosen(Perk perk)
    {

        //If a required perk is missing, then this perk isn't available
        foreach (Perk perkInConsumedList in consumedPerk)
        {
            if (perkInConsumedList.perkCode == perk.requiredPerkCode)
            {
                return true;
            }

        }
        return false;

    }


    // Returns whether or not the player already has a given perk
    public bool IsPerkAllreadyConsumed(string perkCode)
    {
        foreach (Perk potentialPerk in consumedPerk)
        {
            if (potentialPerk.perkCode == perkCode) return true;


        }
        return false;
    }



    // returns the buff using the buff name from a given string
    public Perk ReturnPerk(string perkToReturn)
    {
        Perk tmpPerk = Array.Find(MyPerks, x => x.MyName == perkToReturn);
        return tmpPerk;
    }

    public void UnlockPerk(string perk)
    {
        Perk tmpPerk = ReturnPerk(perk);

        // enable it in the unlock list in main Screens
        foreach (GameObject spell in collectables)
        {
            if (spell.name == tmpPerk.MyName)
            {
                spell.SetActive(true);
            }
        }

        // enable it in perk choises
        tmpPerk.MyIsUnlocked = true;
    }


}
