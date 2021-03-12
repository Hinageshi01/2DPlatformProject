using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigidbody2;

    private int deathID;
    protected virtual void Start() {
        animator = GetComponent<Animator>();
        rigidbody2 = GetComponent<Rigidbody2D>();
        deathID = Animator.StringToHash("Death");
    }
    public void Death() {
        GetComponent<Collider2D>().enabled = false;
        rigidbody2.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger(deathID);
    }
    public void Disappear() {
        Destroy(gameObject);
    }
}
