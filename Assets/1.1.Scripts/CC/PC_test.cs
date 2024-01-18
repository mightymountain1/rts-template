using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_test : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 dir;

    public float thrust;
    public float rotateSpeed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

     
    }

    void Update()
    {
        //rb.AddForce(transform.up * thrust);

        // Alternatively, specify the force mode, which is ForceMode2D.Force by default
        rb.AddForce(transform.up * thrust * Time.deltaTime, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {


         

    }

    private void LateUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        float horizontalInput = Input.GetAxis("Horizontal");
        // Rotate(x,y,z)
        dir = new Vector3(0, 0, -horizontalInput * rotateSpeed * Time.deltaTime);

        transform.Rotate(dir);

    }
    Vector2 lastVelocity;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy");

      //      transform.Rotate(0, 0, 90);

        }
    }

}
