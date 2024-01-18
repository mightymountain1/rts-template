using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PowerupType
{
    health,
    shields,
    crit,
    attackSpeed,
    damage,
    berserker
}

public class PowerUpScript : MonoBehaviour
{
    [Header("States")]
    public PowerupType powerupType;


    [Header("Stats")]
    public int healthAmount;


    [Header ("Unit Buff FX")]
    public GameObject unitHealFX;

    // References
    Unit tmpUnit;
    PlayerUnit tmpPlayerUnit;


    // Start is called before the first frame update
    void Start()
    {

   
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerUnit")
        {
            if (powerupType == PowerupType.health) BuffHealth(other);
            if (powerupType == PowerupType.crit) IncreaseCrit(other);
            if (powerupType == PowerupType.berserker) Berserker(other);
            //    if (powerupType == PowerupType.damage) BuffHealth();
            //   if (powerupType == PowerupType.shields) ShieldsBuff();
            //    if (powerupType == PowerupType.attackSpeed) IncreaseAttackSpeed();


        }
    }


    public void BuffHealth(Collider other)
    {
               
        tmpUnit = other.GetComponent<Unit>();

      //  tmpUnit.IncreaseHealth(healthAmount);
        AudioManager.MyInstance.PlayOneShot("HealBuff1", 0.3f, 0);
        Instantiate(unitHealFX, tmpUnit.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject, 0.1f);
       
    }

    public void IncreaseCrit(Collider other)
    {


        AudioManager.MyInstance.PlayOneShot("CritBuff", 0.3f, 0);
        tmpPlayerUnit = other.GetComponent<PlayerUnit>();

       // tmpPlayerUnit.IncreaseCrit();

        Instantiate(unitHealFX, tmpPlayerUnit.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject, 0.1f);

    }

    public void Berserker(Collider other)
    {
        AudioManager.MyInstance.PlayOneShot("Berserker", 0.3f, 0);

        tmpPlayerUnit = other.GetComponent<PlayerUnit>();

    //    tmpPlayerUnit.Berserker();

        Instantiate(unitHealFX, tmpPlayerUnit.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject, 0.1f);

    }


    public void ShieldsBuff()
    {

      //  _playerBuffs.PowerUpShields();
        Destroy(gameObject, 0.1f);
 
    }

    public void IncreaseAttackSpeed()
    {

    }

}
