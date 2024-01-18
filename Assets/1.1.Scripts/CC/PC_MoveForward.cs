using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PC_MoveForward : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public Weapon weapon;

    Vector2 moveDirection;
    Vector2 mousePosition;
    
    [Header("Rotate Settings")]

    public bool rotateWithKeyBoard;
    public bool rotateWithMouse;
    public enum RotationType { rotateWithKeyboard, rotateWithMouse };
    public RotationType rotationType;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash;

    public int moveOption;

    Vector2 forwardDir;
    Vector3 dir;

    private void Start()
    {
        canDash = true;
         rb.velocity = transform.right * moveSpeed;


        rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Impulse);

      

    }

    public float angleBetween = 0.0f;
    public Transform target;
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;
        Vector3 direction = move.normalized;

        if (move != Vector3.zero)
        {
            transform.up = direction;
        }
    }
  //  void FixedUpdate()
    //{

    //    Vector3 pos = rb.position;
    //    tMov = 0;
    //    sMov = 0;
    //    vMov = 0;
    //    CheckMoveKeys();
    //    rot = rb.rotation;

    //    Vector3 angle = new Vector3(0, 0, tMov * turnSpeed);

    //    Vector3 movement = new Vector3(sMov * strafeSpeed, vMov * speed);

    //    Quaternion rotation = Quaternion.Euler(angle);

    //    //FIX HERE
    //    rb.AddRelativeForce(movement * Time.deltaTime);
    //    rb.MoveRotation(rb.rotation + angle.z * Time.deltaTime);




   // }

    void LateUpdate()
    {
        //Rotate();
    }
    Vector2 lastVelocity;
    private void FixedUpdate()
    {
        //Move();
        lastVelocity = rb.velocity;

    }


    void ProcessInputs()
    {
        if (isDashing)
        {
            return;
        }


        float mmoveX = Input.GetAxisRaw("Horizontal");
        float mmoveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        moveDirection = new Vector2(mmoveX, mmoveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    Vector2 direction;
    // Constant move forward with rotate to mouse point
    void Move()
    {
        if (isDashing)
        {
            return;
        }


        direction = transform.up;

        forwardDir = direction * Time.fixedDeltaTime * moveSpeed;

        rb.velocity = forwardDir;


    }

    public float rotateSpeed;

    private void Rotate()
    {
        if (rotationType == RotationType.rotateWithKeyboard)
        {

               float moveX = Input.GetAxisRaw("Horizontal");

            //var input = new Vector2(Input.GetAxisRaw("Horizontal"), forwardDir.x);
            //rb.rotation = input.normalized * moveSpeed;

            //Vector2 aimDirection = input - rb.position;
            //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            //rb.rotation = aimAngle;


 



            float horizontalInput = Input.GetAxis("Horizontal");
            // Rotate(x,y,z)
            dir = new Vector3(0, 0, -horizontalInput * rotateSpeed * Time.deltaTime);
  
            transform.Rotate(dir);


        }
        else if (rotationType == RotationType.rotateWithMouse)
        {
             
            Vector2 aimDirection = mousePosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = aimAngle;
        }


    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing= false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public float colFloat;

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag =="Walls")
        {
            Debug.Log("Walls");
            dir.z *= -colFloat;
            //   transform.Rotate(0,0,180);
            forwardDir *= -1;
            //Vector2 vel;
            //vel.x = (rb.velocity.x / 2);
            //vel.y = rb.velocity.y;
            //rb.velocity = vel;

            //Vector2 currentDirection = rb.velocity.normalized;

            //// get a direction halfway between the ball's movement direction and straight up
            //Vector2 newDirection = (currentDirection + Vector2.up).normalized;
            //newDirection = Vector2.Reflect(newDirection, Vector2.up);
            //rb.velocity = rb.velocity.magnitude * newDirection;

            //  rb.velocity = forwardDir *-1;


            var speed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * Mathf.Max(speed, 2f);
        }
    }
}
