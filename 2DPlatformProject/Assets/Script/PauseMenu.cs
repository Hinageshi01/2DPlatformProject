using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseMenu.activeSelf) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            ResumeGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    private void OnApplicationPause(bool pause) {
        if (pause) {
            PauseGame();
        }
    }
    public void PauseGame() {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void ResumeGame() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    public void BackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void RestartGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    public void QuitGame() {
        Application.Quit();
    }
    public void SetAudioVolume(float volume) {
        audioMixer.SetFloat("MainVolume", volume);
    }
}
