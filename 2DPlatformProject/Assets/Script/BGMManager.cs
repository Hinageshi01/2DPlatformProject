using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    static BGMManager staticBGM;
    static int sceneIndex = -1;
    public  AudioClip openingBGM, scene1BGM, scene2BGM, scene3BGM, endingBGM, defautBGM;
    private  AudioSource audioSource;
    private void Awake () {
        audioSource = GetComponent<AudioSource>();
        if (staticBGM == null) {
            staticBGM = this;
        }
        else if (staticBGM != this) {
            Destroy(gameObject);
            if (sceneIndex != SceneManager.GetActiveScene().buildIndex) {
                staticBGM.SwitchBGM();
            }
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void SwitchBGM() {
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 0:
                audioSource.clip = openingBGM;
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = scene1BGM;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = scene2BGM;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = scene3BGM;
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = endingBGM;
                audioSource.Play();
                break;
            default:
                audioSource.clip = defautBGM;
                audioSource.Play();
                break;
        }
    }
}
