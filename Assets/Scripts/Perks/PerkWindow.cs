using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkWindow : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI type;
    public Image icon;

    public PerkButton perkButton;

    private List<Perk> availablePerks = new();
    private List<Perk> displayedPerks = new();

    // Start is called before the first frame update
    void Start()
    {
        perkButton = GetComponentInChildren<PerkButton>();
    }


    public void GetPerks()
    {
        // Clear the displayedPerks from last time
        displayedPerks = new();

        // Get perks which are available to be unlocked
        availablePerks = PerkManager.MyInstance.AvailablePerks();

        Debug.Log(availablePerks.Count);

        while (availablePerks.Count > 0 && displayedPerks.Count < 3)
        {
            int index = Random.Range(0, availablePerks.Count);
            displayedPerks.Add(availablePerks[index]);
            availablePerks.RemoveAt(index);
        }


    }

    public void DisplayIndividualPerk(Perk perk)
    {
        title.text = perk.MyName;
        description.text = perk.MyDescription;
        icon.sprite = perk.MyIcon;
        perkButton.MyPerk = perk;
    }
}
