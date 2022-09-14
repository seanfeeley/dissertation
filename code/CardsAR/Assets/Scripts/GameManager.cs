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
        UpdateGameState(this.State);    
    }
    public void UpdateGameStateTo_NoneHighlighted_NoneHeld() { this.UpdateGameState(GameState.NoneHighlighted_NoneHeld); }
    public void UpdateGameStateTo_NoneHighlighted_OneHeld() { this.UpdateGameState(GameState.NoneHighlighted_OneHeld); }
    public void UpdateGameStateTo_NoneHighlighted_ManyHeld() { this.UpdateGameState(GameState.NoneHighlighted_ManyHeld); }
    public void UpdateGameStateTo_OneHighlighted_NoneHeld() { this.UpdateGameState(GameState.OneHighlighted_NoneHeld); }
    public void UpdateGameStateTo_OneHighlighted_OneHeld() { this.UpdateGameState(GameState.OneHighlighted_OneHeld); }
    public void UpdateGameStateTo_OneHighlighted_ManyHeld() { this.UpdateGameState(GameState.OneHighlighted_ManyHeld); }
    public void UpdateGameStateTo_ManyHighlighted_NoneHeld() { this.UpdateGameState(GameState.ManyHighlighted_NoneHeld); }
    public void UpdateGameStateTo_ManyHighlighted_OneHeld() { this.UpdateGameState(GameState.ManyHighlighted_OneHeld); }
    public void UpdateGameStateTo_ManyHighlighted_ManyHeld() { this.UpdateGameState(GameState.ManyHighlighted_ManyHeld); }
    public void UpdateGameStateTo_SpreadHighlighted_NoneHeld() { this.UpdateGameState(GameState.SpreadHighlighted_NoneHeld); }
    public void UpdateGameStateTo_SpreadHighlighted_OneHeld() { this.UpdateGameState(GameState.SpreadHighlighted_OneHeld); }
    public void UpdateGameStateTo_SpreadHighlighted_ManyHeld() { this.UpdateGameState(GameState.SpreadHighlighted_ManyHeld); }
    public void UpdateGameStateTo_DetectingFloor() { this.UpdateGameState(GameState.DetectingFloor); }
    public void UpdateGameStateTo_TableMoving() { this.UpdateGameState(GameState.TableMoving); }
    public void UpdateGameStateTo_SelectingNumber_Split() { this.UpdateGameState(GameState.SelectingNumber_Split); }
    public void UpdateGameStateTo_SelectingNumber_Deal() { this.UpdateGameState(GameState.SelectingNumber_Deal); }



    public void UpdateGameState(GameState newState){
        State = newState;

        switch (newState)
        {
            case GameState.SpreadHighlighted_NoneHeld:
                break;
            case GameState.SpreadHighlighted_OneHeld:
                break;
            case GameState.SpreadHighlighted_ManyHeld:
                break;
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

    SpreadHighlighted_NoneHeld,
    SpreadHighlighted_OneHeld,
    SpreadHighlighted_ManyHeld,


    DetectingFloor,
    TableMoving,
    SelectingNumber_Split,
    SelectingNumber_Deal,


}