using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSelector : MonoBehaviour
{
    public Transform currentBall;
    public Transform[] allBalls;
    public int selectedBallIndex;

    private void Start()
    {
        BallSetup(-1);

    }
    public void BallSetup(int i)
    {
        if (i > -1)
        {
            PlayerPrefs.SetInt(nameof(selectedBallIndex), i);
            selectedBallIndex = i;
        }

        if (currentBall) Destroy(currentBall.gameObject);
        currentBall = Instantiate(allBalls[selectedBallIndex],BallController.instance.transform);
    }
}
