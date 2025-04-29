using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuMainController : MonoBehaviour
{
    [Header("Options Menus")]
    [SerializeField] private GameObject optionsMenuPanel_1;
    [SerializeField] private GameObject optionsMenuPanel_2;
    [Header("Options Buttons")]
    [SerializeField] private GameObject optionsCloseButton_1;
    [SerializeField] private GameObject optionsCloseButton_2;
    [SerializeField] private GameObject optionsOpenButton;
    [Header("Game Select Buttons")]
    [SerializeField] private GameObject tutorialSelectButton;
    [Header("Loading Screens")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loadingScreen;
    [Header("Slider")]
    [SerializeField] private Slider slider;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(tutorialSelectButton);
    }

    public void OptionsMenuClose()
    {
        EventSystem.current.SetSelectedGameObject(optionsOpenButton);
        optionsMenuPanel_1.SetActive(false);
        optionsMenuPanel_2.SetActive(false);
    }

    public void OptionsMenuOpen_1()
    {
        optionsMenuPanel_1.SetActive(true);
        optionsMenuPanel_2.SetActive(false);
        EventSystem.current.SetSelectedGameObject(optionsCloseButton_1);
    }
    public void OptionsMenuOpen_2()
    {
        optionsMenuPanel_1.SetActive(false);
        optionsMenuPanel_2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsCloseButton_2);
    }

    public void EnterTutorial(string levelToLoad)
    {
        mainMenuPanel.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    public void EnterLayout1(string levelToLoad)
    {
        mainMenuPanel.SetActive(false);
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

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}