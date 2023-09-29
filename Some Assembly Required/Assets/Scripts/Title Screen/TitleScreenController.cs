using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    
    public void Play()
    {
        StartCoroutine(LoadYourAsyncScene("Game"));
    }

    public void Tutorial()
    {
        StartCoroutine(LoadYourAsyncScene("Tutorial"));
    }

    public static IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}