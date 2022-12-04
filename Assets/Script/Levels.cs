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
    public Transform currentLevel;
    public Transform[] allLevels;
    public int selectedLevelIndex;
    public BallController ball;

    private void Start()
    {
        LevelSetup(-1);

    }
    public void LevelSetup(int i)
    {
        if (i > -1)
        {
            PlayerPrefs.SetInt(nameof(selectedLevelIndex), i);
            selectedLevelIndex = i;
        }

        if (currentLevel)    Destroy(currentLevel.gameObject);
        currentLevel = Instantiate(allLevels[selectedLevelIndex]);
        ball.SetBallPosition(currentLevel.Find("StartingPoint").transform.position);
    }
}
