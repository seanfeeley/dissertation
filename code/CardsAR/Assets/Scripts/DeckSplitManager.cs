using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckSplitManager : MonoBehaviour
{
    public Slider CardCountSlider;

    public GameObject cardsCount;

    private int _splitCount = 10;
    private int _deckHeight = 0;
    private int _selectionSize = 0;
    private GameObject _bottomCard = null;


    public static DeckSplitManager Instance;
    private void Awake()
    {
        Instance = this;
    }



    void Update()
    {
        
        float sliderValue = CardCountSlider.value;
        _selectionSize = (int)((float)(_deckHeight) * sliderValue);
        cardsCount.GetComponent<TextMeshProUGUI>().text = _selectionSize.ToString();


        //Debug.Log(cardCount.ToString()+"/"+ deckCount.ToString());
    }
    public void TriggerPickup()
    {
        int counter = _selectionSize;
        if (_selectionSize != 0)
        {

            GameObject cardSelect = this._bottomCard.GetComponent<StickyCardManager>().GetTopCard();
            StickyCardManager stickyCardSelect = cardSelect.GetComponent<StickyCardManager>();
            while (counter > 1)
            {
                counter--;
                cardSelect = stickyCardSelect.Below;
                if (cardSelect) {
                    stickyCardSelect = cardSelect.GetComponent<StickyCardManager>();
                }
            }

            NetworkCardManager.Instance.DeckPickup(stickyCardSelect.cardID);
            
        }
        GameManager.Instance.UpdateGameStateTo_ManyHighlighted_ManyHeld();
    }

    internal void SetBottomCard(GameObject BottomCard)
    {
        int previousDeckHeight = this._deckHeight;

        this._bottomCard = BottomCard;
        this._deckHeight = BottomCard.GetComponent<StickyCardManager>().CountAbove();

        float newSliderValue = (float)(this._selectionSize) / (float)(this._deckHeight);

        if (newSliderValue > 1)
        {
            newSliderValue = 1;
        }

        CardCountSlider.value = newSliderValue;
        
    }
}
