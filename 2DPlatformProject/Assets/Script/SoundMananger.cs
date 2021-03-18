using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    public AudioSource audioSource;
    public static SoundMananger instance;

    [SerializeField]
    private AudioClip hurtAudio, collectAudio, enemyDestoryAudio;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
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
