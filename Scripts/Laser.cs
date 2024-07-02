using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool helixMode = false;     // Flag to enable/disable helix mode
    public bool rightLaser = false;     // Flag to enable/disable helix mode
    public float amplitude = 1.0f;     // Amplitude of the sine wave
    public float frequency = 1.0f;     // Frequency of the sine wave
    public float speed = 5.0f;         // Speed of horizontal movement

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (helixMode)
        {
            // Calculate x velocity based on sine wave pattern
            // float xVelocity = speed;
            float xVelocity = amplitude * Mathf.Sin(frequency * Time.time);

            // Update Rigidbody2D velocity
            if(rightLaser)
                rb.velocity = new Vector2(xVelocity, speed);
            else
                rb.velocity = new Vector2(-xVelocity, speed);
        }
        else
        {
            // Reset to default behavior or handle other cases
            // rb.velocity = new Vector2(speed, 0f);
        }
    }

    public static explicit operator Laser(GameObject v)
    {
        throw new NotImplementedException();
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    Debug.Log("Collision Enter with: " + collision.gameObject.name);

    // Draw a red ray from each contact point along the collision normal
    foreach (ContactPoint2D contact in collision.contacts)
    {
        Debug.DrawRay(contact.point, contact.normal, Color.red);
    }
}

void OnCollisionStay2D(Collision2D collision)
{
    Debug.Log("Collision Stay with: " + collision.gameObject.name);

    // Draw a blue ray from each contact point along the collision normal
    foreach (ContactPoint2D contact in collision.contacts)
    {
        Debug.DrawRay(contact.point, contact.normal, Color.blue);
    }
}

void OnCollisionExit2D(Collision2D collision)
{
    Debug.Log("Collision Exit with: " + collision.gameObject.name);

    // Draw a green ray from each contact point along the collision normal
    foreach (ContactPoint2D contact in collision.contacts)
    {
        Debug.DrawRay(contact.point, contact.normal, Color.green);
    }
}
}
