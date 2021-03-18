using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed, jumpForce;
    public LayerMask ground;
    public Text diamondCount;
    public Collider2D usualCollider, crouchCollider;
    public Transform headPoint, footPoint;
    public int finalJumpCount;

    private Rigidbody2D body;
    private Animator animator;
    private AudioSource jumpSource;
    private float time = -1f;
    private int diamond = 0, jumpCount, idlingID, runningID, jumpingID, fallingID, crouchingID, hurtID;
    private bool isHurt = false, standabld = false, jumpPressed = false, isJumped = false;
    void Start(){
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpSource = GetComponent<AudioSource>();

        idlingID = Animator.StringToHash("Idling");
        runningID = Animator.StringToHash("Running");
        jumpingID = Animator.StringToHash("Jumping");
        fallingID = Animator.StringToHash("Falling");
        crouchingID = Animator.StringToHash("Crouching");
        hurtID = Animator.StringToHash("Hurt");
    }
    void Update() {
        //在Update中确保能敏感地接收到起跳/下蹲请求，再去FixedUpDate中进行Rigidbody相关的运算
        if (Input.GetButtonDown("Jump") && jumpCount > 0) {
            jumpPressed = true;
            time = Time.time + 0.05f;
        }
        if (time >= 0 && time < Time.time) {
            jumpPressed = false;
            time = -1f;
            //经过一个短暂的计时后重置jumpPressed，
            //实际上，经过0.05s后jumpPressed仍未在Movement()中重置只有一种情况，即未经跳跃离开平台且在下落途中按下空格，
            //此时若不重置jumpPressed，Player触地后会立刻执行一次跳跃，这是不希望看到的效果
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

        animator.SetFloat(runningID, Mathf.Abs(horizontalMove));//跑步动画参数
        if (animator.GetBool(crouchingID)) {//蹲下移动
            body.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime * 0.25f, body.velocity.y);
        }
        else {//移动
            body.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, body.velocity.y);
        }
        if (forward != 0) {//转身
            transform.localScale = new Vector3(forward, 1, 1);
        }
        if (Physics2D.OverlapCircle(footPoint.position, 0.1f, ground) && (animator.GetBool("Idling") || animator.GetFloat("Running") >= 0.1f)) {
            //代表脚底触地，身体触地可以写成collider.IsTouchingLayers(ground)
            //落地时重置跳跃相关的参数，而且要避免刚起跳时OverlapCircle检测到地面
            jumpCount = finalJumpCount;
            isJumped = false;
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
            //剩余情况为不经跳跃离开地面且没有消灭敌人/吃到樱桃时请求跳跃，不予起跳
        }
    }
    void Crouch() {//趴下
        if (Input.GetButtonUp("Crouch")) {
            standabld = true;
        }
        if (!Time.timeScale.Equals(0)) {
            if (Input.GetButtonDown("Crouch")) {
                usualCollider.enabled = false;
                crouchCollider.enabled = true;
                animator.SetBool(crouchingID, true);
            }
            if (standabld && !Physics2D.OverlapCircle(headPoint.position, 0.2f, ground)) {//趴下->站立
                standabld = false;
                usualCollider.enabled = true;
                crouchCollider.enabled = false;
                animator.SetBool(crouchingID, false);
            }
        }
    }
    void AnimationSwitch() {
        if (body.velocity.y > 0) {//上升
            animator.SetBool(jumpingID, true);
            animator.SetBool(fallingID, false);
        }
        if (body.velocity.y < 0) {//下降
            animator.SetBool(jumpingID, false);
            animator.SetBool(fallingID, true);
        }
        
        if (Physics2D.OverlapCircle(footPoint.position,0.1f, ground)) {
            animator.SetBool(jumpingID, false);
            animator.SetBool(fallingID, false);
            animator.SetBool(idlingID, true);
        }
        if (isHurt) {//受伤
            animator.SetBool(hurtID, true);
            if (body.velocity.y <= 0) {
                animator.SetBool(hurtID, false);
                isHurt = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        //收集
        if (collision.CompareTag("Cherry")) {
            SoundMananger.instance.CollectAudio();
            Destroy(collision.gameObject);
            jumpCount++;
            isJumped = true;
            //无论怎样离开地面，吃到樱桃后解锁跳跃条件
        }
        if (collision.CompareTag("Diamond")) {
            SoundMananger.instance.CollectAudio();
            Destroy(collision.gameObject);
            diamond++;
            diamondCount.text = diamond.ToString();
        }
        if (collision.CompareTag("DeadLine")) {//掉出地图
            Invoke("Restart", 0.5f);
        }
        if (collision.CompareTag("Spikes")) {//踩到刺上
            Invoke("Restart", 0.2f);
            SoundMananger.instance.HurtAudio();
            isHurt = true;
            body.velocity = new Vector2(0, jumpForce * 0.7f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {//触敌
            if (animator.GetBool(fallingID) && transform.position.y - collision.transform.position.y > 0.35f) {//下落触敌
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Death();
                SoundMananger.instance.EnemyDestoryAudio();
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpCount = finalJumpCount;
                isJumped = true;
                //无论怎样离开地面，消灭敌人后解锁跳跃条件
            }
            else {//受伤
                SoundMananger.instance.HurtAudio();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
