using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBox : MonoBehaviour
{
    public GameObject blackBox;
    private Animator animator;
    private void Start() {
        animator = blackBox.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            blackBox.SetActive(true); 
            animator.SetTrigger("Play");
        }
    }
}
