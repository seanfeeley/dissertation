using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public Color playerColor = Color.magenta;

    public GameObject cursor;

    public static PlayerManager Instance;

    public List<GameObject> HighlightedCards = new List<GameObject>();

    public List<GameObject> HeldCards = new List<GameObject>();


    private void Awake()
    {
        Instance = this;

    }

    internal void StartHighlightingCard(GameObject card)
    {

        if (!PlayerManager.Instance.HighlightedCards.Contains(card))
        {
            this.HighlightedCards.Add(card);
            card.GetComponent<StickyCardManager>().highlighted = true;
            card.GetComponent<StickyCardManager>().outlineColor = playerColor;
            this.UpdateGameState();
        }
    }

    private void UpdateGameState()
    {

        if(HeldCards.Count == 0 && HighlightedCards.Count == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_NoneHeld);}
        if(HeldCards.Count == 1 && HighlightedCards.Count == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_OneHeld);}
        if(HeldCards.Count >= 2 && HighlightedCards.Count == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_ManyHeld);}
        if(HeldCards.Count == 0 && HighlightedCards.Count == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_NoneHeld);}
        if(HeldCards.Count == 1 && HighlightedCards.Count == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_OneHeld);}
        if(HeldCards.Count >= 2 && HighlightedCards.Count == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_ManyHeld);}
        if(HeldCards.Count == 0 && HighlightedCards.Count >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_NoneHeld);}
        if(HeldCards.Count == 1 && HighlightedCards.Count >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_OneHeld);}
        if(HeldCards.Count >= 2 && HighlightedCards.Count >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_ManyHeld);}
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SetCursorColor();
    }

    private void SetCursorColor()
    {
        Transform highlightTransform = cursor.transform.Find("Sphere");
        Material highlightMaterial = highlightTransform.GetComponent<Renderer>().material;
        highlightMaterial.SetColor("_Color", playerColor);
    }

    internal void StopHighlightingcard(GameObject card)
    {
        if (PlayerManager.Instance.HighlightedCards.Contains(card))
        {
            this.HighlightedCards.Remove(card);
            card.GetComponent<StickyCardManager>().highlighted = false;
            
            this.UpdateGameState();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
    }
}
