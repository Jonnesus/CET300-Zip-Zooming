using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen, resultsScreen, loadingScreen, resumeButton;
    [SerializeField] private Slider slider;
    [SerializeField] private AdaptiveDifficulty adaptiveDifficulty;

    private void Update()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame(string levelToLoad)
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    public void QuitGame(string levelToLoad)
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        SaveSystem.SaveDifficulty(adaptiveDifficulty);
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            slider.value = progressValue;
            yield return null;
        }
    }
}