
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ReadyText : MonoBehaviour
{

    public GameManager GameManager;
    public GameObject UserInterface;


    public void Update()
    {
        transform.position = new Vector2(0, Mathf.Sin(Time.time));
        if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            this.gameObject.SetActive(false);
            GameManager.SetupBoard();
            UserInterface.SetActive(true);
        }
    }


}