using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SFXPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    public CardAudioDatabase CardAudioDatabase;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlayClickSound() => PlaySound(CardAudioDatabase.Click);

    private void PlaySound(AudioClip clip)
    {
        _audioSource.volume = VolumeController.SFXVolume;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

}
