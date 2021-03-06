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
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !dialog.activeSelf) {
            dialog.SetActive(true);
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
            Invoke("setActiceFalse", 0.1f);//等待退出动画结束
        }
    }
    private void setActiceFalse() {
        dialog.SetActive(false);
    }
}
