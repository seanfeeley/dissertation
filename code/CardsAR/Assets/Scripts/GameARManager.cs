using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameARManager : MonoBehaviour
{

    public static GameARManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStatesChanged += GameStateChanged;
    }


    private void OnDestroy()
    {
        GameManager.OnGameStatesChanged -= GameStateChanged;
    }


    private void GameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.NoneHighlighted_NoneHeld:
                this.ARResetFinished();
                break;
            case GameState.NoneHighlighted_OneHeld:
                this.ARResetFinished();
                break;
            case GameState.NoneHighlighted_ManyHeld:
                this.ARResetFinished();
                break;
            case GameState.OneHighlighted_NoneHeld:
                this.ARResetFinished();
                break;
            case GameState.OneHighlighted_OneHeld:
                this.ARResetFinished();
                break;
            case GameState.OneHighlighted_ManyHeld:
                this.ARResetFinished();
                break;
            case GameState.ManyHighlighted_NoneHeld:
                this.ARResetFinished();
                break;
            case GameState.ManyHighlighted_OneHeld:
                this.ARResetFinished();
                break;
            case GameState.ManyHighlighted_ManyHeld:
                this.ARResetFinished();
                break;
            case GameState.ARReseting:
                this.ARReseting();
                break;
            case GameState.SelectingNumber_Split:
                this.ARResetFinished();
                break;
            case GameState.SelectingNumber_Deal:
                this.ARResetFinished();
                break;
            default:
                break;

        }
    }

    private void ARResetFinished()
    {
        EnvironmentManager.Instance.FinishARReset();
    }


    private void ARReseting()
    {
        EnvironmentManager.Instance.ARReseting();


    }
}
