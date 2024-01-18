
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    public Vector2 panLimit;
    // Update is called once per frame
    void Update()
    {

        ProccessMovementUnPaused();
    }

    public void ProccessMovementUnPaused()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            pos.z += panSpeed * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // limit the scrolling
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }


    public void ProccessMovementPaused()
    {

        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            pos.z += panSpeed * Time.unscaledDeltaTime;

        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.z -= panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= panSpeed * Time.unscaledDeltaTime;
        }

        // limit the scrolling
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;


    }
}
