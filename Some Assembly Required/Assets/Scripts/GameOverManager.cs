using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverScreen;
    [SerializeField]
    private GameObject[] _disableOnGameOver;

    public void GameOver()
    {
        _gameOverScreen.SetActive(true);
        foreach (GameObject obj in _disableOnGameOver)
        {
            obj.SetActive(false);
        }
    }
    
}
