using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject Button_ARResetInfo;
    public GameObject Button_ARResetConfirm;
    public GameObject Button_ResetAR;
    public GameObject Button_CardBottomUp;
    public GameObject Button_CardBottomDown;
    public GameObject Button_DeckShuffle;
    public GameObject Button_DeckDeal;
    public GameObject Button_DeckSplit;
    public GameObject Button_DeckUp;
    public GameObject Button_DeckDown;
    public GameObject Button_CardTopUp;
    public GameObject Button_CardTopDown;
    public GameObject Button_CardMiddleUp;
    public GameObject Button_CardMiddleDown;
    public GameObject Button_DeckFlip;
    public GameObject Button_CardFlip;
    public GameObject Button_Spread;

    public GameObject Button_ValueSelectorDeal;
    public GameObject Button_ValueSelectorSplit;

    public static MenuManager Instance;
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
            case GameState.SpreadHighlighted_NoneHeld:
                this.refreshButtonsFor_SpreadHighlighted_NoneHeld();
                break;
            case GameState.SpreadHighlighted_OneHeld:
                this.refreshButtonsFor_SpreadHighlighted_OneHeld();
                break;
            case GameState.SpreadHighlighted_ManyHeld:
                this.refreshButtonsFor_SpreadHighlighted_ManyHeld();
                break;
            case GameState.NoneHighlighted_NoneHeld:
                this.refreshButtonsFor_NoneHighlighted_NoneHeld();
                break;
            case GameState.NoneHighlighted_OneHeld:
                this.refreshButtonsFor_NoneHighlighted_OneHeld();
                break;
            case GameState.NoneHighlighted_ManyHeld:
                this.refreshButtonsFor_NoneHighlighted_ManyHeld();
                break;
            case GameState.OneHighlighted_NoneHeld:
                this.refreshButtonsFor_OneHighlighted_NoneHeld();
                break;
            case GameState.OneHighlighted_OneHeld:
                this.refreshButtonsFor_OneHighlighted_OneHeld();
                break;
            case GameState.OneHighlighted_ManyHeld:
                this.refreshButtonsFor_OneHighlighted_ManyHeld();
                break;
            case GameState.ManyHighlighted_NoneHeld:
                this.refreshButtonsFor_ManyHighlighted_NoneHeld();
                break;
            case GameState.ManyHighlighted_OneHeld:
                this.refreshButtonsFor_ManyHighlighted_OneHeld();
                break;
            case GameState.ManyHighlighted_ManyHeld:
                this.refreshButtonsFor_ManyHighlighted_ManyHeld();
                break;
            case GameState.ARResetInfo:
                this.refreshButtonsFor_ARResetInfo();
                break;
            case GameState.ARReseting:
                this.refreshButtonsFor_ARReseting();
                break;
            case GameState.SelectingNumber_Split:
                this.refreshButtonsFor_SelectingNumber_Split();
                break;
            case GameState.SelectingNumber_Deal:
                this.refreshButtonsFor_SelectingNumber_Deal();
                break;
            default:
                break;

        }
    }


    private void setCardButtonEnabled(GameObject button, string Child, bool enabled)
    {
        button.transform.Find(Child).gameObject.SetActive(enabled);
    }
    private void hideCardButtonIcons(GameObject button)
    {
        foreach (Transform child in button.transform)
        {
            child.gameObject.gameObject.SetActive(false);
        }
    }
    private void hideAllCardButtonIcons()
    {
        this.hideCardButtonIcons(Button_CardBottomUp);
        this.hideCardButtonIcons(Button_CardBottomDown);
        this.hideCardButtonIcons(Button_DeckShuffle);
        this.hideCardButtonIcons(Button_DeckDeal);
        this.hideCardButtonIcons(Button_DeckSplit);
        this.hideCardButtonIcons(Button_DeckUp);
        this.hideCardButtonIcons(Button_DeckDown);
        this.hideCardButtonIcons(Button_CardTopUp);
        this.hideCardButtonIcons(Button_CardTopDown);
        this.hideCardButtonIcons(Button_CardMiddleUp);
        this.hideCardButtonIcons(Button_CardMiddleDown);
        this.hideCardButtonIcons(Button_DeckFlip);
        this.hideCardButtonIcons(Button_CardFlip);
        this.hideCardButtonIcons(Button_Spread);
    }
    private void setButtonVisible(GameObject button, bool enabled)
    {
        button.gameObject.SetActive(enabled);
    }
    private void hideAllButtons()
    {
        this.setButtonVisible(Button_ARResetInfo, false);
        this.setButtonVisible(Button_ARResetConfirm, false);
        this.setButtonVisible(Button_ResetAR, false);
        this.setButtonVisible(Button_CardBottomUp, false);
        this.setButtonVisible(Button_CardBottomDown, false);
        this.setButtonVisible(Button_DeckShuffle, false);
        this.setButtonVisible(Button_DeckDeal, false);
        this.setButtonVisible(Button_DeckSplit, false);
        this.setButtonVisible(Button_DeckUp, false);
        this.setButtonVisible(Button_DeckDown, false);
        this.setButtonVisible(Button_CardTopUp, false);
        this.setButtonVisible(Button_CardTopDown, false);
        this.setButtonVisible(Button_CardMiddleUp, false);
        this.setButtonVisible(Button_CardMiddleDown, false);
        this.setButtonVisible(Button_DeckFlip, false);
        this.setButtonVisible(Button_CardFlip, false);
        this.setButtonVisible(Button_Spread, false);
        this.setButtonVisible(Button_ValueSelectorDeal, false);
        this.setButtonVisible(Button_ValueSelectorSplit, false);
    }
    private void showAllCardButtons()
    {
        this.setButtonVisible(Button_ResetAR, true);
        this.setButtonVisible(Button_CardBottomUp, true);
        this.setButtonVisible(Button_CardBottomDown, true);
        this.setButtonVisible(Button_DeckShuffle, true);
        this.setButtonVisible(Button_DeckDeal, true);
        this.setButtonVisible(Button_DeckSplit, true);
        this.setButtonVisible(Button_DeckUp, true);
        this.setButtonVisible(Button_DeckDown, true);
        this.setButtonVisible(Button_CardTopUp, true);
        this.setButtonVisible(Button_CardTopDown, true);
        this.setButtonVisible(Button_CardMiddleUp, true);
        this.setButtonVisible(Button_CardMiddleDown, true);
        this.setButtonVisible(Button_DeckFlip, true);
        this.setButtonVisible(Button_CardFlip, true);
        this.setButtonVisible(Button_Spread, true);
    }
    private void disableAllCardButtons()
    {
        this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", true);
        this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckDown, "Disabled", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Disabled", true);
        this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", true);
        this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", true);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", true);
        this.setCardButtonEnabled(Button_Spread, "Disabled", true);
    }
    private void showStaticCardButtonIcons()
    {
        //this.setCardButtonEnabled(Button_CardBottom, "Bottom Up", true);
        //this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        //this.setCardButtonEnabled(Button_CardTop, "Card Up", true);
        //this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        //this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);

        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);

        this.setCardButtonEnabled(Button_CardMiddleDown, "Icon", true);
        this.setCardButtonEnabled(Button_CardMiddleUp, "Icon", true);

        this.setCardButtonEnabled(Button_CardBottomDown, "Icon", true);
        this.setCardButtonEnabled(Button_CardBottomUp, "Icon", true);

        this.setCardButtonEnabled(Button_DeckUp, "Icon", true);



    }

    private void ResetCardButtons()
    {
        this.hideAllButtons();
        this.showAllCardButtons();
        this.hideAllCardButtonIcons();
        this.showStaticCardButtonIcons();
        this.disableAllCardButtons();
    }

    private void refreshButtonsFor_NoneHighlighted_NoneHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);


    }
    private void refreshButtonsFor_NoneHighlighted_OneHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }
    private void refreshButtonsFor_NoneHighlighted_ManyHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }
    private void refreshButtonsFor_OneHighlighted_NoneHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }
    private void refreshButtonsFor_OneHighlighted_OneHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }
    private void refreshButtonsFor_OneHighlighted_ManyHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Top Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Top Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }
    private void refreshButtonsFor_ManyHighlighted_NoneHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Top Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Top Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Top Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }
    private void refreshButtonsFor_ManyHighlighted_OneHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Top Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Top Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Top Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }
    private void refreshButtonsFor_ManyHighlighted_ManyHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Top Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Top Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Top Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Top Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Spread", true);

    }

    private void refreshButtonsFor_SpreadHighlighted_ManyHeld()
    {
        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Top Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Stack", true);
    }

    private void refreshButtonsFor_SpreadHighlighted_OneHeld()
    {
        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Stack", true);
    }

    private void refreshButtonsFor_SpreadHighlighted_NoneHeld()
    {
        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardTopUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTopDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddleDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardBottomDown, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDown, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_Spread, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);


        this.setCardButtonEnabled(Button_DeckDown, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTopUp, "Card Up", true);
        this.setCardButtonEnabled(Button_CardTopDown, "Card Down", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
        this.setCardButtonEnabled(Button_Spread, "Stack", true);
    }




    private void refreshButtonsFor_ARResetInfo()
    {
        this.hideAllButtons();
        this.setButtonVisible(Button_ARResetInfo, true);
    }
    private void refreshButtonsFor_ARReseting()
    {
        this.hideAllButtons();
        this.setButtonVisible(Button_ARResetConfirm, true);
    }
    private void refreshButtonsFor_TableMoving()
    {
        this.hideAllButtons();
        this.setButtonVisible(Button_ARResetConfirm, true);

    }
    private void refreshButtonsFor_SelectingNumber_Split()
    {
        this.hideAllButtons();
        this.setButtonVisible(Button_ValueSelectorSplit, true);
    }
    private void refreshButtonsFor_SelectingNumber_Deal()
    {
        this.hideAllButtons();
        this.setButtonVisible(Button_ValueSelectorDeal, true);
    }

}
