using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;

    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        float mmoveX = Input.GetAxisRaw("Horizontal");
        float mmoveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(mmoveX, mmoveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
      
    }
}
