﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Projectile_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;

    [Range(0, 360)]
    public float angle;
    public float speed;
    public int health;

    public AudioClip netBouns;
    public AudioClip shieldBouns;

    AudioSource speaker;

    void Start()
    {
        speaker = this.GetComponent<AudioSource>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g - 1f, sr.color.b + 100f, sr.color.a);
        Debug.Log(sr.color.r + " R " + sr.color.g + " G " + sr.color.b + " B " +  sr.color.a + " A ");

        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        UpdateDirection(angle);

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Bounce(collision);
    }

    private void Bounce(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        float contactAngle = CS_Utils.GetCollisionAngle(transform, contact.point);

        // Scope between intervals to make sure that it matches perfectly.
        contactAngle = CS_Utils.AngleRound(contactAngle);
        // Transform "motion" into "collision" angle using 90 offset.
        float offset = CS_Utils.Mod(contactAngle + 90, 360);

        // Reset angle to be based on zero.
        float newAngle = angle - offset;
        // Invert in order to reflect.
        newAngle *= -1;
        // Get reflection angle.
        newAngle = offset + newAngle;
        // Rotate 180 degrees to travle in opposite direction.
        newAngle = CS_Utils.Mod(newAngle + 180, 360);

        UpdateDirection(newAngle);

        health--;
        // TODO: Remove, visual representation for testing.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g - 0.3f, sr.color.b - 0.2f, sr.color.a - 0.2f);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(collision.gameObject.tag);
            if (collision.gameObject.tag == "shield")
            {
                speaker.PlayOneShot(shieldBouns);
            }
            else if (collision.gameObject.tag == "player")
            {
                speaker.PlayOneShot(shieldBouns);
            }
            else if (collision.gameObject.tag == "net")
            {
                speaker.PlayOneShot(netBouns);
            }
        }
    }

    private void UpdateDirection(float a)
    {
        angle = a;
        direction = CS_Utils.DegreeToPoint(a);
        rb.velocity = direction * speed;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, a - 90);
    }
}