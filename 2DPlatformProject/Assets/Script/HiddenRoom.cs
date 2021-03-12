using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenRoom : MonoBehaviour
{
    private Animator animator;
    private int playID;
    private void Start() {
        animator = GetComponent<Animator>();
        playID = Animator.StringToHash("Play");
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            animator.SetTrigger(playID);
        }
    }
}
