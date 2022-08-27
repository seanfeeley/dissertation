using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.OnGameStatesChanged += GameStateChanged;
    }


    private void OnDestroy()
    {
        GameManager.OnGameStatesChanged -= GameStateChanged;
    }



    private void GameStateChanged(GameState state)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
