using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigidbody2;
    protected virtual void Start() {
        animator = GetComponent<Animator>();
        rigidbody2 = GetComponent<Rigidbody2D>();
    }
    public void jumpOn() {
        GetComponent<Collider2D>().enabled = false;
        rigidbody2.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Death");
    }
    public void death() {
        Destroy(gameObject);
    }
}
