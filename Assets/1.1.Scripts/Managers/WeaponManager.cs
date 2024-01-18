using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class WeaponManager : MonoBehaviour
{
    [Header("General settings")]
    public float weaponRange;
    public LayerMask enemyLayer;
    public GameObject weapon;
    public float timeBetweenShots;
    public float checKRate;

    [Header ("Bullet settings")]
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    public Transform firePoint;
    public float fireForce = 20f;

    public Enemy nearestTarget;
    public Enemy potentialTarget;
    public Enemy curTarget;

    bool canfire = true;

    public float rotationModifier;
    public float rotateSpeed;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FindEnemy", 0, checKRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (curTarget != null)
        {
            LookAtClosestEnemy(curTarget);
        }
    }

    void FindEnemy()
    {
         potentialTarget = CheckForNearbyEnemies();

        if (potentialTarget != null)
        {
            TargetEnemy(potentialTarget);
        } 
    }

    void TargetEnemy(Enemy target)
    {
        if (!target.isDead)
        {
            curTarget = target;
            AttackTarget(curTarget);
          
        }

    }

    public void AttackTarget(Enemy target)
    {
       if (canfire && curTarget)
       {
            Fire(target);
       }
  
    }

    IEnumerator Cooldown()
    {
        canfire = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canfire = true;  
    }


    public void Fire(Enemy enemy)
    {
        LookAtClosestEnemy(enemy);
        AudioManager.MyInstance.PlayOneShot("Shoot", 1, 0);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

        if (BuffManager.MyInstance.hasDoubleShot)
        {
            AudioManager.MyInstance.PlayOneShot("Shoot", 1, 0);
            GameObject bullet2 = Instantiate(bulletPrefab2, firePoint.position, firePoint.rotation);
            bullet2.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        }

        StartCoroutine(Cooldown());
       
    }



    public void LookAtClosestEnemy(Enemy target)
    {
        Vector3 vectorToTarget = target.transform.position - weapon.transform.position;
        float angle2 = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle2, Vector3.forward);
        weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation, q, Time.deltaTime * rotateSpeed);
    }


    public int angle;
    public Collider2D[] hitColliders;
    Enemy CheckForNearbyEnemies()
    {

        hitColliders = Physics2D.OverlapCircleAll(player.transform.position, weaponRange, enemyLayer);
        float minimumDistance = Mathf.Infinity;
        foreach (Collider2D collider in hitColliders)
        {

            Vector2 directionToTarget = (collider.transform.position - player.transform.position).normalized;

            if (Vector2.Angle(player.transform.up, directionToTarget) < angle /2)
            {
                float distance = Vector3.Distance(player.transform.position, collider.transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;

                    if (!collider.GetComponent<Enemy>().isDead)
                    {
                        nearestTarget = collider.GetComponent<Enemy>();
                    }
                    
                }
            }   
        }
        if (nearestTarget != null) //
        {
            return nearestTarget;
        }
        else
        {

            return null;
        }
    }

}

    //Vector3 relativePosition;
    //Enemy FindNearestTarget(float maxAngle, float maxDistance)
    //{
    //    GameObject targetGO = null;

    //    float minDotProduct = Mathf.Cos(maxAngle * Mathf.Deg2Rad);
    //    float targetDistanceSqrd = maxDistance * maxDistance;

    //    foreach (GameObject go in enemies)
    //    {
    //        if (maxAngle < 90f && Vector3.Dot(relativePosition, transform.forward) <= 0f)
    //               continue; // Early out

    //         relativePosition = go.transform.position - transform.position;

    //        float distanceSqrd = relativePosition.sqrMagnitude;
    //        if ((distanceSqrd < targetDistanceSqrd)
    //             (Vector3.Dot(relativePosition.normalized, transform.forward) > minDotProduct))
    //        {
    //            targetDistanceSqrd = distanceSqrd;
    //            targetGO = go;
    //        }
    //    }
    //    return targetGO;
    //}



    //private bool IsInLineOfSight(Unit unit)
    //{

    //    Vector3 targetCenter = unit.GetComponent<Collider>().bounds.center;

    //    Vector3 direction = targetCenter - weapon.transform.position; // change to viewpoint

    //    float dstToTarget = Vector3.Distance(weapon.transform.position, targetCenter);


    //    Ray ray = new Ray(weapon.transform.position, direction);
    //    RaycastHit hit;


    //    Debug.DrawRay(weapon.transform.position, direction * dstToTarget, Color.green);

    //    if (Physics.Raycast(ray, out hit, dstToTarget))
    //    {


    //        if (hit.collider.tag == "Ground")
    //        {
    //            ResetUnit();
    //            print("Ground in the way");
    //            return false;

    //        }
    //        else if (hit.collider.tag == "Walls")
    //        {
    //            ResetUnit();
    //            print("Walls in the way");
    //            return false;
    //        }
    //        else
    //        {
    //            canAttack = true;
    //            return true;


    //        }
    //    }
    //    return false;
    //}


