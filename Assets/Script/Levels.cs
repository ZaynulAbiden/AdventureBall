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
        foreach (var level in allLevels)
        {
            level.SetActive(false);
        }
        LevelSetup(-1);
    }
    public void LevelSetup(int i)
    {
        if(currentLevel) currentLevel.SetActive(false);

        if (i > -1)
        {
            PlayerPrefs.SetInt(nameof(selectedLevelIndex), i);
            selectedLevelIndex = i;
        }

        currentLevel = allLevels[selectedLevelIndex];

        currentLevel.SetActive(true);
        ball.SetBallPosition();
    }
}
