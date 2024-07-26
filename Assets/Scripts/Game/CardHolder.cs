using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardHolder : MonoBehaviour
{
    public Card cardInfo;
    public Image spriteHolder;
    public Sprite noCardSprite;

    public GameObject secondCopy, thirdCopy, fourthCopy, RotationHolder, transformHolder;

    public bool OtherplayersCard;
    private float startingPosition;
    private Tween CardUp;
    public bool CanPlay = true;
    public bool Top;

    private int NumberCopys;

    public void InitializeCard(Card self)
    {
        NumberCopys = 1;
        if (!OtherplayersCard)
        {
            spriteHolder.sprite = self.Image;
        }
        cardInfo = self;
        gameObject.name = self.value;
        if (secondCopy != null)
        {
            secondCopy.SetActive(false);
            thirdCopy.SetActive(false);
            fourthCopy.SetActive(false);
        }

    }

    public void AddCopy()
    {
        if (!secondCopy.activeSelf)
        {
            secondCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                secondCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }
            NumberCopys = 2;
            return;
        }
        if (!thirdCopy.activeSelf)
        {
            thirdCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                thirdCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }
            NumberCopys = 3;
            return;
        }
        if (!fourthCopy.activeSelf)
        {
            fourthCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                fourthCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }
            NumberCopys = 4;
            return;
        }
    }

    public int GetNumberOfCopys()
    {
        return NumberCopys;
    }

    public Card GetCard()
    {
        return cardInfo;
    }

    public string GetValue()
    {
        return cardInfo.value;
    }


    public void NoCard()
    {
        NumberCopys = 0;
        fourthCopy.SetActive(false);
        thirdCopy.SetActive(false);
        secondCopy.SetActive(false);
        spriteHolder.sprite = noCardSprite;
    }


    private void Setup_MouseEnter()
    {
        startingPosition = RotationHolder.transform.position.y;
        CardUp = RotationHolder.transform.DOMoveY(startingPosition + 50, .3f, true);
        CardUp.Pause();
        CardUp.SetAutoKill(false);
    }

    private void Setup_MouseEnterMultiple()
    {

    }

    public void OnMouseEnter()
    {
        if (CanPlay)
        {
            Top = true;
            if (CardUp == null)
            {
                Setup_MouseEnter();

                CardUp.Play();
            }
            else
            {
                CardUp.Restart();
            }
        }

    }

    public void ForceRestart()
    {
        if (Top)
        {
            Top = false;
            CardUp.PlayBackwards();
        }
    }

    public void OnMouseExit()
    {
        if (CanPlay)
        {
            if (startingPosition != RotationHolder.transform.position.y)
            {
                Top = false;
                CardUp.PlayBackwards();
            }
        }

    }

    private void OffCards(int identifier)
    {
        if (identifier == 2)
        {
            secondCopy.SetActive(false);
            thirdCopy.SetActive(false);
        }
        if (identifier == 3)
        {
            thirdCopy.SetActive(false);
        }
        fourthCopy.SetActive(false);
    }

    public void OnClick_SelectCard(int identifier)
    {
        if (DeckManager.instance.CanItBePlayed(cardInfo))
        {
            CardHolder tempCard = this;

            if (identifier == NumberCopys)
            {
                tempCard.NumberCopys = 1;
            }
            else
            {
                tempCard.NumberCopys = identifier - NumberCopys + 1;
            }
            OffCards(identifier);
            DeckManager.instance.ProcessCard(tempCard);
        }
        else
        {
            UIManager.instance.ErrorEffect();
        }
    }

    public void OnClick_Card()
    {
        if (DeckManager.instance.CanItBePlayed(cardInfo))
        {

            GameManager.instance.GetCurrentPlayer().DeleteCardFromHand(this);
            DeckManager.instance.ProcessCard(this);

            Destroy(this.gameObject);
        }
        else
        {
            UIManager.instance.ErrorEffect();
        }
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
