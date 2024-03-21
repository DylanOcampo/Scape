using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInterface : MonoBehaviour
{
    public GameObject MainPlayer;
    public GameObject OtherPlayersContainer;
    public TextMeshProUGUI CardsInDeck;
    public TextMeshProUGUI CardsInPile;
    public CardHolder PileCard;

    public List<GameObject> possiblePlayers = new List<GameObject>();
    
}
