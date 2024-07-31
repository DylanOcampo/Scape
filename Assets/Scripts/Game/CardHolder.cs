using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardHolder : MonoBehaviour
{
    public List<Card> CardsHeld = new List<Card>();
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
        if (CardsHeld.Count > 0)
        {
            CardsHeld.Clear();
        }
        CardsHeld.Add(self);
        gameObject.name = self.value;
        if (secondCopy != null)
        {
            secondCopy.SetActive(false);
            thirdCopy.SetActive(false);
            fourthCopy.SetActive(false);
        }

    }

    public void AddCopy(Card self)
    {
        CardsHeld.Add(self);
        if (!secondCopy.activeSelf)
        {
            secondCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                secondCopy.GetComponent<Image>().sprite = self.Image;
            }
            NumberCopys = 2;
            return;
        }
        if (!thirdCopy.activeSelf)
        {
            thirdCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                thirdCopy.GetComponent<Image>().sprite = self.Image;
            }
            NumberCopys = 3;
            return;
        }
        if (!fourthCopy.activeSelf)
        {
            fourthCopy.SetActive(true);
            if (!OtherplayersCard)
            {
                fourthCopy.GetComponent<Image>().sprite = self.Image;
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
        return CardsHeld[0];
    }

    public string GetValue()
    {
        return CardsHeld[0].value;
    }


    public void NoCard()
    {
        NumberCopys = 0;
        fourthCopy.SetActive(false);
        thirdCopy.SetActive(false);
        secondCopy.SetActive(false);
        spriteHolder.sprite = noCardSprite;
        CardsHeld.Clear();
    }


    private void Setup_MouseEnter()
    {
        startingPosition = RotationHolder.transform.position.y;
        CardUp = RotationHolder.transform.DOMoveY(startingPosition + 50, .3f, true);
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
        if (DeckManager.instance.CanItBePlayed(CardsHeld[0]))
        {
            AudioManager.instance.PlayClip(11);
            CardHolder tempCard = this;

            if (identifier == NumberCopys)
            {
                tempCard.NumberCopys = 1;
                CardsHeld.RemoveAt(identifier - 1);
            }
            else
            {
                int difference = NumberCopys - identifier + 1;
                tempCard.NumberCopys = difference;
                CardsHeld.RemoveRange(identifier - 1, difference);
            }
            OffCards(identifier);
            DeckManager.instance.ProcessCard(tempCard, true);
        }
        else
        {
            UIManager.instance.ErrorEffect();
        }
    }

    public void OnClick_Card(bool isPlayer = true)
    {
        if (DeckManager.instance.CanItBePlayed(CardsHeld[0]))
        {
            if (isPlayer)
            {
                AudioManager.instance.PlayClip(11);
            }
            GameManager.instance.GetCurrentPlayer().DeleteCardFromHand(this);
            DeckManager.instance.ProcessCard(this);

            Destroy(this.gameObject);
        }
        else
        {
            UIManager.instance.ErrorEffect();
        }
    }

}
