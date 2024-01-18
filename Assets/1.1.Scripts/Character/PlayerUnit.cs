using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : Unit
{


    // UI
    public GameObject unitSelectionMarker;



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

    //
    //  public GameObject readyToLevelUpFX;

    //  public CanvasGroup unitUIPanel;

    //  public Text levelTextPanel;
    //   public Text levelTextHB;


    // public GameObject berserkerFXFeet;






    // buff stuff
    //   float startingCritChance;
    //   int startingAgility;

    // Vector3 originalScale;
    //  Vector3 destinationScale = new Vector3(1.4f, 1.4f, 1.4f);

    //  public bool isInvisible;
    //  public bool hasCritBuff;
    //  public bool hasBerserkerBuff;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerUnitAI = GetComponent<PlayerUnitAI>();
      //  UpdateLevel();
       // xpStat.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.1f)));
      //  healthUIPanel.Initialize(initHealth, initHealth);

    }

    // Update is called once per frame
    void Update()
    {
        
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

        //if (UnitSelection.selectedUnits.Count == 1)
        //{
        //    unitRangeMarker.SetActive(true);
        //}
        //else
        //{
        //    unitRangeMarker.SetActive(false);
        //}
    }

    // Take Damage simple
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        //attacker.GetComponent<PlayerUnit>().GainXP(1);

        if (health.MyCurrentValue <= 0 && !playerUnitAI.isDead)
        {


            //  GameManager.MyInstance.GainXP(50);
            //    attacker.GetComponent<PlayerUnit>().GainXP(100);
            isDead = true;
            //  enemayUnitAi.Die();
            GameManager.MyInstance.playerUnits.Remove(this);
            Destroy(gameObject);


        }

    }


    //public override void TakeDamage(float damage, Unit attacker, bool isHitACrit)
    //{
    //    base.TakeDamage(damage, attacker, isHitACrit);

    //    healthUIPanel.MyCurrentValue -= damage; // second healthbar

    //    if (health.MyCurrentValue <= 0 && !playerUnitAI.isDead)
    //    {
    //        playerUnitAI.Die();
    //        healthBarPaenl.gameObject.SetActive(false);
    //    }

    //}

    //BUFFS



    // public override void IncreaseHealth(float healthIncrease)
    // {
    //     base.IncreaseHealth(healthIncrease);

    //     healthUIPanel.MyCurrentValue += healthIncrease; // second healthbar

    //     Debug.Log(this.name + "healed by this much " + healthIncrease);

    // }

    // public void IncreaseCrit()
    // {
    //  //   StartCoroutine(IncreaseCritCR());

    //     if (hasCritBuff)
    //     {

    //         StopCoroutine(IncreaseCritCR());
    //         critChance = startingCritChance;
    //         hasCritBuff = false;
    //         StartCoroutine(IncreaseCritCR());
    //     } else
    //     {
    //         StartCoroutine(IncreaseCritCR());
    //     }
    // }

    // public IEnumerator IncreaseCritCR()
    // {
    ////     STManager.MyInstance.CreateText(this, "Crit Master", STTYPE.BUFFNAME, false);
    //     startingCritChance = critChance;
    //     hasCritBuff = true;
    //     critChance += 0.25f;
    //     CalculateStats();
    //     yield return new WaitForSeconds(20.5f);

    //     critChance = startingCritChance;
    //     hasCritBuff = false;
    //     CalculateStats();

    // }

    // public void Berserker()
    // {
    //     //   StartCoroutine(IncreaseCritCR());
    //  //   STManager.MyInstance.CreateText(this, "Berserker", STTYPE.BUFFNAME, false);
    //     if (hasBerserkerBuff)
    //     {

    //         StopCoroutine(BerserkerCR());
    //         critChance = startingCritChance;
    //         agility = startingAgility;
    //         hasBerserkerBuff = false;
    //         StartCoroutine(BerserkerCR());
    //     }
    //     else
    //     {
    //         StartCoroutine(BerserkerCR());
    //     }
    // }

    //public IEnumerator BerserkerCR()
    //{

    //    if (!hasBerserkerBuff)
    //    {
    //        StartCoroutine(ScaleUpOverTime(3));
    //    }
    //    berserkerFXFeet.SetActive(true);
    //         startingCritChance = critChance;
    //    startingAgility = agility;
    //    hasBerserkerBuff = true;
    //    critChance += 0.45f;
    //    agility += 200;
    //    CalculateStats();
    //    yield return new WaitForSeconds(20.5f);

    //    critChance = startingCritChance;
    //    agility = startingAgility;
    //    hasBerserkerBuff = false;
    //    berserkerFXFeet.SetActive(false);
    //    StartCoroutine(DecreaseScaleOverTime(3));
    //    CalculateStats();

    //}

    //IEnumerator ScaleUpOverTime(float time)
    //{
    //    originalScale = this.transform.localScale;


    //    float currentTime = 0.0f;

    //    do
    //    {
    //        this.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
    //        currentTime += Time.deltaTime;
    //        yield return null;
    //    } while (currentTime <= time);


    //}

    //IEnumerator DecreaseScaleOverTime(float time)
    //{
    //    Vector3 enlargedScale = this.transform.localScale;

    //    float currentTime = 0.0f;

    //    do
    //    {
    //        this.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, currentTime / time);
    //        currentTime += Time.deltaTime;
    //        yield return null;
    //    } while (currentTime <= time);


    //}





    //public void GainXP(int xp)
    //{

    //    xpStat.MyCurrentValue += xp;



    //    if (xpStat.MyCurrentValue >= xpStat.MyMaxValue)
    //    {
    //        readyToLevelUpFX.SetActive(true);
    //        Upgrade();
    //    }

    //}



    //public void Upgrade()
    //{
    //    MyLevel++;
    //    health.Initialize(initHealth, initHealth);
    //    healthUIPanel.Initialize(initHealth, initHealth);
    ////    GameManager.MyInstance.DeductXP(50);
    //    readyToLevelUpFX.SetActive(false);
    //    xpStat.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
    //    xpStat.MyMaxValue = Mathf.Floor(xpStat.MyMaxValue);
    //    xpStat.MyCurrentValue = 0 + xpStat.MyOverflow;
    //    xpStat.MyCurrentValue = xpStat.MyOverflow;
    //    xpStat.Reset();
    //    CalculateStats();
    //    UpdateLevel();
    //}



    //public void UpdateLevel()
    //{
    //    levelTextPanel.text = "Level " + MyLevel.ToString();
    //    levelTextHB.text = MyLevel.ToString();
    //}

}





