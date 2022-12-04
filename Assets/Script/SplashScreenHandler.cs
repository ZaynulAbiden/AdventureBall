using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenHandler : MonoBehaviour
{
    public Image loadingBar;
    public int loadTimer = 3;
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        loadingBar.fillAmount = 0;
        float timer = 0;
        AsyncOperation scene = SceneManager.LoadSceneAsync("Gameplay");
        scene.allowSceneActivation = false;

        while (!scene.isDone && timer<=loadTimer)
        {
            timer += Time.deltaTime;
            loadingBar.fillAmount = timer / loadTimer;
            yield return null;
        }
        scene.allowSceneActivation = true;
    }
}
