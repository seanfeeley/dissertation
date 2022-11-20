using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState CurrentState;

    public static event Action<GameState> OnGameStatesChanged;

    GameState[] CardPlayingStates = new GameState[]
    {
        GameState.NoneHighlighted_NoneHeld,
        GameState.NoneHighlighted_OneHeld,
        GameState.NoneHighlighted_ManyHeld,

        GameState.OneHighlighted_NoneHeld,
        GameState.OneHighlighted_OneHeld,
        GameState.OneHighlighted_ManyHeld,

        GameState.ManyHighlighted_NoneHeld,
        GameState.ManyHighlighted_OneHeld,
        GameState.ManyHighlighted_ManyHeld,

        GameState.SpreadHighlighted_NoneHeld,
        GameState.SpreadHighlighted_OneHeld,
        GameState.SpreadHighlighted_ManyHeld
    };

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
     
    }

    void Start()
    {
        OnGameStatesChanged?.Invoke(this.CurrentState);
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
    public void UpdateGameStateTo_ResetARInfo() { this.UpdateGameState(GameState.ARResetInfo); }
    public void UpdateGameStateTo_ResetAR() { this.UpdateGameState(GameState.ARReseting); }
    public void UpdateGameStateTo_LockAR() { this.UpdateGameState(GameState.ARLocked); }
    public void UpdateGameStateTo_SelectingNumber_Split() { this.UpdateGameState(GameState.SelectingNumber_Split); }
    public void UpdateGameStateTo_SelectingNumber_Deal() { this.UpdateGameState(GameState.SelectingNumber_Deal); }
    public void UpdateGameStateTo_SettingDisplayName() { this.UpdateGameState(GameState.SettingDisplayName); }

    public bool InCardGameplayMode()
    {

        return CardPlayingStates.Contains(CurrentState);
    }

    private void UpdateGameState(GameState newState){
        GameState OldState = CurrentState;

        CurrentState = newState;
    
        if (OldState != CurrentState)
        {
            OnGameStatesChanged?.Invoke(this.CurrentState);
        }


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


    SettingDisplayName,
    ARResetInfo,
    ARReseting,
    ARLocked,
    SelectingNumber_Split,
    SelectingNumber_Deal,

}
