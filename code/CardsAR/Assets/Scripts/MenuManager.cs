using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject Button_ConfirmFloor;
    public GameObject Button_ConfirmTable;
    public GameObject Button_ResetAR;
    public GameObject Button_CardBottom;
    public GameObject Button_DeckShuffle;
    public GameObject Button_DeckDeal;
    public GameObject Button_DeckSplit;
    public GameObject Button_DeckUp;
    public GameObject Button_CardTop;
    public GameObject Button_CardMiddle;
    public GameObject Button_DeckFlip;
    public GameObject Button_CardFlip;


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
        switch (state)
        {
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
            case GameState.DetectingFloor:
                this.refreshButtonsFor_DetectingFloor();
                break;
            case GameState.TableMoving:
                this.refreshButtonsFor_TableMoving();
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
        this.hideCardButtonIcons(Button_CardBottom);
        this.hideCardButtonIcons(Button_DeckShuffle);
        this.hideCardButtonIcons(Button_DeckDeal);
        this.hideCardButtonIcons(Button_DeckSplit);
        this.hideCardButtonIcons(Button_DeckUp);
        this.hideCardButtonIcons(Button_CardTop);
        this.hideCardButtonIcons(Button_CardMiddle);
        this.hideCardButtonIcons(Button_DeckFlip);
        this.hideCardButtonIcons(Button_CardFlip);
    }
    private void setButtonVisible(GameObject button, bool enabled)
    {
        button.gameObject.SetActive(enabled);
    }
    private void hideAllButtons()
    {
        this.setButtonVisible(Button_ConfirmFloor, false);
        this.setButtonVisible(Button_ConfirmTable, false);
        this.setButtonVisible(Button_ResetAR, false);
        this.setButtonVisible(Button_CardBottom, false);
        this.setButtonVisible(Button_DeckShuffle, false);
        this.setButtonVisible(Button_DeckDeal, false);
        this.setButtonVisible(Button_DeckSplit, false);
        this.setButtonVisible(Button_DeckUp, false);
        this.setButtonVisible(Button_CardTop, false);
        this.setButtonVisible(Button_CardMiddle, false);
        this.setButtonVisible(Button_DeckFlip, false);
        this.setButtonVisible(Button_CardFlip, false);
    }
    private void showAllCardButtons()
    {
        this.setButtonVisible(Button_ResetAR, true);
        this.setButtonVisible(Button_CardBottom, true);
        this.setButtonVisible(Button_DeckShuffle, true);
        this.setButtonVisible(Button_DeckDeal, true);
        this.setButtonVisible(Button_DeckSplit, true);
        this.setButtonVisible(Button_DeckUp, true);
        this.setButtonVisible(Button_CardTop, true);
        this.setButtonVisible(Button_CardMiddle, true);
        this.setButtonVisible(Button_DeckFlip, true);
        this.setButtonVisible(Button_CardFlip, true);
    }
    private void disableAllCardButtons()
    {
        this.setCardButtonEnabled(Button_CardBottom, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", true);
        this.setCardButtonEnabled(Button_CardTop, "Disabled", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Disabled", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", true);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", true);
    }
    private void ResetCardButtonIcons()
    {
        this.setCardButtonEnabled(Button_CardBottom, "Bottom Up", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        this.setCardButtonEnabled(Button_CardTop, "Card Up", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
    }

    private void ResetCardButtons()
    {
        this.hideAllButtons();
        this.showAllCardButtons();
        this.hideAllCardButtonIcons();
        this.disableAllCardButtons();
    }

    private void refreshButtonsFor_NoneHighlighted_NoneHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Up", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        this.setCardButtonEnabled(Button_CardTop, "Card Up", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);


    }
    private void refreshButtonsFor_NoneHighlighted_OneHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Up", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        this.setCardButtonEnabled(Button_CardTop, "Card Down", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);

    }
    private void refreshButtonsFor_NoneHighlighted_ManyHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Up", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Down", true);
        this.setCardButtonEnabled(Button_CardTop, "Card Up", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Top Card Flip", true);
    }
    private void refreshButtonsFor_OneHighlighted_NoneHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Up", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        this.setCardButtonEnabled(Button_CardTop, "Card Up", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
    }
    private void refreshButtonsFor_OneHighlighted_OneHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Down", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        this.setCardButtonEnabled(Button_CardTop, "Top Card Down", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
    }
    private void refreshButtonsFor_OneHighlighted_ManyHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Down", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Top Deck Down", true);
        this.setCardButtonEnabled(Button_CardTop, "Top Card Down", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
    }
    private void refreshButtonsFor_ManyHighlighted_NoneHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Up", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        this.setCardButtonEnabled(Button_CardTop, "Top Card Up", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Up", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Top Card Flip", true);
    }
    private void refreshButtonsFor_ManyHighlighted_OneHeld()
    {

        this.ResetCardButtons();

        this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        //this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Down", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Deck Up", true);
        this.setCardButtonEnabled(Button_CardTop, "Top Card Down", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Down", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Card Flip", true);
    }
    private void refreshButtonsFor_ManyHighlighted_ManyHeld()
    {

        this.ResetCardButtons();

        //this.setCardButtonEnabled(Button_CardBottom, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckShuffle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckDeal, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckSplit, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckUp, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardTop, "Disabled", false);
        //this.setCardButtonEnabled(Button_CardMiddle, "Disabled", false);
        this.setCardButtonEnabled(Button_DeckFlip, "Disabled", false);
        this.setCardButtonEnabled(Button_CardFlip, "Disabled", false);

        this.setCardButtonEnabled(Button_CardBottom, "Bottom Down", true);
        this.setCardButtonEnabled(Button_DeckShuffle, "Icon", true);
        this.setCardButtonEnabled(Button_DeckDeal, "Icon", true);
        this.setCardButtonEnabled(Button_DeckSplit, "Icon", true);
        this.setCardButtonEnabled(Button_DeckUp, "Top Deck Down", true);
        this.setCardButtonEnabled(Button_CardTop, "Top Card Down", true);
        this.setCardButtonEnabled(Button_CardMiddle, "Middle Down", true);
        this.setCardButtonEnabled(Button_DeckFlip, "Icon", true);
        this.setCardButtonEnabled(Button_CardFlip, "Top Card Flip", true);
    }
    private void refreshButtonsFor_DetectingFloor()
    {
        this.hideAllButtons();
        this.setButtonVisible(Button_ConfirmFloor, true);
    }
    private void refreshButtonsFor_TableMoving()
    {
        this.hideAllButtons();
        this.setButtonVisible(Button_ConfirmTable, true);

    }
    private void refreshButtonsFor_SelectingNumber_Split()
    {
        this.hideAllButtons();
    }
    private void refreshButtonsFor_SelectingNumber_Deal()
    {
        this.hideAllButtons();
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
