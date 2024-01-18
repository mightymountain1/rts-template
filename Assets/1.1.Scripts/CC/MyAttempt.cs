using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class MyAttempt : MonoBehaviour
{

    [Header("Method1")]
    public float maxSpeed;
    public float acc;
    public float steering;

    Rigidbody2D rb;

    float x;
    float y = 1;


    Vector3 lastVelocity;
    float direction;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash;
    Vector2 moveDirection;

    public Weapon weapon;

    public float driftMagnitude = 2f;

    [Header("Method2")]
    // Settings
    public float MoveSpeed = 50;
    public float MaxSpeed = 15;
    public float Drag = 0.98f;
    public float SteerAngle = 20;
    public float Traction = 1;

    // Variables
    private Vector3 MoveForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canDash = true;
    }

    // Update is called once per frame

   void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }



        //if (isDashing)
        //{
        //    return;
        //}

        Method1();
       // Method2();
  
    }


    void Method1()
    {
        x = Input.GetAxis("Horizontal");

        Vector2 speed = transform.up * (y * acc);
        rb.AddForce(speed);
        
        direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));

        if (acc > 0)
        {
            if (direction > 0)
            {

                rb.rotation -= x * steering * (rb.velocity.magnitude / maxSpeed);
            }
            else
            {
                rb.rotation += x * steering * (rb.velocity.magnitude / maxSpeed);
            }
        }

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.left)) * driftMagnitude;
        Vector2 relativeForce = Vector2.right * driftForce;

        rb.AddForce(rb.GetRelativeVector(relativeForce));

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        Debug.DrawLine(rb.position, rb.GetRelativePoint(relativeForce), Color.green);
        lastVelocity = rb.velocity;
    }
    void Method2()
    {
        // Moving
        MoveForce += transform.forward * MoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;

        // Steering
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);

        // Drag and max speed limit
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // Traction
        Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Wall");

            FXManager.MyInstance.CreatWallBumpFX(this.transform.position);


            ContactPoint2D point = collision.contacts[0];
            Vector2 newDir = Vector2.zero;
            Vector2 curDire = this.transform.TransformDirection(Vector2.up);

            newDir = Vector2.Reflect(curDire, point.normal);
            transform.rotation = Quaternion.FromToRotation(Vector2.up, newDir);


        }
    }

    float startSpeed;
    public float boostSpeed;
    private IEnumerator Dash()
    {
        FXManager.MyInstance.thrustPS.Play();
        startSpeed = maxSpeed;
        canDash = false;
        isDashing = true;
        maxSpeed = dashSpeed;

        if (BuffManager.MyInstance.hasMegaBoost)
        {
            BuffManager.MyInstance.shield.SetActive(true);
            BuffManager.MyInstance.hasShieldUp = true;
        }

        Vector2 speed2 = transform.up * (y * boostSpeed);
        rb.AddForce(speed2);


        //rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        BuffManager.MyInstance.shield.SetActive(false);
        BuffManager.MyInstance.hasShieldUp = false;
        isDashing = false;
        maxSpeed = startSpeed;
        FXManager.MyInstance.thrustPS.Stop();
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;



    }

}
