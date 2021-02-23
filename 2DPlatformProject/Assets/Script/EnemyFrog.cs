using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrog : Enemy
{
    public Transform leftPoint, rightPoint;
    public float speed;
    public float jumpForce;
    public LayerMask ground;

    private Rigidbody2D body;
    private Collider2D collisionBox;
    private float leftX, rightX;
    private bool isFaceLeft = true;
    protected override void Start() {
        base.Start();
        body = GetComponent<Rigidbody2D>();
        collisionBox = GetComponent<Collider2D>();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }
    void Update() {
        AnimationSwitch();
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
    void movement() {//在Idel动画事件中调用
        if (isFaceLeft) {//朝左
            if (transform.position.x <= leftX) {//转身
                isFaceLeft = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            body.velocity = new Vector2(-transform.localScale.x * speed, jumpForce);
            animator.SetBool("Jumping", true);
        }
        else {//朝右
            if (transform.position.x >= rightX) {//转身
                isFaceLeft = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
            body.velocity = new Vector2(-transform.localScale.x * speed, jumpForce);
            animator.SetBool("Jumping", true);
        }
    }
}
