using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;

    public float speed;
    public float jumpForce;
    public Collider2D playerCollider;
    public LayerMask ground;
    public int cherry = 0;
    public Text cherryCount;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        AnimationSwitch();
    }

    void Movement() {
        float horizontalMove = Input.GetAxis("Horizontal");//-1f -> 1f
        float faceDirection = Input.GetAxisRaw("Horizontal");//-1, 0, 1
        animator.SetFloat("Running", Mathf.Abs(horizontalMove));//0f -> 1f
        body.velocity = new Vector2(horizontalMove * speed, body.velocity.y);//移动
        if (faceDirection != 0) {
            transform.localScale = new Vector3(faceDirection, 1, 1);//转身
        }
        if (Input.GetButtonDown("Jump")) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);//跳跃
        }
    }
    void AnimationSwitch() {
        if (body.velocity.y > 0) {//上升
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
        }
        if (body.velocity.y < 0) {//下降
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
        if (playerCollider.IsTouchingLayers(ground)) {//触地
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Idling", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Collection") {
            Destroy(collision.gameObject);
            cherry++;
            cherryCount.text = cherry.ToString();
        }
    }
}
