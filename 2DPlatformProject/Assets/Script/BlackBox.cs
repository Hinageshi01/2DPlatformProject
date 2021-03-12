using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBox : MonoBehaviour
{
    public GameObject blackBox;

    private Animator animator;
    private bool isPlayed = false;
    private int playID;
    private void Start() {
        animator = blackBox.GetComponent<Animator>();
        playID = Animator.StringToHash("Play");
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isPlayed) {
            blackBox.SetActive(true); 
            animator.SetTrigger(playID);
            isPlayed = true;
        }
    }
}
