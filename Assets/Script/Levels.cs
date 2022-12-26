using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    #region Singleton
    public static Levels instance;
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
    public GameObject currentLevel;
    public GameObject[] allLevels;
    public int selectedLevelIndex;
    public BallController ball;

    private void Start()
    {
        selectedLevelIndex= PlayerPrefs.GetInt(nameof(selectedLevelIndex),0);
        foreach (var level in allLevels)
        {
            level.SetActive(false);
        }
    }
    public void LevelSetup(int i)
    {
        if(currentLevel) currentLevel.SetActive(false);

        PlayerPrefs.SetInt(nameof(selectedLevelIndex), i);
        selectedLevelIndex = i;
        BallController.instance. checkPoint = null;
        currentLevel = allLevels[selectedLevelIndex];

        currentLevel.SetActive(true);
        ball.SetBallPosition();
    }
}
