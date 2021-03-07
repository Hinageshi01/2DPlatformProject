using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBox : MonoBehaviour
{
    public GameObject blackBox;

    private Animator animator;
    private bool isPlayed = false;
    private void Start() {
        animator = blackBox.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isPlayed) {
            blackBox.SetActive(true); 
            animator.SetTrigger("Play");
            isPlayed = true;
        }
    }
}
