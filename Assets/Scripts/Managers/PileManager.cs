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


    //private stuff vvv
    public int placement;

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
        Debug.Log(CardsInPile);
        if (CardsInPile <= 3)
        {
            return BottomCards[GetPlacement() - 1].GetValue();
        }
        else
        {
            if (GetPlacement() == 0)
            {
                return RecycleCards[4].GetValue();
            }
            else
            {
                return RecycleCards[GetPlacement() - 1].GetValue();
            }

        }
    }



    public void InitializeCard(Card self)
    {
        int _placement = GetPlacement();
        if (CardsInPile > 2)
        {
            if (self.value == GetLatestValue() && _placement > 4)
            {
                CardHolder tempCard = RecycleCards[0];
                tempCard.gameObject.SetActive(true);
                tempCard.gameObject.transform.SetAsLastSibling();
                tempCard.InitializeCard(self);
                placement = 0;
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
        CardsInPile++;
        placement++;
    }

    public void OnClick_DealCardPile()
    {
        Player tempPlayer = GameManager.instance.MainPlayer.GetComponent<Player>();
        if (tempPlayer.isMyTurn)
        {
            DeckManager.instance.DealPile(tempPlayer);
        }

    }


    public void CleanCardPile()
    {
        CardsInPile = 0;
        placement = 0;
        foreach (CardHolder item in BottomCards)
        {
            item.gameObject.SetActive(false);
        }
        foreach (CardHolder item in RecycleCards)
        {
            item.gameObject.SetActive(false);
        }


    }

}
