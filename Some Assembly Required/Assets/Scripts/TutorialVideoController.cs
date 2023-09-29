
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
public class TutorialVideoController : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject VideoPlayerDialog;
    public GameObject Dialog;
    public GameObject VideoMenu;
    public GameObject Loading;
    public string[] VideoClipNames;
    // public string[] VideoClipURL;
    // public string url;

    public void PlayVideo(int ix)
    {
        // url = VideoClipURL[ix];
        StopAllCoroutines();
        VideoPlayerDialog.SetActive(true);
        VideoMenu.SetActive(false);
        VideoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "TutorialVideos", VideoClipNames[ix]);
        StartCoroutine(WaitForVideoAndPlay());
    }


    public void DoPlay()
    {
        StopAllCoroutines();
        Loading.SetActive(false);
        VideoPlayer.Play();
    }

    public void DisplayLoading()
    {
        Loading.SetActive(true);
    }

    public IEnumerator WaitForVideoAndPlay()
    {
        VideoPlayer.Prepare();
        
        while (!VideoPlayer.isPrepared)
        {
            DisplayLoading();
            yield return new WaitForEndOfFrame();
        }
        DoPlay();
    }
    public void PlayInBrowser()
    {
        StopAllCoroutines();
        CloseVideo();
        Application.OpenURL("https://www.youtube.com/playlist?list=PL7C8fMD-89DLszA-4Y5R72P8k9y7W3Zep");
    }

    public void ShowOptions()
    {
        StopAllCoroutines();
        Loading.SetActive(false);
        Dialog.SetActive(true);
        VideoMenu.SetActive(true);
        VideoPlayerDialog.SetActive(false);
    }

    public void CloseDialog()
    {
        StopAllCoroutines();
        Loading.SetActive(false);
        CloseVideo();
        Dialog.SetActive(false);
    }
    
    public void CloseVideo()
    {
        StopAllCoroutines();
        VideoPlayer.Stop();
        Loading.SetActive(false);
        VideoPlayerDialog.SetActive(false);
        VideoMenu.SetActive(true);

    }


}