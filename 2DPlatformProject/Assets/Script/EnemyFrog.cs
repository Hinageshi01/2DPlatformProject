﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrog : MonoBehaviour
{
    public Transform leftPoint, rightPoint;
    public float speed;
    public float jumpForce;
    public LayerMask ground;

    private Rigidbody2D body;
    private Animator animator;
    private Collider2D collisionBox;
    private float leftX, rightX;
    private bool isFaceLeft = true;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collisionBox = GetComponent<Collider2D>();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }
    void Update()
    {
        AnimationSwitch();
    }
    void movement() {
        if (isFaceLeft) {//朝左
            if (transform.position.x <= leftX) {
                isFaceLeft = false;
                transform.localScale = new Vector3(-1, 1, 1);//转身
            }
            body.velocity = new Vector2(-transform.localScale.x * speed, jumpForce);
            animator.SetBool("Jumping", true);
        }
        else {//朝右
            if (transform.position.x >= rightX) {
                isFaceLeft = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
            body.velocity = new Vector2(-transform.localScale.x * speed, jumpForce);
            animator.SetBool("Jumping", true);
        }
    }
    void AnimationSwitch() {
        if (animator.GetBool("Jumping") && body.velocity.y <= 0) {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
        if (animator.GetBool("Falling") && collisionBox.IsTouchingLayers(ground)) {
            animator.SetBool("Falling", false);
            body.velocity = new Vector2(0, 0);
        }
    }
    public void jumpOn() {
        animator.SetTrigger("Death");
    }
    void death() {
        Destroy(gameObject);
    }
}
