using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;

    public static event Action<GameState> OnGameStatesChanged;

    private void Awake()
    {
        Instance = this;
     
    }

    void Start()
    {
        UpdateGameState(GameState.OneHighlighted_NoneHeld);    
    }


    public void UpdateGameState(GameState newState){
        State = newState;

        switch (newState)
        {
            case GameState.NoneHighlighted_NoneHeld:
                break;
            case GameState.NoneHighlighted_OneHeld:
                break;
            case GameState.NoneHighlighted_ManyHeld:
                break;
            case GameState.OneHighlighted_NoneHeld:
                break;
            case GameState.OneHighlighted_OneHeld:
                break;
            case GameState.OneHighlighted_ManyHeld:
                break;
            case GameState.ManyHighlighted_NoneHeld:
                break;
            case GameState.ManyHighlighted_OneHeld:
                break;
            case GameState.ManyHighlighted_ManyHeld:
                break;
            case GameState.DetectingFloor:
                break;
            case GameState.TableMoving:
                break;
            case GameState.SelectingNumber_Split:
                break;
            case GameState.SelectingNumber_Deal:
                break;
            default:
                break;

        }

        OnGameStatesChanged?.Invoke(newState);
    }
}



public enum GameState
{
    NoneHighlighted_NoneHeld,
    NoneHighlighted_OneHeld,
    NoneHighlighted_ManyHeld,
    OneHighlighted_NoneHeld,
    OneHighlighted_OneHeld,
    OneHighlighted_ManyHeld,
    ManyHighlighted_NoneHeld,
    ManyHighlighted_OneHeld,
    ManyHighlighted_ManyHeld,

    DetectingFloor,
    TableMoving,
    SelectingNumber_Split,
    SelectingNumber_Deal,


}