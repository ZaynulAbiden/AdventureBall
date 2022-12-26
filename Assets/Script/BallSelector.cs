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
        selectedBallIndex = PlayerPrefs.GetInt(nameof(selectedBallIndex), 0);
        BallSetup(selectedBallIndex);

    }
    public void BallSetup(int i)
    {
        PlayerPrefs.SetInt(nameof(selectedBallIndex), i);
        selectedBallIndex = i;
        if (currentBall) Destroy(currentBall.gameObject);
        currentBall = Instantiate(allBalls[selectedBallIndex],BallController.instance.transform);
        AudioManager.instance.PlaySFX("Ball Select");
    }


}
