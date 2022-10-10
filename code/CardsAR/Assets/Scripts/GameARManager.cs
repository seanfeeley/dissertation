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
                this.StopMovingTable();
                break;
            case GameState.NoneHighlighted_OneHeld:
                this.StopMovingTable();
                break;
            case GameState.NoneHighlighted_ManyHeld:
                this.StopMovingTable();
                break;
            case GameState.OneHighlighted_NoneHeld:
                this.StopMovingTable();
                break;
            case GameState.OneHighlighted_OneHeld:
                this.StopMovingTable();
                break;
            case GameState.OneHighlighted_ManyHeld:
                this.StopMovingTable();
                break;
            case GameState.ManyHighlighted_NoneHeld:
                this.StopMovingTable();
                break;
            case GameState.ManyHighlighted_OneHeld:
                this.StopMovingTable();
                break;
            case GameState.ManyHighlighted_ManyHeld:
                this.StopMovingTable();
                break;
            case GameState.DetectingFloor:
                this.StartDetectingFloor();
                break;
            case GameState.TableMoving:
                this.StartMovingTable();
                break;
            case GameState.SelectingNumber_Split:
                this.StopMovingTable();
                break;
            case GameState.SelectingNumber_Deal:
                this.StopMovingTable();
                break;
            default:
                break;

        }
    }

    private void StopMovingTable()
    {
        EnvironmentManager.Instance.LockTable();
    }

    private void StartMovingTable()
    {
        EnvironmentManager.Instance.LockFloor();
    }

    private void StartDetectingFloor()
    {
        EnvironmentManager.Instance.UnlockFloor();


    }
}
