using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Perk
{
    [SerializeField]
    private string name;

    [SerializeField]
    private string description;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float coolDown;

    [SerializeField]
    private float dropChance;

    [SerializeField]
    private bool isUnlocked;

    public string childBuffCode;

    [SerializeField]
    private float buffDuration;

    private Perk perk;

    public string MyChildBuffCode // the buff that is the previous tier
    {
        get
        {
            return childBuffCode;
        }
    }

    public float MyDropChance
    {
        get
        {
            return dropChance;
        }
    }

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    public string MyName
    {
        get
        {
            return name;
        }
    }

    public string MyDescription
    {
        get
        {
            return description;
        }
    }

    public Perk MyBuff
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


    public float MyBuffDuration
    {
        get
        {
            return buffDuration;
        }
    }


    public bool MyIsUnlocked
    {
        get { return isUnlocked; }
        set { isUnlocked = value; }
    }

    // setting the perkbutton
    public PerkButton perkButton { get; set; }

    private bool isAvailable = true;

    public string perkCode;

    public bool requiresAPrerequisite;

    public string requiredPerkCode;

    public bool MyIsAvailable
    {
        get { return isAvailable; }
        set { isAvailable = value; }
    }

    private bool hasBeenUsed = false;
    public bool MyHasBeenUsed
    {
        get { return hasBeenUsed; }
        set { hasBeenUsed = value; }
    }

    public void SetThisBuff()
    {
        MyBuff = this;
    }
}
