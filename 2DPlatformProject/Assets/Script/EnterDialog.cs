using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialog : MonoBehaviour
{
    public GameObject enterDialog;

    private Animator animator;
    private void Start() {
        animator = enterDialog.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            enterDialog.SetActive(true);
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
        }
    }
}
