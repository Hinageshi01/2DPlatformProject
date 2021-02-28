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
    public AudioSource jumpSource;
    public int finalJumpCount;

    private Rigidbody2D body;
    private Animator animator;
    [SerializeField]
    private int cherry = 0, diamond = 0, jumpCount;
    private bool isHurt = false, standabld = false, jumpPressed = false, isJumped = false;
    void Start(){
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update() {//在Update中确保能敏感地接收到起跳/下蹲请求，再去FixedUpDate中进行Rigidbody相关的运算
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
        float forward = Input.GetAxisRaw("Horizontal");//-1, 0, 1

        animator.SetFloat("Running", Mathf.Abs(horizontalMove));//跑步动画参数
        if (animator.GetBool("Crouching")) {//蹲下移动
            body.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime * 0.25f, body.velocity.y);
        }
        else {//移动
            body.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, body.velocity.y);
        }
        if (forward != 0) {//转身
            transform.localScale = new Vector3(forward, 1, 1);
        }
        if (jumpPressed) {//跳跃
            if (Physics2D.OverlapCircle(footPoint.position, 0.3f, ground)) {//地面起跳
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpSource.Play();
                jumpCount--;
                jumpPressed = false;
                isJumped = true;//代表经过跳跃离开地面
            }
            else if (isJumped) {//空中起跳
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpSource.Play();
                jumpCount--;
                jumpPressed = false;
            }
            //剩余情况为不经跳跃离开地面且没有消灭敌人时请求跳跃，不予起跳
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
        if (standabld && !Physics2D.OverlapCircle(headPoint.position, 0.3f, ground)) {//趴下->站立
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
        if (Physics2D.OverlapCircle(footPoint.position,0.2f,ground)) {//脚底触地，身体触地可以写成collider.IsTouchingLayers(ground)
            if (body.velocity.y <= 0) {//重置跳跃相关参数
                jumpCount = finalJumpCount;
                isJumped = false;
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
            jumpCount++;
        }
        if (collision.tag == "Diamond") {
            SoundMananger.soundMananger.CollectAudio();
            Destroy(collision.gameObject);
            diamond++;
            diamondCount.text = diamond.ToString();
        }
        if (collision.tag == "DeadLine") {//掉出地图
            SoundMananger.soundMananger.GameOver();
            Invoke("Restart", 0.5f);
        }
        if (collision.tag == "Spikes") {//踩到刺上
            Invoke("Restart", 0.23f);
            SoundMananger.soundMananger.HurtAudio();
            isHurt = true;
            body.velocity = new Vector2(0, jumpForce * 0.7f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {//触敌
            if (animator.GetBool("Falling") && transform.position.y - collision.transform.position.y > 0.35f) {//下落触敌
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Death();
                SoundMananger.soundMananger.EnemyDestoryAudio();
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpCount = finalJumpCount;
                isJumped = true;//无论怎样离开地面，消灭敌人后解锁跳跃条件
            }
            else {//受伤
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
    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
