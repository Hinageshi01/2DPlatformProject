using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject whiteBox;

    private Animator animator;
    private bool isPlayed = false;
    private int playID;
    private void Start() {
        animator = whiteBox.GetComponent<Animator>();
        playID = Animator.StringToHash("Play");
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isPlayed) {
            whiteBox.SetActive(true);
            Time.timeScale = 0.25f;
            animator.SetTrigger(playID);
            Invoke("ResetTimeScale", 2f);//动画演出结束后暂停游戏
            isPlayed = true;
        }
    }
    private void ResetTimeScale() {
        Time.timeScale = 0f;
    }
}
