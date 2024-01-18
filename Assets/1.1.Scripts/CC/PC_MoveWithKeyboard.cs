using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_MoveWithKeyboard : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public Weapon weapon;

    Vector2 moveDirection;
    Vector2 mousePosition;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash;

    public int moveOption;

    private void Start()
    {
        canDash = true;
    }


    void Update()
    { 
            ProcessInputs();      
    }


    private void FixedUpdate()
    {
            Move();
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

    void Move()
    {
        if (isDashing)
        {
            return;
        }

        
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);


        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
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
}
