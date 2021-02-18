using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed, jumpForce;
    public LayerMask ground;
    public Text cherryCount, diamondCount;
    public AudioSource jumpAudio, HurtAudio, collectAudio, enemyDestoryAudio;
    new public Collider2D collider;
    public Collider2D crouchCollider;
    public Transform headPoint;
    public int finalJumpCount;

    private Rigidbody2D body;
    private Animator animator;
    private int cherry = 0, diamond = 0, jumpCount;
    private bool isHurt = false, standabld = false, jumpPressed = false;
    void Start(){
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update() {
        if (Input.GetButtonDown("Jump") && jumpCount > 0) {
            jumpPressed = true;
        }
        Crouch();
    }
    void FixedUpdate(){
        if (!isHurt) {
            Movement();
        }
        AnimationSwitch();
    }
    void Movement() {
        float horizontalMove = Input.GetAxis("Horizontal");//-1f -> 1f
        float faceDirection = Input.GetAxisRaw("Horizontal");//-1, 0, 1

        animator.SetFloat("Running", Mathf.Abs(horizontalMove));//跑步动画参数
        if (animator.GetBool("Crouching")) {//蹲下移动
            body.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime * 0.25f, body.velocity.y);
        }
        else {//移动
            body.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, body.velocity.y);
        }
        if (faceDirection != 0) {//转身
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        if (jumpPressed) {//跳跃
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpAudio.Play();
            jumpCount--;
            jumpPressed = false;
        }
    }
    void Crouch() {//趴下
        if (Input.GetButtonDown("Crouch")) {
            collider.enabled = false;
            crouchCollider.enabled = true;
            animator.SetBool("Crouching", true);
        }
        if (Input.GetButtonUp("Crouch")) {
            standabld = true;
        }
        if (standabld && !Physics2D.OverlapCircle(headPoint.position, 0.2f, ground)) {//趴下->站立
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
            jumpCount = finalJumpCount;
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
        //掉出地图
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
                isHurt = true;//用于切换动画以及屏蔽受伤状态下移动相关的输入
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
