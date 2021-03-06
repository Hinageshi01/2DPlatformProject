using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject titleUI, startUI, endUI;
    void Start() {
        StartCoroutine(Delayed(0.65f, titleUI, startUI, endUI));
    }
    IEnumerator Delayed(float delayTime, params GameObject[] list) {
        foreach (GameObject obj in list) {
            yield return new WaitForSeconds(delayTime);
            obj.SetActive(true);
        }
    }
    public void StartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
