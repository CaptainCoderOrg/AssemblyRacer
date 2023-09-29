using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public static bool Loaded = false;
    public GameObject Dialog;
    public Slider MusicSlider;
    public Slider SoundSlider;
    public AudioSource AudioSource;

    public void Awake()
    {
        if (Loaded) { 
            Destroy(this.gameObject);
            return;
        }
        Loaded = true;
        MusicSlider.value = VolumeController.MusicVolume;
        AudioSource.volume = MusicSlider.value;
        SoundSlider.value = VolumeController.SFXVolume;
        MusicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        SoundSlider.onValueChanged.AddListener(UpdateSoundVolume);
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void Toggle()
    {
        Dialog.SetActive(!Dialog.activeInHierarchy);
    }

    public void UpdateMusicVolume(float volume) 
    {
        VolumeController.MusicVolume = volume;
        AudioSource.volume = volume;
    }

    public void UpdateSoundVolume(float volume) => VolumeController.SFXVolume = volume;

    public void ToTitleScreen()
    {
        StartCoroutine(TitleScreenController.LoadYourAsyncScene("TitleSceen"));
    }

}
