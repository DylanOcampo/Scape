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
    [SerializeField]private List<Card> cardDeck = new List<Card>();

    private List<Card> cardPile = new List<Card>();
    public TextMeshProUGUI cardsInDeck;

    public CardHolder pileCard;
    public TextMeshProUGUI cardsInPile;

    public int actualvalue, actualdeckvalue;
    public void InitializeDeck(){
        Shuffle(cardDeck);
        DealCardsToPlayers();
        InitializeFirstPileCard();
    }

    public void UpdateDeckTracker(){
        cardsInDeck.text = cardDeck.Count.ToString();
        cardsInPile.text = cardPile.Count.ToString();
    }

    private void DealCardsToPlayers(){
        foreach (Player item in GameManager.instance.PlayersInGame())
        {
            //FirstPackage
            item.SetFirstPackage(CreatePackage());
            item.SetSecondPackage(CreatePackage());
            item.SetCardHand(CreatePackage());
        }

    }

    public void InitializeFirstPileCard(){
        InitializePileCard(cardDeck[0]);
        cardDeck.RemoveAt(0);
        
    }

    public void InitializePileCard(Card _cardInfo){
        if(!pileCard.gameObject.activeSelf){
            pileCard.gameObject.SetActive(true);
        }
        cardPile.Add(_cardInfo);
        if(pileCard.name ==_cardInfo.value){
            pileCard.GetComponent<CardHolder>().AddCopy();
            return;
        } 
        pileCard.InitializeCard(_cardInfo);
        UpdateDeckTracker();
        
    }

    private bool CheckForBurn(CardHolder cardPlayed){
        Debug.Log(pileCard + " " +cardPlayed);
        if(pileCard.GetNumberOfCopys() + cardPlayed.GetNumberOfCopys() == 4){
            return true;
        }
        return false;
    }

    private Card[] CreatePackage(){
        Card[] Package;
        Package = new Card[3] {cardDeck[0], cardDeck[1], cardDeck[2]};
        cardDeck.RemoveRange(0, 3);
        return Package;
    }

    public void DealPile(Player _player){
        foreach (Card item in cardPile)
        {
            _player.DrawCard(item);
            
        }
        cardPile.Clear();
        UpdateDeckTracker();
        pileCard.NoCard();
    }

    public bool DealCard(Player _player){
        if(cardDeck.Count > 0 ){
            for (int i = _player.cardHand.Count; i < 4; i++)
            {
                _player.DrawCard(cardDeck[0]);
                cardDeck.RemoveAt(0);
                UpdateDeckTracker();
            }
            return true;
        }else{
            return false;
        }
        
    }

    public void Shuffle(List<Card> self){
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
    
    public void ProcessCard(CardHolder cardPlayed){
        if(CheckForBurn(cardPlayed)){
            BurnPile();
            GameManager.instance.TurnLogic(true);
            
        }else{
            if(cardPlayed.GetCard().value == "1"){
                InitializePileCard(cardPlayed.GetCard());
                UIManager.instance.AsCardEffect();
            }else if(cardPlayed.GetCard().value == "10"){
                BurnPile();
                GameManager.instance.TurnLogic(true); 
                }else{
                    InitializePileCard(cardPlayed.GetCard());
                    if(cardPlayed.GetCard().value == "8"){
                        GameManager.instance.TurnLogic(false, cardPlayed.GetNumberOfCopys());
                    }else{
                        GameManager.instance.TurnLogic(false);
                    }
                }
        }
        
    }

    private void BurnPile(){
        cardPile.Clear();
        pileCard.gameObject.SetActive(false);
    }

    public bool CanItBePlayed(Card _card){
        if(cardPile.Count == 0){
            return true;
        } 
        actualdeckvalue = 11;
        if(pileCard.GetValue() != "K" && pileCard.GetValue() != "Q" && pileCard.GetValue() != "J"){
            actualdeckvalue = int.Parse(pileCard.GetValue());
        }
        if(pileCard.GetValue() == "1")
        {
            if(_card.value == "1" || _card.value == "2" || _card.value == "10" ){
                return true;
            }else{
                return false;
            }
        }
        if(_card.value != "K" && _card.value != "Q" && _card.value != "J"){
            actualvalue = int.Parse(_card.value);
            switch (_card.value){
                case "1":
                    if(actualdeckvalue != 7){
                        return true;
                    }else{
                        return false;
                    }
                
            case "2":
                return true;
            case "3":
                if(actualdeckvalue == 7 || actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }

            case "4":
                if(actualdeckvalue == 7 || actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }
            case "5":
                if(actualdeckvalue == 7 || actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }
            case "6":
                if(actualdeckvalue == 7 || actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }
            case "7":
                if( actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }
            case "8":
                if(actualdeckvalue != 7 && actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }
            case "9":
                if(actualdeckvalue != 7 && actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }
            case "10":
                if(actualdeckvalue != 7 && actualdeckvalue <= actualvalue){
                        return true;
                    }else{
                        return false;
                    }
            }
            
        }else{
            switch (_card.value){
                case "J":
                    if(pileCard.GetValue() != "7" && pileCard.GetValue() != "K" && pileCard.GetValue() != "Q"){
                        return true;
                    }else{
                        return false;
                    }
                case "Q":
                    if(pileCard.GetValue() != "7" && pileCard.GetValue() != "K"){
                        return true;
                    }else{
                        return false;
                    }
                case "K":
                    if(pileCard.GetValue() != "7"){
                        return true;
                    }else{
                        return false;
                    }
                }

        }
        return false;
    }




}


