using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
public class PerkButton : MonoBehaviour
{
    public Image MyIcon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }

    }

    [SerializeField]
    private Image icon;

    [SerializeField]
    private string perkName;

    public string MyPerkName
    {
        get
        {
            return perkName;
        }

    }

    public Perk perk;

    public Perk MyPerk
    {
        get
        {
            return perk;
        }
        set
        {
            perk = value;
        }

    }

    public Button MyButton { get; private set; }

    void Awake()
    {
        MyButton = GetComponent<Button>();
    }
   
    public void PerkClick()
    {
        PerkManager.MyInstance.AddPerk(MyPerk);
    }
}
