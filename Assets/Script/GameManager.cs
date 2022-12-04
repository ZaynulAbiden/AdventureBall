using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    #endregion

    #region Data Member
    public bool isGameRunning;
    public int Keys;
    public int coins;


    #endregion

    #region Data Member
    [Header("Main Menu UI Items")]
    public GameObject mainMenu;
    public Transform keysContainer;
    public Text coinTxt;
    [Header("In Game UI Items")]
    public GameObject inGameUI;
    public GameObject pausePanel;
    public Transform ballLeftContainer;
    public Text levelNoTxt;
    [Header(" Other Panels")]
    public GameObject gameOverPanel;
    #endregion

    #region Unity Methods

    public void Start()
    {
        LoadMainMenu();
    }

    #endregion

    #region Gameplay Functions
    public void StartGame()
    {
        isGameRunning = true;
        mainMenu.SetActive(false);
        inGameUI.SetActive(true);
        levelNoTxt.text = "Level " + (Levels.instance.selectedLevelIndex + 1);
        UpdateBallsLeftContainer(3);
    }

    public void GameOver()
    {
        isGameRunning = false;
        inGameUI.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        inGameUI.SetActive(false);

    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);

    }
    public void NextLevel()
    {
        Levels.instance.LevelSetup( ++Levels.instance.selectedLevelIndex);
        levelNoTxt.text = "Level " + (Levels.instance.selectedLevelIndex + 1);
        
    }
    public void LoadMainMenu()
    {
        mainMenu.SetActive(true);
        GameManager.instance.UpdateKeysContainer();
        GameManager.instance.UpdateCoinsContainer();
    }
    #endregion

    


    #region Update Container Values
    public void UpdateCoinsContainer()
    {
        coinTxt.text = GameManager.instance.coins.ToString();
    }
    public void UpdateKeysContainer()
    {
        int i = 0;
        while (i < GameManager.instance.Keys)
            keysContainer.GetChild(i++).gameObject.SetActive(true);
        while (i < keysContainer.childCount)
            keysContainer.GetChild(i++).gameObject.SetActive(false);
    }
    public void UpdateBallsLeftContainer(int balls)
    {
        BallController.instance.ballsLeft = balls;
        int i = 0;
        while (i < BallController.instance.ballsLeft)
            ballLeftContainer.GetChild(i++).gameObject.SetActive(true);
        while (i < ballLeftContainer.childCount)
            ballLeftContainer.GetChild(i++).gameObject.SetActive(false);
    }
    #endregion

}
