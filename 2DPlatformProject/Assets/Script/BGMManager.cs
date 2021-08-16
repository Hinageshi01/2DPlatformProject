using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public static int sceneIndex;

    [SerializeField]
    private AudioClip openingBGM, scene1BGM, scene2BGM, scene3BGM, endingBGM, defautBGM;
    private  AudioSource audioSource;
    private void Awake () {
        audioSource = GetComponent<AudioSource>();
        int crtIndex = SceneManager.GetActiveScene().buildIndex;
        if (instance == null) {
            //只有首个Manager加载时会进入这个分支
            instance = this;
            sceneIndex = crtIndex;
        }
        else if (instance != this) {
            Destroy(gameObject);
            if (sceneIndex != crtIndex) {
                //根据关卡编号切换bgm，重新加载同一关时则不需要切换
                instance.SwitchBGM(crtIndex);
                sceneIndex = crtIndex;
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    public void SwitchBGM(int i) {
        switch (i) {
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
