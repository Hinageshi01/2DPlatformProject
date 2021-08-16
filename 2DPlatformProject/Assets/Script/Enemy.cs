using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;

    private int deathID;
    protected virtual void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        deathID = Animator.StringToHash("Death");
    }
    public void Death() {
        GetComponent<Collider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger(deathID);
    }
    public void Disappear() {//在Death动画的结尾调用
        Destroy(gameObject);
    }
}
