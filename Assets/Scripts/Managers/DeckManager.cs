using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DeckManager : MonoBehaviour
{
    private static DeckManager _instance;

    public static DeckManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DeckManager>();
            }
            return _instance;
        }
    }
    [SerializeField] private List<Card> PrivateDeck = new List<Card>();
    [SerializeField] private List<Card> cardDeck = new List<Card>();

    public List<Card> cardPile = new List<Card>();
    public TextMeshProUGUI cardsInDeck;

    public CardHolder pileCard;
    public TextMeshProUGUI cardsInPile;

    public int actualvalue, actualdeckvalue;
    public void InitializeDeck()
    {
        ResetDeck();
        Shuffle(cardDeck);
        DealCardsToPlayers();
        pileCard.gameObject.SetActive(true);
        ClearStuff();
        UpdateDeckTracker();
        //InitializeFirstPileCard();
    }

    private void ClearStuff()
    {
        pileCard.CardsHeld.Clear();
        cardPile.Clear();
        pileCard.name = "card";
        actualvalue = 0;
        actualdeckvalue = 0;
    }

    private void ResetDeck()
    {
        cardDeck.Clear();
        foreach (var item in PrivateDeck)
        {
            cardDeck.Add(item);
        }
    }

    public void UpdateDeckTracker()
    {
        cardsInDeck.text = cardDeck.Count.ToString();
        cardsInPile.text = cardPile.Count.ToString();
    }

    private void DealCardsToPlayers()
    {
        foreach (Player item in GameManager.instance.PlayersInGame())
        {
            //FirstPackage
            item.SetFirstPackage(CreatePackage());
            item.SetSecondPackage(CreatePackage());
            item.SetCardHand(CreatePackage());
        }
        UpdateDeckTracker();
    }

    public void InitializeFirstPileCard()
    {
        pileCard.gameObject.SetActive(true);
        cardPile.Add(cardDeck[0]);
        PileManager.instance.InitializeCard(cardDeck[0]);
        pileCard.InitializeCard(cardDeck[0]);
        cardDeck.RemoveAt(0);

    }

    public void InitializePileCard(CardHolder _cardInfo)
    {
        if (!pileCard.gameObject.activeSelf)
        {
            pileCard.gameObject.SetActive(true);
        }
        for (int i = 0; i < _cardInfo.CardsHeld.Count; i++)
        {
            cardPile.Add(_cardInfo.CardsHeld[i]);
            if (i != 0)
            {
                pileCard.GetComponent<CardHolder>().AddCopy(_cardInfo.CardsHeld[i]);
            }
            PileManager.instance.InitializeCard(_cardInfo.CardsHeld[i]);
            pileCard.InitializeCard(_cardInfo.CardsHeld[i]);

        }
        UpdateDeckTracker();

    }

    private bool CheckForBurn(CardHolder cardPlayed)
    {
        if (pileCard.GetNumberOfCopys() + cardPlayed.GetNumberOfCopys() == 4)
        {
            return true;
        }
        return false;
    }

    private Card[] CreatePackage()
    {
        Card[] Package;
        Package = new Card[3] { cardDeck[0], cardDeck[1], cardDeck[2] };
        cardDeck.RemoveRange(0, 3);
        return Package;
    }

    public void DealPile(Player _player)
    {
        foreach (Card item in cardPile)
        {
            _player.DrawCard(item);
        }
        PileManager.instance.CleanCardPile();
        cardPile.Clear();
        UpdateDeckTracker();
        pileCard.NoCard();
        if (GameManager.instance.TurnPosition == 0)
        {
            UIManager.instance.DealPileEffect();
        }
    }

    private void PlayerForCards(Player _player)
    {
        int difference = 3 - _player.cardHand.Count;
        for (int i = 0; i < difference; i++)
        {
            _player.DrawCard(cardDeck[0]);
            cardDeck.RemoveAt(0);
        }
        UpdateDeckTracker();
    }

    public bool DealCard(Player _player)
    {
        if (cardDeck.Count > 0)
        {
            if (_player.cardHand.Count < 4)
            {
                PlayerForCards(_player);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public void Shuffle(List<Card> self)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = self.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            Card value = self[k];
            self[k] = self[n];
            self[n] = value;
        }

    }


    public void ProcessCard(CardHolder cardPlayed, bool identifier = false)
    {
        if (CheckForBurn(cardPlayed))
        {
            BurnPile();
            GameManager.instance.TurnLogic(true, 0, cardPlayed, identifier);
        }
        else
        {
            if (cardPlayed.GetCard().value == "1")
            {
                InitializePileCard(cardPlayed);
                if (GameManager.instance.TurnPosition == 0)
                {
                    UIManager.instance.AsCardEffect(GameManager.instance.NumOfPlayersInGame());
                }
                else
                {
                    GameManager.instance.TurnLogicAs();
                }

            }
            else if (cardPlayed.GetCard().value == "10")
            {
                BurnPile();
                GameManager.instance.TurnLogic(true, 0, cardPlayed, identifier);
            }
            else
            {
                InitializePileCard(cardPlayed);
                if (cardPlayed.GetCard().value == "8")
                {
                    GameManager.instance.TurnLogic(false, cardPlayed.GetNumberOfCopys(), cardPlayed, identifier);
                }
                else
                {
                    GameManager.instance.TurnLogic(false, 0, cardPlayed, identifier);
                }
            }
        }
    }

    private void BurnPile()
    {
        cardPile.Clear();
        PileManager.instance.CleanCardPile();
        pileCard.gameObject.SetActive(false);
    }

    public bool CanItBePlayed(Card _card)
    {
        if (cardPile.Count == 0)
        {
            return true;
        }
        actualdeckvalue = 11;
        if (pileCard.GetValue() != "K" && pileCard.GetValue() != "Q" && pileCard.GetValue() != "J")
        {
            actualdeckvalue = int.Parse(pileCard.GetValue());
        }
        if (pileCard.GetValue() == "1")
        {
            if (_card.value == "1" || _card.value == "2" || _card.value == "10")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (_card.value != "K" && _card.value != "Q" && _card.value != "J")
        {
            actualvalue = int.Parse(_card.value);
            switch (_card.value)
            {
                case "1":
                    if (actualdeckvalue != 7)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "2":
                    return true;
                case "3":
                    if (actualdeckvalue == 7 || actualdeckvalue <= actualvalue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case "4":
                    if (actualdeckvalue == 7 || actualdeckvalue <= actualvalue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "5":
                    if (actualdeckvalue == 7 || actualdeckvalue <= actualvalue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "6":
                    if (actualdeckvalue == 7 || actualdeckvalue <= actualvalue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "7":
                    if (actualdeckvalue <= actualvalue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "8":
                    if (actualdeckvalue != 7 && actualdeckvalue <= actualvalue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "9":
                    if (actualdeckvalue != 7 && actualdeckvalue <= actualvalue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "10":
                    if (actualdeckvalue != 7)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }
        else
        {
            switch (_card.value)
            {
                case "J":
                    if (pileCard.GetValue() != "7" && pileCard.GetValue() != "K" && pileCard.GetValue() != "Q")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "Q":
                    if (pileCard.GetValue() != "7" && pileCard.GetValue() != "K")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "K":
                    if (pileCard.GetValue() != "7")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }

        }
        return false;
    }




}


