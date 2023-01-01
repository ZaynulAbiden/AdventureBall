using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSelector : MonoBehaviour
{
    [Header("Ball Selection")]
    public Transform currentBall;
    public List<ball> allBalls = new List<ball>();
    public int selectedBallIndex;

    [Header("Ball Purchase")]
    public GameObject[] purchaseBtns;
    
    private void Start()
    {
        selectedBallIndex = PlayerPrefs.GetInt(nameof(selectedBallIndex), 0);
        BallSetup(selectedBallIndex);
        RefreshBallMenu();
    }   
    public void PurchaseBall(int selectedBall)
    {
        AudioManager.instance.ButtonClick();
        ball requiredBall = allBalls.Find(ball=>ball.ballIndex== selectedBall); 
        if(requiredBall.price > GameManager.instance.Coins)
        {
            print("Not Enough Coins");
            return;
        }
        else
        {
            GameManager.instance.UpdateCoins(requiredBall.price);
            PlayerPrefs.SetInt(purchaseBtns[selectedBall].name, 1);
            RefreshBallMenu();
        }
    }
    public void RefreshBallMenu()
    {
        for (int i = 0; i < purchaseBtns.Length; i++)
        {
            if (PlayerPrefs.GetInt(purchaseBtns[i].name, 0) == 1)
                purchaseBtns[i].SetActive(false);
            else
                purchaseBtns[i].SetActive(true);
        }
    }
    public void BallSetup(int i)
    {
        PlayerPrefs.SetInt(nameof(selectedBallIndex), i);
        selectedBallIndex = i;
        if (currentBall) Destroy(currentBall.gameObject);
        currentBall = Instantiate(allBalls.Find(ball=>ball.ballIndex==selectedBallIndex).prefab.transform,BallController.instance.transform);
        AudioManager.instance.PlaySFX("Ball Select");
    }

    public void UnlockAllBalls()
    {
      //If Successfull -----
        for (int i = 0; i < allBalls.Count; i++)       
            PlayerPrefs.SetInt(purchaseBtns[i].name, 1);
        RefreshBallMenu();
        GameManager.instance.unlockAllBalls.SetActive(true);
    }


  
}

[System.Serializable] public class ball {
    public int ballIndex;
    public int price;
    public GameObject prefab;
}
