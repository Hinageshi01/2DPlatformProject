using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public Text cherryCount;
    public Text diamondCount;
    public AudioSource jumpAudio, HurtAudio, collectAudio, enemyDestoryAudio;
    new public Collider2D collider;

    public Collider2D crouchCollider;
    public Transform headPoint;

    private Rigidbody2D body;
    private Animator animator;
    private int cherry = 0;
    private int diamond = 0;
    private bool isHurt = false;
    private bool standabld = false;
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

        animator.SetFloat("Running", Mathf.Abs(horizontalMove));//跑步动画
        if (animator.GetBool("Crouching")) {//蹲下移动
            body.velocity = new Vector2(horizontalMove * speed * 0.25f, body.velocity.y);
        }
        else {//移动
            body.velocity = new Vector2(horizontalMove * speed, body.velocity.y);
        }
        if (faceDirection != 0) {//转身
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        if (Input.GetButtonDown("Jump")) {//跳跃
            jumpAudio.Play();
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
        if (Input.GetButtonDown("Crouch")) {
            collider.enabled = false;
            crouchCollider.enabled = true;
            animator.SetBool("Crouching", true);
        }
        if (Input.GetButtonUp("Crouch")) {
            standabld = true;
        }
        if (standabld && !Physics2D.OverlapCircle(headPoint.position, 0.2f, ground)) {
            standabld = false;
            collider.enabled = true;
            crouchCollider.enabled = false;
            animator.SetBool("Crouching", false);
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
        if (collider.IsTouchingLayers(ground)) {//触地
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
    private void OnTriggerEnter2D(Collider2D collision) {
        //收集
        if (collision.tag == "Cherry") {
            collectAudio.Play();
            Destroy(collision.gameObject);
            cherry++;
            cherryCount.text = cherry.ToString();
        }
        if (collision.tag == "Diamond") {
            collectAudio.Play();
            Destroy(collision.gameObject);
            diamond++;
            diamondCount.text = diamond.ToString();
        }
        if (collision.tag == "DeadLine") {
            GetComponent<AudioSource>().enabled = false;
            Invoke("restart", 1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {//触敌
        if (collision.gameObject.tag == "Enemy") {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(body.velocity.y < 0 && transform.position.y - collision.transform.position.y >= 0.2f) {//下落触敌
                enemy.jumpOn();
                enemyDestoryAudio.Play();
                body.velocity = new Vector2(body.velocity.x, jumpForce);
            }
            else {//侧面接敌
                HurtAudio.Play();
                isHurt = true;
                if (transform.position.x <= collision.transform.position.x) {//右侧触敌
                    body.velocity = new Vector2(-10f, body.velocity.y + jumpForce * 0.7f);
                }
                else {//左侧接敌
                    body.velocity = new Vector2(10f, body.velocity.y + jumpForce * 0.7f);
                }
            }
        }
    }
    private void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
