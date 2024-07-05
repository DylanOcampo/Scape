using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<CardHolder> cardHand = new List<CardHolder>();

    private List<GameObject> SpawnedCards = new List<GameObject>();

    //public test
    public Card[] firstPackage;
    public Card[] secondPackage;
    public bool CpuPlayer = false;
    //public test


    public GameObject handContainer, PF_Card, PF_OthersCard;

    public bool AmIMainPlayer = false;

    public bool isMyTurn;

    public Card cardToPlay;

    public void ResetPlayer()
    {
        CpuPlayer = false;
        firstPackage = null;
        secondPackage = null;
        cardHand.Clear();

        foreach (GameObject item in SpawnedCards)
        {
            Destroy(item);
        }
        SpawnedCards.Clear();
    }


    public void SetCPUPlayer()
    {
        CpuPlayer = true;
    }

    public void SetFirstPackage(Card[] package)
    {
        firstPackage = package;
    }

    public void SetSecondPackage(Card[] package)
    {
        secondPackage = package;
    }

    public void SetCardHand(Card[] handdealed)
    {
        foreach (Card item in handdealed)
        {
            CreateCardToHand(item);

        }
    }

    public void DeleteCardFromHand(CardHolder _card)
    {
        cardHand.Remove(_card);
    }

    private void CreateCardToHand(Card _cardInfo)
    {

        foreach (CardHolder item in cardHand)
        {

            if (item.name == _cardInfo.value)
            {
                item.GetComponent<CardHolder>().AddCopy();
                return;
            }

        }
        CardHolder _card;
        GameObject Instance;
        if (!CpuPlayer)
        {

            Instance = Instantiate(PF_Card, handContainer.transform);
            _card = Instance.GetComponent<CardHolder>();
        }
        else
        {
            Instance = Instantiate(PF_OthersCard, handContainer.transform);
            _card = Instance.GetComponent<CardHolder>();
        }
        SpawnedCards.Add(Instance);
        _card.OtherplayersCard = CpuPlayer;
        _card.InitializeCard(_cardInfo);
        cardHand.Add(_card);

    }

    public void DealPackage()
    {
        if (cardHand.Count > 0)
        {
            return;
        }
        Debug.Log(1);
        if (firstPackage != null)
        {
            foreach (Card item in firstPackage)
            {
                CreateCardToHand(item);
            }
            firstPackage = null;
            return;
        }

        if (secondPackage != null)
        {
            foreach (Card item in secondPackage)
            {
                CreateCardToHand(item);
            }
            secondPackage = null;
            return;
        }

        UIManager.instance.EndGameEffect();
    }

    public bool NeedsToDealACard()
    {
        if (cardHand.Count < 3)
        {
            return true;
        }
        return false;
    }

    public void DrawCard(Card item)
    {
        CreateCardToHand(item);
    }

    public void CorrectHand()
    {
        if (cardHand.Contains(null))
        {
            cardHand.Remove(null);
            CorrectHand();
        }
        return;
    }


    public bool CheckIfHandIsPossibleToPlay()
    {
        foreach (CardHolder _card in cardHand)
        {
            if (DeckManager.instance.CanItBePlayed(_card.GetCard()))
            {
                cardToPlay = _card.GetCard();
                return true;
            }
        }
        return false;
    }

    private void PlayCheapestCard()
    {
        CardHolder tempcard = null;

        foreach (CardHolder _card in cardHand)
        {
            if (DeckManager.instance.CanItBePlayed(_card.GetCard()))
            {
                tempcard = _card;
                break;
            }
        }
        cardHand.Remove(tempcard);
        UIManager.instance.AnimationEffect(GameManager.instance.TurnPosition).OnComplete(() => tempcard.OnClick_Card());
    }

    public void OnClick_SortHand()
    {
        cardHand.Sort(SortByScore);
        string temp = "";
        foreach (var item in cardHand)
        {
            temp = " " + item;
        }
    }

    static int SortByScore(CardHolder c1, CardHolder c2)
    {
        string card1 = c1.GetCard().value;
        string card2 = c2.GetCard().value;

        if (c1.GetCard().value == "K")
        {
            if (c2.GetCard().value == "1")
            {
                return -1;
            }
            if (c2.GetCard().value == "K")
            {
                return 0;
            }
            return 1;
        }
        if (c1.GetCard().value == "Q")
        {
            if (c2.GetCard().value == "1")
            {
                return -1;
            }
            if (c2.GetCard().value == "K")
            {
                return -1;
            }
            if (c2.GetCard().value == "Q")
            {
                return 0;
            }
            return 1;
        }
        if (c1.GetCard().value == "J")
        {
            if (c2.GetCard().value == "1")
            {
                return -1;
            }
            if (c2.GetCard().value == "K")
            {
                return -1;
            }
            if (c2.GetCard().value == "Q")
            {
                return -1;
            }
            if (c2.GetCard().value == "J")
            {
                return 0;
            }
            return 1;
        }
        if (c1.GetCard().value == "1")
        {
            if (c2.GetCard().value == "1")
            {
                return 0;
            }
            return 1;
        }

        if (c2.GetCard().value == "K")
        {

            return 1;
        }
        if (c2.GetCard().value == "Q")
        {

            return 1;
        }
        if (c2.GetCard().value == "J")
        {

            return -1;
        }
        if (c2.GetCard().value == "1")
        {

            return -1;
        }

        return int.Parse(card1).CompareTo(int.Parse(card2));
    }


    public void CPUPlayerTurn()
    {
        if (CpuPlayer)
        {
            DeckManager.instance.DealCard(this);
            OnClick_SortHand();
            PlayCheapestCard();
        }
    }
}
