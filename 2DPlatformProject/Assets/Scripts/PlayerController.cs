using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed;
    public float jumpForce;

    void Start()
    {
        
    }

    void Update()
    {
        Movement();
    }

    void Movement() {
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
            body.velocity = new Vector2(horizontalMove * speed, body.velocity.y);
        if (faceDirection != 0) {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        if (Input.GetButtonDown("Jump")) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }
}
