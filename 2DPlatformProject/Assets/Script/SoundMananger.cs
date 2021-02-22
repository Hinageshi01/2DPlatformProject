using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    public AudioSource audioSource;
    public static SoundMananger soundMananger;

    [SerializeField]
    private AudioClip jumpAudio, hurtAudio, collectAudio, enemyDestoryAudio;
    private void Awake() {
        soundMananger = this;
    }
    public void JumpAudio() {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }
    public void HurtAudio() {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }
    public void CollectAudio() {
        audioSource.clip = collectAudio;
        audioSource.Play();
    }
    public void EnemyDestoryAudio() {
        audioSource.clip = enemyDestoryAudio;
        audioSource.Play();
    }
}
