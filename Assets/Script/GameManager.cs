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
    public int Coins;

    [Header("Main Menu UI Items")]
    public GameObject mainMenu;
    public Text coinsMenuTxt;
    public Text keysMenuTxt;
    [Header("In Game UI Items")]
    public GameObject inGameUI;
    public GameObject pausePanel;
    public Transform ballLeftContainer;
    public Text keysGameTxt;
    public Text coinsGameTxt;
    public Text levelNoTxt;
    public Image levelBar;
    [Header(" Other Panels")]
    public GameObject gameOverPanel;
    [Header("Controls ")]
    public bool joystickMode;
    public GameObject joystickBtn;
    public Slider volumeBar;
    #endregion
    #region Unity Methods

    public void Start()
    {
        LoadMainMenu();
        volumeBar.value = PlayerPrefs.GetFloat(nameof(volumeBar), 1);

    }

    #endregion
    #region Gameplay Functions
    public void StartGame(int ball=3)
    {
        isGameRunning = true;
        mainMenu.SetActive(false);
        inGameUI.SetActive(true);
        levelNoTxt.text = "Level " + (Levels.instance.selectedLevelIndex + 1);
        if (ball >= 3)
            BallController.instance.checkPoint = null;
        UpdateBallsLeftContainer(ball);
        AudioManager.instance.PlayMusic("Game Running");
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
        inGameUI.SetActive(true);
    }
    public void ResetGameplay()
    {
        isGameRunning = false;
        Time.timeScale = 1;
        Levels.instance.LevelSetup(Levels.instance.selectedLevelIndex);
    }
    public void LoadMainMenu()
    {
        ResetGameplay();
        AudioManager.instance.PlayMusic("Main Menu");
        mainMenu.SetActive(true);
        GameManager.instance.UpdateKeys(0);
        GameManager.instance.UpdateCoins(0);
    }   
    public void ChangeControl()
    {
        joystickMode = !joystickMode;
        joystickBtn.SetActive(joystickMode);
    }

    public void ChangeVolume( ) {
        PlayerPrefs.SetFloat(nameof(volumeBar),volumeBar.value);    
        AudioManager.instance.musicSource.volume = volumeBar.value;
        AudioManager.instance.sfxSource.volume = volumeBar.value;
        BallController.instance.rollingSound.volume = volumeBar.value;
    }

    #endregion
    #region Update Container Values
    public void UpdateCoins(int coin)
    {
        Coins += coin;
        coinsGameTxt.text = Coins.ToString();
        coinsMenuTxt.text = Coins.ToString();
    }
    public void UpdateKeys(int key)
    {
        Keys += key;
        keysGameTxt.text = Keys.ToString();
        keysMenuTxt.text = Keys.ToString();

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
