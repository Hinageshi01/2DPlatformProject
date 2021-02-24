using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public GameObject dialog;

    private Animator animator;
    private void Start() {
        animator = dialog.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            dialog.SetActive(true);
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
            Invoke("setActiceFalse", 0.2f);
        }
    }
    private void setActiceFalse() {
        dialog.SetActive(false);
    }
}
