using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen, resultsScreen, loadingScreen, resumeButton;
    [SerializeField] private Slider slider;

    private void Update()
    {
        if (Input.GetAxis("Cancel") > 0)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
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
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    public void QuitGame(string levelToLoad)
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        loadingScreen.SetActive(true);
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