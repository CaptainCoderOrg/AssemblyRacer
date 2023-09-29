using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public DifficultyData DifficultyData;
    public BossCardData[] Difficulties;
    public AudioClip TitleScreenMusic;

    public void Start()
    {
        if (!MusicController.Instance) { return; }
        if (MusicController.Instance.AudioSource.clip == TitleScreenMusic) { return; }
        MusicController.Instance.AudioSource.clip = TitleScreenMusic;
        MusicController.Instance.AudioSource.Play();
    }
    
    public void Play(int difficulty)
    {
        DifficultyData.BossSetting = Difficulties[difficulty];
        // StartCoroutine(LoadYourAsyncScene("Game"));
        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        SceneManager.LoadScene("Game");
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
