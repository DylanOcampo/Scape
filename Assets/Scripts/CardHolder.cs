using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    private Card cardInfo;
    public Image spriteHolder;

    public Sprite noCardSprite;

    public GameObject secondCopy, thirdCopy, fourthCopy;

    public bool OtherplayersCard;

    public void InitializeCard(Card self){
        if(!OtherplayersCard){
            spriteHolder.sprite = self.Image;
        }
        cardInfo = self;
        gameObject.name = self.value;
        secondCopy.SetActive(false);
        thirdCopy.SetActive(false);
        fourthCopy.SetActive(false);
    }

    public void AddCopy(){
        if(!secondCopy.activeSelf){
            secondCopy.SetActive(true);
            if(!OtherplayersCard){
                secondCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }
            
            return;
        }
        if(!thirdCopy.activeSelf){
            thirdCopy.SetActive(true);
            if(!OtherplayersCard){
                thirdCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }

            return;
        }
        if(!fourthCopy.activeSelf){
            fourthCopy.SetActive(true);
            if(!OtherplayersCard){
                fourthCopy.GetComponent<Image>().sprite = cardInfo.Image;
            }
            return;
        }
    }

    public int GetNumberOfCopys(){
        if(fourthCopy.activeSelf){
            return 4;
        }
        if(thirdCopy.activeSelf){
            return 3;
        }
        if(secondCopy.activeSelf){
            return 2;
        }
        
        
        return 1;
    }

    public Card GetCard(){
        return cardInfo;
    }

    public string GetValue(){
        return cardInfo.value;
    }


    public void NoCard(){
        fourthCopy.SetActive(false);
        thirdCopy.SetActive(false);
        secondCopy.SetActive(false);
        spriteHolder.sprite = noCardSprite;
    }

    public void OnClick_Card(){
        if(DeckManager.instance.CanItBePlayed(cardInfo)){
            DeckManager.instance.ProcessCard(this);
            GameManager.instance.GetCurrentPlayer().DeleteCardFromHand(this);
            Destroy(this.gameObject);
        }else{
            UIManager.instance.ErrorEffect();
        }
    }
}
