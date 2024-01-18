using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public float maxSpeed;
    public float acc;
    public float steering;

    Rigidbody2D rb;

    float x;
    float y = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

     //   rb.AddForce(new Vector2(maxSpeed, maxSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");

        Vector2 speed = transform.up * (y * acc);
        rb.AddForce(speed);

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));

        if (acc > 0)
        {
            if (direction > 0)
            {
                rb.rotation -= x * steering * (rb.velocity.magnitude / maxSpeed);
            } else
            {
                rb.rotation += x * steering * (rb.velocity.magnitude / maxSpeed);
            }
        }

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.left)) * 2f;
        Vector2 relativeForce = Vector2.right * driftForce;

        rb.AddForce(rb.GetRelativeVector(relativeForce));

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        Debug.DrawLine(rb.position, rb.GetRelativePoint(relativeForce), Color.green);

        lastVelocity = rb.velocity;
    }
    Vector3 lastVelocity;
    public float pushForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("YesBlood");

            //   rb.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);

            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.AddForce(direction * Mathf.Max(speed, 0f));

            rb.velocity = direction * Mathf.Max(speed, 0f);
            //   transform.Rotate(0, 0, 180);

            //float proportionPosition = CalculatePosition(transform.position, collision.transform.position, collision.collider.bounds.size.y);
            //Vector2 direction = new Vector2(1, proportionPosition).normalized;
            //rb.velocity = direction * pushForce * 1.1f;

        }
    }
    float CalculatePosition(Vector2 ballPosition, Vector2 paddlePosition, float paddleHeight)
    {
        float y = (ballPosition.y - paddlePosition.y) / paddleHeight;
        return y;
    }
}
