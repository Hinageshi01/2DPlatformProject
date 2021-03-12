using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrog : Enemy
{
    public Transform leftPoint, rightPoint;
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public bool forwardLeft;

    private Rigidbody2D body;
    private Collider2D collisionBox;
    private float leftX, rightX;
    private int jumpingID, fallingID;
    protected override void Start() {
        base.Start();
        body = GetComponent<Rigidbody2D>();
        collisionBox = GetComponent<Collider2D>();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);

        jumpingID = Animator.StringToHash("Jumping");
        fallingID = Animator.StringToHash("Falling");
    }
    void Update() {
        AnimationSwitch();
    }
    void AnimationSwitch() {
        if (animator.GetBool(jumpingID) && body.velocity.y <= 0) {
            animator.SetBool(jumpingID, false);
            animator.SetBool(fallingID, true);
        }
        if (animator.GetBool(fallingID) && collisionBox.IsTouchingLayers(ground)) {
            animator.SetBool(fallingID, false);
            body.velocity = new Vector2(0, 0);
        }
    }
    void movement() {//在Idel动画事件的结尾调用
        if (forwardLeft) {//朝左
            if (transform.position.x <= leftX) {//转身
                forwardLeft = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            body.velocity = new Vector2(-transform.localScale.x * speed, jumpForce);
            animator.SetBool(jumpingID, true);
        }
        else {//朝右
            if (transform.position.x >= rightX) {//转身
                forwardLeft = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
            body.velocity = new Vector2(-transform.localScale.x * speed, jumpForce);
            animator.SetBool(jumpingID, true);
        }
    }
}
