using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject titleUI, startUI, endUI;
    void Start() {
        TitleAppear();
    }
    public void TitleAppear() {
        StartCoroutine(Delayed(0.5f, titleUI, startUI, endUI));
    }
    IEnumerator Delayed(float delayTime, params GameObject[] list) {
        foreach (GameObject obj in list) {
            yield return new WaitForSeconds(delayTime);
            SetActiveTrue(obj);
        }
    }
    private void SetActiveTrue(GameObject obj) {
        obj.SetActive(true);
    }
    public void StartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
