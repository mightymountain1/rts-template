using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float shootAngle;

    public void Fire(bool isPlayersBullet)
    {
        Bullet bulletScript;
        GameObject bulletGO;
        bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletGO.GetComponent<Rigidbody>().AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
        bulletScript = bulletGO.GetComponent<Bullet>();
        bulletScript.MyDamage = damage;
        if (isPlayersBullet)
        {
            bulletScript.isPlayersBullet = true;
        } else
        {
            bulletScript.isPlayersBullet = false;
        }
    }
}

