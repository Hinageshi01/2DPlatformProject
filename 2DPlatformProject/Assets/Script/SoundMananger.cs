using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    public AudioSource audioSource;
    public static SoundMananger soundMananger;

    [SerializeField]
    private AudioClip hurtAudio, collectAudio, enemyDestoryAudio;
    private void Awake() {
        if (soundMananger == null) {
            soundMananger = this;
        }
        else if (soundMananger != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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
