using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public GameObject dialog;

    private Animator animator;
    private int enterID,exitID;
    private void Start() {
        animator = dialog.GetComponent<Animator>();
        enterID = Animator.StringToHash("Enter");
        exitID = Animator.StringToHash("Exit");
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !dialog.activeSelf) {
            dialog.SetActive(true);
            animator.SetBool(enterID, true);
            animator.SetBool(exitID, false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            animator.SetBool(enterID, false);
            animator.SetBool(exitID, true);
            Invoke("setActiceFalse", 0.1f);//等待退出动画结束
        }
    }
    private void setActiceFalse() {
        dialog.SetActive(false);
    }
}
