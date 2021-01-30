using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed;
    public float jumpForce;
    public Animator animator;
    public Collider2D collider2d;
    public LayerMask ground;

    void Start()
    {
        
    }

    void Update()
    {
        Movement();
        AnimationSwitch();
    }

    void Movement() {
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Running", Mathf.Abs(horizontalMove));
        body.velocity = new Vector2(horizontalMove * speed, body.velocity.y);//移动
        if (faceDirection != 0) {
            transform.localScale = new Vector3(faceDirection, 1, 1);//转身
        }
        if (Input.GetButtonDown("Jump")) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);//跳跃
            animator.SetBool("Jumping", true);
        }
    }
    void AnimationSwitch() {
        if (animator.GetBool("Jumping")) {
            if (body.velocity.y <= 0) {
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);
            }
        }
        else if (collider2d.IsTouchingLayers(ground)){
            animator.SetBool("Falling", false);
            animator.SetBool("Idling", true);
        }
    }
}
