using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    protected virtual void Start() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    public void jumpOn() {
        animator.SetTrigger("Death");
    }
    public void death() {
        Destroy(gameObject);
    }
}
