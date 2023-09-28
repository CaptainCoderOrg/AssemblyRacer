using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameWonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameWonScreen;
    [SerializeField]
    private GameObject[] _disableOnGameWon;

    public void Win()
    {
        _gameWonScreen.SetActive(true);
        foreach (GameObject obj in _disableOnGameWon)
        {
            obj.SetActive(false);
        }
    }
    
}
