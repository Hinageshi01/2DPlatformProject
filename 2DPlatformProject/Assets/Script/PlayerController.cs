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
    public Collider2D usualCollider, crouchCollider;
    public Transform headPoint, footPoint;
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
            SoundMananger.soundMananger.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
    }
    void Crouch() {//趴下
        if (Input.GetButtonDown("Crouch")) {
            usualCollider.enabled = false;
            crouchCollider.enabled = true;
            animator.SetBool("Crouching", true);
        }
        if (Input.GetButtonUp("Crouch")) {
            standabld = true;
        }
        if (standabld && !Physics2D.OverlapCircle(headPoint.position, 0.2f, ground)) {//趴下->站立
            standabld = false;
            usualCollider.enabled = true;
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
        if (Physics2D.OverlapCircle(footPoint.position,0.2f,ground)) {//触地，也可以写成collider.IsTouchingLayers(ground)
            if (body.velocity.y <= 0) {//重置跳跃次数
                jumpCount = finalJumpCount;
            }
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
            SoundMananger.soundMananger.CollectAudio();
            Destroy(collision.gameObject);
            cherry++;
            cherryCount.text = cherry.ToString();
        }
        if (collision.tag == "Diamond") {
            SoundMananger.soundMananger.CollectAudio();
            Destroy(collision.gameObject);
            diamond++;
            diamondCount.text = diamond.ToString();
        }
        //掉出地图
        if (collision.tag == "DeadLine") {
            GetComponent<AudioSource>().enabled = false;
            Invoke("restart", 0.5f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {//触敌
        if (collision.gameObject.tag == "Enemy") {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(body.velocity.y < 0 && transform.position.y - collision.transform.position.y >= 0.2f) {//下落触敌
                enemy.jumpOn();
                SoundMananger.soundMananger.EnemyDestoryAudio();
                body.velocity = new Vector2(body.velocity.x, jumpForce);
            }
            else {//侧面接敌
                SoundMananger.soundMananger.HurtAudio();
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
