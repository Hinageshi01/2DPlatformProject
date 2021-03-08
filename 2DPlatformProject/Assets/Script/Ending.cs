using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject whiteBox;

    private Animator animator;
    private bool isPlayed = false;
    private void Start() {
        animator = whiteBox.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isPlayed) {
            whiteBox.SetActive(true);
            Time.timeScale = 0.25f;
            animator.SetTrigger("Play");
            Invoke("ResetTimeScale", 2f);
            isPlayed = true;
        }
    }
    private void ResetTimeScale() {
        Time.timeScale = 0f;
    }
}
