using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected virtual void Start() {
        animator = GetComponent<Animator>();
    }
    public void jumpOn() {
        animator.SetTrigger("Death");
    }
    public void death() {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
