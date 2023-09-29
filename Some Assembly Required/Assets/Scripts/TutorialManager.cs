using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialManager : MonoBehaviour
{
    public AudioClip[] Clips;
    // public GameObject[] Dialogs;
    public Entry[] Entries;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Start()
    {
        PlayClip(0);
    }

    private void TurnOffAllDialogs(int ix)
    {
        HashSet<GameObject> turnOn = new HashSet<GameObject>();
        foreach (GameObject obj in Entries[ix].ObjectToTurnOn)
        {
            obj.SetActive(true);
            turnOn.Add(obj);
        }
        for (int i = 0; i < Entries.Length; i++)
        {
            if (ix == i) { continue; }
            Entry entry = Entries[i];
            foreach (GameObject obj in entry.ObjectToTurnOn)
            {
                if (turnOn.Contains(obj)) { continue; }
                obj.SetActive(false);
            }
        }
    }

    public void PlayClip(int ix)
    {
        TurnOffAllDialogs(ix);
        _audioSource.volume = VolumeController.SFXVolume;
        _audioSource.clip = Clips[ix];
        _audioSource.Play();
    }

    public void MainMenu()
    {
        StartCoroutine(TitleScreenController.LoadYourAsyncScene("Title Sceen"));
    }

    

    [System.Serializable]
    public struct Entry
    {
        public GameObject[] ObjectToTurnOn;
    }
}
