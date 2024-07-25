using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardHolder : MonoBehaviour
{
    private Card cardInfo;
    public Image spriteHolder;
    public Sprite noCardSprite;

    public GameObject secondCopy, thirdCopy, fourthCopy, RotationHolder, transformHolder;

    public bool OtherplayersCard;
    private float startingPosition;
    private Tween CardUp;
    public bool CanPlay = true;
    public bool Top;

    public void InitializeCard(Card self)
    {
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

            return;
        }
        if (!thirdCopy.activeSelf)
        {
            thirdCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                thirdCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }

            return;
        }
        if (!fourthCopy.activeSelf)
        {
            fourthCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                fourthCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }
            return;
        }
    }

    public int GetNumberOfCopys()
    {
        if (fourthCopy.activeSelf)
        {
            return 4;
        }
        if (thirdCopy.activeSelf)
        {
            return 3;
        }
        if (secondCopy.activeSelf)
        {
            return 2;
        }


        return 1;
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
        fourthCopy.SetActive(false);
        thirdCopy.SetActive(false);
        secondCopy.SetActive(false);
        spriteHolder.sprite = noCardSprite;
    }


    private void Setup_MouseEnter()
    {
        startingPosition = RotationHolder.transform.position.y;
        CardUp = RotationHolder.transform.DOMoveY(startingPosition + 100, .5f, true);
        CardUp.Pause();
        CardUp.SetAutoKill(false);
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
