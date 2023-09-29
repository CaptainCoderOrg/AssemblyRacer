using UnityEngine;

public static class VolumeController
{
    private static float? _sfxVolume;
    public static float SFXVolume
    {
        get
        {
            if (_sfxVolume == null)
            {
                _sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.25f);
            }
            return _sfxVolume.Value;
        }
        set
        {
            PlayerPrefs.SetFloat("sfxVolume", value);
            _sfxVolume = value;
        }
    }

    private static float? _musicVolume;
    public static float MusicVolume
    {
        get
        {
            if (_musicVolume == null)
            {
                _musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.25f);
            }
            return _musicVolume.Value;
        }
        set
        {
            PlayerPrefs.SetFloat("musicVolume", value);
            _musicVolume = value;
        }
    }
}