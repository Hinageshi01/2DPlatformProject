using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Collider2D playerCollider;
    public LayerMask ground;
    public Text cherryCount;
    public Text diamondCount;

    private Rigidbody2D body;
    private Animator animator;
    private int cherry = 0;
    private int diamond = 0;
    private bool isHurt = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isHurt) {
            Movement();
        }
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
        if (isHurt) {//受伤
            animator.SetBool("Hurt", true);
            if (body.velocity.y <= 0) {
                animator.SetBool("Hurt", false);
                isHurt = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {//收集
        if (collision.tag == "Cherry") {
            Destroy(collision.gameObject);
            cherry++;
            cherryCount.text = cherry.ToString();
        }
        if (collision.tag == "Diamond") {
            Destroy(collision.gameObject);
            diamond++;
            diamondCount.text = diamond.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {//接敌
        if (collision.gameObject.tag == "Enemy") {
            if(body.velocity.y < 0 && transform.position.y - collision.gameObject.transform.position.y >= 0.5f) {//下落接敌
                Destroy(collision.gameObject);
                body.velocity = new Vector2(body.velocity.x, jumpForce);
            }
            else {//侧面接敌
                if (transform.position.x <= collision.gameObject.transform.position.x) {
                    isHurt = true;
                    body.velocity = new Vector2(-10f, body.velocity.y + jumpForce * 0.7f);
                }
                else {
                    isHurt = true;
                    body.velocity = new Vector2(10f, body.velocity.y + jumpForce * 0.7f);
                }
            }
        }
    }
}
