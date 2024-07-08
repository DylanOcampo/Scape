using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileManager : MonoBehaviour
{

    private static PileManager _instance;

    public static PileManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PileManager>();
            }
            return _instance;
        }
    }

    public int CardsInPile;
    public List<CardHolder> BottomCards = new List<CardHolder>();

    public List<CardHolder> RecycleCards = new List<CardHolder>();

    public int NumberOfCopys;

    private int placement;


    public List<Card> test = new List<Card>();
    int testpos = 0;

    public void OnClick_test()
    {
        InitializeCard(test[testpos]);
        testpos++;
    }

    public int GetNumberOfCopys()
    {
        return NumberOfCopys;
    }

    private int GetPlacement()
    {

        if (placement > 4)
        {
            placement = 0;
        }

        return placement;
    }

    private string GetLatestValue()
    {
        if (CardsInPile > 3)
        {
            return BottomCards[GetPlacement()].GetValue();
        }
        else
        {
            return RecycleCards[GetPlacement()].GetValue();
        }
    }



    public void InitializeCard(Card self)
    {
        int _placement = GetPlacement();
        if (CardsInPile > 3)
        {
            if (self.value == GetLatestValue() && _placement > 3)
            {
                CardHolder tempCard = RecycleCards[0];
                tempCard.gameObject.SetActive(true);
                tempCard.gameObject.transform.SetAsLastSibling();
                tempCard.InitializeCard(self);
                _placement = 0;
            }
            else
            {
                CardHolder tempCard = RecycleCards[_placement];
                tempCard.gameObject.SetActive(true);
                tempCard.gameObject.transform.SetAsLastSibling();
                tempCard.InitializeCard(self);
            }
        }
        else
        {
            BottomCards[_placement].gameObject.SetActive(true);
            BottomCards[_placement].InitializeCard(self);

        }
        _placement++;
    }


    public void OnClick_DealCardPile()
    {
        Player tempPlayer = GameManager.instance.MainPlayer.GetComponent<Player>();
        if (tempPlayer.isMyTurn)
        {
            DeckManager.instance.DealPile(tempPlayer);
        }

    }

}
