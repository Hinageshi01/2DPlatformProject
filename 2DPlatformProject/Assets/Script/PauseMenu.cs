using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;
    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame() {
        Application.Quit();
    }
    public void SetAudioVolume(float volume) {
        audioMixer.SetFloat("MainVolume", volume);
    }
}
