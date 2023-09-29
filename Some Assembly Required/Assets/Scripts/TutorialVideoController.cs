
using UnityEngine;
using UnityEngine.Video;
public class TutorialVideoController : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject VideoPlayerDialog;
    public GameObject Dialog;
    public GameObject VideoMenu;
    public VideoClip[] VideoClips;

    public void PlayVideo(int ix)
    {
        VideoPlayerDialog.SetActive(true);
        VideoMenu.SetActive(false);
        VideoPlayer.clip = VideoClips[ix];
        VideoPlayer.Play();
    }

    public void ShowOptions()
    {
        Dialog.SetActive(true);
        VideoMenu.SetActive(true);
        VideoPlayerDialog.SetActive(false);
    }

    public void CloseDialog()
    {
        CloseVideo();
        Dialog.SetActive(false);
    }
    
    public void CloseVideo()
    {
        VideoPlayer.Stop();
        VideoPlayerDialog.SetActive(false);
        VideoMenu.SetActive(true);

    }
}