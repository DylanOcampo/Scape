
using System;
using UnityEngine;


[Serializable]
public class Card
{
    public string value;
    public Sprite Image;
    public CardType Cardtype;

}
[Serializable]
public enum CardType{
    hearts,
    spades,
    diamonds, 
    clubs
}

